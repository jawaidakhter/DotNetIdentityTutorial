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
