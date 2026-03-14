# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["CardiTrack.sln", "./"]
COPY ["src/Worker/CardiTrack.Worker/CardiTrack.Worker.csproj", "src/Worker/CardiTrack.Worker/"]
COPY ["src/Core/CardiTrack.Application/CardiTrack.Application.csproj", "src/Core/CardiTrack.Application/"]
COPY ["src/Core/CardiTrack.Domain/CardiTrack.Domain.csproj", "src/Core/CardiTrack.Domain/"]
COPY ["src/Infrastructure/CardiTrack.Infrastructure/CardiTrack.Infrastructure.csproj", "src/Infrastructure/CardiTrack.Infrastructure/"]
COPY ["src/Infrastructure/CardiTrack.Shared/CardiTrack.Shared.csproj", "src/Infrastructure/CardiTrack.Shared/"]

# Restore dependencies
RUN dotnet restore "src/Worker/CardiTrack.Worker/CardiTrack.Worker.csproj"

# Copy all source files
COPY . .

# Build
WORKDIR "/src/src/Worker/CardiTrack.Worker"
RUN dotnet build "CardiTrack.Worker.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "CardiTrack.Worker.csproj" -c Release -o /app/publish \
    /p:UseAppHost=false \
    /p:PublishTrimmed=false \
    /p:PublishSingleFile=false

# Stage 3: Runtime - Chiseled Ubuntu (Ultra-minimal, non-root, distroless-like)
# Uses aspnet chiseled (not runtime) — Cloud Run requires an HTTP listener for health probes.
# CardiTrack.Worker exposes a minimal /healthz endpoint via a hosted HTTP listener.
FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled AS final
WORKDIR /app

# Chiseled images are:
# - Ultra-minimal (no package manager, no shell, no unnecessary packages)
# - Non-root by default (runs as user 'app' with UID 1654)
# - Security-hardened (minimal attack surface)
# - Small size

# Copy published files
COPY --from=publish /app/publish .

# Create logs directory with proper ownership
USER root
RUN mkdir -p /app/logs && chown -R 1654:1654 /app/logs
USER 1654

# Expose health check port only (no public traffic)
EXPOSE 8080

# Health check — polls the internal /healthz endpoint
HEALTHCHECK --interval=30s --timeout=3s --start-period=20s --retries=3 \
  CMD ["dotnet", "CardiTrack.Worker.dll", "--health-check"]

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_EnableDiagnostics=0

# Entry point
ENTRYPOINT ["dotnet", "CardiTrack.Worker.dll"]

# Labels
LABEL org.opencontainers.image.title="CardiTrack Worker"
LABEL org.opencontainers.image.description="HIPAA-compliant wearable data sync worker with chiseled Ubuntu base"
LABEL org.opencontainers.image.vendor="CardiTrack"
LABEL org.opencontainers.image.version="1.0.0"
