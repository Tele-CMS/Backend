using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class ChatRoomUser : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]

        public int Id { get; set; }
        [ForeignKey("ChatRoom")]
        public int RoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }
        [ForeignKey("ChatUser")]
        public int UserId { get; set; }
        public User ChatUser { get; set; }
    }
}
