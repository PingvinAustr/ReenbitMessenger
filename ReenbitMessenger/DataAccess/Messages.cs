using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace ReenbitMessenger.DataAccess
{
    public class Messages
    {
        [Key]
        public int Id { get; set; }
        public string MessageText { get; set; }
        public int User_Id{ get; set; }
        public int Chat_Id { get; set; }
        public Messages? ReplyToMessage { get; set; }
        public DateTime time_sent { get; set; }
        //DATE TIME
    }
}
