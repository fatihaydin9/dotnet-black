using AutoMapper;
using Black.Database.Base.RepositoryBase.Abstract;
using Black.Database.Queries.Abstract;
using Black.Infrastructure.Objects;
using Black.Infrastructure.UnitOfWorkBase.Abstract;
using Black.Model.User;
using DotNetCore.Objects;
using System.Linq.Expressions;

namespace Black.Database.Queries;

public class UserQuery : IUserQuery
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserQuery(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<UserModel>> FindAllAsync()
    {
        var result = await _unitOfWork.User.FindAllAsync();
        return _mapper.Map<List<UserModel>>(result);
    }

    public async Task<UserModel> FindByIdAsync(long id)
    {
        var result = await _unitOfWork.User.FindByIdAsync(id);
        return _mapper.Map<UserModel>(result);
    }

    public async Task<UserModel> FindByGuidIdAsync(Guid guidId)
    {
        var result = await _unitOfWork.User.FindByGuidIdAsync(guidId);
        return _mapper.Map<UserModel>(result);
    }

    public async Task<Grid<UserModel>> GridAsync(GridParameters parameters)
    {
        var findAll = await _unitOfWork.User.FindAllAsync();
        var result = findAll.GridAsync(parameters);
        return _mapper.Map<Grid<UserModel>>(result);
    }

    public async Task<long> GetAuthIdByUserIdAsync(long id)
    {
        var result = await _unitOfWork.User.FindByIdAsync(id);
        return result.Id;
    }

    public async Task<List<SelectBox>> GetSelectBoxList()
    {
        var result = await _unitOfWork.User.FindAllAsync();
        return _mapper.Map<List<SelectBox>>(result);
    }

}
