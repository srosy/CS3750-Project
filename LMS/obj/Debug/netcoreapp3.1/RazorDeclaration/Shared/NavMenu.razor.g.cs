// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace LMS.Shared
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\_Imports.razor"
using LMS;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\_Imports.razor"
using LMS.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\_Imports.razor"
using LMS.Pages;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\_Imports.razor"
using LMS.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\_Imports.razor"
using LMS.Data.Enum;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\_Imports.razor"
using LMS.Data.Helper;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\_Imports.razor"
using LMS.Data.Models;

#line default
#line hidden
#nullable disable
    public partial class NavMenu : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 28 "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\Shared\NavMenu.razor"
       
    private bool collapseNavMenu = true;
    private string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

    private void ToggleNavMenu()
    {
        collapseNavMenu = !collapseNavMenu;
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
