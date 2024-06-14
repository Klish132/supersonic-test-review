using core.Jwt;
using domain.Entities;
using domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using services.abstractions.Interfaces;

namespace services.Implementations;

internal class ServicesManager : IServicesManager
{
    private readonly Lazy<IUsersService> _lazyUsersService;
    private readonly Lazy<INotesService> _lazyNotesService;
    private readonly Lazy<IFoldersService> _lazyFoldersService;

    public IUsersService Users => _lazyUsersService.Value;
    public INotesService Notes => _lazyNotesService.Value;
    public IFoldersService Folders => _lazyFoldersService.Value;

    public ServicesManager(UserManager<User> userManager,
        IRepositoryManager repoManager,
        JwtOptions jwtOptions,
        IConfiguration config)
    {
        _lazyUsersService = new Lazy<IUsersService>(() => new UsersService(userManager, jwtOptions));
        _lazyNotesService = new Lazy<INotesService>(() => new NotesService(repoManager));
        _lazyFoldersService = new Lazy<IFoldersService>(() => new FoldersService(repoManager, config));
    }
}