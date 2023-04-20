using Serilog;

namespace amiliur.web.blazor.Services.AppState;

public class PageStateService : IDisposable
{
    public Guid Id = Guid.NewGuid();

    private static Dictionary<string, CacheManager> _data = new();

    public void Dispose()
    {
    }

    private CacheManager GetDataForService(Type serviceOrPageType)
    {
        var serviceName = serviceOrPageType.FullName;
        Console.WriteLine($"GetDataForService serviceName: {serviceName}");
        if (string.IsNullOrEmpty(serviceName))
            serviceName = serviceOrPageType.Name;

        var dataForService = _data.GetValueOrDefault(serviceName, null);
        if (dataForService == null)
        {
            Log.Debug($"GetDataForService serviceName: {serviceName} dataForService NOT FOUND");
            _data[serviceName] = new CacheManager(serviceName);
        }
        else
        {
            Log.Debug($"GetDataForService serviceName: {serviceName} dataForService FOUND");
        }

        return _data[serviceName];
    }

    private T GetData<T>(Type serviceOrPageType, string dataSetName) where T : class
    {
        var data = GetDataForService(serviceOrPageType).GetValue<T>(dataSetName);
        if (data != null)
        {
            Console.WriteLine($"PageStateService:GetData:{serviceOrPageType.FullName}:{dataSetName} => FOUND");
        }
        else
        {
            Console.WriteLine($"PageStateService:GetData:{serviceOrPageType.FullName}:{dataSetName} => NOT FOUND");
        }

        return data;
    }

    private T SetData<T>(Type serviceOrPageType, string dataSetName, TimeSpan lifeTime, T theData)
    {
        GetDataForService(serviceOrPageType).SetData(dataSetName, theData, lifeTime);
        return theData;
    }

    public T GetData<T>(Type serviceOrPageType, string dataSetName, TimeSpan lifeTime, Func<T> callOnMissFunc) where T : class
    {
        Console.WriteLine($"GetData: {serviceOrPageType.Name} - {dataSetName}");

        var dataForDataset = GetData<T>(serviceOrPageType, dataSetName);
        if (dataForDataset != null)
        {
            Console.WriteLine($"GetData: {serviceOrPageType.Name} - {dataSetName}: FOUND");
        }

        if (dataForDataset == null)
        {
            Console.WriteLine($"GetData: {serviceOrPageType.Name} - {dataSetName}: NOT FOUND");
            var data = callOnMissFunc.Invoke();
            if (data != null)
            {
                dataForDataset = SetData(serviceOrPageType, dataSetName, lifeTime, data);
            }
        }

        return dataForDataset;
    }

    public async Task<T> GetData<T>(Type serviceOrPageType, string dataSetName, TimeSpan lifeTime, Func<Task<T>> callOnMissFunc) where T : class
    {
        var dataForDataset = GetData<T>(serviceOrPageType, dataSetName);

        if (dataForDataset == null)
        {
            var data = await callOnMissFunc.Invoke();
            if (data != null)
            {
                dataForDataset = SetData(serviceOrPageType, dataSetName, lifeTime, data);
            }
        }

        return dataForDataset;
    }

    public void ClearData(Type serviceOrPageType, string dataSetName)
    {
        var serviceData = GetDataForService(serviceOrPageType);
        if (serviceData != null)
        {
            serviceData.ClearData(dataSetName);
        }
    }

    public void ClearAllCachedData()
    {
        _data = new();
    }

    public string GenerateId()
    {
        return Guid.NewGuid().ToString("N");
    }

    public void SetCrossPageData<T>(string id, TimeSpan lifeTime, T theData)
    {
        SetData(theData.GetType(), id, lifeTime, theData);
    }

    public T GetCrossPageData<T>(string id) where T : class
    {
        return GetData<T>(typeof(T), id);
    }
}

public class CacheManager
{
    public string ServiceName { get; set; }
    private Dictionary<string, CacheObject> _cacheObjects = new Dictionary<string, CacheObject>();

    public CacheManager(string serviceName)
    {
        ServiceName = serviceName;
    }

    public T GetValue<T>(string objectId) where T : class
    {
        Console.WriteLine($"CacheManager:GetValue: {objectId}");

        var cObj = _cacheObjects.GetValueOrDefault(objectId, null);
        if (cObj == null)
        {
            Console.WriteLine($"CacheManager:GetValue: {objectId} - NOT FOUND");
            return null;
        }

        Console.WriteLine($"CacheManager:GetValue: {objectId} - FOUND");
        if (DateTime.Now < cObj.Expires)
        {
            Console.WriteLine($"CacheManager:GetValue: {objectId} - NOT EXPIRED YET");
            return (T) cObj.Data;
        }

        Console.WriteLine($"CacheManager:GetValue: {objectId} - EXPIRED");
        _cacheObjects.Remove(objectId);
        return null;
    }

    public void SetData(string objectId, object data, TimeSpan lifeTime)
    {
        var cObj = new CacheObject(objectId, lifeTime, data);
        _cacheObjects[objectId] = cObj;
    }

    public void ClearData(string objectId)
    {
        if (objectId == null)
        {
            Console.WriteLine("Clearing all cache");
            _cacheObjects = new Dictionary<string, CacheObject>();
            return;
        }

        if (_cacheObjects.ContainsKey(objectId))
        {
            Console.WriteLine($"Clearing cache for {objectId}");
            _cacheObjects.Remove(objectId);
        }
    }
}

public class CacheObject : IEquatable<CacheObject>
{
    public string ObjectId { get; set; }
    public DateTime Created { get; set; }
    public DateTime Expires { get; set; }
    public object Data { get; set; }

    public CacheObject(string objectId, TimeSpan lifeTime, object data)
    {
        ObjectId = objectId;
        Created = DateTime.Now;
        if (lifeTime == TimeSpan.MaxValue)
            Expires = new DateTime(3000, 1, 1);
        else
            Expires = Created.Add(lifeTime);
        Data = data;
    }

    public bool Equals(CacheObject? other)
    {
        if (other == null)
            return false;
        return ObjectId.Equals(other.ObjectId);
    }
}