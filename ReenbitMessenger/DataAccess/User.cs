using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReenbitMessenger.DataAccess
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserAvatarImage { get; set; }
        public IList<UsersChats> UsersChats { get; set; }
    }
}
