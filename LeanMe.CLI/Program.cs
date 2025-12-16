// See https://aka.ms/new-console-template for more information
 
using LeanMe.LibaryManager;


var service = new LibaryService();

Console.WriteLine("=== Lean Console App by Rudolf Astl===");

service.AddMedium(new Book(1,"Clean Code", 2,"Rudolf Astl","ISBN-PLACEHOLDER"));
service.AddMedium(new DVD(2, "Inception", 1,"Rudolf Astl der 2.",140));

service.AddCostumer(new Customer(100, "Rudolf Astl der 3.","Musterstraße 1"));
service.AddCostumer(new Customer(101, "Rudolf Astl der 3.", "Musterweg 2"));

Console.WriteLine("=== Medien ===");
foreach (var variable in service.ShowMedia())
{
    Console.WriteLine(variable);
}

foreach (var variables in service.ShowCustomers())
{
    Console.WriteLine(variables);
}
