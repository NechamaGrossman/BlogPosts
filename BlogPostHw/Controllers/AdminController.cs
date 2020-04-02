using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogPostHw.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogPostHw.Controllers
{
    public class AdminController : Controller
    {
        string _connectionString = (@"Data Source=.\sqlexpress;Initial Catalog=blogPosts;Integrated Security=True;");
        public IActionResult AddBlogPost()
        {
            return View();
        }
        public IActionResult SubmitBlog(Blog b)
        {
            BlogPostManager bpm = new BlogPostManager(_connectionString);
            return Redirect($"/Home/Read?Id={bpm.AddBlog(b)}");
        }
     }
}