using Microsoft.AspNetCore.Components.Routing;

namespace FinanceApp.FEAdmin.Class.InternalClass
{
    public class NavItem
    {
        public string Title { get; set; } = "";
        public string Icon { get; set; } = "";
        public string Href { get; set; } = "";
        public NavLinkMatch Match { get; set; } = NavLinkMatch.Prefix;
    }
    public class TableAction<T>
    {
        public string Label { get; set; }
        public string CssClass { get; set; }
        public Action<T> Callback { get; set; }
    }
    public class FieldConfig
    {
        public string Label { get; set; }
        public string Property { get; set; } 
        public string Type { get; set; } = "text"; 
        public string Placeholder { get; set; }
        public bool Required { get; set; } = false;
    }
    public class FieldBinding
    {
        public Func<object> Getter { get; set; }
        public Action<object> Setter { get; set; }
    }

}
