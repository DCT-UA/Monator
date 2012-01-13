using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using DCT.Unity;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;

namespace DCT.Monitor.Server.Helpers
{
	public class SessionHelper
	{
		private HttpSessionStateBase _session;
		private DateTime? _sessionTime;

		public User User
		{
			get { return _session["User"] as User; }
			set { _session["User"] = value; }
		}

		public DateTime SessionEndTime {
			get { 
				if(_sessionTime != null) return _sessionTime.Value;
				_sessionTime = _session["ExpirationDateTime"] as DateTime?;

				if (_sessionTime == null)
				{
					_sessionTime = DateTime.MinValue;
					_session["ExpirationDateTime"] = _sessionTime;
				}

				return _sessionTime.Value;
			}
		    set { 
				_sessionTime = value;
				_session["ExpirationDateTime"] = value;
			}
		}

		public SessionHelper(HttpContextBase httpContext)
		{
			_session = httpContext.Session;

            if (httpContext.Request.IsAuthenticated && (User == null || User.Id == Guid.Empty))
            {
                var userModule = ServiceLocator.Current.Resolve<IUserModule>();
                User = userModule.GetUser(httpContext.User.Identity.Name);
            }
            else if(User == null)
            {
                User = new User();
            }
			
		}

        internal void Refresh(HttpContextBase httpContext)
        {
            if (httpContext.Request.IsAuthenticated)
            {
                var userModule = ServiceLocator.Current.Resolve<IUserModule>();
                User = userModule.GetUser(httpContext.User.Identity.Name);
            }
            else
            {
                User = new User();
            }
        }
    }
}