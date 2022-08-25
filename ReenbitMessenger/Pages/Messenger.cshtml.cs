using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReenbitMessenger.DataAccess;
using System.Web;
namespace ReenbitMessenger.Pages
{
    public class AJAX_Response
    {
        public int Id { get; set; }
        public string MessageText { get; set; }
        public DateTime time_sent { get; set; }
        public User User    { get; set; }
        public Chats Chats { get; set; }
        public Messages? Reply { get; set; }
    }

    
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
        
        //public Dictionary<int,string> UserAvatars { get; set; }
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
                    string new_chat_name = "";
                    List<UsersChats> users_of_current_chat = new List<UsersChats>();
                    List<User> users = new List<User>();
                    users_of_current_chat=UsersChats.Where(x=>x.ChatId == chat.ChatId).ToList();
                    users = users_of_current_chat.Select(x => x.User).ToList();
                    users=users.Where(x=>x.Id!=user_id).ToList();

                    new_chat_name = users[0].Name + " " + users[0].Surname;
                    
                    chat.Chat.ChatName = new_chat_name;
                    
                    DirectChatsOfThisUser.Add(chat.Chat);
                }
                else GroupChatsOfThisUser.Add(chat.Chat);
                
            }
        }

       
        User current_user = new User();

       
        public async Task<JsonResult> OnPostUpdateQRCodeAsync(int id, string name, string description)
        {
            return new JsonResult(description);
        }

        public async Task<JsonResult> OnPostOpenChat(int chat_id)
        {
            List<AJAX_Response> Response_list = new List<AJAX_Response>();

           List<Messages> messages_from_current_chat = new List<Messages>();
           messages_from_current_chat=db.Messages.Where(x=>x.Chat_Id==chat_id).ToList();
           



            foreach(var msg in messages_from_current_chat)
            {
                AJAX_Response response_item = new AJAX_Response();
                response_item.Id=msg.Id;
                response_item.MessageText = msg.MessageText;
                response_item.Reply = msg.ReplyToMessage;
                response_item.Chats = db.Chats.Where(x => x.Id == msg.Chat_Id).FirstOrDefault();
                response_item.User=db.Users.Where(x => x.Id==msg.User_Id).FirstOrDefault();
                response_item.time_sent = msg.time_sent;
                Response_list.Add(response_item);
                Console.WriteLine(response_item.User.Name+" "+response_item.MessageText);
            }

            Response_list=Response_list.OrderBy(x => x.time_sent).ToList();

            return new JsonResult(Response_list);
        }


        public void OnGet(int id)
            
        {
            Users = db.Users.ToList();
            Chats = db.Chats.ToList();
            Customers = db.Customers.ToList();
            Messages = db.Messages.ToList();
            
            current_user=Users.Where(x => x.Id == id).FirstOrDefault();
            Console.ForegroundColor= ConsoleColor.Green;
            Console.WriteLine("GET ON MAIN PAGE ["+id+"]");
            ViewData["current_user_avatar"] = current_user.UserAvatarImage;
            ViewData["current_user_name"] = current_user.Name+" "+current_user.Surname;
         
            Console.ResetColor();

            Response.Cookies.Append("Current_user_id", current_user.Id.ToString());


            GetGroupChatsOfUser(id);
            //GetAllAvatars();
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
