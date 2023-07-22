using API.Controllers;
using API.Helper.DTOS;
using API.Helper.Services;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit.Abstractions;

namespace API.Test.Controllers;
[Trait("Controller","Account")]
public class AccountControllerTests
{
    private readonly ITestOutputHelper _outputHelper;

    private TokenService _tokenService { get; }

    public AccountControllerTests(ITestOutputHelper outputHelper)
    {
        var tokenValue = "fake scret key value";
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x["TokenKey"]).Returns(tokenValue);

        _tokenService = new TokenService(configurationMock.Object);
        _outputHelper = outputHelper;
    }
    [Fact]
    public async Task Login_Success()
    {
        var userManagerMock = GetMockUserManager();
        var signInManagerMock = GetMockSignInManager(true);
        var accountController = new AccountController(userManagerMock.Object, signInManagerMock.Object, _tokenService);

        var loginDto = new LoginDto
        {
            Email = "admin@site.com",
            Password = "P@ssw0rd"
        };

        var result = await accountController.Login(loginDto);

        Assert.IsType<ActionResult<UserDto>>(result);
        Assert.NotNull(result.Value);
        Assert.Equal("admin@site.com", result.Value.Email);
    }

    [Fact]
    public async Task Login_Failure_InvalidPassword()
    {
        var userManagerMock = GetMockUserManager();
        var signInManagerMock = GetMockSignInManager(false);
        var accountController = new AccountController(userManagerMock.Object, signInManagerMock.Object, _tokenService);

        var loginDto = new LoginDto
        {
            Email = "admin@site.com",
            Password = "WrongPassword"
        };

        var result = await accountController.Login(loginDto);

        Assert.IsType<UnauthorizedResult>(result.Result);
    }

    [Fact]
    public async Task Register_Success()
    {
        var userManagerMock = GetMockUserManager();
        userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                       .ReturnsAsync(IdentityResult.Success);

        var accountController = new AccountController(userManagerMock.Object, GetMockSignInManager(true).Object, _tokenService);

        var registerDto = new RegisterDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "test@example.com",
            Password = "Test123!",
            PhoneNumber = "1234567890"
        };

        var result = await accountController.Register(registerDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var userDto = Assert.IsType<UserDto>(okResult.Value);
        Assert.Equal("John Doe", userDto.FullName);
        Assert.Equal("test@example.com", userDto.Email);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Register_Failure_DuplicateEmail()
    {
        var userManagerMock = GetMockUserManager();
        userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                       .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "DuplicateEmail" }));

        var accountController = new AccountController(userManagerMock.Object, GetMockSignInManager(true).Object, _tokenService);

        var registerDto = new RegisterDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "admin@site.com",
            Password = "Test123!",
            PhoneNumber = "1234567890"
        };

        var result = await accountController.Register(registerDto);

        var badRequestResult = Assert.IsAssignableFrom<ObjectResult>(result);
        var modelState = Assert.IsAssignableFrom<ValidationProblemDetails>(badRequestResult.Value);
        Assert.True(modelState.Errors.ContainsKey("email"));
        var errorMessage = Assert.Single((string[])modelState.Errors["email"]);
        Assert.Equal("Email taken", errorMessage);
    }

    private Mock<UserManager<User>> GetMockUserManager()
    {
        var users = new List<User>
    {
        new User
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            UserName = "admin@site.com",
            Email = "admin@site.com"
        }
    }.AsQueryable();

        var store = new Mock<IUserStore<User>>();

        var userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        userManagerMock.Setup(x => x.Users).Returns(users);
        userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(It.IsAny<User>());
        userManagerMock.Object.UserValidators.Add(new UserValidator<User>());
        userManagerMock.Object.PasswordValidators.Add(new PasswordValidator<User>());

        return userManagerMock;
    }

    private Mock<SignInManager<User>> GetMockSignInManager(bool signInResult)
    {
        var userManagerMock = GetMockUserManager();
        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        var userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<User>>();
        var signInManagerMock = new Mock<SignInManager<User>>(userManagerMock.Object, contextAccessorMock.Object, userClaimsPrincipalFactoryMock.Object, null, null, null, null);

        signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()))
                         .ReturnsAsync(signInResult ? Microsoft.AspNetCore.Identity.SignInResult.Success : Microsoft.AspNetCore.Identity.SignInResult.Failed);

        return signInManagerMock;
    }

}
