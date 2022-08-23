using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace ReenbitMessenger.DataAccess
{
    public class Chats
    {
        [Key]
        public int Id { get; set; }

        public string ChatName { get; set; }
        //public List<User> UserInChat { get; set; } = new List<User>();
        public IList<UsersChats> UsersChats { get; set; }
    }
}
