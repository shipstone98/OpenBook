namespace Shipstone.OpenBook.Models
{
    public class Following
    {
        public virtual User Followee { get; set; }
        public int FolloweeId { get; set; }
        public virtual User Follower { get; set; }
        public int FollowerId { get; set; }
        public int Id { get; set; }
    }
}
