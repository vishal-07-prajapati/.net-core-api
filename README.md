# .net-core-api
# ⏱️ 3–4 Hour Practical Round Timeline

| Time        | Task                                            | Priority          |
| ----------- | ----------------------------------------------- | ----------------- |
| 0–15 min    | Create project + folders + install EF packages  | 🔥 Critical       |
| 15–30 min   | Setup DbContext + connection string + migration | 🔥 Critical       |
| 30–50 min   | Create Models + Relationships                   | 🔥 Critical       |
| 50–80 min   | Repository + Service Layer + DI                 | 🔥 Critical       |
| 80–110 min  | CRUD APIs/Views working                         | 🔥 Critical       |
| 110–130 min | DTOs + Mapping                                  | 🔥 High           |
| 130–150 min | Pagination + Search + IQueryable                | 🔥 High           |
| 150–165 min | Validation + Error handling                     | 🔥 Medium         |
| 165–180 min | Testing + Cleanup + Refactor                    | 🔥 Very Important |

---

# 🔥 Folder Creation Order

```text id="chart001"
Models
↓
Data
↓
Repositories
↓
Services
↓
Controllers
↓
DTOs
↓
Validators
```

---

# 🔥 Coding Flow

```text id="chart002"
Model
↓
DbContext
↓
Migration
↓
Repository
↓
Service
↓
Controller
↓
Test
```

---

# 🔥 Feature Priority

| Feature        | Must Have? |
| -------------- | ---------- |
| CRUD           | ✅          |
| Relationships  | ✅          |
| DTOs           | ✅          |
| Pagination     | ✅          |
| Validation     | ✅          |
| Error handling | ✅          |
| Authentication | ⚠️ If time |
| Logging        | ⚠️ Bonus   |
| Dapper         | ⚠️ Bonus   |

---

# 🔥 Most Important Rule

```text id="chart003"
Make functionality work FIRST
then improve architecture gradually.
```

---

# 🔥 Golden Rule

```text id="chart004"
Working clean project
>
Half-finished advanced architecture
```


----

# ⏱️ 3–4 Hour Employee Management Practical Round Plan

| Time        | Task                 | What To Build                      | Important Concepts            |
| ----------- | -------------------- | ---------------------------------- | ----------------------------- |
| 0–15 min    | Project Setup        | Create ASP.NET MVC/Web API project | Folder structure, EF packages |
| 15–30 min   | Database Setup       | Connection string + DbContext      | EF Code First                 |
| 30–45 min   | Create Models        | Employee + Company                 | Relationships                 |
| 45–60 min   | Migration            | Add-Migration + Update-Database    | SQL generation                |
| 60–90 min   | Repository Layer     | CRUD methods                       | Repository Pattern            |
| 90–110 min  | Service Layer        | Business logic                     | SOLID, SRP                    |
| 110–130 min | Controller           | CRUD APIs/Views                    | MVC/API flow                  |
| 130–150 min | DTO Mapping          | Employee ↔ DTO                     | Clean architecture            |
| 150–165 min | Pagination + Search  | IQueryable + LINQ                  | Performance                   |
| 165–180 min | Validation + Testing | Validation + bug fixing            | Production readiness          |

---

# 🔥 STEP-BY-STEP FLOW

---

# 1. Project Setup

## Create Folders

```text id="empchart001"
Controllers
Models
DTOs
Repositories
Services
Data
Validators
Extensions
```

---

# 2. Install Packages

## EF Packages

```powershell id="empchart002"
Install-Package EntityFramework
```

---

# 3. Setup Connection String

## Web.config (.NET Framework)

```xml id="empchart003"
<connectionStrings>
  <add name="DefaultConnection"
       connectionString="Server=.;Database=EmployeeDB;Trusted_Connection=True;"
       providerName="System.Data.SqlClient"/>
</connectionStrings>
```

---

# 4. Create Models

---

# Company.cs

```csharp id="empchart004"
public class Company
{
    public int CompanyId { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Employee> Employees
    {
        get;
        set;
    }
}
```

---

# Employee.cs

```csharp id="empchart005"
public class Employee
{
    public int EmployeeId { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public decimal Salary { get; set; }

    public int CompanyId { get; set; }

    public virtual Company Company
    {
        get;
        set;
    }
}
```

---

# 🔥 Concepts Here

| Concept     | Why Important        |
| ----------- | -------------------- |
| CompanyId   | Foreign Key          |
| virtual     | Lazy Loading         |
| ICollection | One-to-Many relation |

---

# 5. Create DbContext

## Data/AppDbContext.cs

```csharp id="empchart006"
public class AppDbContext : DbContext
{
    public AppDbContext()
        : base("name=DefaultConnection")
    {
    }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<Company> Companies { get; set; }
}
```

---

# 🔥 DbContext Responsibility

ONLY:

* DbSets
* Relationships
* EF Config

NOT CRUD methods.

---

# 6. Migration

```powershell id="empchart007"
Add-Migration Initial
Update-Database
```

Creates:

* Tables
* Relationships
* Foreign Keys

---

# 7. Create DTOs

---

# EmployeeDTO.cs

```csharp id="empchart008"
public class EmployeeDTO
{
    public int EmployeeId { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public decimal Salary { get; set; }

    public int CompanyId { get; set; }

    public string CompanyName { get; set; }
}
```

---

# 🔥 Why DTO?

✅ Hide entity structure
✅ API-safe
✅ Cleaner responses

---

# 8. Repository Layer

---

# IEmployeeRepository.cs

```csharp id="empchart009"
public interface IEmployeeRepository
{
    List<EmployeeDTO> ListEmployees(
        int page,
        int pageSize,
        string search);

    EmployeeDTO GetById(int id);

    EmployeeDTO InsertUpdate(EmployeeDTO dto);

    bool Delete(int id);
}
```

---

# EmployeeRepository.cs

```csharp id="empchart010"
public class EmployeeRepository
    : IEmployeeRepository
{
    public List<EmployeeDTO> ListEmployees(
        int page = 1,
        int pageSize = 10,
        string search = "")
    {
        using(var context = new AppDbContext())
        {
            var query = context.Employees
                .AsNoTracking()
                .Include(x => x.Company)
                .AsQueryable();

            if(!string.IsNullOrEmpty(search))
            {
                query = query.Where(x =>
                    x.Name.Contains(search));
            }

            return query
                .OrderBy(x => x.EmployeeId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new EmployeeDTO
                {
                    EmployeeId = x.EmployeeId,
                    Name = x.Name,
                    Email = x.Email,
                    Salary = x.Salary,
                    CompanyId = x.CompanyId,
                    CompanyName = x.Company.Name
                })
                .ToList();
        }
    }
}
```

---

# 🔥 Concepts Here

| Feature        | Why                |
| -------------- | ------------------ |
| AsNoTracking   | Faster read        |
| Include        | Load relations     |
| IQueryable     | DB-level filtering |
| Skip/Take      | Pagination         |
| DTO Projection | Better SQL         |

---

# 9. Service Layer

---

# EmployeeService.cs

```csharp id="empchart011"
public class EmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(
        IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public List<EmployeeDTO> ListEmployees(
        int page,
        int pageSize,
        string search)
    {
        return _repository.ListEmployees(
            page,
            pageSize,
            search);
    }
}
```

---

# 🔥 Why Service Layer?

✅ Business logic
✅ Thin controllers
✅ Reusable logic

---

# 10. Controller

---

# EmployeeController.cs

```csharp id="empchart012"
public class EmployeeController : Controller
{
    private readonly EmployeeService _service;

    public EmployeeController()
    {
        _service = new EmployeeService(
            new EmployeeRepository());
    }

    public ActionResult Index(
        int page = 1,
        int pageSize = 10,
        string search = "")
    {
        var data = _service.ListEmployees(
            page,
            pageSize,
            search);

        return View(data);
    }
}
```

---

# 🔥 MVC Flow

```text id="empchart013"
Browser
↓
Controller
↓
Service
↓
Repository
↓
DbContext
↓
SQL Server
```

---

# 11. Validation

---

# EmployeeDTO.cs

```csharp id="empchart014"
[Required]
[StringLength(50)]
public string Name { get; set; }

[EmailAddress]
public string Email { get; set; }
```

---

# OR FluentValidation

## EmployeeValidator.cs

```csharp id="empchart015"
RuleFor(x => x.Name)
    .NotEmpty();

RuleFor(x => x.Email)
    .EmailAddress();
```

---

# 12. Add Employee Form

---

# View

```csharp id="empchart016"
@Html.HiddenFor(x => x.EmployeeId)

@Html.TextBoxFor(x => x.Name)

@Html.ValidationMessageFor(x => x.Name)
```

---

# Company Dropdown

```csharp id="empchart017"
@Html.DropDownListFor(
    x => x.CompanyId,
    ViewBag.Companies as SelectList,
    "Select Company")
```

---

# 13. Error Handling

---

# Controller

```csharp id="empchart018"
if (!ModelState.IsValid)
{
    return View(model);
}
```

---

# try-catch

```csharp id="empchart019"
try
{
}
catch(Exception ex)
{
}
```

---

# 14. Authentication (Optional)

---

# Login API/MVC

```text id="empchart020"
Email + Password
```

---

# Session/JWT

```csharp id="empchart021"
[Authorize]
```

---

# 🔥 MOST IMPORTANT PRACTICAL ROUND PRIORITY

```text id="empchart022"
Working CRUD
↓
Relationships
↓
DTOs
↓
Pagination
↓
Validation
↓
Error handling
↓
Authentication
```

---

# 🔥 MOST IMPORTANT THINGS INTERVIEWERS CHECK

| Area          | What They Observe    |
| ------------- | -------------------- |
| Architecture  | Layer separation     |
| EF knowledge  | Include, tracking    |
| LINQ          | Filtering/pagination |
| DTO usage     | Clean APIs           |
| Naming        | Maintainability      |
| Validation    | Production thinking  |
| Relationships | DB understanding     |
| Debugging     | Real-world skill     |

---

# 🔥 Final Winning Formula

```text id="empchart023"
Clean Structure
+
Working CRUD
+
Pagination
+
Relationships
+
Validation
=
Strong Practical Round
```


----


# JWT Authentication — Proper Practical Round Setup (.NET Core)

This is the cleanest and safest JWT implementation for interviews/practical rounds.

---

# 🔥 Install Packages

```powershell id="jwt001"
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
Install-Package System.IdentityModel.Tokens.Jwt
```

---

# 🔥 appsettings.json

```json id="jwt002"
{
  "Jwt": {
    "Key": "ThisIsMySecretKey123456",
    "Issuer": "EmployeeAPI",
    "Audience": "EmployeeAPIUsers",
    "DurationInMinutes": 60
  }
}
```

---

# 🔥 Login DTO

## DTOs/LoginDto.cs

```csharp id="jwt003"
public class LoginDto
{
    public string Email { get; set; }

    public string Password { get; set; }
}
```

---

# 🔥 JWT Service Interface

## Interfaces/IJwtService.cs

```csharp id="jwt004"
public interface IJwtService
{
    string GenerateToken(string email);
}
```

---

# 🔥 JWT Service

## Services/JwtService.cs

```csharp id="jwt005"
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(string email)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]));

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(
                Convert.ToDouble(
                    _configuration["Jwt:DurationInMinutes"])),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}
```

---

# 🔥 Auth Controller

## Controllers/AuthController.cs

```csharp id="jwt006"
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtService _jwtService;

    public AuthController(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        // Dummy validation

        if (dto.Email == "admin@test.com"
            && dto.Password == "123")
        {
            var token =
                _jwtService.GenerateToken(dto.Email);

            return Ok(new
            {
                Token = token
            });
        }

        return Unauthorized();
    }
}
```

---

# 🔥 Program.cs Configuration

## Program.cs

```csharp id="jwt007"
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IJwtService, JwtService>();

// JWT Configuration

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer =
                builder.Configuration["Jwt:Issuer"],

            ValidAudience =
                builder.Configuration["Jwt:Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:Key"]))
        };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

---

# 🔥 Protect APIs

## EmployeeController.cs

```csharp id="jwt008"
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
}
```

---

# 🔥 Role-Based Authorization

```csharp id="jwt009"
[Authorize(Roles = "Admin")]
```

---

# 🔥 How Request Works

---

# Step 1 — Login

```http id="jwt010"
POST /api/auth/login
```

Returns:

```json id="jwt011"
{
  "token": "eyJhbGciOi..."
}
```

---

# Step 2 — Use Token

```http id="jwt012"
Authorization: Bearer eyJhbGciOi...
```

---

# Step 3 — Access Protected API

```http id="jwt013"
GET /api/employee
```

---

# 🔥 Important JWT Concepts

| Concept    | Meaning                       |
| ---------- | ----------------------------- |
| Claim      | User information inside token |
| Issuer     | Who created token             |
| Audience   | Who uses token                |
| Secret Key | Used for signing              |
| Expiry     | Token validity                |

---

# 🔥 Common Interview Questions

---

# ❓ Why JWT Stateless?

Because:

```text id="jwt014"
Server does not store session
```

Token contains user identity.

---

# ❓ Difference Between Session and JWT?

| Session               | JWT                 |
| --------------------- | ------------------- |
| Server stores session | Client stores token |
| Stateful              | Stateless           |
| Better for MVC        | Better for APIs     |

---

# ❓ Why Use Claims?

Claims store:

* Email
* Role
* UserId

inside token.

---

# 🔥 Practical Round Advice

## Minimum JWT Setup Enough

✅ Login API
✅ Token generation
✅ Authorize attribute

That alone is already strong.

DO NOT overbuild auth system in practical round.


# JWT Authentication — Proper Practical Round Setup (.NET Core)

This is the cleanest and safest JWT implementation for interviews/practical rounds.

---

# 🔥 Install Packages

```powershell id="jwt001"
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
Install-Package System.IdentityModel.Tokens.Jwt
```

---

# 🔥 appsettings.json

```json id="jwt002"
{
  "Jwt": {
    "Key": "ThisIsMySecretKey123456",
    "Issuer": "EmployeeAPI",
    "Audience": "EmployeeAPIUsers",
    "DurationInMinutes": 60
  }
}
```

---

# 🔥 Login DTO

## DTOs/LoginDto.cs

```csharp id="jwt003"
public class LoginDto
{
    public string Email { get; set; }

    public string Password { get; set; }
}
```

---

# 🔥 JWT Service Interface

## Interfaces/IJwtService.cs

```csharp id="jwt004"
public interface IJwtService
{
    string GenerateToken(string email);
}
```

---

# 🔥 JWT Service

## Services/JwtService.cs

```csharp id="jwt005"
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(string email)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]));

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(
                Convert.ToDouble(
                    _configuration["Jwt:DurationInMinutes"])),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}
```

---

# 🔥 Auth Controller

## Controllers/AuthController.cs

```csharp id="jwt006"
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtService _jwtService;

    public AuthController(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        // Dummy validation

        if (dto.Email == "admin@test.com"
            && dto.Password == "123")
        {
            var token =
                _jwtService.GenerateToken(dto.Email);

            return Ok(new
            {
                Token = token
            });
        }

        return Unauthorized();
    }
}
```

---

# 🔥 Program.cs Configuration

## Program.cs

```csharp id="jwt007"
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IJwtService, JwtService>();

// JWT Configuration

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer =
                builder.Configuration["Jwt:Issuer"],

            ValidAudience =
                builder.Configuration["Jwt:Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:Key"]))
        };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

---

# 🔥 Protect APIs

## EmployeeController.cs

```csharp id="jwt008"
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
}
```

---

# 🔥 Role-Based Authorization

```csharp id="jwt009"
[Authorize(Roles = "Admin")]
```

---

# 🔥 How Request Works

---

# Step 1 — Login

```http id="jwt010"
POST /api/auth/login
```

Returns:

```json id="jwt011"
{
  "token": "eyJhbGciOi..."
}
```

---

# Step 2 — Use Token

```http id="jwt012"
Authorization: Bearer eyJhbGciOi...
```

---

# Step 3 — Access Protected API

```http id="jwt013"
GET /api/employee
```

---

# 🔥 Important JWT Concepts

| Concept    | Meaning                       |
| ---------- | ----------------------------- |
| Claim      | User information inside token |
| Issuer     | Who created token             |
| Audience   | Who uses token                |
| Secret Key | Used for signing              |
| Expiry     | Token validity                |

---

# 🔥 Common Interview Questions

---

# ❓ Why JWT Stateless?

Because:

```text id="jwt014"
Server does not store session
```

Token contains user identity.

---

# ❓ Difference Between Session and JWT?

| Session               | JWT                 |
| --------------------- | ------------------- |
| Server stores session | Client stores token |
| Stateful              | Stateless           |
| Better for MVC        | Better for APIs     |

---

# ❓ Why Use Claims?

Claims store:

* Email
* Role
* UserId

inside token.

---

# 🔥 Practical Round Advice

## Minimum JWT Setup Enough

✅ Login API
✅ Token generation
✅ Authorize attribute

That alone is already strong.

DO NOT overbuild auth system in practical round.


# Global Exception Middleware for Error Logging (.NET Core)

This is a VERY strong practical-round addition. ✅

It shows:

* Production thinking
* Centralized error handling
* Clean architecture

---

# 🔥 Purpose

Instead of:

```csharp id="mid001"
try-catch everywhere
```

Use:

# ✅ One global middleware

---

# 🔥 Folder

```text id="mid002"
Middleware
```

---

# 🔥 File

## Middleware/ExceptionMiddleware.cs

```csharp id="mid003"
using System.Net;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;

        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log Error

            _logger.LogError(ex, ex.Message);

            // Response

            context.Response.ContentType =
                "application/json";

            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsJsonAsync(
                new
                {
                    Message = "Something went wrong",
                    Error = ex.Message
                });
        }
    }
}
```

---

# 🔥 Register Middleware

## Program.cs

```csharp id="mid004"
app.UseMiddleware<ExceptionMiddleware>();
```

IMPORTANT:

```text id="mid005"
Place BEFORE MapControllers()
```

---

# 🔥 Full Order Example

```csharp id="mid006"
app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();
```

---

# 🔥 How It Works

---

# Request

```text id="mid007"
GET /api/employee
```

---

# Exception Happens

```csharp id="mid008"
throw new Exception();
```

---

# Middleware Catches

```text id="mid009"
Centralized handling
```

---

# Response

```json id="mid010"
{
  "message": "Something went wrong",
  "error": "Object reference..."
}
```

---

# 🔥 Why Middleware Better?

| try-catch everywhere   | Middleware         |
| ---------------------- | ------------------ |
| Duplicate code         | Centralized        |
| Hard maintenance       | Cleaner            |
| Inconsistent responses | Standard responses |

---

# 🔥 BEST Practice Improvement

In production:

```text id="mid011"
Do NOT expose actual exception message
```

Better:

```csharp id="mid012"
await context.Response.WriteAsJsonAsync(
    new
    {
        Message = "Internal Server Error"
    });
```

---

# 🔥 Add Custom Status Codes

Example:

```csharp id="mid013"
catch(KeyNotFoundException ex)
{
    context.Response.StatusCode = 404;
}
```

Very professional addition.

---

# 🔥 Logging Options

Default:

```text id="mid014"
ILogger
```

Advanced:

* [Serilog](https://serilog.net?utm_source=chatgpt.com)
* [NLog](https://nlog-project.org?utm_source=chatgpt.com)

---

# 🔥 Interview Questions

---

# ❓ Why Middleware Used?

## ✅ Answer

Middleware provides centralized exception handling and logging for all requests, reducing duplicate try-catch code and ensuring consistent API responses.

---

# ❓ Why Global Exception Handling Important?

## ✅ Answer

It improves maintainability, standardizes error responses, simplifies debugging, and prevents application crashes from unhandled exceptions.

---

# 🔥 Practical Round Advice

This is enough to impress:

✅ Middleware
✅ ILogger
✅ JSON error response
✅ Proper status code

No need for complex enterprise logging setup in short rounds.
