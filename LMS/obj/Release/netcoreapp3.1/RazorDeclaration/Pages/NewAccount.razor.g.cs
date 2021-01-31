#pragma checksum "C:\Users\Water\Documents\3750\CS3750-Project\LMS\Pages\NewAccount.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7b5d689dd40bd6b2e53a29a96d3e87ecefd5b26a"
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
#line 1 "C:\Users\Water\Documents\3750\CS3750-Project\LMS\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Water\Documents\3750\CS3750-Project\LMS\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Water\Documents\3750\CS3750-Project\LMS\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Water\Documents\3750\CS3750-Project\LMS\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Water\Documents\3750\CS3750-Project\LMS\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\Water\Documents\3750\CS3750-Project\LMS\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\Water\Documents\3750\CS3750-Project\LMS\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\Water\Documents\3750\CS3750-Project\LMS\_Imports.razor"
using LMS;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\Water\Documents\3750\CS3750-Project\LMS\_Imports.razor"
using LMS.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\Water\Documents\3750\CS3750-Project\LMS\_Imports.razor"
using LMS.Pages;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "C:\Users\Water\Documents\3750\CS3750-Project\LMS\_Imports.razor"
using LMS.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "C:\Users\Water\Documents\3750\CS3750-Project\LMS\_Imports.razor"
using LMS.Data.Enum;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "C:\Users\Water\Documents\3750\CS3750-Project\LMS\_Imports.razor"
using LMS.Data.Helper;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "C:\Users\Water\Documents\3750\CS3750-Project\LMS\_Imports.razor"
using LMS.Data.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Water\Documents\3750\CS3750-Project\LMS\Pages\NewAccount.razor"
using Shared.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Water\Documents\3750\CS3750-Project\LMS\Pages\NewAccount.razor"
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
#line 71 "C:\Users\Water\Documents\3750\CS3750-Project\LMS\Pages\NewAccount.razor"
       
    private AccountViewModel acctModel = new AccountViewModel();
    private DbService db = new DbService();
    private string message = string.Empty;
    private string ConfirmPassword;
    private string InputtedResetCode;
    private bool EmailSent;
    private DateTime MinDate = DateTime.UtcNow.AddYears(-100);
    private DateTime MaxDate = DateTime.UtcNow.AddYears(-18);
    private DateTime today;

    protected async override Task OnInitializedAsync()
    {
        acctModel.Birthday = DateTime.UtcNow.AddYears(-18); // must be at least 18
    }

    private async void CreateAccount()
    {
        if (acctModel.Role <= 0)
        {
            message = "User must select a type";
            return;
        }

        if (acctModel.Birthday > MaxDate || acctModel.Birthday < MinDate)
        {
            message = $"DOB must be between {MinDate.ToShortDateString()} and {MaxDate.ToShortDateString()}";
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
            EmailSent = await db.SendEmail(acctModel, AzureDb);
        }

        message = "Error trying to create account. Account may already exist.";
    }

    private async void VerifyEmail()
    {
        if (InputtedResetCode == acctModel.Auth.ResetCode)
        {
            await db.VerifyEmail(acctModel.Email, AzureDb);
        }
    }

    private async void ResendEmail()
    {
        await db.SendEmail(acctModel, AzureDb);
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
