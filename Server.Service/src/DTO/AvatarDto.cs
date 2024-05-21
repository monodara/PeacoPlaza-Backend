using Server.Core.src.Entity;

namespace Server.Service.src.DTO
{
    public class AvatarReadDto
    {
        public byte[] Data { get; set; }
        public AvatarReadDto Transform(Avatar avatar)
        {
            Data = avatar.Data;
            return this;
        }
    }
}