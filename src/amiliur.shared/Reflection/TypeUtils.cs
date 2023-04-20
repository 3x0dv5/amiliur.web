using System.Reflection;
using Serilog;

namespace amiliur.shared.Reflection;

public static class TypeUtils
{
    /// <summary>
    /// Checks, if the specified type is a nullable
    /// </summary>
    public static bool IsNullableType(this Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    public static Type GetNonNullableType(this Type type)
    {
        return type.IsNullableType() ? type.GetGenericArguments()[0] : type;
    }
    
    public static Type? FindTypeByName(string typeName, List<string> assemblies)
    {
        foreach (var assemblyName in assemblies)
        {
            try
            {
                var assembly = Assembly.Load(assemblyName);
                var type = assembly.GetTypes().FirstOrDefault(m => m.Name == typeName || m.FullName == typeName);
                if (type != null)
                {
                    Log.Debug("Type found for {typeName}. Assembly {assembly}, fullname type:{fullname}", typeName, assembly.GetName().ToString(), type.FullName);
                    return type;
                }
            }
            catch (Exception e)
            {
                Log.Error("Assembly {assemblyNam} not found. Error: {e}", assemblyName, e);
            }
        }

        return null;
    }
    
    public static string GetFullTypeName(Type typeToConvert)
    {
        var typeName = $"{typeToConvert.FullName}, {typeToConvert.Assembly.GetName().Name}";

        if (typeName.Trim() == ",")
            throw new Exception("TYPENAME BOOM!");

        return typeName;
    }
   
}