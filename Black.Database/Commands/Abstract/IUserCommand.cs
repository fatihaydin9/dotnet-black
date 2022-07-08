using Black.Domain.Enumeration;
using Black.Model.User;

namespace Black.Database.Commands.Abstract;

public interface IUserCommand
{
    Task Insert(UserModel model);
    Task Update(UserModel model);
    Task Delete(long id);
    Task BulkInsert(List<UserModel> modelLst);
    Task BulkUpdate(List<UserModel> modelLst);
    Task BulkDelete(List<Guid> guidIdLst);
    Task BulkDelete(List<long> idLst);
    Task ChangeStatus(long id, Status status);
}
