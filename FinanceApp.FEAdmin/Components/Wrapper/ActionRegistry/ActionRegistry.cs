namespace FinanceApp.FEAdmin.Components.Wrapper.ActionRegistry
{
    public static class TableActionRegistry
    {
        public static Dictionary<string, TableActionTemplate> DefaultTemplates = new()
    {
        { "View", new TableActionTemplate { Label = "View", CssClass = "btn-primary" } },
        { "Delete", new TableActionTemplate { Label = "Delete", CssClass = "btn-danger" } },
        { "Run", new TableActionTemplate { Label = "Run", CssClass = "btn-success" } },
        { "Edit", new TableActionTemplate { Label = "Edit", CssClass = "btn-warning" } },
    };
    }

    public class TableActionTemplate
    {
        public string Label { get; set; }
        public string CssClass { get; set; }
    }

}
