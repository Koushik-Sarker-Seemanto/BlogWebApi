using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using ContractsService;
using ContractsService.v1.UserContracts.Requests;
using ModelsService.Managers.UserManager;
using ModelsService.Models;
using Moq;
using Xunit;

namespace AuthenticationService.UnitTest
{
    [ExcludeFromCodeCoverage]
    public class UserAuthenticatorTest
    {
        private readonly IUserAuthenticator _authenticator;
        public UserAuthenticatorTest()
        {
            _authenticator = new UserAuthenticator(GetMockUserManager().Object);
        }

        [Fact]
        public async Task LoginUserTestMustLogin()
        {
            LoginRequest request = new LoginRequest()
            {
                Email = "testpass@gmail.com",
                Password = "abcd",
            };
            var result = await _authenticator.LoginUser(request);
            
            Assert.NotEqual(result, StatusCode.NotFound.ToString());
            Assert.NotEqual(result, StatusCode.InvalidArgument.ToString());
        }
        
        [Fact]
        public async Task LoginUserUserTestUserNotFound()
        {
            LoginRequest request = new LoginRequest()
            {
                Email = "abcd@gmail.com",
                Password = "abcd",
            };
            var result = await _authenticator.LoginUser(request);
            
            Assert.Equal(result, StatusCode.NotFound.ToString());
        }
        
        [Fact]
        public async Task LoginUserTestInvalidArgument()
        {
            LoginRequest request = new LoginRequest();
            var result = await _authenticator.LoginUser(request);
            
            Assert.Equal(result, StatusCode.InvalidArgument.ToString());
        }
        
        [Fact]
        public async Task LoginUserTestInvalidPassword()
        {
            LoginRequest request = new LoginRequest()
            {
                Email = "testpass@gmail.com",
                Password = "qwe",
            };
            var result = await _authenticator.LoginUser(request);
            
            Assert.Equal(result, StatusCode.InvalidArgument+" password");
        }

        [Fact]
        public async Task RegisterUserTestMustRegister()
        {
            RegisterRequest request = new RegisterRequest()
            {
                Name = "test user",
                Email = "testreg@gmail.com",
                Password = "abcd",
            };
            var result = await _authenticator.RegisterUser(request);
            
            Assert.NotEqual(result, StatusCode.Internal.ToString());
            Assert.NotEqual(result, StatusCode.InvalidArgument.ToString());
            Assert.NotEqual(result, StatusCode.AlreadyExists.ToString());
        }
        
        [Fact]
        public async Task RegisterUserTestUserAlreadyExist()
        {
            RegisterRequest request = new RegisterRequest()
            {
                Name = "test user",
                Email = "testpass@gmail.com",
                Password = "abcd",
            };
            var result = await _authenticator.RegisterUser(request);
            
            Assert.Equal(result, StatusCode.AlreadyExists.ToString());
        }
        
        [Fact]
        public async Task RegisterUserTestInvalidArgument()
        {
            RegisterRequest request = new RegisterRequest()
            {
                Email = "test@gmail.com",
                Password = "abcd",
            };
            var result = await _authenticator.RegisterUser(request);
            
            Assert.Equal(result, StatusCode.InvalidArgument.ToString());
        }
        
        [Fact]
        public async Task RegisterUserTestCantInsertUser()
        {
            RegisterRequest request = new RegisterRequest()
            {
                Name = "test user",
                Email = "test@gmail.com",
                Password = "abcd",
            };
            var result = await _authenticator.RegisterUser(request);
            
            Assert.Equal(result, StatusCode.Internal.ToString());
        }
        
        [Fact]
        public async Task ReturnProfileTestMustReturn()
        {
            string id = "1";
            var result = await _authenticator.ReturnProfile(id);
            Assert.Equal("test user",result.Name);
            Assert.Equal("test", result.Email);
        }
        
        [Fact]
        public async Task ReturnProfileTestNullReturn()
        {
            string id = "2";
            var result = await _authenticator.ReturnProfile(id);
            Assert.Null(result);
        }
        
        private Mock<IUserManager> GetMockUserManager()
        {
            Mock<IUserManager> mock = new Mock<IUserManager>();
            mock.Setup(e => e.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync((string target) =>
                {
                    User user = null;
                    if (target == "testpass@gmail.com")
                    {
                        user = new User()
                        {
                            Name = "test user",
                            Email = "testpass@gmail.com",
                            Password = UserExtension.GetEncryptedPassword("abcd"),
                        };
                    }
                    return user;
                });

            mock.Setup(e => e.InsertUser(It.IsAny<User>()))
                .ReturnsAsync((User target) =>
                {
                    if (target.Email == "testreg@gmail.com")
                    {
                        return true;
                    }
                    return false;
                });

            mock.Setup(e => e.GetUser(It.IsAny<string>()))
                .ReturnsAsync((string target) =>
                {
                    if (target == "1")
                    {
                        return new User()
                        {
                            Id = "1",
                            Email = "test",
                            Name = "test user",
                        };
                    }

                    return null;
                });
            
            return mock;
        }
    }
}