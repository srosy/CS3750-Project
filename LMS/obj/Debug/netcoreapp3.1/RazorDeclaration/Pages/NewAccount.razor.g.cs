// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

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
#nullable restore
#line 3 "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\Pages\NewAccount.razor"
using Shared.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\Pages\NewAccount.razor"
using Data;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.LayoutAttribute(typeof(UnauthenticatedLayout))]
    [Microsoft.AspNetCore.Components.RouteAttribute("/newaccount")]
    public partial class NewAccount : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 65 "E:\School\Spring 2021\CS3750\CS3750-Project\LMS\Pages\NewAccount.razor"
       
    private AccountModel acctModel = new AccountModel();
    private string message = string.Empty;
    private string ConfirmPassword;
    private DateTime today;

    private async void CreateAccount()
    {
        if (acctModel.Role <= 0)
        {
            message = "User must select a type";
            return;
        }

        if (!ConfirmPassword.Equals(acctModel.Auth.Password))
        {
            message = "Passwords do not match!";
            return;
        }


        var acctCreated = await DbService.CreateAccount(AzureDb, acctModel);
        if (acctCreated)
        {
            message = "Account Created. Proceed to login page.";
            NavMan.NavigateTo("/");
        }

        message = "Error trying to create account. Account may already exist.";
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private AzureDbContext AzureDb { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IDbService DbService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private NavigationManager NavMan { get; set; }
    }
}
#pragma warning restore 1591
