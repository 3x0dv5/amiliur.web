using amiliur.web.blazor.Services.AppState;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace amiliur.web.blazor.Services.Base;

public class DefaultServerService : ServerServiceBase
{
    public DefaultServerService(HttpClient httpClient, AppStateService stateService, PageStateService pageStateService, IAccessTokenProvider accessTokenProvider) : base(httpClient, stateService, pageStateService, accessTokenProvider)
    {
    }

    protected override string SubPath { get; } = "";
}