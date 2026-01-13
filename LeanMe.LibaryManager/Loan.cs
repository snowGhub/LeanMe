namespace LeanMe.LibaryManager;

public class Loan
{
    public int Id { get;  }
    public Medium LoaneMedium { get; }
    public DateTime LoanDate { get; }
    public Customer Customer { get; }
    public DateTime DueDate { get; }
    // Nullable, da das Datum erst bei der tatsächlichen Rückgabe gesetzt wird
    public DateTime? ReturnDate { get; set; }

    public Loan(int id, Medium medium, Customer customer, DateTime loanDate, DateTime dueDate)
    {
        // Validierung der Eingabewerte im Konstruktor (Defensive Programming)
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Id muss < 0 sein");
        LoaneMedium = medium ?? throw new ArgumentNullException(nameof(medium));
        Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        
        // Logik-Check: Das Rückgabedatum darf nicht vor dem Ausleihdatum liegen
        if (dueDate < loanDate) throw new ArgumentException("DueDate muss nach LoanDate liegen");

        Id = id;
        LoanDate = loanDate;
        DueDate = dueDate;
    }

    // Eine Ausleihe gilt als 'aktiv', solange die Rückgabe noch nicht erfolgt ist
    public bool isActive() => ReturnDate is null;

    public void MarkReturned(DateTime returnDate)
    {
        // Verhindert, dass eine bereits abgeschlossene Ausleihe erneut zurückgegeben wird
        if (!isActive())
            throw new InvalidOperationException("Diese Ausleihe ist bereits zurückgegeben.");

        // Zeitliche Konsistenzprüfung für die Rückgabe
        if (returnDate < LoanDate)
            throw new ArgumentException("ReturnDate darf nicht vor LoanDate liegen.", nameof(returnDate));
        
        ReturnDate = returnDate;
    }

    public override string ToString()
    {
        // Formatiert den Status für die Konsolenausgabe (Aktiv vs. Datum der Rückgabe)
        var status = isActive() ? "AKTIV" : $"ZURÜCK am {ReturnDate:yy-MM-dd}";
        return
            $"Loan [{Id}] | Kunde: {Customer.Name} ({Customer.Id}) | Medium: {LoaneMedium.Title} ({LoaneMedium.Id}) {LoanDate:yy-MM-dd} -> {DueDate:yy-MM-dd} | {status}";
    }
}