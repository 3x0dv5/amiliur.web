using System.Linq.Expressions;
using amiliur.shared.Json;

namespace amiliur.web.shared.Models.Generic;

public abstract class BaseEditModel : IEditableModel, ISerializableModel
{
    protected BaseEditModel()
    {
        Id = Guid.NewGuid().ToString();
    }

    protected BaseEditModel(string id)
    {
        Id = id;
    }

    public bool IsInsert => string.IsNullOrEmpty(Id);

    public bool IsEdit => !IsInsert;

    public string? Id { get; set; }

    public virtual void Validate()
    {
    }

    public Expression For(string fieldName, Type type)
    {
        return Expression.Property(Expression.Parameter(type), fieldName);
    }
}