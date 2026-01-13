namespace LeanMe.LibaryManager;

public class DVD : Medium
{
    public string Director { get; }
    public int DurationMinutes { get;  }
    
    public DVD(int id, string title, int totalCopies, string director, int durationMinutes) 
        : base(id, title, totalCopies) // Ruft den Konstruktor der Basisklasse auf
    {
        // Validierung: Ein DVD benötigt zwingend einen Regisseur und eine Dauer
        if (string.IsNullOrWhiteSpace(director))
            throw new ArgumentNullException("Director darf nicht leer sein.", nameof(director));

        if (durationMinutes <= 0)
            throw new ArgumentOutOfRangeException(nameof(durationMinutes), "DurationMinutes muss > 0 sein.");

        // Trim() entfernt unnötige Leerzeichen am Anfang und Ende
        Director = director.Trim();
        DurationMinutes = durationMinutes;
    }

    // Überschreibt die abstrakte Methode der Basisklasse für eine spezifische DVD-Beschreibung
    public override string GetDescription() => $"DVD | Regie: {Director} | Dauer: {DurationMinutes} min";
}