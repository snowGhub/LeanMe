namespace LeanMe.LibaryManager;

public abstract class Medium
{
    private int _availableCopies;

    public int Id
    {
        get;
        private set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(Id), "Id muss > 0 sein.");
            field = value;
        }
    }

    public string Title
    {
        get;
        private set
        {
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
    }

    public bool IsAvailable() => _availableCopies > 0;

    public void DecreaseAvailableCopies()
    {
        if (!IsAvailable())
            throw new InvalidOperationException("Medium ist nicht verfügbar (keine Exemplare frei).");

        AvailableCopies--;
    }

    public void IncreaseAvailableCopies()
    {
        if (AvailableCopies >= TotalCopies)
            throw new InvalidOperationException("Bestand kann nicht erhöht werden (beriets max. verfügbar).");
        
        AvailableCopies++;
    }
    
    public abstract string GetDescription();

    public override string ToString() =>
        $"[{Id}] {Title} | Verfügbar: {AvailableCopies}/{TotalCopies} | {GetDescription()}";
}