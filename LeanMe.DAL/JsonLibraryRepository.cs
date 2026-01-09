using System.Text.Json;
using LeanMe.DAL.models;

namespace LeanMe.DAL;

public sealed class JsonLibraryRepository : ILibraryRepository
{
    private readonly string _filePath;

    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public JsonLibraryRepository(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Der Pfad darf nicht leer sein.", nameof(filePath));
        
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        
        _filePath = Path.Combine(documentsPath, filePath);
    }

    public async Task<LibrarySnapshotDto> LoadAsync(CancellationToken ct = default)
    {
        if (!File.Exists(_filePath))
        {
            Console.WriteLine($"Datei {_filePath} nicht gefunden. Bitte stelle sicher, dass sie existiert!");
            return new LibrarySnapshotDto(); 
        }
        
        Console.WriteLine($"Lade Library-Daten aus {_filePath}.");
        
        await using var stream = File.OpenRead(_filePath);
        var snapshot = await JsonSerializer.DeserializeAsync<LibrarySnapshotDto>(stream, _options, ct);
        return snapshot ?? new LibrarySnapshotDto();
    }
    
    public async Task SaveAsync(LibrarySnapshotDto snapshot, CancellationToken ct = default)
    {
        if (snapshot is null) throw new ArgumentException(nameof(snapshot));

        var dir = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(dir))
            Directory.CreateDirectory(dir);
        
        await using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, snapshot, _options, ct);
    }
}