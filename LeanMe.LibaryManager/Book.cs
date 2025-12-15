namespace LeanMe.LibaryManager;

public class Book : Medium
{
    private string Author { get; }
    private string Isbn { get; }

    public Book(int id, string title, int totalCopies, string author, string isbn)
        : base(id, title, totalCopies)
    {
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Author darf nicht leer sein.", nameof(author));
        
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN darf nicht leer sein.", nameof(isbn));

        Author = author.Trim();
        Isbn = isbn.Trim();
    }

    public override string GetDescription() => $"Book | Author: {Author} | ISBN: {Isbn}";
}