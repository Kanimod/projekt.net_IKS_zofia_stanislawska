using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fischt.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ModerateModel : PageModel
    {
        public void OnGet() { }
    }
}