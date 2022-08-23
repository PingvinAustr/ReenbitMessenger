using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReenbitMessenger.DataAccess
{
    public class UsersChats
    {
       public int UserId { get; set; }
        public User User { get; set; }

        public int ChatId { get; set; }
        public Chats Chat { get; set; }
      

    }
}
