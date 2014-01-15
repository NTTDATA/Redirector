Redirector
==================

Sitecore module that supports 301 redirects using database mapping, regular expressions and custom rules


**Requirements:**

- .NET 4.0+
- IIS 7+
- Entity Framework 4.1
- Tested on Sitecore 6.5


**Installation:**

1. Install Sitecore package

2. Create the database table by running the Redirectory-Sql.txt Script on 

3. Add the connection string with then name redirector:

	<add name="redirector" providerName="System.Data.SqlClient" 
	connectionString="dbconnection"/>

**Contact Info:**  
<matthew.schultz@nttdata.com>  
<http://www.nttdatasitecore.com>
