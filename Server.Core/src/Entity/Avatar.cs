namespace Server.Core.src.Entity
{
    public class Avatar : BaseEntity
    {
        public byte[] Data { get; set; }
        public Guid UserId { get; set; }
        public User User{ get; set; }
    }
}