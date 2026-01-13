// See https://aka.ms/new-console-template for more information

using LeanMe;
using LeanMe.DAL;
using LeanMe.DAL.models;
using LeanMe.LibaryManager;

var repo = new JsonLibraryRepository("data/library.json");
var service = new LibraryService();

var snapshot = await repo.LoadAsync();
var (media, customers, loans) = LibrarySnapshotMapper.ToDomain(snapshot);
service.LoadSnapshot(media, customers, loans);

Console.WriteLine("Working Directory: " + Environment.CurrentDirectory);

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== Bibliotheks-CLI ===");
    Console.WriteLine("1) Medien anzeigen");
    Console.WriteLine("2) Kunden anzeigen");
    Console.WriteLine("3) Ausleihen");
    Console.WriteLine("4) Rückgabe");
    Console.WriteLine("5) Aktive Ausleihen anzeigen");
    Console.WriteLine("0) Beenden");
    Console.Write("Auswahl: ");

    var choice = Console.ReadLine()?.Trim();

    try
    {
        // Wählt jenachdem welche Auswahl der User gewählt hat, die Richtige Operation aus
        switch (choice)
        {
            case "1":
                Presentation.ShowMedia(service);
                break;
        
            case "2":
                Presentation.ShowCustomers(service);
                break;
        
            case "3":
                Presentation.LendFlow(service);
                await repo.SaveAsync(LibrarySnapshotMapper.ToDto(service));
                break;
        
            case "4":
                Presentation.ReturnFlow(service);
                await repo.SaveAsync(LibrarySnapshotMapper.ToDto(service));
                break;
        
            case "5":
                Presentation.ShowActiveLoans(service);
                break;
        
            case "0":
                return;
        
            default:
                Console.WriteLine("Ungültige Auswahl!");
                break;
        }
    }
    catch (Exception e)
    {
        // Abgefangene Exceptions werden hier ausgeworfen
        Console.WriteLine("FEHLER: " + e.Message);
    }
}
