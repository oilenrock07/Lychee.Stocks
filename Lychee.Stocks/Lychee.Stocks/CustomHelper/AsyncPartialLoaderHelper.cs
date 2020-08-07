using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Lychee.Stocks.CustomHelper
{
    public static class AsyncPartialLoaderHelper
    {
        public static IHtmlString AsyncPartialLoader(this HtmlHelper helper, string actionName, string controllerName,
            RouteValueDictionary routeValueDictionary = null, 
            HttpVerbs verb = HttpVerbs.Get)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var tagBuilder = new TagBuilder("partial-loader");
            tagBuilder.MergeAttribute("data-url", urlHelper.Action(actionName, controllerName, routeValueDictionary));
            tagBuilder.MergeAttribute("data-verb", verb.ToString());

            var loadingImageTagBuilder = new TagBuilder("img");
            loadingImageTagBuilder.MergeAttribute("src", "/Images/loading.gif");
            tagBuilder.InnerHtml = loadingImageTagBuilder.ToString(TagRenderMode.SelfClosing);

            return new MvcHtmlString(tagBuilder.ToString());
        }
    }
}