using System;
namespace BlogWebApi.Contracts.v1.Responses
{
    public class InsertPostRespnse
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}