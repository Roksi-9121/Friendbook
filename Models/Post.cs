using DocumentFormat.OpenXml.Office.PowerPoint.Y2022.M03.Main;
using DocumentFormat.OpenXml.Presentation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SocialApp.Models
{
    public class Post
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("userId")]
        public ObjectId UserId { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } 

        [BsonElement("reactions")]
        public List<Reaction> Reactions { get; set; } = new List<Reaction>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }

}
