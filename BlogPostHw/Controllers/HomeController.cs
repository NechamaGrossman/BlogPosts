using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogPostHw.Models;

namespace BlogPostHw.Controllers
{
    public class HomeController : Controller
    {
        string _connectionString = (@"Data Source=.\sqlexpress;Initial Catalog=blogPosts;Integrated Security=True;");
        public IActionResult Index()
        {
            BlogPostManager bpm = new BlogPostManager(_connectionString);
            return View(bpm.GetBlogs());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Read(int Id)
        {
            BlogPostManager bpm = new BlogPostManager(_connectionString);
            BlogAndName ban = new BlogAndName();
            List<Blog> b = bpm.GetBlogsNot200();
            List<Comment> c = bpm.GetComments();
            List<Blog> complete = bpm.CommentsToBlogs(c, b);
            ban.blog= bpm.GetBlogForId(Id, complete);
            ban.Name  = Request.Cookies["Name"];
            return View(ban);
        }
        public IActionResult AddComment(Comment c)
        {

            string value = Request.Cookies["Name"];
            bool isFirstTime = String.IsNullOrEmpty(value);
            if (isFirstTime)
            {
                Response.Cookies.Append("Name", $"{c.Name}");
            }
            BlogPostManager bpm = new BlogPostManager(_connectionString);
            bpm.AddComment(c);
            return Redirect($"/Home/Read?Id={c.BlogId}");
        }
       
    }
}
