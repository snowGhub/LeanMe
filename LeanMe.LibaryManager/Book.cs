namespace LeanMe.LibaryManager;

// Spezialisierung der Basisklasse 'Medium' für Bücher
public class Book : Medium
{
    public string Author { get; }
    public string Isbn { get; }

    public Book(int id, string title, int totalCopies, string author, string isbn)
        : base(id, title, totalCopies) // Ruft den Konstruktor der Basisklasse auf
    {
        // Validierung: Ein Buch benötigt zwingend einen Autor und eine ISBN
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Author darf nicht leer sein.", nameof(author));
        
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN darf nicht leer sein.", nameof(isbn));

        // Trim() entfernt unnötige Leerzeichen am Anfang und Ende
        Author = author.Trim();
        Isbn = isbn.Trim();
    }

    // Überschreibt die abstrakte Methode der Basisklasse für eine spezifische Buch-Beschreibung
    public override string GetDescription() => $"Book | Author: {Author} | ISBN: {Isbn}";
}