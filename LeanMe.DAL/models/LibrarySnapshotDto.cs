using System.Text.Json.Serialization;

namespace LeanMe.DAL.models;

public sealed class LibrarySnapshotDto
{
    [JsonPropertyName("media")]
    public List<MediumDto> Media { get; set; } = [];
    
    [JsonPropertyName("customers")]
    public List<CustomerDto> Customers { get; set; } = [];
    
    [JsonPropertyName("loans")]
    public List<LoanDto> Loans { get; set; } = [];
}