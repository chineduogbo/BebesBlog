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
    public class responsedto
    {
        public List<Comment> Comments { get; set; }
    }
}
