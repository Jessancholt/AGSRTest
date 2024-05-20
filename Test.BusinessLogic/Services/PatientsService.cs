using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Test.Core.Exceptions;
using Test.Core.Extensions;
using Test.Core.Models;
using Test.Core.Models.Predefined;
using Test.Core.Services.Interfaces;
using Test.DataAccess.Entities;
using Test.DataAccess.Storages.Finders.Interfaces;
using Test.DataAccess.Storages.Interfaces;

namespace Test.Core.Services
{
    internal sealed class PatientsService : IPatientsService
    {
        private readonly ICacheService<string, PatientContext> _cache;
        private readonly IRepository<Patient> _patientRepository;
        private readonly IRepository<Given> _givenRepository;
        private readonly IFinder<Patient> _patientFinder;
        private readonly IFinder<Given> _givenFinder;
        private readonly ILogger<PatientsService> _logger;
        private readonly IUnitOfWork _uow;

        public PatientsService(
            ICacheService<string, PatientContext> cacheService,
            IRepository<Patient> repository,
            IRepository<Given> givenRepository,
            IFinder<Patient> patientFinder,
            IFinder<Given> givenFinder,
            ILogger<PatientsService> logger,
            IUnitOfWork unitOfWork)
        {
            _cache = cacheService;
            _patientRepository = repository;
            _givenRepository = givenRepository;
            _patientFinder = patientFinder;
            _givenFinder = givenFinder;
            _logger = logger;
            _uow = unitOfWork;
        }

        public async Task<List<PatientContext>> GetAsync(CancellationToken cancellationToken)
        {
            try
            {
                var patients = await _cache.Get(PatientsCacheConstants.GET_ID, async () =>
                {
                    var patients = await _patientFinder.GetAsync(x => x.Active, cancellationToken);
                    return patients.Select(x => new PatientContext(x)).ToList();
                });

                return patients.Where(x => x.Active).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new CoreException("Failed to get list of patients", ex);
            }
        }

        public async Task<PatientContext> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var context = await _cache.Get(PatientsCacheConstants.GET_ID + id, async () =>
                {
                    var patient = await _patientFinder.FirstOrDefaultAsync(x => x.Id == id && x.Active, cancellationToken);
                    return new PatientContext(patient);
                });

                if (context is not null && context.Active)
                {
                    return context;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new CoreException("Failed to get list of patients", ex);
            }

            throw new NotFoundCoreException();
        }

        public async Task<List<PatientContext>> GetByDateAsync(string dateStr, CancellationToken cancellationToken)
        {
            try
            {
                if (dateStr.TryDateParse(out var parsed))
                {
                    var date = parsed.Value.Item2;
                    var prefix = parsed.Value.Item1;

                    var patients = await _patientFinder.GetAsync(Compare(date, prefix), cancellationToken);
                    return patients.Select(x => new PatientContext(x)).ToList();
                }

                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new CoreException("Failed to get list of patients by date", ex);
            }
        }

        public async Task<PatientContext> CreateAsync(PatientCreateModel model, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(model);

            try
            {
                var given = await GetGivenInner(model.Given, cancellationToken);

                var entity = GetForCreate(model, given);

                var sResult = await _patientRepository.CreateAsync(entity, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);

                return new PatientContext(sResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new CoreException("Failed to create patient", ex);
            }
        }

        public async Task CreateAsync(List<PatientCreateModel> models, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(models);

            try
            {
                var entities = new List<Patient>();
                var givenUnique = new Dictionary<string, Given>();
                foreach (var model in models)
                {
                    var given = await GetGivenInner(model.Given, cancellationToken);
                    entities.Add(GetForCreate(model, given));
                }

                await _patientRepository.CreateAsync(entities, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new CoreException("Failed to create list of patients", ex);
            }
        }

        public async Task<PatientContext> UpdateAsync(Guid id, PatientEditModel model, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(model);

            try
            {
                var patient = await _patientFinder.GetAsync(id, cancellationToken);
                if (patient is not null && patient.Active)
                {
                    var given = await GetGivenInner(model.Given, cancellationToken);

                    var entity = GetForUpdate(patient, model, given);

                    var uResult = _patientRepository.Update(entity);
                    await _uow.SaveChangesAsync(cancellationToken);

                    return new PatientContext(uResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new CoreException("Failed to update patient", ex);
            }

            throw new NotFoundCoreException();
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var patient = await _patientFinder.GetAsync(id, cancellationToken);
                if (patient is not null && patient.Active)
                {
                    patient.Active = false;

                    var dResult = _patientRepository.Update(patient);
                    await _uow.SaveChangesAsync(cancellationToken);

                    return dResult != null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new CoreException("Failed to delete patient", ex);
            }

            throw new NotFoundCoreException();
        }

        private async Task<List<Given>> GetGivenInner(List<string> given, CancellationToken cancellationToken)
        {
            var givenEntities = await _givenFinder.GetAsync(x => given.Contains(x.Name), cancellationToken);
            if (givenEntities is null || givenEntities.Count == 0)
            {
                var entities = given.Select(given => new Given()
                {
                    Name = given,
                }).ToList();
                await _givenRepository.CreateAsync(entities, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);
                givenEntities = await _givenFinder.GetAsync(x => given.Contains(x.Name), cancellationToken);
            }

            return givenEntities;
        }

        private Patient GetForCreate(PatientCreateModel model, List<Given> given)
        {
            var entity = new Patient();
            entity.Id = Guid.NewGuid();
            entity.Active = true;

            if (DateTime.TryParse(model.BirthDate, out var birthDate))
            {
                entity.BirthDate = birthDate;
            }

            if (GenderConstants.StringToGenders.TryGetValue(model.Gender.ToLower(), out var gender))
            {
                entity.Gender = gender;
            }

            entity.GivenNames = given.ToHashSet();
            entity.Use = model.Use;
            entity.Family = model.Family;
            return entity;
        }

        private Patient GetForUpdate(Patient patient, PatientEditModel model, List<Given> given)
        {
            var entity = new Patient();
            entity.Id = patient.Id;
            entity.Active = patient.Active;

            if (DateTime.TryParse(model.BirthDate, out var birthDate))
            {
                entity.BirthDate = birthDate;
            }

            if (GenderConstants.StringToGenders.TryGetValue(model.Gender.ToLower(), out var gender))
            {
                entity.Gender = gender;
            }

            entity.GivenNames = given.ToHashSet();
            entity.Use = model.Use;
            entity.Family = model.Family;
            return entity;
        }

        private Expression<Func<Patient, bool>> Compare(DateTime date, string prefix)
        {
            Expression<Func<Patient, bool>> result;
            switch (prefix)
            {
                case "eq":
                    result = (entity) => entity.Active && date.StartOfDay() <= entity.BirthDate && date.EndOfDay() >= entity.BirthDate;
                    break;
                case "ne":
                    result = (entity) => entity.Active && date != entity.BirthDate;
                    break;
                case "gt":
                    result = (entity) => entity.Active && date < entity.BirthDate;
                    break;
                case "lt":
                    result = (entity) => entity.Active && date > entity.BirthDate;
                    break;
                case "ge":
                    result = (entity) => entity.Active && date >= entity.BirthDate;
                    break;
                case "le":
                    result = (entity) => entity.Active && date <= entity.BirthDate;
                    break;
                case "sa":
                    result = (entity) => entity.Active && date.EndOfDay() > entity.BirthDate;
                    break;
                case "eb":
                    result = (entity) => entity.Active && date.StartOfDay() < entity.BirthDate;
                    break;
                case "ap":
                    result = (entity) => entity.Active && date == entity.BirthDate;
                    break;
                default:
                    result = (entity) => false;
                    break;
            }

            return result;
        }
    }
}
