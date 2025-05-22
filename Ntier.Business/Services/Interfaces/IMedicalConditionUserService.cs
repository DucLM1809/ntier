using Ntier.Shared.Dtos;
using Ntier.Shared.Models;

namespace Ntier.Business.Service;

public interface IMedicalConditionUserService
{
    Task<List<MedicalConditionUser>> GetFilteredMedicalConditionUsersAsync(
        QueryParameters queryParameters, CancellationToken cancellationToken);

    Task<MedicalConditionUser> GetMedicalConditionUserByIdAsync(int id, CancellationToken cancellationToken);

    Task<List<MedicalConditionUser>> AddMedicalConditionUserAsync(
        MedicalConditionUserDto medicalConditionUserDto, CancellationToken cancellationToken);

    Task DeleteMedicalConditionUserAsync(int id, CancellationToken cancellationToken);
}