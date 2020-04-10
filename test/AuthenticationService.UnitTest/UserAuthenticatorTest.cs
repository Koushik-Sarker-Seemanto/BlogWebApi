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
            
            Assert.Equal(StatusCode.Ok, result.StatusCode);
            Assert.NotNull(result.Token);
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
            
            Assert.Equal(StatusCode.NotFound, result.StatusCode);
        }
        
        [Fact]
        public async Task LoginUserTestInvalidArgument()
        {
            LoginRequest request = new LoginRequest();
            var result = await _authenticator.LoginUser(request);
            
            Assert.Equal(StatusCode.InvalidArgument, result.StatusCode);
            Assert.Equal("Empty argument provided", result.ErrorMessage);
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
            
            Assert.Equal(StatusCode.InvalidArgument, result.StatusCode);
            Assert.Equal("Wrong password", result.ErrorMessage);
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
            
            Assert.Equal(StatusCode.Ok, result.StatusCode);
            Assert.NotNull(result.Token);
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
            
            Assert.Equal(StatusCode.AlreadyExists, result.StatusCode);
            Assert.Equal("Email already exist", result.ErrorMessage);
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
            
            Assert.Equal(StatusCode.InvalidArgument, result.StatusCode);
            Assert.Equal("Empty argument provided", result.ErrorMessage);
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
            
            Assert.Equal(StatusCode.Internal, result.StatusCode);
            Assert.Equal("Internal error! Couldn't insert user", result.ErrorMessage);
        }

        [Fact]
        public async Task ReturnProfileTestMustReturn()
        {
            string id = "1";
            var result = await _authenticator.ReturnProfile(id);
            Assert.Equal("test user",result.User.Name);
            Assert.Equal("test", result.User.Email);
        }
        
        [Fact]
        public async Task ReturnProfileTestNullReturn()
        {
            string id = "2";
            var result = await _authenticator.ReturnProfile(id);
            Assert.Equal(StatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task UpdateUserTestMustUpdate()
        {
            string id = "1";
            UpdateUserRequest request = new UpdateUserRequest()
            {
                Name = "string",
                Email = "test",
                Password = "qwer",
            };

            var result = await _authenticator.UpdateUser(request, id);
            
            Assert.Equal(StatusCode.Ok,result.StatusCode);
            Assert.Equal("string", result.Name);
            Assert.Equal("test", result.Email);
        }
        
        [Fact]
        public async Task UpdateUserTestUnauthenticatedUser()
        {
            string id = "2";
            UpdateUserRequest request = new UpdateUserRequest()
            {
                Name = "string",
                Email = "test",
                Password = "qwer",
            };

            var result = await _authenticator.UpdateUser(request, id);
            
            Assert.Equal(StatusCode.Unauthenticated,result.StatusCode);
            Assert.Equal("Unauthenticated user", result.ErrorMessage);
        }
        
        [Fact]
        public async Task UpdateUserTestInvalidArgument()
        {
            string id = "1";
            UpdateUserRequest request = new UpdateUserRequest()
            {
                Email = "test",
                Password = "qwer",
            };

            var result = await _authenticator.UpdateUser(request, id);
            
            Assert.Equal(StatusCode.InvalidArgument,result.StatusCode);
            Assert.Equal("Empty argument provided", result.ErrorMessage);
        }
        
        [Fact]
        public async Task UpdateUserTestEmailAlreadyExists()
        {
            string id = "1";
            UpdateUserRequest request = new UpdateUserRequest()
            {
                Name = "string",
                Email = "testpass@gmail.com",
                Password = "qwer",
            };

            var result = await _authenticator.UpdateUser(request, id);
            
            Assert.Equal(StatusCode.AlreadyExists,result.StatusCode);
            Assert.Equal("This Email already exists", result.ErrorMessage);
        }
        
        [Fact]
        public async Task UpdateUserTestCanNotUpdateUser()
        {
            string id = "0";
            UpdateUserRequest request = new UpdateUserRequest()
            {
                Name = "string",
                Email = "test",
                Password = "qwer",
            };

            var result = await _authenticator.UpdateUser(request, id);
            
            Assert.Equal(StatusCode.Internal,result.StatusCode);
            Assert.Equal("Internal Error! Couldn't update user", result.ErrorMessage);
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

                    if(target == "0")
                    {
                        return new User()
                        {
                            Id = "0",
                            Email = "test",
                            Name = "test user",
                        };
                    }

                    return null;
                });
            mock.Setup(e => e.UpdateUser(It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync((string targetId, User target) =>
                {
                    if (targetId == "1")
                    {
                        return true;
                    }

                    return false;
                });
            
            return mock;
        }
    }
}