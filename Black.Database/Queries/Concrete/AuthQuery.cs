using AutoMapper;
using Black.Database.Queries.Abstract;
using Black.Infrastructure.Objects;
using Black.Infrastructure.UnitOfWorkBase.Abstract;
using Black.Model.Authentication;
using DotNetCore.Objects;

namespace Black.Database.Queries;

public class AuthQuery : IAuthQuery
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AuthQuery(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<AuthModel>> FindAllAsync()
    {
        var result = await _unitOfWork.Auth.FindAllAsync();
        return _mapper.Map<List<AuthModel>>(result);
    }

    public async Task<AuthModel> FindByIdAsync(long id)
    {
        var result = await _unitOfWork.Auth.FindByIdAsync(id);
        return _mapper.Map<AuthModel>(result);
    }

    public async Task<AuthModel> FindByGuidIdAsync(Guid guidId)
    {
        var result = await _unitOfWork.Auth.FindByGuidIdAsync(guidId);
        return _mapper.Map<AuthModel>(result);
    }

    public async Task<Grid<AuthModel>> GridAsync(GridParameters parameters)
    {
        var findAll = await _unitOfWork.Auth.FindAllAsync();
        var result = findAll.GridAsync(parameters);
        return _mapper.Map<Grid<AuthModel>>(result);
    }

    public async Task<bool> AnyByLoginAsync(string login)
    {
        return await _unitOfWork.Auth.AnyAsync(i => i.Login == login);
    }

    public async Task<List<SelectBox>> GetSelectBoxList()
    {
        var result = await _unitOfWork.Auth.FindAllAsync();
        return  _mapper.Map<List<SelectBox>>(result);
    }
}
