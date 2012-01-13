using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.IdentityModel.Claims;

namespace DCT.Monitor.Server.Providers
{
    public class ServiceAuthorization : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            string action = operationContext.RequestContext.RequestMessage.Headers.Action;

            foreach (ClaimSet cs in operationContext.ServiceSecurityContext.AuthorizationContext.ClaimSets)
            {
                if (cs.Issuer == ClaimSet.System)
                {
                    foreach (Claim c in cs.FindClaims("http://www.contoso.com/claims/allowedoperation", Rights.PossessProperty))
                    {
                        if (action == c.Resource.ToString())
                            return true;
                    }
                }
            }

            return false;
        }
    }
}