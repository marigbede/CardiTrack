# CardiTrack Solution Manifest

## Executive Summary

**CardiTrack** is a multi-device elderly health monitoring platform that provides affordable, preventive health monitoring for families using existing wearable devices with AI-powered pattern analysis.

**Core Value Proposition:** Family members get peace of mind and early warning of health issues at $8-15/month (vs $40-70/month for medical alert systems), using hardware their elderly parents likely already own.

**Target Market:** 150M+ adults aged 65+ across the US and EU, and their family caregivers — 53M in the US *(AARP/NAC, 2020)* and 44M in the EU *(Eurocarers)* — who bear primary responsibility for monitoring elderly relatives living independently.

---

## Product Vision

### Mission Statement

To empower families with affordable, preventive health monitoring for their elderly loved ones, enabling early detection of health concerns and peace of mind through intelligent pattern analysis.

### Core Differentiators

1. **Preventive vs Reactive**: Catches health issues BEFORE emergencies (not just fall detection)
2. **Affordable**: 50-70% cheaper than medical alert systems
3. **Non-Intrusive**: Uses existing devices, not new medical equipment
4. **AI-Powered**: Learns individual baselines, reduces false alerts
5. **Device-Agnostic**: Works with Fitbit, Apple Watch, Garmin, Samsung, and more
6. **No Hardware Lock-in**: Works with devices people already own

---

## Business Model

### Pricing Tiers

#### Tier 1: "Basic Care" - $8/month
- Bring your own wearable device
- Daily activity dashboard
- Email alerts for major deviations
- Single CardiMember monitoring
- 7-day data history

#### Tier 2: "Complete Care" - $15/month
- Support for any supported device
- Real-time SMS/email alerts
- Weekly health reports
- Pattern analysis AI
- Up to 3 CardiMembers
- 90-day data history
- Multi-device support per member

#### Tier 3: "Guardian Plus" - $29.99/month
- Everything in Tier 2
- 24/7 monitoring dashboard
- Unlimited family member access
- Unlimited CardiMembers
- Integration with telemedicine
- Priority support
- 2-year data history
- API access for integration

### Device Bundle Option (Add-on)
- **Fitbit Charge 6 Bundle**: +$100 upfront (includes device)
- **Annual Subscription**: 15% discount (Tier 2: $153/year, saves $27)

### Unit Economics (Tier 2 Example)

```
Monthly revenue per user: $15
Monthly costs per user: ~$2 (hosting, SMS, support)
Monthly profit per user: $13
Annual profit per user: $156

With Device Bundle:
Hardware cost (bulk): $100
Recovery period: ~8 months
Year 1 profit per user: $56
Year 2+ profit per user: $156/year
Customer LTV (3 years): $368
```

---

## Technical Architecture

### Technology Stack

**Backend:**
- .NET 10 (ASP.NET Core Web API)
- Entity Framework Core
- SQL Server / PostgreSQL
- Azure Functions (background jobs)
- ML.NET (pattern analysis & anomaly detection)

**Frontend:**
- Blazor Server (web dashboard)
- .NET MAUI (cross-platform mobile app)
- Bootstrap 5 (UI framework)
- SignalR (real-time updates)

**Infrastructure:**
- Azure Cloud Platform
- Terraform (Infrastructure as Code)
- Docker (containerization)
- GitHub Actions (CI/CD)

**External Integrations:**
- Fitbit Web API
- Apple HealthKit
- Garmin Connect API
- Samsung Health SDK
- Withings API
- Twilio (SMS)
- SendGrid (Email)
- Auth0 (authentication)

### System Architecture

```
┌─────────────────────────────────────────────────────────────┐
│              FAMILY DASHBOARD (Web/Mobile)                  │
│           (Blazor Server / .NET MAUI)                       │
└─────────────────────────────────────────────────────────────┘
                            │
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                    API GATEWAY                              │
│              (ASP.NET Core Web API)                         │
└─────────────────────────────────────────────────────────────┘
            │                    │                    │
            ↓                    ↓                    ↓
┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐
│  Device Services │  │  Alert Service   │  │  User Service    │
│  - Multi-device  │  │  - AI Analysis   │  │  - Auth          │
│  - Data Adapters │  │  - Notifications │  │  - Profiles      │
│  - OAuth Tokens  │  │  - Rules Engine  │  │  - Family Mgmt   │
└──────────────────┘  └──────────────────┘  └──────────────────┘
            │                    │                    │
            └────────────────────┴────────────────────┘
                            │
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                   DATABASE LAYER                            │
│             (SQL Server / PostgreSQL)                       │
│  - Organizations / Users / CardiMembers                     │
│  - Device Connections (Multi-device support)                │
│  - Activity Logs / Baselines / Alerts                       │
│  - Audit Logs (HIPAA compliance)                            │
└─────────────────────────────────────────────────────────────┐
                            │
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                BACKGROUND JOBS (Azure Functions)            │
│  - Device Data Sync (every 30 mins)                         │
│  - Pattern Analysis (ML anomaly detection)                  │
│  - Token Refresh (OAuth management)                         │
│  - Baseline Recalculation (weekly)                          │
│  - Alert Processing (multi-channel)                         │
└─────────────────────────────────────────────────────────────┘
```

### Multi-Device Architecture

CardiTrack uses the **Adapter Pattern** to support multiple wearable devices:

```
Device APIs (Fitbit, Apple, Garmin, Samsung, Withings, Oura, Whoop)
                    ↓
        Device-Specific Adapters
        (Normalize device data formats)
                    ↓
        Unified Health Data Model
        (Steps, Heart Rate, Sleep, SpO2, etc.)
                    ↓
        Pattern Analysis Engine
        (ML.NET anomaly detection)
                    ↓
        Contextual Family Alerts
        (SMS, Email, Push Notifications)
```

---

## Core Features

### 1. Multi-Device Support

**Supported Devices (Roadmap):**

**Phase 1 (MVP - Months 1-3):**
- ✅ Fitbit (Charge 6, Inspire 3, Sense 2, Versa 4)

**Phase 2 (Months 3-6):**
- 🔄 Apple Watch (Series 8+, Ultra)
- 🔄 Garmin (Venu, Forerunner, Vivoactive)
- 🔄 Samsung Galaxy Watch (5, 6)

**Phase 3 (Months 6-12):**
- ⏳ Withings (ScanWatch, Body+)
- ⏳ Oura Ring (Gen 3)
- ⏳ Whoop (4.0)

**Device Capabilities Matrix:**

| Device          | Heart Rate | SpO2 | ECG | Steps | Sleep | GPS |
|-----------------|-----------|------|-----|-------|-------|-----|
| Fitbit Charge 6 | ✅        | ✅   | ✅  | ✅    | ✅    | ✅  |
| Apple Watch 9   | ✅        | ✅   | ✅  | ✅    | ✅    | ✅  |
| Garmin Venu     | ✅        | ✅   | ❌  | ✅    | ✅    | ✅  |
| Samsung Watch 6 | ✅        | ✅   | ✅  | ✅    | ✅    | ✅  |
| Withings Scan   | ✅        | ✅   | ✅  | ✅    | ✅    | ❌  |
| Oura Ring       | ✅        | ✅   | ❌  | ✅    | ✅    | ❌  |
| Whoop 4.0       | ✅        | ✅   | ❌  | ❌    | ✅    | ❌  |

### 2. AI-Powered Pattern Analysis

**Technology:** ML.NET (Microsoft's machine learning framework)

**Algorithms:**
- **Anomaly Detection**: IidSpikeDetector for sudden changes
- **Time Series Forecasting**: SSA (Singular Spectrum Analysis)
- **Pattern Classification**: Activity level categorization

**Learning Process:**
1. Collect baseline data per CardiMember — default 30 days (configurable up to 90)
2. Calculate personalized patterns (steps, heart rate, sleep)
3. Run daily anomaly detection comparing current vs baseline
4. Generate contextual alerts with severity levels
5. Continuously improve models with new data

**Personalized Baseline Metrics:**
- Average daily steps (with standard deviation)
- Resting heart rate patterns
- Sleep duration and quality
- Day-of-week activity patterns
- Typical sleep/wake times

### 3. Preventive Health Alerts

**Alert Types:**

#### Activity Alerts (Preventive)
**Example:**
```
Alert: "Unusual Inactivity"
Trigger: Steps < 50% of baseline for 2+ consecutive days
Severity: Yellow
Message: "Dad's activity has dropped 60% this week. Might be worth a call."
Prevention: Could indicate illness, injury, or depression BEFORE emergency
```

#### Heart Rate Alerts (Preventive)
```
Alert: "Elevated Resting Heart Rate"
Trigger: Resting HR >15% above baseline for 3+ days
Severity: Orange
Message: "Mom's resting heart rate has been elevated. Consider doctor visit."
Prevention: Could indicate infection, stress, or developing cardiac issue
```

#### Sleep Disruption Alerts (Preventive)
```
Alert: "Sleep Pattern Change"
Trigger: Sleep efficiency < 70% for 5+ days
Severity: Yellow
Message: "Dad's sleep quality has declined. Might indicate pain or anxiety."
Prevention: Sleep issues often precede other health problems
```

#### Sudden Pattern Break (Reactive)
```
Alert: "No Morning Activity"
Trigger: No movement detected by 11am (typical wake: 7am)
Severity: Red
Message: "Mom hasn't moved this morning. Please check on her."
Prevention: Fall, illness, or emergency detected early
```

#### Long-term Trend Alerts (Preventive)
```
Alert: "Declining Mobility Trend"
Trigger: Steps declining 5% per week for 4 consecutive weeks
Severity: Orange
Message: "Dad's activity trending down 20% this month. May need PT evaluation."
Prevention: Catches gradual decline before it becomes severe
```

**Target Metrics:**
- False positive rate: <5%
- Alert response time: <30 seconds
- Detection accuracy: >95%

### 4. Family Dashboard

**Web Dashboard (Blazor Server):**
- Real-time health metrics for all CardiMembers
- Activity, heart rate, and sleep trend charts
- Alert management (acknowledge, dismiss, add notes)
- Multi-member overview
- Weekly/monthly health reports
- Device connection management
- Family member access control

**Mobile App (.NET MAUI):**
- Cross-platform (iOS & Android)
- Push notifications for critical alerts
- Quick health overview
- Offline support with local SQLite cache
- Platform-specific integrations (HealthKit on iOS)

### 5. HIPAA Compliance

**Technical Safeguards:**
- ✅ Encryption at rest (Azure SQL TDE)
- ✅ Encryption in transit (TLS 1.2+)
- ✅ Field-level encryption (OAuth tokens, medical notes)
- ✅ Access controls (RBAC, MFA for admins)
- ✅ Session timeout (15 minutes)
- ✅ Comprehensive audit logging

**Administrative Safeguards:**
- Privacy policies
- Security policies
- Breach notification procedures
- Workforce training program
- Business Associate Agreements (BAAs) with all vendors

**Audit Logging:**
- All PHI access tracked
- User ID, CardiMember ID, action, timestamp
- IP address and user agent tracking
- 90-day minimum retention
- Exportable audit trail for compliance

---

## Data Model

### Core Entities

**Organization**
- Multi-tenant support
- Types: Family, Business
- Subscription management

**User**
- Family members/caregivers
- Roles: Admin, Staff, Member
- Authentication via Auth0/JWT

**CardiMember**
- Elderly individuals being monitored
- Personal information
- Health baseline data
- Medical notes (encrypted)

**DeviceConnection**
- Multi-device support per CardiMember
- OAuth tokens (encrypted)
- Connection status tracking
- Primary device designation

**ActivityLog**
- Device-agnostic normalized health data
- Daily metrics: steps, heart rate, sleep, SpO2
- Links to source device
- Time-series data for analysis

**PatternBaseline**
- AI-learned normal patterns
- Personalized per CardiMember
- Recalculated weekly
- Day-of-week variations

**Alert**
- Generated by pattern analysis
- Severity levels: Green (Normal), Yellow (Caution), Orange (Urgent), Red (Critical)
- Acknowledgment tracking
- Resolution workflow

**AuditLog**
- HIPAA compliance tracking
- All PHI access logged
- 90-day retention minimum

---

## Go-to-Market Strategy

### Phase 1: MVP Launch (Months 1-3)

**Month 1: Build MVP**
- Core .NET backend with Fitbit integration
- Basic Blazor dashboard
- Simple alert rules (statistical, no ML yet)
- Database schema and migrations

**Month 2: Beta Testing**
- Recruit 10-20 families (friends, family, local community)
- Use Fitbit "Personal" app type for testing
- Collect feedback on alert accuracy
- Iterate on UX based on feedback

**Month 3: Apply for Device Approvals**
- Submit Fitbit intraday access request
- Apply for Apple HealthKit integration
- Refine baseline algorithms
- Prepare for launch

### Phase 2: Public Launch (Months 4-6)

- Launch with BYOD (Bring Your Own Device) pricing
- Content marketing: SEO, blog, YouTube tutorials
- Partnership outreach: senior centers, retirement communities
- Facebook/Google ads targeting caregivers (45-65 age group)

### Phase 3: Scale (Months 7-12)

- Add Apple Watch and Garmin support
- Launch device bundle option (subsidized hardware)
- Healthcare provider referral program
- Enterprise offering for assisted living facilities

### User Acquisition Channels

**Channel 1: Content Marketing**
- Blog: "How to monitor elderly parents remotely"
- SEO: Target "elderly health monitoring", "Fitbit for seniors"
- YouTube: Setup tutorials, testimonials
- Webinars: Caregiver education

**Channel 2: Senior Community Partnerships**
- Senior centers
- Retirement communities
- AARP partnerships
- Free trial for community members

**Channel 3: Healthcare Provider Referrals**
- Geriatric physicians
- Home health agencies
- Position as "peace of mind" tool (not medical device)

**Channel 4: Direct Advertising**
- Facebook ads: 45-65 year olds (caregiver demographic)
- Google Ads: "monitor elderly parents health", "aging parents safety"
- Retargeting campaigns

---

## Key Metrics & KPIs

### Product Metrics

**Health Metrics:**
- False positive rate: Target <5%
- Alert response time: <30 seconds
- Data sync success rate: >99%
- Token refresh success rate: >99.5%

**Engagement Metrics:**
- Daily active users (family members)
- Alert acknowledgment rate
- Time to acknowledge alert
- Dashboard session duration

### Business Metrics

**Acquisition:**
- Customer acquisition cost (CAC): <$50
- Conversion rate (trial → paid): >20%
- Channel performance tracking

**Retention:**
- Monthly churn rate: <5%
- Customer lifetime value (LTV): >$300
- Net Promoter Score (NPS): >50

**Revenue:**
- Monthly recurring revenue (MRR)
- Average revenue per user (ARPU): $15-20
- LTV/CAC ratio: >3:1

---

## Infrastructure & Operations

### Cloud Infrastructure (Azure)

**MVP Phase (0-100 users):**
- App Service (Basic): $13/month
- Azure SQL (Basic): $5/month
- Functions (Consumption): ~$5/month
- **Total**: ~$25-30/month

**Growth Phase (1,000-10,000 users):**
- App Service (Premium P1V2): $146/month
- Azure SQL (Standard S2): $75/month
- Functions: ~$100/month
- SignalR (Standard): $50/month
- **Total**: ~$371/month
- **Per user cost**: $0.037/month
- **Margin**: $14.96/user/month

### Database Storage

**Per CardiMember Per Year:**
- Activity logs: 365 rows × ~500 bytes = 183 KB
- Pattern baselines: 12 rows × 2 KB = 24 KB
- Alerts: ~50 rows × 1 KB = 50 KB
- **Total**: ~260 KB/member/year

**10,000 CardiMembers:**
- Data: 2.6 GB/year
- With indexes: ~5 GB/year
- Azure SQL Standard S2 (250 GB): Ample headroom

### Scaling Strategy

**Horizontal Scaling:**
- Auto-scale App Service based on CPU (>70%) and memory (>80%)
- Azure Functions auto-scale with queue depth
- Max instances: 10 (API), unlimited (Functions)

**Database Scaling:**
- Read replicas for dashboard queries
- Partition ActivityLogs by CardiMemberId
- Archive logs >2 years to cold storage (Azure Blob)

**Caching:**
- Redis for user sessions and dashboard data
- In-memory cache for reference data
- CDN for static assets

---

## Risk Factors & Mitigation

### Technical Risks

**Risk 1: Device API Changes**
- **Mitigation**: Abstract integrations behind interfaces
- **Backup**: Support multiple device types

**Risk 2: High False Positive Rate**
- **Mitigation**: 30-90 day personalized baselines
- **Backup**: User-configurable sensitivity settings

**Risk 3: OAuth Token Management**
- **Mitigation**: Proactive token refresh every 4 hours
- **Monitoring**: Alert team if refresh rate drops

### Business Risks

**Risk 1: Market Rejection**
- **Mitigation**: Focus on "peace of mind" not "surveillance"
- **Validation**: Beta test with real families

**Risk 2: Competitor Launches Similar Feature**
- **Mitigation**: Move fast, build brand, capture market share
- **Strategy**: Position for acquisition by Google/Fitbit/Apple

**Risk 3: Regulatory Changes**
- **Mitigation**: HIPAA compliance from day 1
- **Legal**: Regular healthcare attorney consultations

### HIPAA/Legal Risks

**Risk 1: Data Breach**
- **Mitigation**: Multi-layer encryption, regular audits
- **Insurance**: Cyber liability with HIPAA coverage ($1-2M)

**Risk 2: Unauthorized Access**
- **Mitigation**: RBAC, audit logging, MFA
- **Monitoring**: Real-time suspicious access alerts

---

## Team Requirements

### MVP Phase (Months 1-3)
- 1 Full-Stack .NET Developer
- 1 Part-time Mobile Developer (.NET MAUI)
- 1 Part-time UI/UX Designer
- 1 Healthcare Compliance Consultant

### Growth Phase (Months 4-12)
- 2-3 Backend Developers (.NET/C#)
- 1 Frontend Developer (Blazor)
- 1 Mobile Developer (.NET MAUI)
- 1 DevOps Engineer (part-time)
- 1 Data Scientist (ML models)
- 1 Customer Support (part-time → full-time)
- 1 Marketing/Growth (contractor)
- 1 Compliance Officer (part-time)

---

## Development Roadmap

### Q1 2026 (Months 1-3): MVP Development
- ✅ Core backend (.NET 10, EF Core, Azure SQL)
- ✅ Fitbit integration (OAuth, data sync)
- ✅ Blazor dashboard (basic features)
- ✅ Statistical anomaly detection
- ✅ SMS/Email alerts
- ✅ Database schema & migrations
- 🔄 Beta testing with 20 families

### Q2 2026 (Months 4-6): Public Launch
- 🔄 ML.NET pattern analysis
- 🔄 .NET MAUI mobile app (iOS & Android)
- 🔄 Advanced dashboard features
- 🔄 Subscription management
- 🔄 Apply for device intraday access
- 🔄 Public launch (BYOD model)

### Q3 2026 (Months 7-9): Multi-Device Support
- ⏳ Apple Watch integration
- ⏳ Garmin integration
- ⏳ Samsung Health integration
- ⏳ Device bundle option
- ⏳ Healthcare provider partnerships

### Q4 2026 (Months 10-12): Enterprise & Scale
- ⏳ Enterprise features (assisted living)
- ⏳ Withings, Oura, Whoop support
- ⏳ Advanced ML models (LSTM, deep learning)
- ⏳ Telemedicine integration
- ⏳ Scale to 1,000+ users

---

## Success Criteria

### MVP Success (Month 3)
- [ ] 20+ beta families onboarded
- [ ] <10% false positive rate
- [ ] >95% data sync success
- [ ] >80% user satisfaction (NPS >50)

### Launch Success (Month 6)
- [ ] 100+ paying customers
- [ ] <5% churn rate
- [ ] $1,500+ MRR
- [ ] CAC <$50

### Growth Success (Month 12)
- [ ] 1,000+ paying customers
- [ ] <3% churn rate
- [ ] $15,000+ MRR
- [ ] LTV/CAC >3:1
- [ ] Support for 3+ device types

---

## Contact & Support

**Website**: https://carditrack.com
**Email**: info@carditrack.com
**Support**: support@carditrack.com
**GitHub**: https://github.com/marigbede/CardiTrack

---

## License

Proprietary and confidential. All rights reserved.

---

**Last Updated**: February 24, 2026
**Version**: 1.0.0
**Status**: In Development
