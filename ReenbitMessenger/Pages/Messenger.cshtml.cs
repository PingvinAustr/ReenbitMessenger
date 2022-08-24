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
        public List<UsersChats> UsersChats { get; set; }

        public List<Chats> GroupChatsOfThisUser = new List<Chats>();
        public List<Chats> DirectChatsOfThisUser = new List<Chats>();

        public IndexModel(AppDbContext db)
        {
            this.db = db;
        }

        public void GetGroupChatsOfUser(int user_id)
        {
            Users = db.Users.ToList();
            Chats = db.Chats.ToList();
            UsersChats=db.UsersChats.ToList();
            List<UsersChats> AllChatsOfThisUser = db.UsersChats.Where(x => x.UserId == user_id).ToList();

           

            foreach (var chat in AllChatsOfThisUser)
            {
                int num_of_users_in_current_chat=UsersChats.Where(x=>x.ChatId == chat.ChatId).Count();                
                if (num_of_users_in_current_chat == 2)
                {
                    DirectChatsOfThisUser.Add(chat.Chat);
                }
                else GroupChatsOfThisUser.Add(chat.Chat);
                
            }
        }

        public void OnGet(int id)
            
        {
            Users = db.Users.ToList();
            Chats = db.Chats.ToList();
            Customers = db.Customers.ToList();
            Messages = db.Messages.ToList();

            User current_user=Users.Where(x => x.Id == id).FirstOrDefault();
            Console.ForegroundColor= ConsoleColor.Green;
            Console.WriteLine("GET ON MAIN PAGE ["+id+"]");
            ViewData["current_user_avatar"] = current_user.UserAvatarImage;
            ViewData["current_user_name"] = current_user.Name+" "+current_user.Surname;
            Console.ResetColor();

            GetGroupChatsOfUser(id);

            /*
            foreach (var user in Users)
            {
                Console.WriteLine("User - " + user.Name);
                var messages_of_user = Messages.Where(x => x.User == user).ToList();

                if (messages_of_user.Count > 0)
                {
                    Console.WriteLine("has written messages" + messages_of_user.Count());
                }

            }
            */


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
