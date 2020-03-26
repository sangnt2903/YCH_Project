When you have somethings in database, please running command at Package Manager Console !
You have to change server with your sqlserver name and database to run !

---Command Here---
Scaffold-DbContext "Server=MSI\SQL_EXPRESS;Database=FleetsTrips;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force

