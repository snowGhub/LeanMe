namespace LeanMe.LibaryManager;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public List<Loan> BorrowdedLoans { get; set; }

    public override string ToString()
    {
        return base.ToString();
    }
}