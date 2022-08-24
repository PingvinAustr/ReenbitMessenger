using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReenbitMessenger.DataAccess;

namespace ReenbitMessenger.Pages
{
    public class ChooseUserModel : PageModel
    {
        public ChooseUserModel(AppDbContext db)
        {
            this.db = db;
        }

        public readonly AppDbContext db;

      
        public List<User> Users { get; set; }
       
        public void OnGet()
        {
            Users=db.Users.ToList();

            
        }

        public IActionResult OnPostLogin(int id)

        {
            Users = db.Users.ToList();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("User has chosen to log in using USER_ID=["+id+"]");
            Console.ResetColor();
            string url = Url.Page("Messenger", new { id = id });
            return Redirect(url);
            //return RedirectToPage("Messenger",null,id);
          
        }

    }
}
