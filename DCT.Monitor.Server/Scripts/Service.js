(function() {
	var requestId;

	var jQueryCookie = function(name, value, options) {
		if (typeof value != 'undefined') { // name and value given, set cookie
			options = options || {};
			if (value === null) {
				value = '';
				options.expires = -1;
			}
			var expires = '';
			if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
				var date;
				if (typeof options.expires == 'number') {
					date = new Date();
					date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
				} else {
					date = options.expires;
				}
				expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
			}
			var path = options.path ? '; path=' + (options.path) : '';
			var domain = options.domain ? '; domain=' + (options.domain) : '';
			var secure = options.secure ? '; secure' : '';
			document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
		} else { // only name given, get cookie
			var cookieValue = null;
			if (document.cookie && document.cookie != '') {
				var cookies = document.cookie.split(';');
				for (var i = 0; i < cookies.length; i++) {
					var cookie = jQuery.trim(cookies[i]);
					// Does this cookie string begin with the name we want?
					if (cookie.substring(0, name.length + 1) == (name + '=')) {
						cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
						break;
					}
				}
			}
			return cookieValue;
		}
	};

	if (!window.XMLHttpRequest) window.XMLHttpRequest = function window$XMLHttpRequest() {
		var progIDs = ['Msxml2.XMLHTTP.3.0', 'Msxml2.XMLHTTP'];
		for (var i = 0, l = progIDs.length; i < l; i++) {
			try {
				return new ActiveXObject(progIDs[i]);
			}
			catch (ex) {
			}
		}
		return null;
	};

	var xhr = new XMLHttpRequest();
	var onreadystatechange = xhr.onreadystatechange = function(isTimeout) {
		if (xhr.readyState === 4 && isTimeout !== "timeout") {
			eval("var obj = " + xhr.responseText + ";");
			requestId = obj.RequestId;
			jQueryCookie("Skyner.SessionId", obj.SessionId);
			var expd = new Date();
			expd.setFullYear(expd.getFullYear() + 5, expd.getMonth(), 1);	// cookie for 5 yeears
			jQueryCookie("Skyner.VisitorUid", obj.VisitorUid, {expires: expd});
		}
	};

	xhr.open("GET", "/Service/?referrer=" + document.referrer, true);
	xhr.send("");
})();