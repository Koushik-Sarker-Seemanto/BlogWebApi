using System;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace BlogWebApi.Contracts.v1.Requests
{
    public class UpdatePostRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime updatedAt { get; set; } = DateTime.Now;
    }
}