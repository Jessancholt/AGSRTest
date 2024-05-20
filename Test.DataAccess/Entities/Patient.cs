using System.ComponentModel.DataAnnotations;
using Test.DataAccess.Entities.Predefined;

namespace Test.DataAccess.Entities
{
    public record class Patient
    {
        [Key]
        public Guid Id { get; set; }
        public string Use { get; set; }
        [Required]
        public string Family { get; set; }
        public Gender Gender { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public bool Active { get; set; }
        public virtual HashSet<Given> GivenNames { get; set; } = new HashSet<Given>();
    }
}
