using LeanMe.DAL.models;

namespace LeanMe.DAL;

public interface ILibraryRepository
{
    Task<LibrarySnapshotDto> LoadAsync(CancellationToken ct = default);
    Task SaveAsync(LibrarySnapshotDto snapshot, CancellationToken ct = default);
}