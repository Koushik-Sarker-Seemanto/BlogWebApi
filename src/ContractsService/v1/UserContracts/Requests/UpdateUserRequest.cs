namespace ContractsService.v1.UserContracts.Requests
{
    public class UpdateUserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Name))
            {
                return false;
            }
            return true;
        }
    }
}