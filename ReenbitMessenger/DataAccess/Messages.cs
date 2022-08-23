using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace ReenbitMessenger.DataAccess
{
    public class Messages
    {
        [Key]
        public int Id { get; set; }
        public string MessageText { get; set; }
        public User User { get; set; }
        public Chats Chat { get; set; }
        public Messages? ReplyToMessage { get; set; }
    }
}
