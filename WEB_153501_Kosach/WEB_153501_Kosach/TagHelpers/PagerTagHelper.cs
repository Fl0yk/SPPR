using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace WEB_153501_Kosach.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    //[HtmlTargetElement("pager")]
    public class PagerTagHelper : TagHelper
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _contextAccessor;

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Category { get; set; }
        public bool Admin {  get; set; } = false;

        private int _next;
        private int _prev;

        public PagerTagHelper(LinkGenerator linkGenerator,
                        IHttpContextAccessor contextAccessor)
        {
            _linkGenerator = linkGenerator;
            _contextAccessor = contextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            _prev = CurrentPage == 1 ? 1 : CurrentPage - 1;
            _next = CurrentPage == TotalPages
                                    ? TotalPages
                                    : CurrentPage + 1;

            TagBuilder ul = new("ul");
            ul.AddCssClass("pagination");

            //Prev
            TagBuilder prevSpan = new("span");
            prevSpan.Attributes.Add("aria-hidden", "true");
            prevSpan.InnerHtml.Append("«");
            ul.InnerHtml.AppendHtml(CreateItem(_prev, prevSpan));

            //midle
            for(int i = 1; i <= TotalPages; i++)
            {
                ul.InnerHtml.AppendHtml(CreateItem(i, null));
            }

            //next
            TagBuilder nextSpan = new("span");
            nextSpan.Attributes.Add("aria-hidden", "true");
            nextSpan.InnerHtml.Append("»"); //"&raquo;");
            ul.InnerHtml.AppendHtml(CreateItem(_next, nextSpan));

            TagBuilder nav = new TagBuilder("nav");
            nav.Attributes.Add("arial-label", "Page navigation");
            nav.InnerHtml.AppendHtml(ul);

            output.Content.SetHtmlContent(nav);
        }

        private TagBuilder CreateItem(int pageno, TagBuilder? span)
        {
            TagBuilder li = new("li");
            li.AddCssClass("page-item");
            li.InnerHtml.AppendHtml(GetLinkTag(pageno, span));

            if(pageno == CurrentPage && span is null)
            {
                li.AddCssClass("active");
            }

            return li;
        }

        private TagBuilder GetLinkTag(int pageno, TagBuilder? span)
        {
            string? path = string.Empty;
            if (Admin)
            {

                path = _linkGenerator.GetPathByPage(_contextAccessor.HttpContext);
            }
            else
            {
                path = _linkGenerator.GetPathByAction(_contextAccessor.HttpContext,
                                                        values: new { pageno=pageno, category=Category });
            }

            if(path is null)
                throw new ArgumentNullException("path");

            TagBuilder link = new TagBuilder("a");
            link.Attributes.Add("href", path);
            link.AddCssClass("page-link");

            if(span is not null)
            {
                link.InnerHtml.AppendHtml(span);
            }
            else
            {
                link.InnerHtml.Append(pageno.ToString());
            }

            return link;
        }
    }
}
