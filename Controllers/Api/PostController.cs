using System;
using Microsoft.AspNetCore.Mvc;
using SqlKata.Execution;
using SqlKataApi.Models;

namespace SqlKataApi.Controllers.Api
{
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        private readonly QueryFactory db;

        public PostController(QueryFactory db)
        {
            this.db = db;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var posts = db.Query("Posts").Get();

            return Ok(posts);
        }

        [HttpGet("{id}")]
        public IActionResult Find(int id)
        {
            var post = db.Query("Posts").Where("Id", id).First();

            return Ok(post);
        }

        [HttpPost]
        public IActionResult Store([FromBody] Post post)
        {

            post.Id = db.Query("Posts").InsertGetId<int>(post);

            return CreatedAtAction(nameof(Store), post);

        }

        [HttpPut]
        public IActionResult Update([FromBody] Post post)
        {
            db.Query("Posts").Where("Id", post.Id)
                .Update(new
                {
                    Title = post.Title,
                    Body = post.Body,
                    UpdatedAt = DateTime.UtcNow,
                });

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            db.Query("Posts").Where("Id", id).Delete();

            return Ok();
        }

    }
}