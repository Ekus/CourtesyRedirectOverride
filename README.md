# CourtesyRedirectOverride
Prevents IIS from issuing a courtesy redirect when URL is lacking the trailing slash. This module performs the same function, but uses it using 302 response and RELATIVE path instead of 301 response with ABSOLUTE path issued by IIS.

This helps prevent "leaking" of internal IP/hostname from internal nodes and redirecting users to not-reachable URL when using some simple load balancers, e.g. Azure Web Application Gateway v1 (v2 is able to translate the URLs in return headers and body if needed but is not available in internal Azure networks)

Based on https://forums.iis.net/t/1153462.aspx?Is+it+possible+to+disable+the+courtesy+301+redirect+for+URL+requests+that+lack+a+trailing+slash+

Requires .net 4.0.

Just add the compiled CourtesyRedirectOverride.dll to your app's bin folder, and make sure to add this to web.config:

  	<system.webServer>
  		<modules runAllManagedModulesForAllRequests="true">
  			<!-- prevent IIS from redirecting with full hostname (breaks Azure Web Gateway) when user requests the app root without trailing slash -->
  			<add name="CourtesyRedirectOverride" preCondition="integratedMode" type="CourtesyRedirectOverride.CourtesyRedirectOverride"/>
  		</modules>
  	</system.webServer>
