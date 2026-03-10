using System.ComponentModel.DataAnnotations;

namespace CardiTrack.Domain.Enums;

public enum ReportFormat
{
    [Display(Name = "PDF")]
    Pdf = 1,

    [Display(Name = "CSV")]
    Csv = 2,

    /// <summary>FHIR R4 Bundle (application/fhir+json) — MVP 1</summary>
    [Display(Name = "FHIR R4")]
    FhirR4 = 3,

    /// <summary>HL7 v2 ORU^R01 (application/hl7-v2+er7) — MVP 2</summary>
    [Display(Name = "HL7 v2")]
    Hl7V2 = 4
}
