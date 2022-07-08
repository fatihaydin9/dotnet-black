using AutoMapper;
using Black.Database.Commands.Abstract;
using Black.Domain.Entity;
using Black.Domain.Enumeration;
using Black.Infrastructure.UnitOfWorkBase.Abstract;
using Black.Model.User;

namespace Black.Database.Commands.Concrete;

public class UserCommand : IUserCommand
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserCommand(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Insert(UserModel model)
    {
        var data = _mapper.Map<User>(model);
        await _unitOfWork.User.AddAsync(data);
        await _unitOfWork.SaveAsync();
    }

    public async Task Update(UserModel model)
    {
        var data = _mapper.Map<User>(model);
        await _unitOfWork.User.UpdateAsync(data);
        await _unitOfWork.SaveAsync();
    }

    public async Task Delete(long id)
    {
        await _unitOfWork.User.DeleteAsync(id);
        await _unitOfWork.SaveAsync();
    }

    public async Task BulkInsert(List<UserModel> modelLst)
    {
        var dataLst = _mapper.Map<List<User>>(modelLst);
        await _unitOfWork.User.AddRangeAsync(dataLst);
        await _unitOfWork.SaveAsync();
    }

    public async Task BulkUpdate(List<UserModel> modelLst)
    {
        var data = _mapper.Map<List<User>>(modelLst);
        _unitOfWork.User.UpdateRange(data);
        await _unitOfWork.SaveAsync();
    }

    public async Task BulkDelete(List<Guid> guidIdLst)
    {
        List<User> dataLst = new List<User>();
        foreach (var guidId in guidIdLst)
        {
            var data = await _unitOfWork.User.FindByGuidIdAsync(guidId);
            dataLst.Add(data);
        }

        _unitOfWork.User.DeleteRange(dataLst);
        await _unitOfWork.SaveAsync();
    }

    public async Task BulkDelete(List<long> idLst)
    {
        List<User> dataLst = new List<User>();
        foreach (var id in idLst)
        {
            var data = await _unitOfWork.User.FindByIdAsync(id);
            dataLst.Add(data);
        }

        _unitOfWork.User.DeleteRange(dataLst);
        await _unitOfWork.SaveAsync();
    }

    public async Task ChangeStatus(long id, Status status)
    {
        var data = await _unitOfWork.User.FindByIdAsync(id);
        data.Status = status;
        await _unitOfWork.User.UpdateAsync(data);
        await _unitOfWork.SaveAsync();
    }
}
