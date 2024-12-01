 # Business Card Manager API

## Description
**Business Card Manager API** is a web API built using the .NET framework that enables efficient management of business cards. The project is designed to work seamlessly with an Angular frontend.

## Purpose and Features
- Easily manage and organize business cards via the API.
- Integrate with an Angular frontend for user interaction.
- Perform CRUD operations for business cards.

## Installation Instructions 

### Libraries or Dependencies Used in .NET API Project
The following libraries are used within the .NET API project:
```csproj
 <ItemGroup>
   <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.8" />
   <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.8">
     <PrivateAssets>all</PrivateAssets>
     <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
   </PackageReference>
   <PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="8.0.5" />
   <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
   <PackageReference Include="Swashbuckle.AspNetCore" Version="6.8.1" />
 </ItemGroup>
```

### Platforms
- Backend: .NET Core 8
- Database: SQL Server
- Frontend (Angular project) is separate.

## How to Run the Project

1. **Download Dependencies**  
   Ensure all necessary dependencies and libraries are downloaded. You can use `NuGet` to manage and install the required packages.

2. **Configure the Database Connection**  
   Navigate to the `appsettings.json` file located in the root directory of the project. Update the connection strings section to match your SQL Server configuration. Here's a screenshot to help clarify: 
   
   ```json
   {
     "ConnectionStrings": {
     "constrDebug": "YOUR_CONNECTION_STRINGS"
     }
   }
# Project Setup

## Add Migration and Update Database

Add migration and update the database using the Package Manager Console in Visual Studio, or any other method you prefer:
 ```
Add-Migration InitialCreate
Update-Database
```
# Run The Application
Run the project through **Visual Studio** or any preferred IDE. Ensure the database is correctly configured and updated before running the API.

# Notes :

## 1- Avoiding Cross-Origin Resource Sharing (CORS) Issues in an Angular Project

To avoid a problem with **Cross-Origin Resource Sharing (CORS)** in an Angular project, make sure the allowed URLs are configured in the `Program.cs` file.

```csharp
// Cross-Origin Resource Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(policyName,
        builder => builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader()); 
});
```

## 2- Angular Frontend Integration
The Angular frontend requires the Base URL of this API project. Copy the Base URL (or domain) of this project and paste it into the Angular project configuration as the `baseURL`.

### Replace `ts` code in `AppConstants`.
 Put your base url:  
Such as: `http://localhost:YOUR_PORT`
```typescript
export class AppConstants {
  static readonly baseURL: string = 'BASE_API_URL';
}  
```


Thank you for using **Business Card Manager API**! 😊