namespace LeanMe.LibaryManager;

public class Loan
{
    public int Id { get; set;  }
    public Medium LoaneMedium { get; set; }
    public DateTime LoanDate { get; set; }
    public Customer Customer { get; set; }
    public DateTime DueDate { get; set; }
    // Optional:
    public DateTime? ReturnDate { get; set; }

    public Loan(int id, Medium medium, Customer customer, DateTime loanDate, DateTime dueDate)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Id muss < 0 sein");
        LoaneMedium = medium ?? throw new ArgumentNullException(nameof(medium));
        Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        if (dueDate < loanDate) throw new ArgumentException("DueDate muss nach LoanDate liegen");

        Id = id;
        LoanDate = loanDate;
        DueDate = dueDate;
    }

    public bool isActive() => ReturnDate is null;

    public void MarkReturned(DateTime returnDate)
    {
        if (!isActive())
            throw new InvalidOperationException("Diese Ausleihe ist bereits zurückgegeben.");

        if (returnDate < LoanDate)
            throw new ArgumentException("ReturnDate darf nicht vor LoanDate liegen.", nameof(returnDate));
        
        ReturnDate = returnDate;
    }

    public override string ToString()
    {
        var status = isActive() ? "AKTIV" : $"ZURÜCK am {ReturnDate:yy-MM-dd}";
        return
            $"Loan [{Id}] | Kunde: {Customer.Name} ({Customer.Id}) | Medium: {LoaneMedium.Title} ({LoaneMedium.Id}) {LoanDate:yy-MM-dd} -> {DueDate:yy-MM-dd} | {status}";
    }
}