namespace LeanMe.LibaryManager;

public class Loan
{
    public int Id { get; set;  }
    public Medium LoaneMedium { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime dueDate { get; set; }
    // Optional:
    public DateTime? ReturnDate { get; set; }

    public bool isActive()
    {
        return ReturnDate == null;
    }
}