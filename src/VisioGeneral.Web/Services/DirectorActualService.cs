using Microsoft.EntityFrameworkCore;
using VisioGeneral.Web.Data;
using VisioGeneral.Web.Models.Entities;

namespace VisioGeneral.Web.Services;

public interface IDirectorActualService
{
    Task<Director?> ObtenirDirectorActualAsync();
    Task<int?> ObtenirDirectorActualIdAsync();
}

public class DirectorActualService : IDirectorActualService
{
    private readonly VisioGeneralDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DirectorActualService(VisioGeneralDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Director?> ObtenirDirectorActualAsync()
    {
        var username = ObtenirUsername();
        if (string.IsNullOrEmpty(username))
            return null;

        // Buscar director pel UsernameAD
        return await _context.Directors
            .FirstOrDefaultAsync(d => d.Actiu && d.UsernameAD == username);
    }

    public async Task<int?> ObtenirDirectorActualIdAsync()
    {
        var director = await ObtenirDirectorActualAsync();
        return director?.Id;
    }

    private string? ObtenirUsername()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true)
            return null;

        // El format pot ser DOMAIN\username o username@domain
        var username = user.Identity.Name;

        if (string.IsNullOrEmpty(username))
            return null;

        // Netejar el domini si és necessari
        if (username.Contains('\\'))
            username = username.Split('\\').Last();
        else if (username.Contains('@'))
            username = username.Split('@').First();

        return username;
    }
}
