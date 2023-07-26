namespace amiliur.web.shared.Models.Generic;

public interface IEditableModel
{
    public bool IsInsert { get; }
    public bool IsEdit { get; }
    public string Id { get; set; }
    public void Validate();
}