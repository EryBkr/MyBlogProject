#pragma checksum "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\Article\Detail.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c1c44aa70735eef8ad999b92cbc1d46594ca8d14"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Article_Detail), @"mvc.1.0.view", @"/Views/Article/Detail.cshtml")]
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
#nullable restore
#line 2 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\_ViewImports.cshtml"
using MyBlog.Mvc.Areas.Admin.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\_ViewImports.cshtml"
using MyBlog.Mvc.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\_ViewImports.cshtml"
using MyBlog.Entities.Dtos.CategoryDtos;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\_ViewImports.cshtml"
using MyBlog.Entities.Dtos.UserDtos;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\_ViewImports.cshtml"
using MyBlog.Entities.Dtos.EmailDtos;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\_ViewImports.cshtml"
using MyBlog.Entities.Dtos.ArticleDtos;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\_ViewImports.cshtml"
using MyBlog.Entities.Dtos.CommentDtos;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\_ViewImports.cshtml"
using MyBlog.Entities.Dtos.RoleDtos;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\_ViewImports.cshtml"
using MyBlog.Entities.Concrete;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\_ViewImports.cshtml"
using MyBlog.Shared.Utilities.Results.ComplexTypes;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\_ViewImports.cshtml"
using MyBlog.Shared.Entities.Concrete;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\_ViewImports.cshtml"
using System.Text.RegularExpressions;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c1c44aa70735eef8ad999b92cbc1d46594ca8d14", @"/Views/Article/Detail.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2e3cd265a9f92f23d958551ca814fce4c738b549", @"/Views/_ViewImports.cshtml")]
    public class Views_Article_Detail : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ArticleDetailViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("img-fluid rounded"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "~/Views/Comment/_CommentAddPartial.cshtml", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("d-flex mr-3 rounded-circle"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("width:30px;"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/img/userImages/defaultUser.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("alt", new global::Microsoft.AspNetCore.Html.HtmlString(""), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_ArticleDetailRightSideBarPartial", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/blogLTE/js/articleDetail.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\Article\Detail.cshtml"
  
    ViewBag.Title = Model.ArticleDto.Article.Title;
    ViewBag.Description = Model.ArticleDto.Article.SeoDescription;
    ViewBag.Author = Model.ArticleDto.Article.SeoAuthor;
    ViewBag.Tags = Model.ArticleDto.Article.SeoTags;
    Layout = "_ArticleLayout";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<!-- Post Content Column -->\r\n<div class=\"col-lg-8\">\r\n\r\n    <!-- Title -->\r\n    <h1 class=\"mt-4\">");
#nullable restore
#line 15 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\Article\Detail.cshtml"
                Write(Model.ArticleDto.Article.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n\r\n    <!-- Author -->  <!-- Date/Time -->\r\n    <p class=\"lead\">\r\n        ");
#nullable restore
#line 19 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\Article\Detail.cshtml"
   Write(Model.ArticleDto.Article.User.UserName);

#line default
#line hidden
#nullable disable
            WriteLiteral(" tarafından ");
#nullable restore
#line 19 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\Article\Detail.cshtml"
                                                      Write(Model.ArticleDto.Article.Date.ToShortDateString());

#line default
#line hidden
#nullable disable
            WriteLiteral(" tarihinde paylaşılmıştır\r\n    </p>\r\n\r\n\r\n    <hr>\r\n\r\n    <!-- Preview Image -->\r\n    <div class=\"text-center\">\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "c1c44aa70735eef8ad999b92cbc1d46594ca8d149752", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 772, "~/img/", 772, 6, true);
#nullable restore
#line 27 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\Article\Detail.cshtml"
AddHtmlAttributeValue("", 778, Model.ArticleDto.Article.Thumbnail, 778, 35, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "alt", 1, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
#nullable restore
#line 27 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\Article\Detail.cshtml"
AddHtmlAttributeValue("", 820, Model.ArticleDto.Article.Title, 820, 31, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    </div>\r\n\r\n\r\n    <hr>\r\n\r\n    <!-- Post Content -->\r\n    ");
#nullable restore
#line 34 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\Article\Detail.cshtml"
Write(Html.Raw(Model.ArticleDto.Article.Content));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n    <hr>\r\n\r\n    <p class=\"text-center\">\r\n        Okunma Sayısı: <span class=\"badge badge-info\">");
#nullable restore
#line 39 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\Article\Detail.cshtml"
                                                 Write(Model.ArticleDto.Article.ViewsCount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n        Yorum Sayısı: <span class=\"badge badge-warning\">");
#nullable restore
#line 40 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\Article\Detail.cshtml"
                                                   Write(Model.ArticleDto.Article.CommentsCount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n    </p>\r\n\r\n    <!-- Comments Form -->\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "c1c44aa70735eef8ad999b92cbc1d46594ca8d1412968", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
#nullable restore
#line 44 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\Article\Detail.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Model = new CommentAddDto {ArticleId=Model.ArticleDto.Article.Id };

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext.AddTagHelperAttribute("model", __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Model, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n    <div id=\"comments\">\r\n");
#nullable restore
#line 47 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\Article\Detail.cshtml"
         foreach (var comment in Model.ArticleDto.Article.Comments)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <!-- Single Comment -->\r\n            <div class=\"media mb-4\">\r\n                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "c1c44aa70735eef8ad999b92cbc1d46594ca8d1415001", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                <div class=\"media-body\">\r\n                    <h5 class=\"mt-0\">");
#nullable restore
#line 53 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\Article\Detail.cshtml"
                                Write(comment.CreatedByName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h5>\r\n                    ");
#nullable restore
#line 54 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\Article\Detail.cshtml"
               Write(comment.Text);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </div>\r\n            </div>\r\n");
#nullable restore
#line 57 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\Article\Detail.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </div>\r\n\r\n</div>\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "c1c44aa70735eef8ad999b92cbc1d46594ca8d1417182", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_6.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_6);
#nullable restore
#line 63 "C:\Users\Blackerback\OneDrive\Masaüstü\MyBlog\MyBlog.Mvc\Views\Article\Detail.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Model = Model.ArticleDetailRightSideBarViewModel;

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext.AddTagHelperAttribute("model", __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Model, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral(" \r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c1c44aa70735eef8ad999b92cbc1d46594ca8d1418893", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_7);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
            }
            );
            WriteLiteral("\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ArticleDetailViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
