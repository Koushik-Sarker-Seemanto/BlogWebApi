using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ModelsService.Models
{
    [Serializable]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [BsonRequired]
        public string Name { get; set; }
        [BsonRequired]
        public string Email { get; set; }
        [BsonRequired]
        public string Password { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<string> Friends { get; set; }
        public List<string> FriendRequests { get; set; }
        public List<string> FriendRequestsSent { get; set; }
    }
}