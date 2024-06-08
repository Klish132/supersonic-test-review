namespace services.abstractions.Interfaces;

public interface IServicesManager
{
    IUsersService Users { get; }
    INotesService Notes { get; }
    IFoldersService Folders { get; }
}