using System.Text.Json;
using LeanMe.DAL.models;

namespace LeanMe.DAL;

public sealed class JsonLibraryRepository : ILibraryRepository
{
    private readonly string _filePath;

    // Konfiguration für den Serializer: Schönere Formatierung (Einrückung) 
    // und Toleranz bei der Groß-/Kleinschreibung von Properties
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public JsonLibraryRepository(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Der Pfad darf nicht leer sein.", nameof(filePath));
        
        // Bestimmt den Pfad zum "Eigene Dokumente"-Ordner des aktuellen Benutzers
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        
        // Erstellt den vollständigen absoluten Dateipfad
        _filePath = Path.Combine(documentsPath, filePath);
    }

    public async Task<LibrarySnapshotDto> LoadAsync(CancellationToken ct = default)
    {
        // Sicherstellen, dass die Datei existiert, bevor der Stream geöffnet wird
        if (!File.Exists(_filePath))
        {
            Console.WriteLine($"Datei {_filePath} nicht gefunden. Bitte stelle sicher, dass sie existiert!");
            return new LibrarySnapshotDto(); 
        }
        
        Console.WriteLine($"Lade Library-Daten aus {_filePath}.");
        
        // Öffnet die Datei asynchron zum Lesen und deserialisiert den JSON-Inhalt in ein DTO
        await using var stream = File.OpenRead(_filePath);
        var snapshot = await JsonSerializer.DeserializeAsync<LibrarySnapshotDto>(stream, _options, ct);
        return snapshot ?? new LibrarySnapshotDto();
    }
    
    public async Task SaveAsync(LibrarySnapshotDto snapshot, CancellationToken ct = default)
    {
        if (snapshot is null) throw new ArgumentException(nameof(snapshot));

        // Falls der Zielordner noch nicht existiert, wird er hier angelegt
        var dir = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(dir))
            Directory.CreateDirectory(dir);
        
        // Erstellt (oder überschreibt) die Datei und schreibt das DTO als JSON hinein
        await using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, snapshot, _options, ct);
    }
}