using System.Linq.Dynamic.Core;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using amiliur.shared.Extensions;
using amiliur.shared.Json;
using amiliur.web.blazor.Services.AppState;
using amiliur.web.shared.Filtering;
using amiliur.web.shared.Models;
using amiliur.web.shared.Models.Results;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace amiliur.web.blazor.Services.Base;

public abstract class ServerServiceBase : ServiceBase
{
    private readonly IAccessTokenProvider _accessTokenProvider;

    // ReSharper disable once InconsistentNaming
    protected const string API = "api";

    // ReSharper disable once InconsistentNaming
    protected HttpClient httpClient { get; }
    protected virtual string SubPath => GetType().Name.Replace("Service", "").ToKebabCase().ToLower();

    protected AppStateService AppStateService { get; }

    public ServerServiceBase(HttpClient httpClient, AppStateService stateService, PageStateService pageStateService, IAccessTokenProvider accessTokenProvider) : base(pageStateService)
    {
        _accessTokenProvider = accessTokenProvider;
        this.httpClient = httpClient;
        AppStateService = stateService;
        httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
        {
            NoCache = true
        };
    }

    protected string ServerService()
    {
        return SubUrl();
    }

    protected string SubUrl()
    {
        return $"{API}/{SubPath}";
    }

    protected string ActionUrl(string actionName)
    {
        return ActionUrl(actionName, new Dictionary<string, string>());
    }

    protected string ActionUrl(string actionName, Dictionary<string, string>? filter)
    {
        if (filter == null || filter.Count == 0)
        {
            return string.IsNullOrEmpty(actionName)
                ? ServerService()
                : $"{ServerService()}/{actionName}";
        }

        return QueryHelpers.AddQueryString(ActionUrl(actionName), filter);
    }

    public async Task<string> GetAuthorizationToken()
    {
        (await _accessTokenProvider.RequestAccessToken()).TryGetToken(out AccessToken token);
        return token.Value;
    }

    public async Task<bool> HasClaimValue(string claimType, string claimValue)
    {
        return await AppStateService.HasClaimValue(claimType, claimValue);
    }

    private static JsonSerializerOptions GetJsonOptions()
    {
        var o = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        o.Converters.Add(new DynamicFormJsonConverterFactory());
        return o;
    }

    protected async Task<T> GetJson<T>(string url)
    {
        try
        {
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>(GetJsonOptions());
            }

            var error = await response.Content.ReadFromJsonAsync<ErrorSaveResult>(GetJsonOptions());
            throw new Exception(error.ErrorMessage);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            AppStateService.ErrorMessage = e.Message;
            return default;
        }
    }

    protected async Task<string> GetString(string url)
    {
        try
        {
            return await httpClient.GetStringAsync(url);
        }
        catch (Exception e)
        {
            AppStateService.ErrorMessage = e.Message;
            return string.Empty;
        }
    }

    protected async Task<List<ValueTextModel>> GetAllValueText(string endpoint, bool insertEmpty)
    {
        var list = await GetAll<ValueTextModel>(endpoint);
        if (insertEmpty && list.All(m => !string.IsNullOrEmpty(m.Value)))
            list.Insert(0, new ValueTextModel());
        return list;
    }

    protected override async Task<List<ValueTextModel>> DoGetAllAtivos()
    {
        return await GetAll<ValueTextModel>("ativos");
    }

    public void ClearCache()
    {
        PageStateService.ClearAllCachedData();
    }

    public async Task<List<T>> GetAll<T>(string endpoint = "", int cacheTimeout = 5 * 60)
    {
        Func<Task<List<T>>> call = async () =>
        {
            var path = ActionUrl(endpoint);
            return await GetJson<List<T>>(path);
        };

        var _type = GetType();
        var dsName = "GetAll";
        if (!string.IsNullOrEmpty(endpoint))
            dsName = $"{dsName}_{endpoint}";
        return await PageStateService.GetData(_type, dsName, TimeSpan.FromSeconds(cacheTimeout), call);
    }

    public async Task<T> Get<T>(string id, string endpoint)
    {
        return await GetJson<T>(ActionUrl($"{endpoint}/{id}"));
    }

    public async Task<T> GetEdit<T>(string id, string endpoint = "")
    {
        if (string.IsNullOrEmpty(endpoint))
        {
            return await GetJson<T>(ActionUrl($"edit/{id}"));
        }

        return await Get<T>(id, endpoint);
    }

    public async Task<T> GetView<T>(string id, string endpoint = "")
    {
        if (string.IsNullOrEmpty(endpoint))
        {
            return await GetJson<T>(ActionUrl($"view/{id}"));
        }

        return await Get<T>(id, endpoint);
    }

    public async Task<List<T>> Search<T>(ISearchInputModel searchInput)
    {
        var hasFilter = searchInput.HasFilters(FilterEnvironment.Server);

        if (searchInput.HasFilters(FilterEnvironment.Server))
        {
            var jsonContent = JsonContent.Create(searchInput, searchInput.GetType(), options: GetJsonOptions());
            var response = await httpClient.PostAsync(ActionUrl("search"), jsonContent);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                AppStateService.ErrorMessage = e.Message;
                return default;
            }

            var results = await response.Content.ReadFromJsonAsync<List<T>>(options: GetJsonOptions());
            return results;
        }

        Console.WriteLine($"{searchInput.GetType()} does not have filters defined.");
        return await GetAll<T>();
    }

    public async Task<TReturn?> PostRetrieveAsync<TInput, TReturn>(TInput input, string? targetPath = null, bool passErrorToAppStateService = true) where TReturn : class
    {
        if (targetPath == null)
        {
            targetPath = ActionUrl("search");
        }
        else if (!targetPath.StartsWith("api/")) // the target path is the relative path to the api 
        {
            targetPath = ActionUrl(targetPath);
        }

        Log.Debug("PostRetrieveAsync: {targetPath}", targetPath);
        var response = await httpClient.PostAsJsonAsync(targetPath, input, options: GetJsonOptions());
        try
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TReturn>(options: GetJsonOptions());
                if (result != null)
                    return result;

                throw new Exception($"Ocorreu um erro: Result retrieved is null"); // todo: translate
            }

            var error = await response.Content.ReadFromJsonAsync<ErrorSaveResult>(options: GetJsonOptions());
            if (typeof(TReturn).Name == "SaveBaseResult" && error != null)
                return error as TReturn;

            if (error != null)
                throw new Exception(error.ErrorMessage);
            throw new Exception("Ocorreu um erro");
        }
        catch (Exception e)
        {
            if (passErrorToAppStateService)
            {
                AppStateService.ErrorMessage = e.Message;
                return default;
            }

            throw;
        }
    }

    public async Task<TReturn?> Save<TInput, TReturn>(TInput input, string? targetPath = null, bool handleException = true) where TReturn : class
    {
        ClearCache();

        if (targetPath == null)
        {
            targetPath = ActionUrl("save");
        }
        else if (!targetPath.StartsWith("api/")) // the target path is the relative path to the api 
        {
            targetPath = ActionUrl(targetPath);
        }

        return await PostRetrieveAsync<TInput, TReturn>(input, targetPath, handleException);
    }


    public async Task<SaveBaseResult> Delete(string id)
    {
        ClearCache();
        var response = await httpClient.DeleteAsync($"{ActionUrl("")}/{id}");

        try
        {
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SaveBaseResult>(options: GetJsonOptions());
            }

            return await response.Content.ReadFromJsonAsync<ErrorSaveResult>(options: GetJsonOptions());
        }
        catch (Exception e)
        {
            AppStateService.ErrorMessage = e.Message;
            return new SaveBaseResult
            {
                Success = false,
                ErrorMessage = e.Message
            };
        }
    }

    public List<TResult> ClientFiltering<TResult>(List<TResult> resultados, ISearchInputModel input)
    {
        if (resultados == null || !resultados.Any() || !input.HasFilters(FilterEnvironment.Client))
        {
            return resultados;
        }

        var res = resultados.AsQueryable();

        var properties = input.GetFilterProperties(FilterEnvironment.Client);

        var expressionString = ExpressionFilterGenerator.ExpressionString<TResult>(input, properties);

        if (!string.IsNullOrEmpty(expressionString))
        {
            var config = new ParsingConfig
            {
                DateTimeIsParsedAsUTC = true,
                CustomTypeProvider = new LinqCustomProvider()
            };
            return res.Where(config, expressionString).ToList();
        }

        return resultados;
    }


    public static ServerServiceBase From(string dataSource, IServiceProvider services)
    {
        var type = ServiceBase.GetMap(dataSource.GenerateSlug());
        if (type != null)
        {
            return (ServerServiceBase) services.GetRequiredService(type);
        }

        return (ServerServiceBase) services.GetRequiredService(typeof(DefaultServerService));
    }
}