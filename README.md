# ASP.NET Core Identity Tutorial
The purpose of this tutorial is to give you a brief introduction to the Microsoft Identity framework, and step by step approach to configure it. Before beginning, you require to be familiar with basic terminologies.
1. Principal
2. Identities
3. Claims

## Principal
The security-related information is encapsulated in single object  Claims' Principal. Whether a user is a login or not this object is accessible. A principal has one or more identities. It is also interchangeable with Term User.

## Identities:
In simple terms, identity is a user's role. For example, a user can be a Manager, Accountant, Storekeeper, etc. Principal contains one primary Identity. Each identity has one or more claims.

## Claims:
A claim carries information related to identity. For example, if a principal has a student identity then a claim may contain, semester, campus, supervisor, etc. A claim is a key-pair object.


To understand it practically you have to create an Asp.Net Core Web App without authentication.
After creating the project, create an "Account" folder in the "Pages" folder, and then add a "Login" razor page.
#### Login.cshtml
```html
@page
@model WebApplication2.Pages.Account.LoginModel
@{
}
<form method="post">
	<div class="container p-3 border">
		<div class="row mb-2">
			<div class="col-md-2">
				<label asp-for="Credential.UserName"></label>
			</div>
			<div class="col-md-4">
				<input type="text" value="" asp-for="Credential.UserName" class="form-control" />
			</div>
		</div>
		<div class="row mb-2">
			<div class="col-md-2">
				<label asp-for="Credential.Password"></label>
			</div>
			<div class="col-md-4">
				<input type="password" value="" asp-for="Credential.Password" class="form-control" />
			</div>
		</div>
		<div class="row mb-2">
			<div class="col-md-2">
				<button type="submit" class="btn btn-primary">Login</button>
			</div>
		</div>
	</div>
</form>

```
#### Login.cshtml.cs
```csharp
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication2.Pages.Account
{
	public class LoginModel : PageModel
	{
		[BindProperty] public LoginVM Credential { get; set; }
		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			// If posted data is invalid then redirect to same login page
			if (!ModelState.IsValid) return Page();
			// Use database/active directory or any custom login validation code here
			if (Credential.UserName == "admin" && Credential.Password == "admin")
			{
				// if user is valid then get its claim from database and set its claim
				var claims = new List<Claim>
				{
					new Claim("name", "admin"),
					new Claim("email", Credential.UserName)
				};
				// user identities are clubbed into a Collection object name "MyCookiesAuth"
				var identities = new ClaimsIdentity(claims, "MyCookieAuth");
				// assign identities to Principal
				var principal = new ClaimsPrincipal(identities);
				// pass this principal HttpContext by its extension method SignInAsync
				await HttpContext.SignInAsync("MyCookieAuth", principal);
				// After successful login redirect to home page
				return RedirectToPage("/index");
			}

			return Page();
		}
	}

	public class LoginVM
	{
		[Required]
		[StringLength(20)]
		[Display(Name = "User Name")]
		public string UserName { get; set; }
		[Required]
		[StringLength(20)]
		[DisplayName("Password")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}


```


### Authentication Service Configuration
The second step is to configure authentication. There are various ways to configure authentication. You can use Cookies or  JWT. In this tutorial, I am configuring authentication based on Cookies.
To configure authentication you have to authentication services.
```csharp
// Default scheme, Authentication scheme and cookie name must be same as identities name
// used in Login.cshtml.cs,  
builder.Services.AddAuthentication("MyCookieAuth")
	.AddCookie("MyCookieAuth", options =>
	{
		options.Cookie.Name = "MyCookieAuth";

	});
builder.Services.AddRazorPages();
```

In configuration the second step is to use authentication in middleware.
```csharp
app.UseRouting();
// Add Authentication middleware 
app.UseAuthentication();
app.UseAuthorization();
```
