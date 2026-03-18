FROM ollama/ollama:latest AS builder
ARG MODEL_TAG
RUN ollama serve & SERVER_PID=$! && \
    sleep 5 && \
    ollama pull ${MODEL_TAG} && \
    kill $SERVER_PID

FROM ollama/ollama:latest
COPY --from=builder /root/.ollama /root/.ollama
# Cloud Run requires port 8080; Ollama defaults to 11434
ENV OLLAMA_HOST=0.0.0.0:8080
EXPOSE 8080
ENTRYPOINT ["ollama", "serve"]
