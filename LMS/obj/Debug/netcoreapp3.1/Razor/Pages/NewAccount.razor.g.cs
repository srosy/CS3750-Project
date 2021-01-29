#pragma checksum "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\NewAccount.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3ce332a0c57c0184a3889fdf6a146cec687bfaff"
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
using LMS.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\NewAccount.razor"
using Shared.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\NewAccount.razor"
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
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "row text-center");
            __builder.AddMarkupContent(2, "<div class=\"col-md-4\"></div>\r\n    ");
            __builder.OpenElement(3, "div");
            __builder.AddAttribute(4, "class", "col-md-6");
            __builder.OpenElement(5, "div");
            __builder.AddAttribute(6, "class", "card LMS-card");
            __builder.AddMarkupContent(7, "<div class=\"card-header LMS-title\"><h3>New Account</h3></div>\r\n            ");
            __builder.OpenElement(8, "div");
            __builder.AddAttribute(9, "class", "card card-body");
            __builder.OpenComponent<Microsoft.AspNetCore.Components.Forms.EditForm>(10);
            __builder.AddAttribute(11, "Model", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Object>(
#nullable restore
#line 17 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\NewAccount.razor"
                                  acctModel

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(12, "OnValidSubmit", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<Microsoft.AspNetCore.Components.Forms.EditContext>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Forms.EditContext>(this, 
#nullable restore
#line 17 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\NewAccount.razor"
                                                             CreateAccount

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
                __builder2.AddAttribute(19, "class", "form-group has-success");
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Forms.InputText>(20);
                __builder2.AddAttribute(21, "id", "fname");
                __builder2.AddAttribute(22, "class", "form-control LMS-input");
                __builder2.AddAttribute(23, "type", "text");
                __builder2.AddAttribute(24, "placeholder", "First Name");
                __builder2.AddAttribute(25, "Value", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String>(
#nullable restore
#line 26 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\NewAccount.razor"
                                                                                                                               acctModel.FirstName

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(26, "ValueChanged", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => acctModel.FirstName = __value, acctModel.FirstName))));
                __builder2.AddAttribute(27, "ValueExpression", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Linq.Expressions.Expression<System.Func<System.String>>>(() => acctModel.FirstName));
                __builder2.CloseComponent();
                __builder2.CloseElement();
                __builder2.AddMarkupContent(28, "\r\n                    ");
                __builder2.OpenElement(29, "div");
                __builder2.AddAttribute(30, "class", "form-group has-success");
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Forms.InputText>(31);
                __builder2.AddAttribute(32, "id", "lname");
                __builder2.AddAttribute(33, "class", "form-control LMS-input");
                __builder2.AddAttribute(34, "type", "text");
                __builder2.AddAttribute(35, "placeholder", "Last Name");
                __builder2.AddAttribute(36, "Value", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String>(
#nullable restore
#line 30 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\NewAccount.razor"
                                                                                                                              acctModel.LastName

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(37, "ValueChanged", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => acctModel.LastName = __value, acctModel.LastName))));
                __builder2.AddAttribute(38, "ValueExpression", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Linq.Expressions.Expression<System.Func<System.String>>>(() => acctModel.LastName));
                __builder2.CloseComponent();
                __builder2.CloseElement();
                __builder2.AddMarkupContent(39, "\r\n                    ");
                __builder2.OpenElement(40, "div");
                __builder2.AddAttribute(41, "class", "form-group has-success");
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Forms.InputText>(42);
                __builder2.AddAttribute(43, "id", "email");
                __builder2.AddAttribute(44, "class", "form-control LMS-input");
                __builder2.AddAttribute(45, "type", "text");
                __builder2.AddAttribute(46, "placeholder", "Email");
                __builder2.AddAttribute(47, "Value", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String>(
#nullable restore
#line 34 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\NewAccount.razor"
                                                                                                                          acctModel.Email

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(48, "ValueChanged", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => acctModel.Email = __value, acctModel.Email))));
                __builder2.AddAttribute(49, "ValueExpression", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Linq.Expressions.Expression<System.Func<System.String>>>(() => acctModel.Email));
                __builder2.CloseComponent();
                __builder2.CloseElement();
                __builder2.AddMarkupContent(50, "\r\n                    ");
                __builder2.OpenElement(51, "div");
                __builder2.AddAttribute(52, "class", "form-group has-success");
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Forms.InputText>(53);
                __builder2.AddAttribute(54, "id", "password");
                __builder2.AddAttribute(55, "class", "form-control LMS-input");
                __builder2.AddAttribute(56, "type", "password");
                __builder2.AddAttribute(57, "placeholder", "Password");
                __builder2.AddAttribute(58, "Value", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String>(
#nullable restore
#line 42 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\NewAccount.razor"
                                                                                                                                    acctModel.Auth.Password

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(59, "ValueChanged", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => acctModel.Auth.Password = __value, acctModel.Auth.Password))));
                __builder2.AddAttribute(60, "ValueExpression", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Linq.Expressions.Expression<System.Func<System.String>>>(() => acctModel.Auth.Password));
                __builder2.CloseComponent();
                __builder2.CloseElement();
                __builder2.AddMarkupContent(61, "\r\n                    ");
                __builder2.OpenElement(62, "div");
                __builder2.AddAttribute(63, "class", "form-group has-success");
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Forms.InputText>(64);
                __builder2.AddAttribute(65, "id", "confirm-password");
                __builder2.AddAttribute(66, "class", "form-control LMS-input");
                __builder2.AddAttribute(67, "type", "password");
                __builder2.AddAttribute(68, "placeholder", "Re-enter password");
                __builder2.AddAttribute(69, "Value", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String>(
#nullable restore
#line 46 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\NewAccount.razor"
                                                                                                                                                     ConfirmPassword

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(70, "ValueChanged", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => ConfirmPassword = __value, ConfirmPassword))));
                __builder2.AddAttribute(71, "ValueExpression", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Linq.Expressions.Expression<System.Func<System.String>>>(() => ConfirmPassword));
                __builder2.CloseComponent();
                __builder2.CloseElement();
                __builder2.AddMarkupContent(72, "\r\n                    ");
                __builder2.OpenElement(73, "div");
                __builder2.AddAttribute(74, "class", "form-group");
                __builder2.OpenElement(75, "select");
                __builder2.AddAttribute(76, "id", "role");
                __builder2.AddAttribute(77, "class", "form-control LMS-Input");
                __builder2.AddAttribute(78, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 50 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\NewAccount.razor"
                                                                                acctModel.Role

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(79, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => acctModel.Role = __value, acctModel.Role));
                __builder2.SetUpdatesAttributeName("value");
                __builder2.OpenElement(80, "option");
                __builder2.AddAttribute(81, "value", "0");
                __builder2.AddAttribute(82, "selected", true);
                __builder2.AddContent(83, "- Select Role -");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(84, "\r\n                            ");
                __builder2.OpenElement(85, "option");
                __builder2.AddAttribute(86, "value", "1");
                __builder2.AddContent(87, "Student");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(88, "\r\n                            ");
                __builder2.OpenElement(89, "option");
                __builder2.AddAttribute(90, "value", "2");
                __builder2.AddContent(91, "Instructor");
                __builder2.CloseElement();
                __builder2.CloseElement();
                __builder2.CloseElement();
                __builder2.AddMarkupContent(92, "\r\n                    ");
                __builder2.AddMarkupContent(93, "<button class=\"btn btn-primary LMS-btn\" type=\"submit\">Create Account</button>");
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(94, "\r\n\r\n                ");
            __builder.AddMarkupContent(95, "<a href=\"/login\">Already have an account?</a>");
#nullable restore
#line 61 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\NewAccount.razor"
                 if (!string.IsNullOrEmpty(message))
                {

#line default
#line hidden
#nullable disable
            __builder.OpenElement(96, "p");
            __builder.AddAttribute(97, "class", "LMS-message");
            __builder.AddContent(98, 
#nullable restore
#line 63 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\NewAccount.razor"
                                            message

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
#nullable restore
#line 64 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\NewAccount.razor"
                }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(99, "\r\n    <div class=\"col-md-4\"></div>");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 71 "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\NewAccount.razor"
       
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
