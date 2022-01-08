var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Default scheme, Authentication scheme and cookie name must be same as identities name
// used in Login.cshtml.cs,  
builder.Services.AddAuthentication("MyCookieAuth")
	.AddCookie("MyCookieAuth", options =>
	{
		options.Cookie.Name = "MyCookieAuth";

	});
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// Add Authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
