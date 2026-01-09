namespace LeanMe.DAL.models;

public sealed class LoanDto
{
    public int Id { get; set; }
    public int MediumId { get; set; }
    public int CustomerId { get; set; }
    
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}