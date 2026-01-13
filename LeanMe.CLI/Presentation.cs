using LeanMe.LibaryManager;

namespace LeanMe;

public static class Presentation
{
    // Listet alle verfügbaren Medien (Bücher, DVDs etc.) in der Konsole auf
    public static void ShowMedia(LibraryService service)
    {
        Console.WriteLine();
        Console.WriteLine("== Medien ==");
        // Holt die Liste der Medien vom Service und gibt jedes einzelne aus
        foreach (var var in service.ShowMedia())
        {
            Console.WriteLine(var);
        }
    }

    // Listet alle registrierten Kunden der Bibliothek auf
    public static void ShowCustomers(LibraryService service)
    {
        Console.WriteLine();
        Console.WriteLine("== Kunden ==");
        // Nutzt die ToString-Implementierung der Customer-Klasse für die Anzeige
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
        
        // Prüfen, ob überhaupt Medien ausgeliehen sind, um leere Listen abzufangen
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

    // Führt den Benutzer schrittweise durch den Ausleihvorgang
    public static void LendFlow(LibraryService service)
    {
        Console.WriteLine();
        Console.WriteLine("== AUSLEIHE ==");
        
        // 1. Kunden auswählen (ID wird für die Verknüpfung benötigt)
        ShowCustomers(service);
        Console.WriteLine("Kundennummer eingeben: ");
        var customerId = Utility.ReadInt();
        
        // 2. Medium auswählen
        ShowMedia(service);
        Console.WriteLine("Medium-Id eingeben: ");
        var mediumId = Utility.ReadInt();
        
        // 3. Dauer festlegen mit Logik für Standardwert
        Console.Write("Leihdauer in Tagen (Enter für 14 Tage):");
        var daysText = Console.ReadLine()?.Trim();
        var days = string.IsNullOrWhiteSpace(daysText) ? 14 : int.Parse(daysText);
        
        // 4. Den eigentlichen Leihvorgang im Service ausführen (verbucht Bestände und erstellt Loan)
        var loan = service.LendMedium(mediumId, customerId, days);
        
        Console.WriteLine("Ausleihe OK!");
        Console.WriteLine(loan);
    }

    // Führt den Benutzer durch die Rückgabe eines Mediums
    public static void ReturnFlow(LibraryService service)
    {
        Console.WriteLine();
        Console.WriteLine("== RÜCKGABE ==");

        // Nur aktive Ausleihen anzeigen, damit der User weiß, welche IDs gültig sind
        ShowActiveLoans(service);
        Console.WriteLine("Loan-Id eingeben:");
        var loanId = Utility.ReadInt();
        
        // Rückgabe im Service verarbeiten (setzt Rückgabedatum und erhöht Bestand)
        service.ReturnMedium(loanId);
        
        Console.WriteLine("Rückgabe OK!");
    }
}