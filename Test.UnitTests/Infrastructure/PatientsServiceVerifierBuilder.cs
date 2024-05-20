using FizzWare.NBuilder;
using Microsoft.Extensions.Logging;
using Moq;
using Test.Core.Models;
using Test.Core.Services.Interfaces;
using Test.DataAccess.Entities;
using Test.DataAccess.Storages.Finders.Interfaces;
using Test.DataAccess.Storages.Interfaces;

namespace Test.UnitTests.Infrastructure
{
    public class PatientsServiceVerifierBuilder
    {
        private readonly Mock<ICacheService<string, PatientContext>> _cache;

        private List<PatientContext> _getAllPatients;
        private PatientContext _getPatient;

        public PatientsServiceVerifierBuilder()
        {
            _cache = new Mock<ICacheService<string, PatientContext>>();
        }

        public PatientsServiceVerifierBuilder SetPatientsServiceGetAllResult()
        {
            _getAllPatients = Builder<PatientContext>
                .CreateListOfSize(3)
                .Build()
                .Where(x => x.Active)
                .ToList();

            return this;
        }

        public PatientsServiceVerifierBuilder SetPatientsServiceGetByIdResult()
        {
            _getPatient = Builder<PatientContext>
                .CreateNew()
                .With(x => x.Active = true)
                .Build();

            return this;
        }

        public PatientsServiceTestsVerifier Build()
        {
            return new PatientsServiceTestsVerifier(
                _cache,
                _getAllPatients,
                _getPatient);
        }

        public PatientsServiceVerifierBuilder SetupPatientsServiceCacheWrapperGetList()
        {
            _cache.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<Func<Task<List<PatientContext>>>>()))
                .ReturnsAsync(_getAllPatients)
                .Verifiable();

            return this;
        }

        public PatientsServiceVerifierBuilder SetupPatientsServiceCacheWrapperGet()
        {
            _cache.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<Func<Task<PatientContext>>>()))
                .ReturnsAsync(_getPatient)
                .Verifiable();

            return this;
        }
    }
}
