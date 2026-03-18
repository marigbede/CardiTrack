# CardiTrack — Claude Instructions

## Code Quality
- All changes must be verified best practices before being applied.

## Architecture
- Background workers and DB polling belong exclusively in `CardiTrack.Worker`. No other project should host background services.
- `CronBackgroundService`, `WorkerOptions`, and `WorkerServiceExtensions` live in `CardiTrack.Worker` — they are not shared infrastructure.
