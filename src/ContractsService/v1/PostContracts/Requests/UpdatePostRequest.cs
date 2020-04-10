using System;

namespace ContractsService.v1.PostContracts.Requests
{
    public class UpdatePostRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Body))
            {
                return false;
            }
            return true;
        }
    }
}