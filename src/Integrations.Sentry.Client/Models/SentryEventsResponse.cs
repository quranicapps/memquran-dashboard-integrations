using System.Text.Json.Serialization;

namespace Integrations.Sentry.Client.Models;

public class SentryEventsResponse
{
    [JsonPropertyName("data")] public List<Datum> Data { get; set; } = null!;
    [JsonPropertyName("meta")] public Meta Meta { get; set; } = null!;
    [JsonPropertyName("confidence")] public List<Confidence> Confidence { get; set; } = null!;
}

public class Datum
{
    [JsonPropertyName("count(span.duration)")] public double? CountSpanDuration { get; set; }
}

public class Meta
{
    [JsonPropertyName("fields")] public Fields Fields { get; set; } = null!;
    [JsonPropertyName("units")] public Fields Units { get; set; } = null!;
    [JsonPropertyName("isMetricsData")] public bool IsMetricsData { get; set; }
    [JsonPropertyName("isMetricsExtractedData")] public bool IsMetricsExtractedData { get; set; }
    [JsonPropertyName("tips")] public Confidence Tips { get; set; } = null!;
    [JsonPropertyName("datasetReason")] public string DatasetReason { get; set; } = null!;
    [JsonPropertyName("dataset")] public string Dataset { get; set; } = null!;
    [JsonPropertyName("dataScanned")] public string DataScanned { get; set; } = null!;
    [JsonPropertyName("bytesScanned")] public long BytesScanned { get; set; }
    [JsonPropertyName("accuracy")] public Accuracy Accuracy { get; set; } = null!;
}

public class Fields
{
    [JsonPropertyName("count(span.duration)")] public string CountSpanDuration { get; set; } = null!;
}

public class DatumResponse
{
    [JsonPropertyName("code")] public string Code { get; set; } = null!;
    [JsonPropertyName("additional_info")] public List<object> AdditionalInfo { get; set; } = null!;
    [JsonPropertyName("message")] public string Message { get; set; } = null!;
}

public class Confidence;

public class Accuracy
{
    [JsonPropertyName("confidence")] public List<Confidence> Confidence { get; set; } = null!;
}