using LeanMe.LibaryManager;

namespace LeanMe;

public static class Presentation
{
    public static void ShowMedia(LibraryService service)
    {
        Console.WriteLine();
        Console.WriteLine("== Medien ==");
        foreach (var var in service.ShowMedia())
        {
            Console.WriteLine(var);
        }
    }

    public static void ShowCustomers(LibraryService service)
    {
        Console.WriteLine();
        Console.WriteLine("== Kunden ==");
        foreach (var var in service.ShowCustomers())
        {
            Console.WriteLine(var);
        }
    }

    public static void ShowActiveLoans(LibraryService service)
    {
        Console.WriteLine();
        Console.WriteLine("== Aktive Ausleihen ==");
        var active = service.ShowActiveLoans();
        if (active.Count == 0)
        {
            Console.WriteLine("Keine aktiven Ausleihen vorhanden.");
            return;
        }

        foreach (var l in active)
        {
            Console.WriteLine(l);
        }
    }

    // Ausleih Flow wird hier drin gestartet
    public static void LendFlow(LibraryService service)
    {
        Console.WriteLine();
        Console.WriteLine("== AUSLEIHE ==");
        
        ShowCustomers(service);
        Console.WriteLine("Kundennummer eingeben: ");
        var customerId = Utility.ReadInt();
        
        ShowMedia(service);
        Console.WriteLine("Medium-Id eingeben: ");
        var mediumId = Utility.ReadInt();
        
        Console.Write("Leihdauer in Tagen (Enter für 14 Tage):");
        var daysText = Console.ReadLine()?.Trim();
        var days = string.IsNullOrWhiteSpace(daysText) ? 14 : int.Parse(daysText);
        
        var loan = service.LendMedium(mediumId, customerId, days);
        
        Console.WriteLine("Ausleihe OK!");
        Console.WriteLine(loan);
    }

    // Hier wollen wir ein Buch zurückgeben
    public static void ReturnFlow(LibraryService service)
    {
        Console.WriteLine();
        Console.WriteLine("== RÜCKGABE ==");

        ShowActiveLoans(service);
        Console.WriteLine("Loan-Id eingeben:");
        var loanId = Utility.ReadInt();
        
        service.ReturnMedium(loanId);
        
        Console.WriteLine("Rückgabe OK!");
    }
}