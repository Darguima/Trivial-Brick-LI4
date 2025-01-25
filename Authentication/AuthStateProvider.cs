using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using TrivialBrick.Data.Models;

namespace TrivialBrick.Authentication;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public AuthStateProvider(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }
    private async Task UpdateAuthenticationState(UserSession? userSession)
    {
        ClaimsPrincipal claimsPrincipal;

        if (userSession != null)
        {
            await _sessionStorage.SetAsync("UserSession", userSession);
            claimsPrincipal = CreateClaimsPrincipal(userSession);
        }
        else
        {
            await _sessionStorage.DeleteAsync("UserSession");
            claimsPrincipal = _anonymous;
        }


        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    private ClaimsPrincipal CreateClaimsPrincipal(UserSession userSession)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userSession.ID.ToString()),
            new Claim(ClaimTypes.Role, userSession.IsAdmin ? "Admin" : "User")
        };

        var identity = new ClaimsIdentity(claims, "Auth");
        return new ClaimsPrincipal(identity);
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {

            var userSession = await GetUserSession();

            if (userSession == null)
            {

                return new AuthenticationState(_anonymous);
            }

            var claimsPrincipal = CreateClaimsPrincipal(userSession);
            return new AuthenticationState(claimsPrincipal);
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Erro ao obter o estado de autenticação: {ex.Message}");
            return new AuthenticationState(_anonymous);
        }
    }


    public async Task Login(User user, bool isAdmin)
    {
        var userSession = new UserSession
        {
            ID = user.ID,
            IsAdmin = isAdmin
        };

        await UpdateAuthenticationState(userSession);
    }


    public async Task Logout()
    {
        await UpdateAuthenticationState(null);
    }


    public async Task<UserSession?> GetUserSession()
    {
        var userSessionResult = await _sessionStorage.GetAsync<UserSession>("UserSession");
        var userSession = userSessionResult.Success ? userSessionResult.Value : null;


        return userSession;
    }
}
