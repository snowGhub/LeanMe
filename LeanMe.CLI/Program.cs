// See https://aka.ms/new-console-template for more information

using LeanMe;
using LeanMe.LibaryManager;


var service = new LibaryService();

Console.WriteLine("=== Lean Console App by Rudolf Astl===");

service.AddMedium(new Book(1,"Clean Code", 2,"Rudolf Astl","ISBN-PLACEHOLDER"));
service.AddMedium(new DVD(2, "Inception", 1,"Rudolf Astl der 2.",140));

service.AddCostumer(new Customer(100, "Rudolf Astl der 3.","Musterstraße 1"));
service.AddCostumer(new Customer(101, "Rudolf Astl der 3.", "Musterweg 2"));

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
                break;
        
            case "4":
                Presentation.ReturnFlow(service);
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
        Console.WriteLine("FEHLER: " + e.Message);
    }
}
