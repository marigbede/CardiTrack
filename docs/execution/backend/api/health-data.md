# Health Data API

Provides health metrics, daily summaries, trend charts, baselines, and multi-member dashboard data for caregivers. Also supports offline caching via timestamped data and data export.

**User Stories:** 2.1 (Daily Health Overview), 2.2 (Multi-Member Dashboard), 2.3 (Trend Charts), 5.2 (Mobile Widget), 10.1 (Offline Support)

---

## GET `/api/v1/dashboard`

Returns a summary for all CardiMembers accessible to the authenticated user. Used as the main dashboard view and mobile widget data source.

**Priority:** P0 | **Auth Required:** Yes

### Query Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `sort` | string | `"status"` (alerts first, default) or `"name"` |
| `filter` | string | `"alerts"` — show only members with active alerts |

### Response `200 OK`

```json
{
  "generatedAt": "2026-03-09T10:00:00Z",
  "cardimembers": [
    {
      "id": "cm_01J8K2...",
      "name": "Margaret Doe",
      "photoUrl": "https://cdn.carditrack.com/photos/cm_01J8K2.jpg",
      "healthStatus": "yellow",
      "activeAlertCount": 1,
      "lastSyncedAt": "2026-03-09T08:30:00Z",
      "monitoringPaused": false,
      "summary": {
        "steps": {
          "value": 2500,
          "baseline": 5000,
          "changePercent": -50,
          "unit": "steps/day"
        },
        "restingHeartRate": {
          "value": 68,
          "baseline": 65,
          "changePercent": 4.6,
          "unit": "bpm"
        },
        "sleepHours": {
          "value": 7.2,
          "baseline": 7.5,
          "changePercent": -4,
          "unit": "hours"
        }
      }
    }
  ],
  "total": 1
}
```

**Health Status Values:**

| Value | Meaning |
|-------|---------|
| `green` | All metrics within normal range |
| `yellow` | Minor deviation — worth monitoring |
| `orange` | Notable deviation — recommend action |
| `red` | Critical alert — immediate attention |
| `unknown` | Insufficient data (baseline still learning) |

---

## GET `/api/v1/cardimembers/{id}/health/summary`

Get the daily health overview for a single CardiMember, including all key metrics and comparison to baseline.

**Priority:** P0 | **Auth Required:** Yes

### Path Parameters

| Parameter | Description |
|-----------|-------------|
| `id` | CardiMember ID |

### Query Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `date` | string (ISO 8601) | Specific date (default: today) |

### Response `200 OK`

```json
{
  "cardiMemberId": "cm_01J8K2...",
  "date": "2026-03-09",
  "healthStatus": "yellow",
  "lastSyncedAt": "2026-03-09T08:30:00Z",
  "metrics": {
    "steps": {
      "value": 2500,
      "baseline": 5000,
      "changePercent": -50,
      "unit": "steps",
      "status": "yellow"
    },
    "restingHeartRate": {
      "value": 68,
      "baseline": 65,
      "changePercent": 4.6,
      "unit": "bpm",
      "status": "green"
    },
    "sleepHours": {
      "value": 7.2,
      "baseline": 7.5,
      "changePercent": -4,
      "unit": "hours",
      "status": "green"
    },
    "activeMinutes": {
      "value": 18,
      "baseline": 45,
      "changePercent": -60,
      "unit": "minutes",
      "status": "yellow"
    }
  },
  "deviceSource": {
    "deviceId": "dev_01J9...",
    "provider": "fitbit",
    "displayName": "Fitbit Charge 6"
  }
}
```

### Errors

| Code | Status | Description |
|------|--------|-------------|
| `NO_DEVICE_CONNECTED` | 422 | No active devices for this CardiMember |
| `NO_DATA_FOR_DATE` | 404 | No health data available for the requested date |

---

## GET `/api/v1/cardimembers/{id}/health/trends`

Get time-series trend data for charts. Supports predefined and custom date ranges.

**Priority:** P1 | **Auth Required:** Yes

### Query Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `range` | string | Yes* | `7d`, `30d`, `90d`, or `custom` |
| `from` | string (ISO 8601) | If `range=custom` | Start date |
| `to` | string (ISO 8601) | If `range=custom` | End date (max 365 days) |
| `metrics` | string | No | Comma-separated: `steps,heartRate,sleep,activeMinutes` (default: all) |

### Response `200 OK`

```json
{
  "cardiMemberId": "cm_01J8K2...",
  "range": "30d",
  "from": "2026-02-07",
  "to": "2026-03-09",
  "baseline": {
    "steps": 5000,
    "restingHeartRate": 65,
    "sleepHours": 7.5,
    "activeMinutes": 45
  },
  "series": [
    {
      "date": "2026-02-07",
      "steps": 4800,
      "restingHeartRate": 66,
      "sleepHours": 7.3,
      "activeMinutes": 40,
      "alerts": []
    },
    {
      "date": "2026-03-09",
      "steps": 2500,
      "restingHeartRate": 68,
      "sleepHours": 7.2,
      "activeMinutes": 18,
      "alerts": ["alert_xyz_001"]
    }
  ]
}
```

> `alerts` array in each data point contains alert IDs that occurred on that date — enables chart annotations.

---

## GET `/api/v1/cardimembers/{id}/health/baseline`

Get the current calculated baseline values and learning progress for a CardiMember.

**Priority:** P1 | **Auth Required:** Yes

### Response `200 OK`

```json
{
  "cardiMemberId": "cm_01J8K2...",
  "learningProgress": {
    "daysCaptured": 12,
    "daysRequired": 30,
    "percentComplete": 40,
    "estimatedReadyDate": "2026-04-01"
  },
  "baselineValues": {
    "steps": 5000,
    "restingHeartRate": 65,
    "sleepHours": 7.5,
    "activeMinutes": 45,
    "typicalWakeTime": "07:00",
    "typicalSleepTime": "22:30"
  },
  "usingStatisticalAlerts": true,
  "baselineLastUpdatedAt": "2026-03-08T23:00:00Z"
}
```

---

## GET `/api/v1/cardimembers/{id}/health/export`

Export health data as a PDF or CSV file. Used for doctor visit preparation.

**Priority:** P1 | **Auth Required:** Yes

### Query Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `format` | string | Yes | `pdf` or `csv` |
| `range` | string | Yes | `7d`, `30d`, `90d`, or `custom` |
| `from` | string (ISO 8601) | If `range=custom` | Start date |
| `to` | string (ISO 8601) | If `range=custom` | End date |
| `sections` | string | No | Comma-separated: `metrics,alerts,notes` (default: all) |

### Response `200 OK`

```
Content-Type: application/pdf  (or text/csv)
Content-Disposition: attachment; filename="carditrack-margaret-doe-2026-03-09.pdf"
```

Binary file stream.

### Errors

| Code | Status | Description |
|------|--------|-------------|
| `EXPORT_RANGE_TOO_LARGE` | 400 | Range exceeds 365 days |
| `NO_DATA_IN_RANGE` | 404 | No health data in the specified range |

---

**Related:** [README.md](README.md) | [alerts.md](alerts.md) | [reports.md](reports.md) | [User Stories 2.1, 2.2, 2.3, 5.2, 10.1](../UI/MOBILE/USER_STORIES.md)
