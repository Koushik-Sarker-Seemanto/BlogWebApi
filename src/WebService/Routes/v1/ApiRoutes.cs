namespace WebService.Routes.v1
{
    public static class ApiRoutes
    {
        public static class UserRoute
        {
            public const string GetUsers = "api/v1/users/";
            public const string Login = "api/v1/user/login";
            public const string Registration = "api/v1/user/registration";
            public const string Profile = "api/v1/user/profile";
            public const string UpdateProfile = "api/v1/user/update";
        }
        public static class PostRoute
        {
            public const string GetPostList = "api/v1/posts";
            public const string GetPost = "api/v1/post/{id}";
            public const string InsertPost = "api/v1/posts";
            public const string UpdatePost = "api/v1/posts/{id}";
            public const string DeletePost = "api/v1/posts/{id}";
            public const string AddReact ="api/v1/post/{id}/react";
            public const string ReactByUser = "api/v1/post/{id}/react";
            public const string UserPost = "api/v1/user/posts";
        }

        public static class FriendshipRoute
        {
            public const string AddFriend = "api/v1/friend/addfriend/{id}";
            public const string FriendList = "api/v1/friends";
            public const string FriendRequestList = "api/v1/friend/friendrequests";
            public const string AcceptRequest = "ap1/v1/friend/accept/{id}";
            public const string RejectRequest = "api/v1/friend/reject/{id}";
        }
    }
}