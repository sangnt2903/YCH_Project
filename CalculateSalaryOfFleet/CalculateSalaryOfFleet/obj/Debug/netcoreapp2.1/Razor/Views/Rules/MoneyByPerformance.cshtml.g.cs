#pragma checksum "C:\Users\ADMIN\Desktop\CalculateSalaryFleet\CalculateSalaryOfFleet\CalculateSalaryOfFleet\Views\Rules\MoneyByPerformance.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "63e92aebf8bdf3c032f7d7fc5662f7384ea2526e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Rules_MoneyByPerformance), @"mvc.1.0.view", @"/Views/Rules/MoneyByPerformance.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Rules/MoneyByPerformance.cshtml", typeof(AspNetCore.Views_Rules_MoneyByPerformance))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\ADMIN\Desktop\CalculateSalaryFleet\CalculateSalaryOfFleet\CalculateSalaryOfFleet\Views\_ViewImports.cshtml"
using CalculateSalaryOfFleet;

#line default
#line hidden
#line 2 "C:\Users\ADMIN\Desktop\CalculateSalaryFleet\CalculateSalaryOfFleet\CalculateSalaryOfFleet\Views\_ViewImports.cshtml"
using CalculateSalaryOfFleet.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"63e92aebf8bdf3c032f7d7fc5662f7384ea2526e", @"/Views/Rules/MoneyByPerformance.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"10124c27bed95f0672c9b1204070b69b0b35ba99", @"/Views/_ViewImports.cshtml")]
    public class Views_Rules_MoneyByPerformance : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<CalculateSalaryOfFleet.Models.MoneyJob>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(60, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "C:\Users\ADMIN\Desktop\CalculateSalaryFleet\CalculateSalaryOfFleet\CalculateSalaryOfFleet\Views\Rules\MoneyByPerformance.cshtml"
  
    ViewData["Title"] = "MoneyByPerformance";
    Layout = "~/Views/Shared/_frontEnd.cshtml";

#line default
#line hidden
            BeginContext(165, 80, true);
            WriteLiteral("\r\n<div style=\"text-align:center\"><h2>CẤU HÌNH CÁC HỆ SỐ TÀI</h2></div>\r\n\r\n");
            EndContext();
            BeginContext(410, 603, true);
            WriteLiteral(@"<div class=""container"">
    <table class=""table table-bordered"">
        <thead>
            <tr>
                <th style=""text-align:center"">
                    Mã hệ số
                </th>
                <th style=""text-align:center"">
                    Thứ tự chuyến
                </th>
                <th style=""text-align:center"">
                    Mô tả
                </th>
                <th style=""text-align:center"">
                    Số tiền (VNĐ)
                </th>
                <th></th>
            </tr>
        </thead>
        <tbody>
");
            EndContext();
#line 33 "C:\Users\ADMIN\Desktop\CalculateSalaryFleet\CalculateSalaryOfFleet\CalculateSalaryOfFleet\Views\Rules\MoneyByPerformance.cshtml"
             foreach (var item in Model)
            {

#line default
#line hidden
            BeginContext(1070, 98, true);
            WriteLiteral("                <tr>\r\n                    <td style=\"text-align:center\">\r\n                        ");
            EndContext();
            BeginContext(1169, 37, false);
#line 37 "C:\Users\ADMIN\Desktop\CalculateSalaryFleet\CalculateSalaryOfFleet\CalculateSalaryOfFleet\Views\Rules\MoneyByPerformance.cshtml"
                   Write(Html.DisplayFor(modelItem => item.Id));

#line default
#line hidden
            EndContext();
            BeginContext(1206, 105, true);
            WriteLiteral("\r\n                    </td>\r\n                    <td style=\"text-align:center\">\r\n                        ");
            EndContext();
            BeginContext(1312, 43, false);
#line 40 "C:\Users\ADMIN\Desktop\CalculateSalaryFleet\CalculateSalaryOfFleet\CalculateSalaryOfFleet\Views\Rules\MoneyByPerformance.cshtml"
                   Write(Html.DisplayFor(modelItem => item.JobOrder));

#line default
#line hidden
            EndContext();
            BeginContext(1355, 105, true);
            WriteLiteral("\r\n                    </td>\r\n                    <td style=\"text-align:center\">\r\n                        ");
            EndContext();
            BeginContext(1461, 54, false);
#line 43 "C:\Users\ADMIN\Desktop\CalculateSalaryFleet\CalculateSalaryOfFleet\CalculateSalaryOfFleet\Views\Rules\MoneyByPerformance.cshtml"
                   Write(Html.DisplayFor(modelItem => item.DescriptionJobOrder));

#line default
#line hidden
            EndContext();
            BeginContext(1515, 105, true);
            WriteLiteral("\r\n                    </td>\r\n                    <td style=\"text-align:center\">\r\n                        ");
            EndContext();
            BeginContext(1621, 38, false);
#line 46 "C:\Users\ADMIN\Desktop\CalculateSalaryFleet\CalculateSalaryOfFleet\CalculateSalaryOfFleet\Views\Rules\MoneyByPerformance.cshtml"
                   Write(item.PerformenceMoney.ToString("##,#"));

#line default
#line hidden
            EndContext();
            BeginContext(1659, 105, true);
            WriteLiteral("\r\n                    </td>\r\n                    <td style=\"text-align:center\">\r\n                        ");
            EndContext();
            BeginContext(1765, 71, false);
#line 49 "C:\Users\ADMIN\Desktop\CalculateSalaryFleet\CalculateSalaryOfFleet\CalculateSalaryOfFleet\Views\Rules\MoneyByPerformance.cshtml"
                   Write(Html.ActionLink("Sửa", "EditMoneyByPerformance", new { id = item.Id }));

#line default
#line hidden
            EndContext();
            BeginContext(1836, 28, true);
            WriteLiteral(" |\r\n                        ");
            EndContext();
            BeginContext(1865, 55, false);
#line 50 "C:\Users\ADMIN\Desktop\CalculateSalaryFleet\CalculateSalaryOfFleet\CalculateSalaryOfFleet\Views\Rules\MoneyByPerformance.cshtml"
                   Write(Html.ActionLink("Xóa", "Delete", new { id = item.Id }));

#line default
#line hidden
            EndContext();
            BeginContext(1920, 52, true);
            WriteLiteral("\r\n                    </td>\r\n                </tr>\r\n");
            EndContext();
#line 53 "C:\Users\ADMIN\Desktop\CalculateSalaryFleet\CalculateSalaryOfFleet\CalculateSalaryOfFleet\Views\Rules\MoneyByPerformance.cshtml"
            }

#line default
#line hidden
            BeginContext(1987, 44, true);
            WriteLiteral("        </tbody>\r\n    </table>\r\n</div>\r\n\r\n\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<CalculateSalaryOfFleet.Models.MoneyJob>> Html { get; private set; }
    }
}
#pragma warning restore 1591