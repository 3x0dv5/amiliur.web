using System.Reflection;
using amiliur.web.shared.Environments;

namespace amiliur.web.shared.Models;

public interface ISearchInputModel : IFormModel
{
    public bool HasFilters(FilterEnvironment env);
    public List<PropertyInfo> GetFilterProperties(FilterEnvironment env);
    public DateTime Ts { get; set; }
    public DateOnly AsOf { get; set; }
}