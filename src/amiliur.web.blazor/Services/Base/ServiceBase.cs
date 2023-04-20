using System.Reflection;
using System.Text.Json;
using amiliur.shared.Json;
using amiliur.shared.Reflection;
using amiliur.web.blazor.Services.AppState;
using amiliur.web.shared.Models;
using Serilog;

namespace amiliur.web.blazor.Services.Base;

public abstract class ServiceBase
{
    protected JsonSerializerOptions JsonSerializerOptions => new();
    public PageStateService PageStateService { get; private set; }

    public ServiceBase(PageStateService pageStateService)
    {
        PageStateService = pageStateService;
        JsonSerializerOptions.Converters.Add(new DynamicFormJsonConverterFactory());
    }

    public async Task<List<ValueTextModel>> GetAllAtivos(bool insertEmpty)
    {
        var list = await DoGetAllAtivos();
        if (insertEmpty && list.All(m => !string.IsNullOrEmpty(m.Value)))
            list.Insert(0, new ValueTextModel());
        return list;
    }

    protected abstract Task<List<ValueTextModel>> DoGetAllAtivos();

    private static Dictionary<string, Type> ServicesMap = new Dictionary<string, Type>();
    private static List<string> _registeredAssemblies = new List<string>();

    public static void AddMap(string mapName, Type mapType)
    {
        Console.WriteLine($"Adding map {mapName} -> {mapType.Name}");
        ServicesMap[mapName] = mapType;
    }

    public static Type GetMap(string map)
    {
        return ServicesMap.GetValueOrDefault(map);
    }

    public static void RegisterAssemblies(Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var assemblyName = assembly.FullName;
            Log.Debug($"Registering types assembly: {assemblyName}");
            if (!_registeredAssemblies.Contains(assemblyName))
                _registeredAssemblies.Add(assemblyName);
        }
    }

    public static Type FindTypeByName(string typeName)
    {
        return TypeUtils.FindTypeByName(typeName, _registeredAssemblies);
    }
}
