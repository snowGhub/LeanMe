namespace LeanMe.LibaryManager;

public class DVD : Medium
{
    public string Director { get; }
    public int DurationMinutes { get;  }
    
    public DVD(int id, string title, int totalCopies, string director, int durationMinutes) 
        : base(id, title, totalCopies)
    {
        if (string.IsNullOrWhiteSpace(director))
            throw new ArgumentNullException("Director darf nicht leer sein.", nameof(director));

        if (durationMinutes <= 0)
            throw new ArgumentOutOfRangeException(nameof(durationMinutes), "DurationMinutes muss > 0 sein.");

        Director = director.Trim();
        DurationMinutes = durationMinutes;
    }

    public override string GetDescription() => $"DVD | Regie: {Director} | Dauer: {DurationMinutes} min";
}