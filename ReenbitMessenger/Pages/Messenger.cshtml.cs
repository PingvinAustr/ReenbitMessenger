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

        public async Task<JsonResult> OnPostOpenChat(int chat_id, int page_to_open)
        {
           List<AJAX_Response> Response_list = new List<AJAX_Response>();
           List<AJAX_Response> Response_list_paged = new List<AJAX_Response>();

           List<Messages> messages_from_current_chat = new List<Messages>();
           messages_from_current_chat=db.Messages.Where(x=>x.Chat_Id==chat_id).ToList();
            //Console.WriteLine(messages_from_current_chat.Count()+"!!!");

            int msg_from = 20 * page_to_open - 20;
            int msg_to = 20*page_to_open-1;

            if (page_to_open == -1)
            {
                int counter = 0;
                int len=messages_from_current_chat.Count-1;
               while (len - 20 >= 0)
                {
                    len -= 20;
                    counter++;
                }
                msg_from = counter * 20;
                msg_to = messages_from_current_chat.Count - 1;
            }

            Console.WriteLine(msg_from + " " + msg_to);

            //0-4
            //5-9
            //10-14
            int i = 0;
            foreach(var msg in messages_from_current_chat)
            {
               
                    AJAX_Response response_item = new AJAX_Response();
                    response_item.Id = msg.Id;
                    response_item.MessageText = msg.MessageText;
                    response_item.Reply = msg.ReplyToMessage;
                    response_item.Chats = db.Chats.Where(x => x.Id == msg.Chat_Id).FirstOrDefault();
                    response_item.User = db.Users.Where(x => x.Id == msg.User_Id).FirstOrDefault();
                    response_item.time_sent = msg.time_sent;
                    Response_list.Add(response_item);
                    Console.WriteLine(response_item.User.Name + " " + response_item.MessageText);
                
                
            }

            Response_list=Response_list.OrderBy(x => x.time_sent).ToList();

            foreach (var item in Response_list)
            {
                if (i>=msg_from && i <= msg_to)
                {
                    Response_list_paged.Add(item);
                }
                i++;
            }

            return new JsonResult(Response_list_paged);
        }

        public async Task<JsonResult> OnPostSendMessage(string msg,string author_id,string chat_id)
        {
            
            Messages new_msg=new Messages() { Chat_Id=int.Parse(chat_id), MessageText=msg, User_Id=int.Parse(author_id), time_sent=DateTime.Now};

            db.Messages.Add(new_msg);
            db.SaveChanges();
            //Console.WriteLine('{' + msg + '}' + author_id + chat_id+" - saved");


            return new JsonResult(null);
        }

        public async Task<JsonResult> OnPostEditMessage(string msg_id,string msg_new_text)
        {
            Messages edited_msg = db.Messages.Where(x => x.Id == int.Parse(msg_id)).FirstOrDefault();
            edited_msg.MessageText = msg_new_text;

            db.Messages.Remove(db.Messages.Where(x => x.Id == int.Parse(msg_id)).FirstOrDefault());
            db.SaveChanges();
            db.Messages.Add(edited_msg);
            db.SaveChanges();
            Console.WriteLine("Edited msg to " + msg_new_text);

            return new JsonResult(null);
        }


        public async Task<JsonResult> OnPostDeleteMessage(string msg_id)
        {
            db.Messages.Remove(db.Messages.Where(x=>x.Id==int.Parse(msg_id)).FirstOrDefault());
            db.SaveChanges();
            Console.WriteLine(msg_id + " deleted");

            return new JsonResult(null);
        }

        public async Task<JsonResult> OnPostReplyToMessage(string msg_id,string msg_text,string author_id)
        {
            Messages message_original = db.Messages.Where(x=>x.Id== int.Parse(msg_id)).FirstOrDefault(); 
            Messages message_reply=new Messages() { MessageText= msg_text, time_sent=DateTime.Now, User_Id=int.Parse(author_id), Chat_Id=message_original.Chat_Id, ReplyToMessage=message_original  };
            db.Messages.Add(message_reply);
            db.SaveChanges();
            return new JsonResult(null);
        }
         public async Task<JsonResult> OnPostPaginationFunc(int chat_id)
        {
            List<Messages> messages_from_current_chat = new List<Messages>();
            messages_from_current_chat = db.Messages.Where(x => x.Chat_Id == chat_id).ToList();
            KeyValuePair<string, string> map_ = new KeyValuePair<string, string>(key: "len", value: messages_from_current_chat.Count().ToString());





            return new JsonResult(messages_from_current_chat.Count());
        }
          public async Task<JsonResult> OnPostGetNumOfChatUsers(int chat_id)
        {
            string current_user_id = Request.Cookies["Current_user_id"];

            string opponent_image = "";           
            string opponent_name = "";           
            int users_num = 0;

            foreach (var item in db.UsersChats.ToList())
            {
                if (item.ChatId == chat_id)
                {
                    users_num++;
                }
            }

            if (users_num == 2)
            {
                foreach (var item in db.UsersChats.ToList())
                {
                    if (item.ChatId == chat_id && item.UserId != int.Parse(current_user_id))
                    {
                        
                       
                        User user = db.Users.Where(x=>x.Id==item.UserId).FirstOrDefault();
                        opponent_image = user.UserAvatarImage;
                        opponent_name= user.Name+" "+user.Surname;
                    }
                }
            }
            else
            {
                opponent_name = db.Chats.Where(x => x.Id == chat_id).FirstOrDefault().ChatName;
                opponent_image = "chat.png";
            }
            List<string> info = new List<string>();
            info.Add(users_num.ToString());
            info.Add(opponent_image.ToString());
            info.Add(opponent_name.ToString());
           // Console.WriteLine(users_num+"!"+ opponent_image+"!"+opponent_name);
            

            return new JsonResult(info);
        }

        public async Task<JsonResult> OnPostGetUserNameById(int user_id)
        {
            string name = db.Users.Where(x => x.Id == user_id).FirstOrDefault().Name + " " + db.Users.Where(x => x.Id == user_id).FirstOrDefault().Surname;
            return new JsonResult(name);
        }

        public async Task<JsonResult> OnPostSendDM(int opponent_id)
        {
            

            int existing_chat_to_return = 0;
            foreach (var item in db.UsersChats)
            {
                foreach (var item1 in db.UsersChats)
                {
                    if (item.ChatId==item1.ChatId)
                    {
                        if ((item.UserId==opponent_id && item1.UserId == int.Parse(Request.Cookies["Current_user_id"])) || (item1.UserId==opponent_id && item.UserId==int.Parse(Request.Cookies["Current_user_id"])))
                        {
                            int user_num = 0;
                            foreach (var uc in db.UsersChats)
                            {
                                if (uc.ChatId == item.ChatId) user_num++;

                            }

                            if (user_num == 2)
                            {
                                existing_chat_to_return = item.ChatId;
                            }

                        }
                    }

                    
                }
            }


            Console.WriteLine(existing_chat_to_return + "!!!!");
            if (existing_chat_to_return == 0)
            {
                Chats new_chat = new Chats() { ChatName = "DirectChat(" + opponent_id + "," + Request.Cookies["Current_user_id"] + ")" };
                UsersChats uc1 = new UsersChats() { UserId = opponent_id, ChatId = new_chat.Id, Chat = new_chat, User = db.Users.Where(x => x.Id == opponent_id).FirstOrDefault() };
                UsersChats uc2 = new UsersChats() { UserId = int.Parse(Request.Cookies["Current_user_id"]), ChatId = new_chat.Id, Chat = new_chat, User = db.Users.Where(x => x.Id == int.Parse(Request.Cookies["Current_user_id"])).FirstOrDefault() };
                db.Chats.Add(new_chat);
                db.UsersChats.Add(uc1);
                db.UsersChats.Add(uc2);
                db.SaveChanges();

                existing_chat_to_return = new_chat.Id;
                Console.WriteLine("TTTT" + db.Chats.Where(x => x.ChatName == new_chat.ChatName).FirstOrDefault().ChatName);
            }

            return new JsonResult(existing_chat_to_return);
        }


         public IActionResult OnPost()
        {
            Console.WriteLine("POST");
            return RedirectToPage("Messenger/?id=1");
        }

            public IActionResult OnGet(int id)
            
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
            string url = Url.Page("Messenger", new { id = id });
            return Page();
             
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
