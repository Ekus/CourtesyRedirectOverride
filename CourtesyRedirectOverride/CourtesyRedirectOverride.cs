using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// 
//Default redirect for a subdir, ie. host/subdir is host/subdir/, which is not what you want when using a proxy in front, we need relative
// like so :host/subdir redirect to /subdir/, so the host is not transmitted.
/// https://forums.iis.net/t/1153462.aspx?Is+it+possible+to+disable+the+courtesy+301+redirect+for+URL+requests+that+lack+a+trailing+slash+

namespace CourtesyRedirectOverride
{
    /// <summary>
    /// This module can be used to handle requests to application root without trailing slash.
    /// Normally, IIS will handle such request by sending a 301 Moved Permanently response with the Location header set to the ABSOLUTE url WITH trailing slash.
    /// e.g. request to /app  will be redirected to http://server/app/   - in most cases this is OK but it introduces the servername to the HTTP response, which interferes with load balancers and proxies unless they handle it explicitly.
    /// This module will instead respond with 302 Found and Location header set to RELATIVE url, e.g. /app/
    /// </summary>
    /// <remarks>
    /// Now the module also handles querystring by carrying it to the new URL
    /// </remarks>
    public class CourtesyRedirectOverride : IHttpModule
    {
        public CourtesyRedirectOverride()
        {

        }

        #region IHttpModule Members

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        void context_BeginRequest(object sender, EventArgs e)
        {

            var pathAndQuery = HttpContext.Current.Request.Url.PathAndQuery.Split('?');
            var path = pathAndQuery[0];
            var query = pathAndQuery.Length > 1 ? pathAndQuery[1] : null;
            var physicalAppPath = HttpContext.Current.Server.MapPath(path);

            if (
                !path.EndsWith("/")
                    &&
                    (
                        path.Equals(HttpContext.Current.Request.ApplicationPath, StringComparison.OrdinalIgnoreCase)
                        || isSubfolder(physicalAppPath)
                    )
                )
            {
                HttpContext.Current.Response.AddHeader("X-Test", "CourtesyRedirectOverride by .net module");
                HttpContext.Current.Response.Redirect(path + "/"
                    + (query != null ? "?" + query : "") // also carry the querystring if any
                    );  // handle the redirection ourselves (will use relative path in HTTP response), so that we avoid letting IIS do it (since it uses absolute path with server name in the response, which breaks load balancers)
            }


            //var Response = HttpContext.Current.Response;
            //Response.Write("<br/> " + HttpContext.Current.Request.Url.Host);          // test                       
            //Response.Write("<br/> " + HttpContext.Current.Request.Url.Authority);     // test                       
            //Response.Write("<br/> " + HttpContext.Current.Request.Url.Port);          // 80                         
            //Response.Write("<br/> " + HttpContext.Current.Request.Url.AbsolutePath);  // /timeout                   
            //Response.Write("<br/> " + HttpContext.Current.Request.ApplicationPath);   // /TimeOut                   
            //Response.Write("<br/> " + HttpContext.Current.Request.Url.AbsoluteUri);   // http://test/timeOut?abc
            //Response.Write("<br/> " + HttpContext.Current.Request.Url.PathAndQuery);  // /timeOut?abc     
            //Response.End();

        }

        private bool isSubfolder(string filePath)
        {
            return System.IO.Directory.Exists(filePath);
        }
        #endregion
    }
}