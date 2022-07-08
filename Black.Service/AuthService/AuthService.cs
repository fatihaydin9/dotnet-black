using Black.Domain.Entity;
using Black.Infrastructure.UnitOfWorkBase.Abstract;
using Black.Model.Authentication;
using DotNetCore.Extensions;
using DotNetCore.Results;
using DotNetCore.Security;
using DotNetCore.Validation;
using System.Security.Claims;

namespace Black.Service.AuthService;

public sealed class AuthService : IAuthService
{
    private readonly IAuthFactory _authFactory;
    private readonly IHashService _hashService;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService
    (
        IAuthFactory authFactory,
        IUnitOfWork unitOfWork,
        IHashService hashService,
        IJwtService jwtService
    )
    {
        _authFactory = authFactory;
        _unitOfWork = unitOfWork;
        _hashService = hashService;
        _jwtService = jwtService;
    }


    public async Task<IResult<Auth>> AddAsync(AuthModel model)
    {
        var validation = new AuthModelValidator().Validation(model);

        if (validation.Failed) return validation.Fail<Auth>();

        if (await _unitOfWork.Auth.AnyAsync(i => i.Login == model.Login)) return Result<Auth>.Fail("Login exists!");

        var auth = _authFactory.Create(model);

        var password = _hashService.Create(auth.Password, auth.GuiddId);

        auth.UpdatePassword(password);

        await _unitOfWork.Auth.AddAsync(auth);

        return auth.Success();
    }

    public async Task DeleteAsync(long id)
    {
        await _unitOfWork.Auth.DeleteAsync(id);
    }

    public async Task<IResult<TokenModel>> SignInAsync(SignInModel model)
    {
        var failResult = Result<TokenModel>.Fail("Invalid login or password!");

        var validation = new SignInModelValidator().Validation(model);

        if (validation.Failed) return failResult;

        var auth = await _unitOfWork.Auth.GetOnlyAsync(i => i.Login == model.Login);

        if (auth is null) return failResult;

        var password = _hashService.Create(model.Password, auth.GuiddId);

        if (auth.Password != password) return failResult;

        return CreateToken(auth);
    }

    private IResult<TokenModel> CreateToken(Auth auth)
    {
        var claims = new List<Claim>();

        claims.AddSub(auth.Id.ToString());

        claims.AddRoles(auth.Roles.ToArray());

        var token = _jwtService.Encode(claims);

        return new TokenModel(token).Success();
    }
}