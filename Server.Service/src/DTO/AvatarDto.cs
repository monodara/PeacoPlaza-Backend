using Server.Core.src.Entity;

namespace Server.Service.src.DTO
{
    public class AvatarReadDto
    {
        public Guid Id;
        public string Data { get; set; }
        public Guid UserId;
        public AvatarReadDto Transform(Avatar avatar)
        {
            Id = avatar.Id;
            Data = "data:image/jpeg;base64," + Convert.ToBase64String(avatar.Data);
            UserId = avatar.UserId;
            return this;
        }
    }
}