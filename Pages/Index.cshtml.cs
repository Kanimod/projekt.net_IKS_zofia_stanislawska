using Microsoft.AspNetCore.Mvc.RazorPages;

using Fischt.Models;



// Pages/Index.cshtml.cs
public class IndexModel : PageModel
{
    private readonly FischtDbContext _context;

    public IndexModel(FischtDbContext context)
    {
        _context = context;
    }

    public List<User> Users { get; set; }

    public void OnGet()
    {
        Users = _context.Users.ToList();
    }

    public void OnPost(string username)
    {
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Username = username,
            Mail = "test@test.com",
            PasswordHash = "hash",
            Admin = false,
            Premium = false
        };

        _context.Users.Add(user);
        _context.SaveChanges();
        OnGet();
    }
}
