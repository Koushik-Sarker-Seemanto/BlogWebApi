using System;
using System.Collections.Generic;
using ContractsService.v1.UserContracts.Responses;
using ModelsService.Models;

namespace ContractsService.v1.PostContracts.Response
{
    public class PostResponse
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public UserResponse Author { get; set; }
        public List<UserResponse> Likes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Liked { get; set; }
    }
}