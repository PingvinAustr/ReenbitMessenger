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
            //Refresh ID of current logged in user to 0 (log out)
            Response.Cookies.Append("Current_user_id", "0");
            Users =db.Users.ToList();

            
        }

        //Sending form that will redirect current user to main messenger page
        public IActionResult OnPostLogin(int id)

        {
            Users = db.Users.ToList();
            Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine("User has chosen to log in using USER_ID=["+id+"]");
            Console.ResetColor();
            string url = Url.Page("Messenger", new { id = id });
            return Redirect(url);
          
        }

    }
}
