using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPostApi.Model.Dto
{
    public class BlogDto
    {
        public string username { get; set; }
        public string Comment { get; set; }
        public string BlogId { get; set; }
    }
    public class ReplyBlogDto
    {
        public string username { get; set; }
        public string Comment { get; set; }
        public string BlogId { get; set; }
        public string ParentId { get; set; }
    }
    public class responsedto
    {
        public List<Replydto> Comments { get; set; }
    }
    public class Replydto
    {
        public string username { get; set; }
        public string Comment { get; set; }
        public string CommentId { get; set; }
        public string BlogId { get; set; }
        public DateTime DateCreated { get; set; }
        public List<Replies> replies { get; set; }
    }
}
