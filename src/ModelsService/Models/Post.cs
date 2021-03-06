using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ModelsService.Models
{
    [Serializable]
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        
        [BsonRequired]
        public string Title { get; set; }
        
        [BsonRequired]
        public string Body { get; set; }
        
        [BsonRequired]
        public User Author { get; set; }
        
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [BsonElement]
        public DateTime? UpdatedAt { get; set; }
        // public double Rating { get; set; }
        
        [BsonElement]
        public List<User> Likes { get; set; }
    }
}