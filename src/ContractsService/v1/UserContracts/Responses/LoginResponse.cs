using System;

namespace ContractsService.v1.UserContracts.Responses
{
    public class LoginResponse : IResponse
    {
        public string Token { get; set; }
        public StatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}