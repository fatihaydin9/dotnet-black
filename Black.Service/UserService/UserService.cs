using Black.Domain.Entity;
using Black.Infrastructure.UnitOfWorkBase.Abstract;
using Black.Model.User;
using Black.Service.AuthService;
using DotNetCore.Objects;
using DotNetCore.Results;
using DotNetCore.Validation;

namespace Black.Service.UserService;

public sealed class UserService : IUserService
{
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserFactory _userFactory;

    public UserService
    (
        IAuthService authService,
        IUnitOfWork unitOfWork,
        IUserFactory userFactory
    )
    {
        _authService = authService;
        _unitOfWork = unitOfWork;
        _userFactory = userFactory;
    }

    public async Task<IResult<long>> AddAsync(UserModel model)
    {
        var validation = new AddUserModelValidator().Validation(model);

        if (validation.Failed) return validation.Fail<long>();

        var auth = await _authService.AddAsync(model.Auth);

        if (auth.Failed) return auth.Fail<long>();

        var user = _userFactory.Create(model, auth.Data);

        await _unitOfWork.User.AddAsync(user);

        return user.Id.Success();
    }

    public async Task<IResult> DeleteAsync(long id)
    {
        var auth = await _unitOfWork.User.FindByIdAsync(id);

        await _unitOfWork.User.DeleteAsync(id);

        await _unitOfWork.Auth.DeleteAsync(auth.Auth.Id);

        return Result.Success();
    }

    public async Task<UserModel> GetAsync(long id)
    {
        var result = await _unitOfWork.User.GetModelAsync(id);
        return result;
    }

    public Task<Grid<UserModel>> GridAsync(GridParameters parameters)
    {
        return _unitOfWork.User.GridAsync(parameters);
    }

    public async Task<IResult> PassiveAsync(long id)
    {
        var user = new User(id);

        user.Passive();

        await _unitOfWork.User.UpdateStatusAsync(user);

        return Result.Success();
    }

    public async Task<IEnumerable<UserModel>> ListAsync()
    {
        return await _unitOfWork.User.ListModelAsync();
    }

    public async Task<IResult> UpdateAsync(UserModel model)
    {
        var validation = new UpdateUserModelValidator().Validation(model);

        if (validation.Failed) return validation;

        var user = await _unitOfWork.User.FindByIdAsync(model.Id);

        if (user is null) return Result.Success();

        user.Update(model.FirstName, model.LastName, model.Email);

        await _unitOfWork.User.UpdateAsync(user);

        return Result.Success();
    }
}
