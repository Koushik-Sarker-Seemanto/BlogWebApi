using System;

namespace BlogWebApi.Routes.v1
{
    public static class ApiRoutes
    {
        public static class UserRoute
        {
            public const string GetAllusers = "api/v1/users/";
        }
        public static class PostRoute
        {
            public const string GetPostList = "api/v1/posts";
            public const string GetPost = "api/v1/post/{id}";
            public const string InsertPost = "api/v1/posts";
            public const string UpdatePost = "api/v1/posts/{id}";
            public const string DeletePost = "api/v1/posts/{id}";
        }
    }
}