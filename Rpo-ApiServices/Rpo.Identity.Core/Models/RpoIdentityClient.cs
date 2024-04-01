using Rpo.Identity.Core.Models.Enumerations;
using System.ComponentModel.DataAnnotations;

// TODO: Criar um diretório Poco
namespace Rpo.Identity.Core.Models
{
    public class RpoIdentityClient
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Secret { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public ApplicationTypes ApplicationType { get; set; }
        public bool Active { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        [MaxLength(100)]
        public string AllowedOrigin { get; set; }
    }
}
