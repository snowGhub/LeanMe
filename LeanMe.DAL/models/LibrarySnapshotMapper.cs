using LeanMe.LibaryManager;

namespace LeanMe.DAL.models;

public static class LibrarySnapshotMapper
{
    public static LibrarySnapshotDto ToDto(LibraryService service)
    {
        return new LibrarySnapshotDto
        {
            Media = service.ShowMedia().Select(ToDtoMedium).ToList(),
            Customers = service.ShowCustomers().Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Address = c.Address
            }).ToList(),
            Loans = service.ShowLoans().Select(l => new LoanDto
            {
                Id = l.Id,
                MediumId = l.LoaneMedium.Id,
                CustomerId = l.Customer.Id,
                LoanDate = l.LoanDate,
                DueDate = l.DueDate,
                ReturnDate = l.ReturnDate,
            }).ToList(),
        };
    }
    
    public static (List<Medium> media, List<Customer> customers, List<Loan> loans) ToDomain(LibrarySnapshotDto dto)
    {
        if (dto is null) throw new ArgumentException(nameof(dto));

        var media = dto.Media.Select(ToDomainMedium).ToList();
        var customers = dto.Customers.Select(c => new Customer(c.Id, c.Name, c.Address)).ToList();

        var mediaById = media.ToDictionary(m => m.Id);
        var customerbyId = customers.ToDictionary(c => c.Id);

        var loans = new List<Loan>();
        foreach (var l in dto.Loans)
        {
            if (!mediaById.TryGetValue(l.MediumId, out var medium))
                throw new InvalidOperationException(
                    $"JSON ungültig: MediumId {l.MediumId} in Loan {l.Id} existiert nicht.");
            
            if (!customerbyId.TryGetValue(l.CustomerId, out var customer))
                throw new InvalidOperationException(
                    $"JSON ungültig: CustomerId {l.CustomerId} in Loan {l.Id} existiert nicht.");
            
            var loan = new Loan(l.Id, medium, customer, l.LoanDate, l.DueDate);
            if (l.ReturnDate is not null)
                loan.MarkReturned(l.ReturnDate.Value);
            
            loans.Add(loan);
            customer.BorrowdedLoans.Add(loan);
        }
        
        foreach (var var in dto.Media)
        {
            var m = mediaById[var.Id];
            SyncAvailableCopies(m, var.AvailableCopies);
        }
        
        return (media, customers, loans);
    }

    private static MediumDto ToDtoMedium(Medium medium)
    {
        if (medium is DVD dvd)
        {
            return new MediumDto
            {
                Type = "DVD",
                Id = dvd.Id,
                Title = dvd.Title,
                TotalCopies = dvd.TotalCopies,
                AvailableCopies = dvd.AvailableCopies,
                Director = dvd.Director,
                DurationMinutes = dvd.DurationMinutes
            };
        }

        if (medium is Book book)
        {
            return new MediumDto
            {
                Type = "Book",
                Id = book.Id,
                Title = book.Title,
                TotalCopies = book.TotalCopies,
                AvailableCopies = book.AvailableCopies,
                Author = book.Author,
                Isbn = book.Isbn
            };
        }
        
        throw new InvalidOperationException($"Unbekannter Medientyp '{medium.GetType().Name}'");
    }
    
    private static Medium ToDomainMedium(MediumDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Type))
            throw new InvalidOperationException($"JSON ungültig: Medium.Type fehlt bei Id {dto.Id}.");

        return dto.Type.Trim().ToLowerInvariant() switch
        {
            "dvd" => new DVD(dto.Id, dto.Title, dto.TotalCopies,
                dto.Director ?? throw new InvalidOperationException($"DVD {dto.Id}: Director fehlt."),
                dto.DurationMinutes ?? throw new InvalidOperationException($"DVD {dto.Id}: DurationMinutes fehlt.")),

            "book" => new Book(dto.Id, dto.Title, dto.TotalCopies,
                dto.Author ?? throw new InvalidOperationException($"Book {dto.Id}: Author fehlt."),
                dto.Isbn ?? throw new InvalidOperationException($"Book {dto.Id}: ISBN fehlt.")),
            
            _ => throw new InvalidOperationException($"Unbekannter Medientyp  '{dto.Type}' bei Medium {dto.Id}.")
        };
    }

    private static void SyncAvailableCopies(Medium medium, int desiredAvailable)
    {
        while (medium.AvailableCopies < desiredAvailable)
            medium.IncreaseAvailableCopies();

        while (medium.AvailableCopies > desiredAvailable)
            medium.DecreaseAvailableCopies();
    }
}