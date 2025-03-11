dotnet ef dbcontext scaffold "User Id=myuser;Password=mypassword;Data Source=myhost:1521/myservice" Oracle.EntityFrameworkCore -o Models --context MyDbContext --no-pluralize


dotnet add package Oracle.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools

