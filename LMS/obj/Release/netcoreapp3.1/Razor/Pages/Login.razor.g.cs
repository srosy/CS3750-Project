#pragma checksum "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\Login.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "eef02d3233cff95a7756cab33cf43989b752e512"
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
#line 1 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\_Imports.razor"
using LMS;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\_Imports.razor"
using LMS.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\_Imports.razor"
using LMS.Pages;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\_Imports.razor"
using LMS.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\_Imports.razor"
using LMS.Data.Enum;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\_Imports.razor"
using LMS.Data.Helper;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\_Imports.razor"
using LMS.Data.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\Login.razor"
using Shared.Models;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.LayoutAttribute(typeof(UnauthenticatedLayout))]
    [Microsoft.AspNetCore.Components.RouteAttribute("/login")]
    public partial class Login : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "row ");
            __builder.AddMarkupContent(2, "<div class=\"col-md-4\"></div>\r\n    ");
            __builder.OpenElement(3, "div");
            __builder.AddAttribute(4, "class", "col-md-4");
            __builder.OpenElement(5, "div");
            __builder.AddAttribute(6, "class", "card LMS-card");
            __builder.AddMarkupContent(7, "<div class=\"card-header LMS-title\"><h3>Login</h3></div>\r\n            ");
            __builder.OpenElement(8, "div");
            __builder.AddAttribute(9, "class", "card-body");
            __builder.OpenComponent<Microsoft.AspNetCore.Components.Forms.EditForm>(10);
            __builder.AddAttribute(11, "Model", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Object>(
#nullable restore
#line 18 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\Login.razor"
                                  authModel

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(12, "OnValidSubmit", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<Microsoft.AspNetCore.Components.Forms.EditContext>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Forms.EditContext>(this, 
#nullable restore
#line 18 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\Login.razor"
                                                             TryAuthenticate

#line default
#line hidden
#nullable disable
            )));
            __builder.AddAttribute(13, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment<Microsoft.AspNetCore.Components.Forms.EditContext>)((context) => (__builder2) => {
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Forms.DataAnnotationsValidator>(14);
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(15, "\r\n                    ");
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Forms.ValidationSummary>(16);
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(17, "\r\n                    ");
                __builder2.OpenElement(18, "div");
                __builder2.AddAttribute(19, "class", "form-group");
                __builder2.OpenElement(20, "div");
                __builder2.AddAttribute(21, "class", "login-box");
                __builder2.AddMarkupContent(22, "<label for=\"LMSusername\"><strong>Username</strong></label>\r\n                            ");
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Forms.InputText>(23);
                __builder2.AddAttribute(24, "id", "LMSusername");
                __builder2.AddAttribute(25, "class", "form-control LMS-input");
                __builder2.AddAttribute(26, "type", "text");
                __builder2.AddAttribute(27, "placeholder", "Username/Email");
                __builder2.AddAttribute(28, "Value", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String>(
#nullable restore
#line 24 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\Login.razor"
                                                                                                                                             authModel.UserName

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(29, "ValueChanged", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => authModel.UserName = __value, authModel.UserName))));
                __builder2.AddAttribute(30, "ValueExpression", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Linq.Expressions.Expression<System.Func<System.String>>>(() => authModel.UserName));
                __builder2.CloseComponent();
                __builder2.CloseElement();
                __builder2.AddMarkupContent(31, "\r\n                        ");
                __builder2.OpenElement(32, "div");
                __builder2.AddMarkupContent(33, "<label for=\"LMSpassword\" style=\"padding-bottom:-3px\"><strong>Password</strong></label>\r\n                            ");
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Forms.InputText>(34);
                __builder2.AddAttribute(35, "id", "LMSpassword");
                __builder2.AddAttribute(36, "class", "form-control LMS-input");
                __builder2.AddAttribute(37, "type", "password");
                __builder2.AddAttribute(38, "placeholder", "Password");
                __builder2.AddAttribute(39, "Value", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String>(
#nullable restore
#line 28 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\Login.razor"
                                                                                                                                           authModel.Password

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(40, "ValueChanged", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => authModel.Password = __value, authModel.Password))));
                __builder2.AddAttribute(41, "ValueExpression", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Linq.Expressions.Expression<System.Func<System.String>>>(() => authModel.Password));
                __builder2.CloseComponent();
                __builder2.CloseElement();
                __builder2.AddMarkupContent(42, "\r\n\r\n                        ");
                __builder2.AddMarkupContent(43, "<button class=\"btn btn-primary LMS-btn login-btn\" type=\"submit\">Submit</button>");
                __builder2.CloseElement();
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(44, "\r\n\r\n                ");
            __builder.AddMarkupContent(45, "<a href=\"/newaccount\">Don\'t have an account?</a>");
#nullable restore
#line 37 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\Login.razor"
                 if (!string.IsNullOrEmpty(message))
                {

#line default
#line hidden
#nullable disable
            __builder.OpenElement(46, "p");
            __builder.AddAttribute(47, "class", "LMS-message");
            __builder.AddContent(48, 
#nullable restore
#line 39 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\Login.razor"
                                            message

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
#nullable restore
#line 40 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\Login.razor"
                }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(49, "\r\n    <div class=\"col-md-4\"></div>");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 47 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\Login.razor"
       
    private AuthenticationViewModel authModel = new AuthenticationViewModel();
    private string message = string.Empty;

    /// <summary>
    /// Attempt to authenticate a user.
    /// </summary>
    private async void TryAuthenticate()
    {
        var authenticated = await DbService.Authenticate(Storage, AzureDb, authModel);
        if (authenticated)
        {
            NavMan.NavigateTo("dashboard");
        }
        else
        {
            message = "Login failed.";
            await JS.InvokeVoidAsync("Toast", new[] { "error", "Login Failed.", "3000" }); // toasttype, message, duration
        }
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JS { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private Blazored.LocalStorage.ILocalStorageService Storage { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private AzureDbContext AzureDb { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IDbService DbService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private NavigationManager NavMan { get; set; }
    }
}
#pragma warning restore 1591
