Redirector
==================

Sitecore module that supports 301 redirects using database mapping, regular expressions and custom rules


**Requirements:**

- .NET 4.5+
- IIS 7+
- Entity Framework 6.0.2
- Tested on Sitecore 7.1


**Installation:**

1. Install Sitecore package

2. Create the database table by running the Redirectory-Sql.txt Script.

3. Add the connection string with then name redirector:

	<add name="redirector" providerName="System.Data.SqlClient" 
	connectionString="dbconnection"/>

4. Add the following rule to the Sitecore.AntiCSRF.config file

        <rule name="redirector">
          <urlPrefix>/sitecore/shell</urlPrefix>
          <ignore wildcard="/sitecore/shell/*Applications/Redirector/Redirect Manager.aspx*\?*" />
          <ignore wildcard="/sitecore/shell/default.aspx*\?xmlcontrol=EditRedirectForm*"/>
        </rule>

	
**Contact Info:**  
<matthew.schultz@nttdata.com>  
<http://www.nttdatasitecore.com>
