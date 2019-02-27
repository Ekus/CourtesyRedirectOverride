# CourtesyRedirectOverride
Prevents IIS from issuing a courtesy redirect when URL is lacking the trailing slash. This module also performs the courtesy redirect, but uses it using 302 response and RELATIVE path instead of 301 response with ABSOLUTE path issued by IIS.
