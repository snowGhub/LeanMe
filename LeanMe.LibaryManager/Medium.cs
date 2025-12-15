namespace LeanMe.LibaryManager;

public abstract class Medium
{
    private int id;
    private string title;
    private int totalCopies;
    private int availableCopies;
    
    public abstract string GetDescription();
}