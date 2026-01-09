namespace LeanMe.LibaryManager;

public class LibraryService
{
    private int _nextLoanId = 1;
    private List<Medium> mediaList { get; } = [];
    private List<Customer> customerList { get; } = [];
    private List<Loan> loanList { get; set; } = [];

    public void LoadSnapshot(IEnumerable<Medium> media, IEnumerable<Customer> customers, IEnumerable<Loan> loans)
    {
        mediaList.AddRange(media);
        customerList.AddRange(customers);
        loanList.AddRange(loans);

        _nextLoanId = loanList.Count == 0 ? 1 : loanList.Max(l => l.Id) + 1;
    }
    
    public void AddMedium(Medium medium)
    {
        if (medium is null) throw new ArgumentNullException(nameof(medium));
        if (mediaList.Any(m => m.Id == medium.Id))
            throw new InvalidOperationException($"Medium mit Id {medium.Id} existiert bereits.");
        
        mediaList.Add(medium);
    }
    
    public IReadOnlyList<Medium> ShowMedia() => mediaList;

    public List<Medium> SearchMedia(string titleOrId)
    {
        if (string.IsNullOrWhiteSpace(titleOrId))
            return [];
        
        titleOrId = titleOrId.Trim();

        if (int.TryParse(titleOrId, out var id))
            return mediaList.Where(m => m.Id == id).ToList();
        
        return mediaList
            .Where(m => m.Title.Contains(titleOrId, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public void AddCostumer(Customer customer)
    {
        if (customer is null) throw new ArgumentNullException(nameof(customer));
        if (customerList.Any(c => c.Id == customer.Id))
            throw new InvalidOperationException($"Kund*in mit Id {customer.Id} existiert bereits.");
        
        customerList.Add(customer);
    }
    
    public IReadOnlyList<Customer> ShowCustomers() => customerList;

    public Customer SearchCustomer(string nameOrId)
    {
        if (string.IsNullOrWhiteSpace(nameOrId))
            throw new ArgumentException("Suchtext darf nicht leer sein.", nameof(nameOrId));
        
        nameOrId = nameOrId.Trim();

        if (int.TryParse(nameOrId, out var id))
        {
            return customerList.FirstOrDefault(c => c.Id == id)
                   ?? throw new KeyNotFoundException($"Kunde mit Id {id} nicht gefunden.");
        }

        return customerList.FirstOrDefault(c => c.Name.Contains(nameOrId, StringComparison.OrdinalIgnoreCase))
               ?? throw new KeyNotFoundException($"Kunde mit Name '{nameOrId}' nicht gefunden.");
    }

    public Loan LendMedium(int mediumId, int customerId, int loanDays = 14)
    {
        var medium = mediaList.FirstOrDefault(m => m.Id == mediumId)
                     ?? throw new KeyNotFoundException($"Medium mit Id {mediumId} nicht gefunden.");

        var customer = customerList.FirstOrDefault(c => c.Id == customerId)
                       ?? throw new KeyNotFoundException($"Kunde mit Id {customerId} nicht gefunden.");

        if (!medium.IsAvailable())
            throw new KeyNotFoundException($"Das Medium '{medium.Title}' ist nicht mehr verfügbar");
        
        medium.DecreaseAvailableCopies();

        var loanDate = DateTime.Now;
        var dueDate = loanDate.Date.AddDays(loanDays);

        var loan = new Loan(_nextLoanId++, medium, customer, loanDate, dueDate);
        loanList.Add(loan);
        
        customer.BorrowdedLoans.Add(loan);

        return loan;
    }

    public void ReturnMedium(int loanId)
    {
        var loan = loanList.FirstOrDefault(l => l.Id == loanId)
                   ?? throw new KeyNotFoundException($"Ausleihe mit Id {loanId} nicht gefunden.");

        if (!loan.isActive())
            throw new InvalidOperationException(
                "Rückgabe nicht möglich: Es gibt keine aktive Ausleihe (bereits zurückgegeben).");
        
        loan.MarkReturned(DateTime.Now);
        
        loan.LoaneMedium.IncreaseAvailableCopies();
    }
    
    public IReadOnlyList<Loan> ShowLoans() => loanList;

    public IReadOnlyList<Loan> ShowActiveLoans() => loanList.Where(l => l.isActive()).ToList();
}