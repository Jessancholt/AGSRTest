namespace Test.Core.Models
{
    public sealed class PatientCreateModel
    {
        public string Use { get; set; }
        public string Family { get; set; }
        public List<string> Given { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
    }
}
