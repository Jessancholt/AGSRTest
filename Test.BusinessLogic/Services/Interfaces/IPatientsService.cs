using Test.Core.Models;

namespace Test.Core.Services.Interfaces
{
    public interface IPatientsService
    {
        Task<List<PatientContext>> GetAsync(CancellationToken cancellationToken);

        Task<PatientContext> GetAsync(Guid id, CancellationToken cancellationToken);

        Task<List<PatientContext>> GetByDateAsync(string date, CancellationToken cancellationToken);

        Task<PatientContext> CreateAsync(PatientCreateModel model, CancellationToken cancellationToken);

        Task CreateAsync(List<PatientCreateModel> models, CancellationToken cancellationToken);

        Task<PatientContext> UpdateAsync(Guid id, PatientEditModel model, CancellationToken cancellationToken);

        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
