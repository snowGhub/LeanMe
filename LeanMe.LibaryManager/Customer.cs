namespace LeanMe.LibaryManager;

// Repräsentiert eine Person, die Medien in der Bibliothek ausleihen kann
public class Customer
{
    public int Id { get; }
    public string Name { get; }
    public string Address { get; }
    // Liste aller Ausleihvorgänge, die diesem Kunden zugeordnet sind (aktiv und abgeschlossen)
    public List<Loan> BorrowdedLoans { get; } = [];

    public Customer(int id, string name, string address)
    {
        // Validierung der Stammdaten: ID, Name und Adresse sind Pflichtfelder
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Id muss > 0 sein.");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name darf nicht leer sein.", nameof(name));
        if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Adresse darf nicht leer sein.", nameof(address));

        Id = id;
        // Speichert bereinigte Strings ohne führende/folgende Leerzeichen
        Name = name.Trim();
        Address = address.Trim();
    }
    
    // Standard-Formatierung für die Anzeige in Listen (z.B. in der Presentation-Klasse)
    public override string ToString() => $"[{Id}] {Name}, {Address}";
}