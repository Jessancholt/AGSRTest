using System.Globalization;
using Test.Core.Models.Predefined;
using Test.DataAccess.Entities;

namespace Test.Core.Models
{
    public sealed class PatientContext
    {
        public NameContext Name { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public bool Active { get; set; }

        public PatientContext() { }

        public PatientContext(Patient patient)
        {
            ArgumentNullException.ThrowIfNull(patient);

            if (GenderConstants.GendersToStrings.TryGetValue(patient.Gender, out var gender))
            {
                Gender = gender;
            }

            Name = new NameContext(patient.Id, patient.Use, patient.Family, patient.GivenNames.ToList());
            BirthDate = patient.BirthDate.ToString("s", CultureInfo.InvariantCulture);
            Active = patient.Active;
        }
    }
}
