using Black.Infrastructure.Objects;
using Black.Model.Authentication;
using DotNetCore.Objects;

namespace Black.Database.Queries.Abstract;

public interface IAuthQuery
{
    Task<List<AuthModel>> FindAllAsync();
    Task<AuthModel> FindByIdAsync(long id);
    Task<AuthModel> FindByGuidIdAsync(Guid guidId);
    Task<Grid<AuthModel>> GridAsync(GridParameters parameters);
    Task<bool> AnyByLoginAsync(string login);
    Task<List<SelectBox>> GetSelectBoxList();
}
