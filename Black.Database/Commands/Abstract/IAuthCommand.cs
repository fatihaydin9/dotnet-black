using Black.Domain;
using Black.Model.Authentication;

namespace Black.Database.Commands.Abstract;

public interface IAuthCommand
{
    Task Insert(AuthModel model);
    Task Update(AuthModel model);
    Task Delete(long id);
    Task Delete(Guid guidId);
    Task BulkInsert(List<AuthModel> modelLst);
    Task BulkUpdate(List<AuthModel> modelLst);
    Task BulkDelete(List<Guid> guidIdLst);
    Task BulkDelete(List<long> idLst);
}
