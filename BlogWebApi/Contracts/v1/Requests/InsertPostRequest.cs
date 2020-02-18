using System;
namespace BlogWebApi.Contracts.v1.Requests
{
    public class InsertPostRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }
}