#pragma checksum "C:\Users\hamky\OneDrive\Documents\Dev\CS3750-Project\LMS\Pages\Index.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "bbe287af83815bd72af91e18b3bcf85ad266f88d"
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
    [Microsoft.AspNetCore.Components.LayoutAttribute(typeof(UnauthenticatedLayout))]
    [Microsoft.AspNetCore.Components.RouteAttribute("/")]
    public partial class Index : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<div class=\"welcome_card\"><h3 class=\"welcome\">Welcome to LMS for Weber State\'s CS3750!</h3></div>\r\n\r\n");
            __builder.AddMarkupContent(1, @"<div class=""gallery""><figure class=""gallery__item gallery__item--1""><img src=""https://photos.smugmug.com/photos/i-249M8rf/0/XL/i-249M8rf-XL.jpg"" class=""gallery__img"" alt=""Waldo""></figure>
    <figure class=""gallery__item gallery__item--2""><img src=""https://photos.smugmug.com/photos/i-rJ5HpZQ/0/X2/i-rJ5HpZQ-X2.jpg"" class=""gallery__img"" alt=""Campus""></figure>
    <figure class=""gallery__item gallery__item--3""><img src=""https://photos.smugmug.com/photos/i-TWv5Sht/0/XL/i-TWv5Sht-XL.jpg"" class=""gallery__img"" alt=""W-Sign""></figure>
    <figure class=""gallery__item gallery__item--4""><a class=""btn btn-primary"" href=""/login"">Login</a></figure>
    <figure class=""gallery__item gallery__item--5""><a class=""btn btn-secondary"" href=""/newaccount"">Create Account</a></figure></div>

");
            __builder.AddMarkupContent(2, "<footer class=\"copyright\">&copy; Copyright 2021 All rights reserved</footer>");
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
