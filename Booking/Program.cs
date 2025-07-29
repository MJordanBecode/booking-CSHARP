using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Booking.Data;
using Booking.Service;
using Microsoft.AspNetCore.Identity.UI.Services;
using Solution.Models;

var builder = WebApplication.CreateBuilder(args);

// Connexion SQL Server ici
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)); // <-- SQL Server

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Permet de cibler le fichier IOfferService et OfferService 
builder.Services.AddScoped<IOfferService, OfferService>();

// Configuration Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => 
        options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configuration du cookie d'authentification => il faut toujours la mettre après la config identity, ainsi ça peut fonctionner sans problèmes
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(14);
    options.LoginPath = "/Identity/Account/Login"; //chemin de login, qui permet de récupérer le token d'authentification
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

// ** IMPORTANT : Ajouter Razor Pages pour Identity **
builder.Services.AddRazorPages();

// Ajout du service IEmailSender "factice" pour éviter l’erreur lors de l’inscription
builder.Services.AddSingleton<IEmailSender, NullEmailSender>();

// Controllers + Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Mapper les Razor Pages (ex: pages d'Identity)
app.MapRazorPages().WithStaticAssets();

app.Run();


// Implémentation "vide" pour IEmailSender (ne fait rien)
public class NullEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // Pas d’envoi réel, juste un retour réussi
        return Task.CompletedTask;
    }
}
