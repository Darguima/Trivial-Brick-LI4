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
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userSession.ID.ToString()),
                new(ClaimTypes.Role, userSession.IsAdmin ? "Admin" : "User")
            }, "Auth"));
        }
        else
        {
            await _sessionStorage.DeleteAsync("UserSession");
            claimsPrincipal = _anonymous;
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSession = await GetUserSession();

            if (userSession == null)
                return await Task.FromResult(new AuthenticationState(_anonymous));

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userSession.ID.ToString()),
                new(ClaimTypes.Role, userSession.IsAdmin ? "Admin" : "User")
            }, "Auth"));

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        catch (Exception)
        {
            return await Task.FromResult(new AuthenticationState(_anonymous));
        }
    }

    public async Task Login(Client client, bool isAdmin)
    {
        await UpdateAuthenticationState(new UserSession
        {
            ID = client.ID,
            IsAdmin = isAdmin
        });
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