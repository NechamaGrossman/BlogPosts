using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPostHw.Models
{
    public class BlogPostManager
    {
        string _connectionString;
       
        public BlogPostManager(string ConnectionString)
        {
            _connectionString = ConnectionString;
        }
        public List<Blog> GetBlogs()
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"Select * From Blog";
            conn.Open();
            List<Blog> blogs = new List<Blog>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Blog b = new Blog
                {
                    Text = getFirst200Letters((string)reader["Text"]),
                    Title = (string)reader["Title"],
                    Date = (DateTime)reader["Date"],
                    Id = (int)reader["id"]
                };
                blogs.Add(b);
        }
            return blogs;
            conn.Close();
            conn.Dispose();
        }
        public List<Blog> GetBlogsNot200()
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"Select * From Blog";
            conn.Open();
            List<Blog> blogs = new List<Blog>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Blog b = new Blog
                {
                    Text = (string)reader["Text"],
                    Title = (string)reader["Title"],
                    Date = (DateTime)reader["Date"],
                    Id = (int)reader["id"]
                };
                blogs.Add(b);
            }
            return blogs;

        }
        public List<Comment> GetComments()
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"Select * From comment";
            conn.Open();
            List<Comment> comments = new List<Comment>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {Comment c = new Comment
                {
                    Name = (string)reader["Name"],
                    Text = (string)reader["Text"],
                    Date = (DateTime)reader["Date"],
                    BlogId = (int)reader["blogId"]
                };
                comments.Add(c);
            }

            return comments;
            conn.Close();
            conn.Dispose();
        }
        public List<Blog> CommentsToBlogs(List<Comment> comments, List<Blog>blogs)
        {
            foreach(Comment c in comments)
            {
                foreach(Blog b in blogs)
                {
                    if(c.BlogId==b.Id)
                    {
                        b.Comments.Add(c);
                    }
                }
            }
            return blogs;
        }
        public string getFirst200Letters(string text)
        {
            if(text.Length>=200)
            {
                string updated = "";
                for (int i = 0; i <= 200; i++)
                {
                    updated += (text[i]);
                }
                updated += "...";
                return updated;
            }
            else
            {
                return text;
            }
            
        }
        public Blog GetBlogForId(int id, List<Blog> b )
        {
            Blog newBlog = new Blog();
            foreach(Blog blog in b)
            {
                if(blog.Id==id)
                {
                    newBlog = blog;
                }
            }
            return newBlog;
        }
        public void AddComment(Comment c)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"Insert into Comment(Name, Text, Date, BlogId) Values (@Name, @Text, @Date, @BlogId) ";
            cmd.Parameters.AddWithValue("@Name", c.Name);
            cmd.Parameters.AddWithValue("@Text", c.Text);
            cmd.Parameters.AddWithValue("@date", c.Date);
            cmd.Parameters.AddWithValue("@BlogId", c.BlogId);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
        }
        public int AddBlog(Blog b )
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"Insert into Blog(Title, Text, Date) Values (@Title, @Text, @Date) ";
            cmd.Parameters.AddWithValue("@Title", b.Title);
            cmd.Parameters.AddWithValue("@Text", b.Text);
            cmd.Parameters.AddWithValue("@date", b.Date);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
            conn.Dispose();
            return GetIdForBlog(b);
        }
       public int GetIdForBlog(Blog b)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select id From Blog b Where b.Title = @Title And b.Text = @Text ";
            cmd.Parameters.AddWithValue("@Title", b.Title);
            cmd.Parameters.AddWithValue("@Text", b.Text);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            
            int id = new int();
            while (reader.Read())
            {
                id = (int)reader["Id"];
            }
            return id;
        }
    }
}
