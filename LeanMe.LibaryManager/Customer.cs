namespace LeanMe.LibaryManager;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public List<Loan> BorrowdedLoans { get; } = [];

    public Customer(int id, string name, string address)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Id muss > 0 sein.");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name darf nicht leer sein.", nameof(name));
        if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Adresse darf nicht leer sein.", nameof(address));

        Id = id;
        Name = name.Trim();
        Address = address.Trim();
    }
    
    public override string ToString() => $"[{Id}] {Name}, {Address}";
}