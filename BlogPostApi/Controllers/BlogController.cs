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

        public async Task<List<Comment>> GetAll(string BlogId)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("BebesBlog").GetCollection<Blog>("BebesBlog").AsQueryable().Where(x => x.BlogId == BlogId).FirstOrDefault();
            if (dblist != null)
            {
                return dblist.Comment.ToList();
            }
            else
            {
                List<Comment> mode = null;
                return mode;
            }
        }

        [HttpPost]
        public async Task<bool> Create(BlogDto model)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("BebesBlog").GetCollection<Blog>("BebesBlog").AsQueryable().Where(x => x.BlogId == model.BlogId).FirstOrDefault();
            if (dblist == null)
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
                   Comment value = new Comment() { DateCommented = DateTime.Now, comment = model.Comment, username = model.username };
                     stocks.Add(value);
                    createdstock = stocks.ToArray();
                var filter = Builders<Blog>.Filter.Eq("BlogId", model.BlogId);
                var update = Builders<Blog>.Update.Set("Comment", createdstock);
                await dbClient.GetDatabase("BebesBlog").GetCollection<Blog>("BebesBlog").UpdateOneAsync(filter, update);
                return true;
              
            }
            return false;
        }
    }
}
