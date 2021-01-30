#pragma checksum "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\Pages\Dashboard.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e0a0c83efb968dbdf994629c99cceca35aeb0003"
// <auto-generated/>
#pragma warning disable 1591
namespace LMS.Pages
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
    [Microsoft.AspNetCore.Components.RouteAttribute("/dashboard")]
    public partial class Dashboard : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<h3>Dashboard</h3>");
        }
        #pragma warning restore 1998
#nullable restore
#line 9 "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\Pages\Dashboard.razor"
       


    protected override async Task OnInitializedAsync()
    {
        var validSession = await SessionObj.VerifySession(AzureDb, Storage);
        if (!validSession)
        {
            NavMan.NavigateTo("sessionexpired");
        }
    }


#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private Blazored.LocalStorage.ILocalStorageService Storage { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private AzureDbContext AzureDb { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IDbService DbService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private NavigationManager NavMan { get; set; }
    }
}
#pragma warning restore 1591