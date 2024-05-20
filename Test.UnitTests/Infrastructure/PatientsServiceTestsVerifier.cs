using Microsoft.Extensions.Logging;
using Moq;
using Test.Core.Models;
using Test.Core.Services;
using Test.Core.Services.Interfaces;
using Test.DataAccess.Entities;
using Test.DataAccess.Storages.Finders.Interfaces;
using Test.DataAccess.Storages.Interfaces;

namespace Test.UnitTests.Infrastructure
{
    public class PatientsServiceTestsVerifier
    {
        private readonly Mock<ICacheService<string, PatientContext>> _cache;
        private readonly Mock<IRepository<Patient>> _patientRepository;
        private readonly Mock<IRepository<Given>> _givenRepository;
        private readonly Mock<IFinder<Patient>> _patientFinder;
        private readonly Mock<IFinder<Given>> _givenFinder;
        private readonly Mock<ILogger<PatientsService>> _logger;
        private readonly Mock<IUnitOfWork> _uow;

        public List<PatientContext> GetAllPatients;
        public PatientContext GetPatient;

        internal readonly PatientsService Service;

        public PatientsServiceTestsVerifier(
            Mock<ICacheService<string, PatientContext>> cache,
            List<PatientContext> getAllPatients,
            PatientContext getPatient)
        {
            _cache = cache;
            _patientRepository = new Mock<IRepository<Patient>>();
            _givenRepository = new Mock<IRepository<Given>>();
            _patientFinder = new Mock<IFinder<Patient>>();
            _givenFinder = new Mock<IFinder<Given>>();
            _uow = new Mock<IUnitOfWork>();
            _logger = new Mock<ILogger<PatientsService>>();

            GetAllPatients = getAllPatients;
            GetPatient = getPatient;
            Service = new PatientsService(
                _cache.Object,
                _patientRepository.Object,
                _givenRepository.Object,
                _patientFinder.Object,
                _givenFinder.Object,
                _logger.Object,
                _uow.Object);
        }

        public PatientsServiceTestsVerifier VerifyPostsControllerCacheWrapperGetList()
        {
            _cache.Verify(x => x.Get(It.IsAny<string>(), It.IsAny<Func<Task<List<PatientContext>>>>()), Times.Once);

            return this;
        }

        public PatientsServiceTestsVerifier VerifyPostsControllerCacheWrapperGet()
        {
            _cache.Verify(x => x.Get(It.IsAny<string>(), It.IsAny<Func<Task<PatientContext>>>()), Times.Once);

            return this;
        }
    }
}
