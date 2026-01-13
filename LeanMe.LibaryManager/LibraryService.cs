namespace LeanMe.LibaryManager;

public class LibraryService
{
    // Zähler für die nächste vergebene Loan-ID, um Eindeutigkeit zu garantieren
    private int _nextLoanId = 1;
    
    // Interne Datenspeicher (In-Memory Listen)
    private List<Medium> mediaList { get; } = [];
    private List<Customer> customerList { get; } = [];
    private List<Loan> loanList { get; } = [];

    public void LoadSnapshot(IEnumerable<Medium> media, IEnumerable<Customer> customers, IEnumerable<Loan> loans)
    {
        mediaList.AddRange(media);
        customerList.AddRange(customers);
        loanList.AddRange(loans);

        // Initialisiert den ID-Zähler basierend auf dem höchsten vorhandenen Wert,
        // damit neue Ausleihen nach einem Neustart keine ID-Konflikte verursachen.
        _nextLoanId = loanList.Count == 0 ? 1 : loanList.Max(l => l.Id) + 1;
    }
    
    public IReadOnlyList<Medium> ShowMedia() => mediaList;
    
    public IReadOnlyList<Customer> ShowCustomers() => customerList;

    public Loan LendMedium(int mediumId, int customerId, int loanDays = 14)
    {
        // Abrufen der Objekte aus den Listen; wirft Fehler, falls IDs ungültig sind
        var medium = mediaList.FirstOrDefault(m => m.Id == mediumId)
                     ?? throw new KeyNotFoundException($"Medium mit Id {mediumId} nicht gefunden.");

        var customer = customerList.FirstOrDefault(c => c.Id == customerId)
                       ?? throw new KeyNotFoundException($"Kunde mit Id {customerId} nicht gefunden.");

        // Prüfung der Geschäftsregel: Ist noch ein Exemplar im Regal?
        if (!medium.IsAvailable())
            throw new KeyNotFoundException($"Das Medium '{medium.Title}' ist nicht mehr verfügbar");
        
        // Bestandsänderung: Ein Exemplar wird aus dem System "entnommen"
        medium.DecreaseAvailableCopies();

        var loanDate = DateTime.Now;
        var dueDate = loanDate.Date.AddDays(loanDays);

        // Erstellung des Ausleih-Datensatzes
        var loan = new Loan(_nextLoanId++, medium, customer, loanDate, dueDate);
        loanList.Add(loan);
        
        // Verknüpft die Ausleihe auch direkt mit dem Kunden-Objekt
        customer.BorrowdedLoans.Add(loan);

        return loan;
    }

    public void ReturnMedium(int loanId)
    {
        var loan = loanList.FirstOrDefault(l => l.Id == loanId)
                   ?? throw new KeyNotFoundException($"Ausleihe mit Id {loanId} nicht gefunden.");

        // Verhindert doppelte Rückgaben
        if (!loan.isActive())
            throw new InvalidOperationException(
                "Rückgabe nicht möglich: Es gibt keine aktive Ausleihe (bereits zurückgegeben).");
        
        // Status im Loan-Objekt auf "zurückgegeben" setzen
        loan.MarkReturned(DateTime.Now);
        
        // Das Medium wieder für andere Kunden verfügbar machen
        loan.LoaneMedium.IncreaseAvailableCopies();
    }
    
    public IReadOnlyList<Loan> ShowLoans() => loanList;

    public IReadOnlyList<Loan> ShowActiveLoans() => loanList.Where(l => l.isActive()).ToList();
}