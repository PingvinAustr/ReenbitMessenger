using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReenbitMessenger.DataAccess;

namespace ReenbitMessenger.Pages
{
    public class IndexModel : PageModel
    {

        public readonly AppDbContext db;

        public List<Customer> Customers { get; set; }
        public List<User> Users { get; set; }
        public List<Chats> Chats { get; set; }
        public List<Messages> Messages { get; set; }

        public IndexModel(AppDbContext db)
        {
            this.db = db;
        }
        public void OnGet()
        {
            Users = db.Users.ToList();
            Chats = db.Chats.ToList();

            Customers = db.Customers.ToList();
            Messages = db.Messages.ToList();

            foreach (var user in Users)
            {
                Console.WriteLine("User - " + user.Name);
                var messages_of_user = Messages.Where(x => x.User == user).ToList();

                if (messages_of_user.Count > 0)
                {
                    Console.WriteLine("has written messages");
                }

            }



        }

        public void OnPostUpdate()
        {
            Users = db.Users.ToList();
            Chats = db.Chats.ToList();
            Customers = db.Customers.ToList();
            /*foreach (var item in Customers)
            {
                item.CustomerName += " Changed";
            }*/
            db.SaveChanges();
        }
    }
}
