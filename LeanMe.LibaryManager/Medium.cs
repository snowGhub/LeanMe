namespace LeanMe.LibaryManager;

// Abstrakte Basisklasse für alle Arten von Medien (Bücher, DVDs, etc.)
public abstract class Medium
{
    private int _availableCopies;

    public int Id
    {
        get;
        private set
        {
            // Validierung: Verhindert ungültige oder negative IDs
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(Id), "Id muss > 0 sein.");
            field = value;
        }
    }

    public string Title
    {
        get;
        private set
        {
            // Sicherstellen, dass ein Titel vorhanden ist
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Titel darf nicht leer sein.", nameof(Title));
            field = value.Trim();
        }
    }

    public int TotalCopies
    {
        get;
        private set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(TotalCopies), "TotalCopies darf nicht negativ sein.");
            
            field = value;
        }
    }

    public int AvailableCopies
    {
        get => _availableCopies;
        private set
        {
            // Konsistenzprüfung: Man kann nicht mehr Exemplare verfügbar haben, als man insgesamt besitzt
            if (value < 0 || value > TotalCopies)
                throw new ArgumentOutOfRangeException(nameof(AvailableCopies),
                    "AvailableCopies muss zwischen 0 und TotalCopies liegen.");
            
            _availableCopies = value;
        }
    }

    protected Medium(int id, string title, int totalCopies)
    {
        Id = id;
        Title = title;
        TotalCopies = totalCopies;
        AvailableCopies = totalCopies;
        // Beim Initialisieren sind im Normalfall alle Exemplare verfügbar
        AvailableCopies = totalCopies;
    }

    
    // Hilfsmethode zur schnellen Bestandsprüfung
    public bool IsAvailable() => _availableCopies > 0;

    public void DecreaseAvailableCopies()
    {
        // Wird beim Ausleihen aufgerufen
        if (!IsAvailable())
            throw new InvalidOperationException("Medium ist nicht verfügbar (keine Exemplare frei).");

        AvailableCopies--;
    }

    public void IncreaseAvailableCopies()
    {
        // Wird bei der Rückgabe aufgerufen
        if (AvailableCopies >= TotalCopies)
            throw new InvalidOperationException("Bestand kann nicht erhöht werden (beriets max. verfügbar).");
        
        AvailableCopies++;
    }
    
    // Muss von abgeleiteten Klassen (Book, DVD) implementiert werden
    public abstract string GetDescription();

    public override string ToString() =>
        $"[{Id}] {Title} | Verfügbar: {AvailableCopies}/{TotalCopies} | {GetDescription()}";
}