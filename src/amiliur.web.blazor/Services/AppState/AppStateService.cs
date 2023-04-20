using System.Security.Claims;
using amiliur.security.shared.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace amiliur.web.blazor.Services.AppState;

public class AppStateService : IDisposable
{
    public Action? ErrorMessageChanged;
    public Action? AuthenticationStateChanged;

    private string _errorMessage;

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            RaiseErrorMessageChanged();
        }
    }

    private void RaiseErrorMessageChanged()
    {
        if (ErrorMessageChanged != null)
            ErrorMessageChanged.Invoke();
    }

    private AuthenticationStateProvider AuthenticationState { get; set; }
    public ClaimService ClaimService { get; }

    public AppStateService(AuthenticationStateProvider authenticationState, ClaimService claimService)
    {
        AuthenticationState = authenticationState;
        ClaimService = claimService;
        AuthenticationState.AuthenticationStateChanged += AuthenticationState_AuthenticationStateChanged;
    }

    private void AuthenticationState_AuthenticationStateChanged(Task<AuthenticationState> task)
    {
        AuthenticationStateChanged?.Invoke();
    }

    public async Task<bool> IsAuthenticated()
    {
        var auth = await AuthenticationState.GetAuthenticationStateAsync();
        try
        {
            return auth.User.Identity is {IsAuthenticated: true};
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<IEnumerable<Claim>> GetUserClaims()
    {
        var authState = await AuthenticationState.GetAuthenticationStateAsync();
        return authState.User.Claims;
    }

    public void Dispose()
    {
        AuthenticationState.AuthenticationStateChanged -= AuthenticationState_AuthenticationStateChanged;
    }

    public async Task<ClaimsPrincipal> GetUser()
    {
        var authState = await AuthenticationState.GetAuthenticationStateAsync();
        return authState.User;
    }

    public async Task<bool> HasClaimValue(string claimType, string claimValue)
    {
        return ClaimService.HasClaimValue(await GetUser(), claimType, claimValue);
    }
}