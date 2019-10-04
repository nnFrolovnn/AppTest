using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace WebApp.Helpers
{
    public static class Extension
    {
        public static IOwinContext GetOwinContext(this HttpRequestMessage request)
        {
            var context = request.Properties["MS_HttpContext"] as HttpContextWrapper;
            if (context != null)
            {
                return HttpContextBaseExtensions.GetOwinContext(context.Request);
            }
            return null;
        }
    }
}