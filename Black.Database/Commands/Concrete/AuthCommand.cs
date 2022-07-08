using AutoMapper;
using Black.Database.Commands.Abstract;
using Black.Domain;
using Black.Domain.Entity;
using Black.Infrastructure.UnitOfWorkBase.Abstract;
using Black.Model.Authentication;

namespace Black.Database.Commands.Concrete;

public class AuthCommand : IAuthCommand
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AuthCommand(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Insert(AuthModel model)
    {
        var data = _mapper.Map<Auth>(model);
        await _unitOfWork.Auth.AddAsync(data);
        await _unitOfWork.SaveAsync();
    }

    public async Task Update(AuthModel model)
    {
        var data = _mapper.Map<Auth>(model);
        await _unitOfWork.Auth.UpdateAsync(data);
        await _unitOfWork.SaveAsync();
    }

    public async Task Delete(long id)
    {
        await _unitOfWork.Auth.DeleteAsync(id);
        await _unitOfWork.SaveAsync();
    }

    public async Task Delete(Guid guidId)
    {
        await _unitOfWork.Auth.DeleteAsync(guidId);
        await _unitOfWork.SaveAsync();
    }

    public async Task BulkInsert(List<AuthModel> modelLst)
    {
        var dataLst = _mapper.Map<List<Auth>>(modelLst);
        await _unitOfWork.Auth.AddRangeAsync(dataLst);
        await _unitOfWork.SaveAsync();
    }

    public async Task BulkUpdate(List<AuthModel> modelLst)
    {
        var dataLst = _mapper.Map<List<Auth>>(modelLst);
        _unitOfWork.Auth.UpdateRange(dataLst);
        await _unitOfWork.SaveAsync();
    }

    public async Task BulkDelete(List<Guid> guidIdLst)
    {
        List<Auth> dataLst = new List<Auth>();
        foreach (var guidId in guidIdLst)
        {
            var data = await _unitOfWork.Auth.FindByGuidIdAsync(guidId);
            dataLst.Add(data);
        }

        _unitOfWork.Auth.DeleteRange(dataLst);
        await _unitOfWork.SaveAsync();
    }

    public async Task BulkDelete(List<long> idLst)
    {
        List<Auth> dataLst = new List<Auth>();
        foreach (var id in idLst)
        {
            var data = await _unitOfWork.Auth.FindByIdAsync(id);
            dataLst.Add(data);
        }

        _unitOfWork.Auth.DeleteRange(dataLst);
        await _unitOfWork.SaveAsync();
    }
}
