namespace Contellation.Custom.Controls
{
    public interface IDataGridColumn
    {
        string FieldName { get; set; }
        bool IsColumnFiltered { get; set; }
    }
}
