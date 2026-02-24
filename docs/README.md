# CardiTrack Documentation

Welcome to the CardiTrack documentation. This directory contains comprehensive documentation for the entire CardiTrack platform.

## 📚 Documentation Structure

### Core Documentation

#### [SOLUTION_MANIFEST.md](./SOLUTION_MANIFEST.md)
**The complete solution overview and product vision.**
- Executive summary and business model
- Technical architecture overview
- Core features and capabilities
- Pricing tiers and unit economics
- Development roadmap and milestones
- Team requirements and success criteria

**Start here** if you're new to CardiTrack or need a comprehensive overview.

#### [MARKET_ANALYSIS.md](./MARKET_ANALYSIS.md)
**Comprehensive competitive analysis and market positioning.**
- Market size and growth projections
- Target customer segments
- Detailed competitor analysis with feature comparisons
- Value-added features vs each competitor
- Market positioning strategy
- Go-to-market strategy and risks

**Read this** to understand the competitive landscape and CardiTrack's market position.

#### [LLM_DESIGN.md](./LLM_DESIGN.md)
**MedGemma 1.5 4B inference design and Azure Container Apps deployment.**
- Model selection rationale (4B vs 27B, T4 vs A100)
- vLLM serving configuration and flags
- Fitbit data ingestion pipeline (Event Hubs + 5-min batching)
- Prompt structure and prefix caching strategy
- Cost estimates and important caveats

#### [INFRASTRUCTURE.md](./INFRASTRUCTURE.md)
**Complete infrastructure and database documentation.**
- Database schema and entity relationships
- Entity Framework Core setup and migrations
- Security and encryption (AES-256-GCM)
- Cloud infrastructure (Azure resources)
- Terraform configuration and deployment
- CI/CD pipeline and monitoring
- Scaling strategy and disaster recovery

**Reference this** for infrastructure setup, deployment, and database operations.

---

### Application Documentation

Located in `/apps/` - Each application has its own comprehensive README.

#### [apps/api/](./apps/api/)
**ASP.NET Core Web API Documentation**
- API endpoints and request/response formats
- Authentication and authorization (Auth0/JWT)
- Error handling and status codes
- Rate limiting and HIPAA compliance
- Configuration and local development
- Health checks and deployment

#### [apps/web/](./apps/web/)
**Blazor Server Web Dashboard Documentation**
- Component structure and organization
- SignalR real-time integration
- Authentication flow and protected pages
- Running locally and deployment
- Performance optimization
- Testing strategies

#### [apps/mobile/](./apps/mobile/)
**.NET MAUI Mobile App Documentation**
- Cross-platform architecture (iOS, Android)
- MVVM pattern and ViewModels
- Platform-specific implementations (HealthKit, Health Connect)
- Push notifications and offline support
- Building and publishing to app stores
- Testing and troubleshooting

#### [apps/functions/](./apps/functions/)
**Azure Functions Background Jobs Documentation**
- Function implementations and triggers
- CRON schedules and queue triggers
- Configuration and monitoring
- Deployment and CI/CD
- Performance optimization and cost management

---

### Technical Reference

Located in `/technical/` - Detailed technical guides and specifications.

#### [AUTH0_INTEGRATION.md](./technical/AUTH0_INTEGRATION.md)
Complete guide to Auth0 authentication integration, OAuth flows, and security configuration.

#### [ENTITY_SUMMARY.md](./technical/ENTITY_SUMMARY.md)
Detailed summary of all domain entities, their properties, and relationships.

#### [ENUM_EXTENSIONS_GUIDE.md](./technical/ENUM_EXTENSIONS_GUIDE.md)
Guide to enum extensions and helper methods used throughout the solution.

#### [USER_ONBOARDING_PROCESS.md](./technical/USER_ONBOARDING_PROCESS.md)
Step-by-step guide to the user onboarding process, device connection flows, and OAuth integration.

---

### Additional Documentation

#### `/architecture/`
System architecture diagrams, design patterns, and architectural decision records (ADRs).

#### `/compliance/`
HIPAA compliance documentation, security policies, privacy policies, and audit procedures.

#### `/deployment/`
Deployment guides for different environments (dev, staging, production), Azure setup, and CI/CD configuration.

#### `/developer-guide/`
Getting started guides, coding standards, contribution guidelines, and development best practices.

#### `/devices/`
Device-specific integration guides (Fitbit, Apple Watch, Garmin, Samsung, etc.) and device adapter pattern documentation.

#### `/reference/`
API specifications, database diagrams, configuration references, and other technical references.

#### `/archive/`
Deprecated or superseded documentation kept for historical reference.

---

## 🚀 Quick Start Guides

### For New Developers

1. **Read**: [SOLUTION_MANIFEST.md](./SOLUTION_MANIFEST.md) - Understand the product
2. **Read**: [INFRASTRUCTURE.md](./INFRASTRUCTURE.md) - Understand the architecture
3. **Setup**: Follow developer-guide/getting-started.md
4. **Explore**: Review application docs in /apps/ for your area of work

### For Business Stakeholders

1. **Read**: [SOLUTION_MANIFEST.md](./SOLUTION_MANIFEST.md) - Product vision and roadmap
2. **Read**: [MARKET_ANALYSIS.md](./MARKET_ANALYSIS.md) - Market opportunity and competition
3. **Review**: Pricing tiers and unit economics in SOLUTION_MANIFEST.md

### For DevOps/Infrastructure

1. **Read**: [INFRASTRUCTURE.md](./INFRASTRUCTURE.md) - Complete infrastructure guide
2. **Review**: /deployment/ for environment-specific configurations
3. **Reference**: Terraform modules and Azure resource setup

### For API Consumers

1. **Read**: [apps/api/README.md](./apps/api/README.md) - API documentation
2. **Reference**: API specifications in /reference/
3. **Test**: Use Swagger UI at https://localhost:7001/swagger (local development)

---

## 📖 Documentation Conventions

### File Naming
- `UPPERCASE.md` - Core documentation files (manifest, analysis, infrastructure)
- `lowercase.md` - Supporting documentation and guides
- `README.md` - Index files for directories

### Sections
All major documentation files include:
- **Table of Contents** - For easy navigation
- **Overview** - High-level summary
- **Detailed Content** - Organized by topic
- **Code Examples** - Where applicable
- **References** - Links to related docs

### Code Blocks
Code examples specify language for syntax highlighting:
```csharp
// C# example
public class Example { }
```

```bash
# Bash example
dotnet build
```

---

## 🔄 Keeping Documentation Updated

### When to Update Documentation

**Always update documentation when:**
- Adding new features or endpoints
- Changing database schema
- Modifying infrastructure
- Adding new integrations
- Changing pricing or business model
- Updating deployment procedures

### Documentation Ownership

| Documentation | Owner | Update Frequency |
|--------------|-------|------------------|
| SOLUTION_MANIFEST.md | Product Lead | Monthly or on major changes |
| MARKET_ANALYSIS.md | Business/Marketing | Quarterly |
| INFRASTRUCTURE.md | DevOps Lead | On infrastructure changes |
| apps/api/ | Backend Team | On API changes |
| apps/web/ | Frontend Team | On UI changes |
| apps/mobile/ | Mobile Team | On mobile app changes |
| apps/functions/ | Backend Team | On function changes |
| /technical/ | Tech Lead | As needed |

---

## 📝 Documentation Version History

### Version 2.0 (January 8, 2026)
- ✅ Reorganized documentation structure
- ✅ Created comprehensive SOLUTION_MANIFEST.md
- ✅ Created detailed MARKET_ANALYSIS.md
- ✅ Created INFRASTRUCTURE.md consolidating infrastructure docs
- ✅ Created app-specific documentation in /apps/
- ✅ Moved technical guides to /technical/
- ✅ Archived deprecated documentation

### Version 1.0 (January 5, 2026)
- Initial documentation structure
- Basic technical documentation
- Entity and infrastructure setup guides

---

## 🆘 Getting Help

### Documentation Issues
If you find errors, outdated information, or missing documentation:
1. Create an issue on GitHub
2. Tag with `documentation` label
3. Assign to documentation owner (see table above)

### Questions
For questions about:
- **Product/Business**: Contact product team
- **Technical Architecture**: Contact tech lead
- **API Usage**: See apps/api/README.md or contact backend team
- **Deployment**: Contact DevOps team

---

## 🔗 External Resources

### CardiTrack Resources
- **GitHub Repository**: https://github.com/marigbede/CardiTrack
- **Website**: (Coming soon)
- **Support**: support@carditrack.com

### Technology Documentation
- [.NET 10 Documentation](https://docs.microsoft.com/dotnet/)
- [ASP.NET Core](https://docs.microsoft.com/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Blazor](https://docs.microsoft.com/aspnet/core/blazor/)
- [.NET MAUI](https://docs.microsoft.com/dotnet/maui/)
- [Azure Documentation](https://docs.microsoft.com/azure/)
- [Terraform Azure Provider](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs)

### Device Integration Documentation
- [Fitbit Web API](https://dev.fitbit.com/build/reference/web-api/)
- [Apple HealthKit](https://developer.apple.com/documentation/healthkit)
- [Garmin Connect API](https://developer.garmin.com/gc-developer-program/)
- [Samsung Health SDK](https://developer.samsung.com/health)

---

## 📄 License

All documentation is proprietary and confidential.

---

**Last Updated**: February 24, 2026
**Maintained By**: CardiTrack Development Team
**Version**: 2.0
