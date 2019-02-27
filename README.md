# CourtesyRedirectOverride
Prevents IIS from issuing a courtesy redirect when URL is lacking the trailing slash. This module also performs the courtesy redirect, but uses it using 302 response and RELATIVE path instead of 301 response with ABSOLUTE path issued by IIS.

Based on https://forums.iis.net/t/1153462.aspx?Is+it+possible+to+disable+the+courtesy+301+redirect+for+URL+requests+that+lack+a+trailing+slash+
