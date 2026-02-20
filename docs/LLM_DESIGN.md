# LLM Design — CardiTrack

## Overview

CardiTrack uses MedGemma 1.5 4B as its inference model for near-realtime cardiovascular analysis of wearable data from up to 10,000 Fitbit devices.

---

## Model

| Property | Value |
|----------|-------|
| Model | `google/medgemma-1.5-4b-it` |
| Version | MedGemma 1.5 (updated Jan 2026) |
| Parameters | 4B |
| Type | Multimodal instruction-tuned |
| Source | HuggingFace |

MedGemma 1.5 4B was chosen over the 27B variant for cost and latency reasons — at 4B parameters it fits on a single T4 GPU (~8GB in float16) with sufficient KV cache headroom for concurrent batched requests. It delivers improved accuracy on medical text reasoning and EHR understanding, both directly applicable to structured Fitbit time-series data.

> **⚠️ HuggingFace access required:** This model requires acceptance of the [Health AI Developer Foundations terms of use](https://huggingface.co/google/medgemma-1.5-4b-it) on HuggingFace before the weights can be pulled. Ensure `HF_TOKEN` is from an account that has accepted these terms.

---

## Inference Infrastructure

### Platform: Azure Container Apps (Serverless GPU)

Azure App Service does not support GPUs. The closest PaaS equivalent is Azure Container Apps (ACA) with serverless GPU workload profiles — same managed/serverless feel, no cluster to operate, per-second billing, and scale-to-zero.

| Property | Value |
|----------|-------|
| Platform | Azure Container Apps |
| GPU | NVIDIA T4 (16GB VRAM) — `Consumption-GPU-NC8as-T4` |
| Serving engine | vLLM (OpenAI-compatible endpoint) |
| Min replicas | 1 (keep warm — GPU cold start is 3–5 min) |
| Max replicas | 5 (autoscale on HTTP concurrency) |

### Why T4 over A100

T4 is the cheapest GPU available in ACA (~$0.59/hr active vs ~$3.67/hr for A100). MedGemma 4B fits comfortably on 16GB VRAM in float16. A100 would be overkill for this model size.

### vLLM flags

```
--model google/medgemma-1.5-4b-it
--dtype float16                  # T4 is compute capability 7.5; bfloat16 requires 8.0+
--enable-prefix-caching          # system prompt is identical across users = big throughput win
--max-num-seqs 32
--gpu-memory-utilization 0.90
--max-model-len 8192
```

---

## Deployment

```bash
LOCATION="swedencentral"
RG="carditrack-rg"
ENV="carditrack-env"
APP="medgemma-inference"

az group create --name $RG --location $LOCATION

az containerapp env create \
  --name $ENV --resource-group $RG --location $LOCATION

az containerapp env workload-profile add \
  --name $ENV --resource-group $RG \
  --workload-profile-name "NC8as-T4" \
  --workload-profile-type "Consumption-GPU-NC8as-T4"

az containerapp create \
  --name $APP \
  --resource-group $RG \
  --environment $ENV \
  --workload-profile-name "NC8as-T4" \
  --image vllm/vllm-openai:latest \
  --command "python,-m,vllm.entrypoints.openai.api_server" \
  --args "--model,google/medgemma-1.5-4b-it,--dtype,float16,--enable-prefix-caching,--max-num-seqs,32,--gpu-memory-utilization,0.90,--max-model-len,8192" \
  --env-vars "HF_TOKEN=secretref:hf-token" \
  --ingress external --target-port 8000 \
  --min-replicas 1 --max-replicas 5 \
  --cpu 8 --memory 56Gi
```

> **Note:** Request `Consumption-GPU-NC8as-T4` quota before deploying — this requires an Azure support ticket and can take 1–2 days. Recommended region: `swedencentral` (most available).

---

## Data Ingestion Pipeline

```
Fitbit devices (up to 10,000)
  ↓
Fitbit Subscriptions API (webhook push — no polling)
  ↓
Azure Function (HTTP trigger) — validates signature, forwards event
  ↓
Azure Event Hubs (fitbit-raw)
  ↓
Azure Function (timer, every 5 min) — aggregates per user
  ↓
ACA vLLM endpoint (MedGemma 1.5 4B)
  ↓
Azure Cosmos DB (results store)
```

### Why not Terra?

Terra provides a unified wearable API but costs $499+/month minimum — too expensive at 10,000 users. CardiTrack integrates directly with the Fitbit Subscriptions API.

### Why Event Hubs + 5-min batching?

10,000 devices at ~1 event/30s = ~333 events/s peak. Feeding each event directly to MedGemma would saturate the GPU. Batching per user over 5-minute windows reduces inference requests from ~333/s to a manageable ~33/s burst, significantly improving GPU utilisation and cost.

### Token storage

OAuth tokens for 10,000 Fitbit accounts are stored in Azure Table Storage — cheap, simple, and sufficient for key-value token lookups.

---

## Prompt Structure

Each inference request covers a single user's 5-minute aggregated window.

**System prompt** (fixed — benefits from vLLM prefix caching):
```
[CARDITRACK_SYSTEM_PROMPT]
You are a medical AI assistant analysing cardiovascular wearable data.
Identify anomalies, patterns, or trends that may require clinical attention.
Be concise. Flag severity. Do not diagnose — flag for review.
```

**User prompt** (per user, per 5-min window):
```
Patient wearable data (5-minute window):
- Heart rate: avg=Xbpm, max=Xbpm, min=Xbpm
- HRV (RMSSD): Xms
- SpO2: X%
- Steps: X
- Active zone minutes: X

Assess for cardiovascular anomalies or patterns requiring attention.
```

---

## Cost Estimates

| Component | Estimated Cost |
|-----------|---------------|
| ACA T4 GPU (1 replica, always-on) | ~$0.59/hr (~£430/mo) |
| ACA T4 GPU (scale-to-zero, active hours only) | ~$0.24/hr when active |
| Azure Event Hubs (Standard) | ~£9/mo |
| Azure Functions (consumption plan) | Near-zero at this scale |
| Azure Cosmos DB | ~£20/mo |
| Fitbit API | Free |
| Terra API | Not used — $499+/mo |

---

## Important Caveats

- MedGemma is **not clinical-grade** out of the box. Outputs must be validated before use in any production health context.
- MedGemma 1.5 is **not optimised for multi-turn conversation**. Treat each inference request as stateless.
- All patient data processed through MedGemma must comply with applicable health data regulations (HIPAA, GDPR, etc.).
- The system prompt is identical across all users, making it an ideal candidate for vLLM prefix caching — ensure it is never personalised per user to preserve this benefit.
