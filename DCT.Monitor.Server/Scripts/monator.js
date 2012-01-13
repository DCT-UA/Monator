if(typeof(_monator_core) == "undefined"){
_monator_core = {
	delay: 0,

	init: function (data) {
		var requestId = data.Request.Id;
		var sessionId = data.Request.SessionIdentifier;
		var siteId = data.Request.SiteId;
		var uniqueId = "asdasf"; //"<%=Model.Session.VisitorUid %>";

		var img;
		var domain = "@Request.Url.Host";
		//domain = "itexperts.com.ua";
		var path = "/service/";

		var initialize = function () {
			var dt = new Date();
			dt.setFullYear(dt.getFullYear() + 10, 1, 1);

			var img = new Image(1, 1);
			img.rel = "nofollow";

			dt = new Date();
			var ref = document.referrer;
			var pageUrl = escape(document.location);

			setInterval(function () {
				var dtNew = new Date();
				var dif = Math.round((dtNew - dt) / 1000)
				img.src = "http://" + domain + path + "tick?requestId=" + requestId + "&duration=" + dif + "&refferer=" + escape(ref) + "&siteId=" + siteId + "&page=" + pageUrl;
			
			}, _monator_core.delay);
		}

		function trim(string) {
			return string.replace(/(^\s+)|(\s+$)/g, "");
		}

		initialize();
	}
};