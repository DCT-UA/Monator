using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System;
using System.Web.Mvc.Html;
using System.Web.WebPages.Html;

namespace DCT.Monitor.Server.Helpers
{
    public static class MvcHtmlStringExtensions
    {
        public static MvcHtmlString Format(this MvcHtmlString str, string format, params object[] values)
        {
            var prms = new object[values.Length + 1];
            Array.Copy(values, 0, prms, 1, values.Length);
            prms[0] = str.ToHtmlString();
            return new MvcHtmlString(string.Format(format, prms));
        }
    }

	public class RouteHelper
	{
		public string RouteName { get; set; }

		public RouteValueDictionary RouteParams { get; set; }

		public UrlHelper Url { get; set; }

		public string HrefTarget { get; set; }

		public RouteHelper(UrlHelper url, string routeName, object routeParams)
		{
			RouteName = routeName;
			RouteParams = new RouteValueDictionary(routeParams);
			Url = url;
		}

		public virtual MvcHtmlString AsUrl()
		{
			var ret = Url.RouteUrl(RouteName, RouteParams).Replace("#", "%23");
			if(HrefTarget != null) ret += "#" + HrefTarget;
			return new MvcHtmlString(ret.ToLower());
		}

        public virtual MvcHtmlString AsUrl(object queryData)
		{
			var dictionary = new RouteValueDictionary(queryData);
			foreach(var key in RouteParams) dictionary.Add(key.Key, key.Value);
			var ret = Url.RouteUrl(RouteName, dictionary).Replace("#", "%23");
			if (HrefTarget != null) ret += "#" + HrefTarget;
			return new MvcHtmlString(ret);
		}

        public MvcHtmlString AsLink(string text)
		{
			return AsUrl().Format("<a href='{0}'>{1}</a>", text);
		}

        public MvcHtmlString AsLink(string text, string className)
		{
			return AsUrl().Format("<a href='{0}' class='{1}'>{2}</a>", className, text);
		}

        public MvcHtmlString AsLink(string text, object attributes)
		{
			var sb = new StringBuilder("<a");
			var d = new RouteValueDictionary(attributes);
			d["href"] = AsUrl().ToHtmlString();
			foreach (var key in d)
			{
				sb.Append(" ").Append(key.Key).Append("='").Append(key.Value).Append("'");
			}
			sb.Append(">").Append(text).Append("</a>");
			return new MvcHtmlString(sb.ToString());
		}

        public MvcHtmlString AsLink(string text, object queryData, object attributes)
		{
			var sb = new StringBuilder("<a");
			var d = new RouteValueDictionary(attributes);
			d["href"] = AsUrl(queryData).ToHtmlString();
			foreach (var key in d)
			{
				sb.Append(" ").Append(key.Key).Append("='").Append(key.Value).Append("'");
			}
			sb.Append(">").Append(text).Append("</a>");
			return new MvcHtmlString(sb.ToString());
		}

        public MvcHtmlString AsTabLink(string text, TabPosition tab, string className, int tabPos)
		{
			className = tab.CheckPos(className, tabPos);
			return AsUrl().Format("<li {1}><a href='{0}'>{2}</a></li>", className, text);
		}

        public MvcHtmlString AsLinkButton(string text)
        {
			return AsLink("<input type='button' value='" + text + "' />");
		}

        public MvcHtmlString AsLinkButton(string text, string className)
		{
			return AsLink("<input type='button' value='" + text + "' />", className);
		}

		public MvcHtmlString AsLinkButton(string text, string linkClass, string inputClass)
		{
			return AsLink("<input type='button' class='" + inputClass + "' value='" + text + "' />", linkClass);
		}

		public MvcHtmlString AsLinkButton(string text, string linkClass, string inputClass, string id)
		{
			return AsLink("<input type='button' " + "id='" + id + "' class='" + inputClass + "' value='" + text + "' />", linkClass);
		}

        public MvcHtmlString AsLinkButton(string text, object linkAttributes)
		{
			return AsLink("<input type='button' value='" + text + "' />", linkAttributes);
		}

        public MvcHtmlString AsSpanLink(string text, string className)
		{
			return AsLink(text).Format("<span class='button {1}'>{0}</span>", className);
		}

        public MvcHtmlString AsLiLink(string text)
		{
			return AsLink(text).Format("<li>{0}</li>");
		}

        public MvcHtmlString AsLiLink(string text, string liClassName)
		{
            return AsLink(text).Format("<li class='{1}'>{0}</li>", liClassName);
		}

        public MvcHtmlString AsLiLink(string text, string liClassName, string linkClassName)
		{
            return AsLink(text, linkClassName).Format("<li class='{1}'>{0}</li>", liClassName);
		}

        public MvcHtmlString AsLiLink(string text, object liAttributes)
		{
			var sb = new StringBuilder("<li");
			var d = new RouteValueDictionary(liAttributes);
			foreach(var key in d)
			{
				sb.Append(" ").Append(key.Key).Append("='").Append(key.Value).Append("'");
			}
			sb.Append(">").Append(AsLink(text).ToHtmlString()).Append("</li>");
			return new MvcHtmlString(sb.ToString());
		}

		public MvcHtmlString AsLiLink(string text, object liAttributes, object linkAttributes)
		{
			var sb = new StringBuilder("<li");
			var d = new RouteValueDictionary(liAttributes);
			foreach (var key in d)
			{
				sb.Append(" ").Append(key.Key).Append("='").Append(key.Value).Append("'");
			}
			sb.Append(">").Append(AsLink(text, linkAttributes).ToHtmlString()).Append("</li>");
			return new MvcHtmlString(sb.ToString());
		}
	}

	public class EmptyRouteHelper: RouteHelper
	{
		private readonly string _href;

		public EmptyRouteHelper(string href) : base(null, null, null)
		{
			_href = href;
		}

		public EmptyRouteHelper(string href, string target)
			: base(null, null, null)
		{
			_href = href + "#" + target;
			HrefTarget = target;
		}

		public override MvcHtmlString AsUrl()
		{
			return new MvcHtmlString(_href);
		}

        public override MvcHtmlString AsUrl(object queryData)
		{
			return new MvcHtmlString(_href);
		}
	}
}
