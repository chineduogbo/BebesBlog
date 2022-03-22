using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPostApi.Model
{
    [BsonIgnoreExtraElements]
    public class Blog
    {
      
      
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string? _id { get; set; }
            public string BlogId { get; set; }
            public Comment[] Comment { get; set; }
         

    }
    public class Comment
    {
        public string username { get; set; }
        public string comment { get; set; }
        public DateTime? DateCommented { get; set; }

    }
}
