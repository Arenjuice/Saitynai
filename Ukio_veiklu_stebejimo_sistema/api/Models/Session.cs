using api.Auth.Model;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Session
    {
        public Guid Id { get; set; }
        public string LastRefreshToken { get; set; }
        public DateTimeOffset InitiatedAt { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public bool IsReovoked { get; set; }
        [Required]
        public required string UserId { get; set; }
        public SystemUser User { get; set; }
    }
}
