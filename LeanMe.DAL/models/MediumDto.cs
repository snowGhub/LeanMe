namespace LeanMe.DAL.models;

public sealed class MediumDto
{
    public string Type { get; set; } = string.Empty;
    public int Id { get; set; }
    public string Title { get; set;  } = string.Empty;
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    
    // Book
    public string? Author { get; set; }
    public string? Isbn { get; set; }
    
    // DVD
    public string? Director { get; set; }
    public int? DurationMinutes { get; set; }
}