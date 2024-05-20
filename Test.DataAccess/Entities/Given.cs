using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.DataAccess.Entities
{
    public record class Given
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; init; }
        public string Name { get; init; }
        public virtual HashSet<Patient> Patients { get; set; } = new HashSet<Patient>();
    }
}
