using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;       
using Fischt.Models;
using Fischt.Repositories;
using projekt.net_IKS_zofia_stanislawska.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<FischtDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<User>(options =>
{

    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    
    options.SignIn.RequireConfirmedAccount = false; 
})
.AddRoles<IdentityRole>()                        
.AddEntityFrameworkStores<FischtDbContext>();   

builder.Services.AddAuthorization();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Rejestracja Repository w kontenerze DI
// AddScoped = jeden obiekt na jedno żądanie HTTP
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();   // NOWE
builder.Services.AddScoped<IMessageRepository, MessageRepository>(); // NOWE
builder.Services.AddScoped<ISpecieRepository, SpecieRepository>();
builder.Services.AddScoped<IModerateProfileRepository, ModerateProfileRepository>(); // NOWE

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

app.UseAuthentication();   
app.UseAuthorization();   
app.UseSession();          


app.UseAuthorization();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<FischtDbContext>();


    if (!dbContext.Species.Any())
    {
        dbContext.Species.AddRange(
            new Specie { Name = "Nornik zwyczajny" },
            new Specie { Name = "Szczupak pospolity" },
            new Specie { Name = "Karp" },
            new Specie { Name = "Okoń" },
            new Specie { Name = "Sandacz" }
        );
        await dbContext.SaveChangesAsync();
    }

    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    if (!await roleManager.RoleExistsAsync("User"))
        await roleManager.CreateAsync(new IdentityRole("User"));


    async Task<User?> CreateUserWithProfile(
        string email, string password, string name, int age,
        string bio, string gender, string pronouns, int specieId, float length, bool isAdmin = false)
    {
        var existing = await userManager.FindByEmailAsync(email);
        if (existing != null) return existing;

        var user = new User { UserName = email, Email = email, Premium = isAdmin };
        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded) return null;

        await userManager.AddToRoleAsync(user, isAdmin ? "Admin" : "User");

        bool profileExists = dbContext.Profiles.Any(p => p.UserId == user.Id);
        if (!profileExists)
        {
            dbContext.Profiles.Add(new Profile
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                Name = name,
                Age = age,
                Bio = bio,
                Gender = gender,
                Sex = gender == "Kobieta" ? "Female" : "Male",
                Preferences = gender == "Kobieta" ? "Mężczyźni" : "Kobiety",
                Pronouns = pronouns,
                SpecieId = specieId,
                Length = length,
                PhotoPath = null
            });
            await dbContext.SaveChangesAsync();
        }

        return user;
    }


    var admin  = await CreateUserWithProfile("admin@fischt.com",  "Admin123!", "Admin Rybka",  30, "Zarządzam tym stawem.",        "Mężczyzna", "on/jego",  1, 185f, isAdmin: true);
    var zosia  = await CreateUserWithProfile("zosia@fischt.com",  "User123!",  "Zosia",        22, "Lubię pływać pod prąd 🌊",      "Kobieta",   "ona/jej",  2, 167f);
    var kacper = await CreateUserWithProfile("kacper@fischt.com", "User123!",  "Kacper",       25, "Szukam kogoś na głębsze wody.", "Mężczyzna", "on/jego",  3, 180f);
    var marta  = await CreateUserWithProfile("marta@fischt.com",  "User123!",  "Marta",        23, "Fanatyczka korali i kawy ☕",   "Kobieta",   "ona/jej",  4, 162f);
    var piotrek= await CreateUserWithProfile("piotrek@fischt.com","User123!",  "Piotrek",      27, "Spokojny jak karp w stawie.",   "Mężczyzna", "on/jego",  1, 178f);
    var ania   = await CreateUserWithProfile("ania@fischt.com",   "User123!",  "Ania",         21, "Wolę rzeki od jezior ",       "Kobieta",   "ona/jej",  5, 165f);

    if (zosia != null && kacper != null)
    {
        bool inviteZK = dbContext.Invites.Any(i => i.SenderId == zosia.Id && i.ReceiverId == kacper.Id);
        bool inviteKZ = dbContext.Invites.Any(i => i.SenderId == kacper.Id && i.ReceiverId == zosia.Id);

        if (!inviteZK)
        {
            dbContext.Invites.Add(new Invite { SenderId = zosia.Id, ReceiverId = kacper.Id });
            await dbContext.SaveChangesAsync();
        }
        if (!inviteKZ)
        {
            dbContext.Invites.Add(new Invite { SenderId = kacper.Id, ReceiverId = zosia.Id });
            await dbContext.SaveChangesAsync();
        }

        bool contactExists = dbContext.Contacts.Any(c =>
            (c.UserId == zosia.Id && c.ContactId == kacper.Id) ||
            (c.UserId == kacper.Id && c.ContactId == zosia.Id));

        if (!contactExists)
        {
            var contact = new Contact
            {
                Id = Guid.NewGuid().ToString(),
                UserId = zosia.Id,
                ContactId = kacper.Id
            };
            dbContext.Contacts.Add(contact);
            await dbContext.SaveChangesAsync();

            var conversation = new Conversation
            {
                Id = Guid.NewGuid().ToString(),
                ContactId = contact.Id
            };
            dbContext.Conversations.Add(conversation);
            await dbContext.SaveChangesAsync();


            await dbContext.SaveChangesAsync();
        }
    }
}

app.Run();
