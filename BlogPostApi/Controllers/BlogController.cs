using BlogPostApi.Model;
using BlogPostApi.Model.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPostApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string baseUrl;
      
        public BlogController(IConfiguration configuration)
        {
            _configuration = configuration;
            baseUrl = _configuration.GetValue<string>("BaseUrl:root");
        }
        [HttpGet]

        public async Task<responsedto> GetAll(string BlogId)
        {
            responsedto model = new responsedto();

            model.Comments = new List<Replydto>();
            List<Replies> replies = new List<Replies>();
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("BebesBlog").GetCollection<Blog>("BebesBlog").AsQueryable().Where(x => x.BlogId == BlogId).FirstOrDefault();
            if (dblist != null)
            {
                foreach(var item in dblist.Comment)
                {
                    Replydto replydto = new Replydto() { BlogId = BlogId, Comment = item.comment, replies = dblist.Replies?.Count() > 0 ? dblist.Replies.ToList().Where(x => x.CommentId == item.Id).ToList() : replies ,CommentId = item.Id,username = item.username};
                    model.Comments.Add(replydto);
                }
                //model.Comments = dblist.Comment.ToList();
            }
            return model;
        }

        [HttpPost]
        public async Task<bool> Create(BlogDto model)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("BebesBlog").GetCollection<Blog>("BebesBlog").AsQueryable().Where(x => x.BlogId == model.BlogId).FirstOrDefault();
            if (dblist?.Comment == null)
            {
                Comment[] created = new Comment[] { new Comment() { DateCommented = DateTime.Now,comment = model.Comment,username = model.username } };
                Blog blogpost = new() { BlogId = model.BlogId, Comment = created };
                await dbClient.GetDatabase("BebesBlog").GetCollection<Blog>("BebesBlog").InsertOneAsync(blogpost);

                return true;
            }
            else
            {
               
                    Comment[] createdstock = null;
                    List<Comment> stocks = new List<Comment>();
                    stocks.AddRange(dblist.Comment.ToList());
                   Comment value = new Comment() { DateCommented = DateTime.Now, comment = model.Comment, username = model.username,Id= Guid.NewGuid().ToString() };
                     stocks.Add(value);
                    createdstock = stocks.ToArray();
                var filter = Builders<Blog>.Filter.Eq("BlogId", model.BlogId);
                var update = Builders<Blog>.Update.Set("Comment", createdstock);
                await dbClient.GetDatabase("BebesBlog").GetCollection<Blog>("BebesBlog").UpdateOneAsync(filter, update);
                return true;
              
            }
            return false;
        }
        [HttpPost]
        public async Task<bool> ReplyBlog(ReplyBlogDto model)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("BebesBlog").GetCollection<Blog>("BebesBlog").AsQueryable().Where(x => x.BlogId == model.BlogId).FirstOrDefault();
            if (dblist?.Replies == null)
            {
               
                Replies[] createdstock = null;
                List<Replies> stocks = new List<Replies>();
            
                Replies value = new Replies() { DateCommented = DateTime.Now, comment = model.Comment, username = model.username, CommentId = model.ParentId };
                stocks.Add(value);
                createdstock = stocks.ToArray();
                var filter = Builders<Blog>.Filter.Eq("BlogId", model.BlogId);
                var update = Builders<Blog>.Update.Set("Replies", createdstock);
                await dbClient.GetDatabase("BebesBlog").GetCollection<Blog>("BebesBlog").UpdateOneAsync(filter, update);

                //await dbClient.GetDatabase("BebesBlog").GetCollection<Blog>("BebesBlog").InsertOneAsync(blogpost);

                return true;
            }
            else
            {

                Replies[] createdstock = null;
                List<Replies> stocks = new List<Replies>();
                stocks.AddRange(dblist.Replies.ToList());
                Replies value = new Replies() { DateCommented = DateTime.Now, comment = model.Comment, username = model.username, CommentId = model.ParentId };
                stocks.Add(value);
                createdstock = stocks.ToArray();
                var filter = Builders<Blog>.Filter.Eq("BlogId", model.BlogId);
                var update = Builders<Blog>.Update.Set("Replies", createdstock);
                await dbClient.GetDatabase("BebesBlog").GetCollection<Blog>("BebesBlog").UpdateOneAsync(filter, update);
                return true;

            }
            return false;
        }
    }
}
