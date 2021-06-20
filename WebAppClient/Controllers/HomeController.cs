using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using WebAppClient.Models;

namespace WebAppClient.Controllers
{
    
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<PostsModel> posts = new List<PostsModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

                //HTTP GET
                var responseTask = client.GetAsync("posts");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<PostsModel>>();
                    readTask.Wait();

                    posts = readTask.Result;
           
                }
                else //web api sent error response 
                {
                    //log response status here..

                    posts = Enumerable.Empty<PostsModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(posts.OrderBy(o=>o.Title).ToList());
        }

        [HttpGet]
        public ActionResult Filter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Filter(int FilterPostId)
        {
           
            if (FilterPostId != 0)
            {

                using (var client = new HttpClient())
                {
                    PostsModel posts = new PostsModel();

                    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

                    var responseTask = client.GetAsync($"posts/{FilterPostId}");

                    responseTask.Wait();

                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<PostsModel>();
                        readTask.Wait();

                        posts = readTask.Result;

                    }

                    return View(posts);
                }
            }
            
            return View();
        }

        [HttpGet]
        public ActionResult FilterBody()
        {
            IEnumerable<PostsModel> posts = new List<PostsModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

                //HTTP GET
                var responseTask = client.GetAsync("posts");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<PostsModel>>();
                    readTask.Wait();

                    posts = readTask.Result;
           
                }
                else //web api sent error response 
                {
                    //log response status here..

                    posts = Enumerable.Empty<PostsModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(posts.OrderBy(o=>o.Title).ToList());
        }

        [HttpPost]
        public ActionResult FilterBody(String FilterBody)
        {
            if (FilterBody != "")
            {
                IEnumerable<PostsModel> posts = new List<PostsModel>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

                    //HTTP GET
                    var responseTask = client.GetAsync("posts");
                    responseTask.Wait();

                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<PostsModel>>();
                        readTask.Wait();

                        posts = readTask.Result;
                        List<PostsModel> postBody = new List<PostsModel>();

                        foreach (var item in posts)
                        {
                            String pBody = item.Body;
                            bool b = pBody.Contains(FilterBody);
                            if (b)
                            {
                                postBody.Add(new PostsModel
                                {
                                    Id = item.Id,
                                    UserId = item.UserId,
                                    Body = item.Body,
                                    Title = item.Title
                                });
                            }
                        }
                        return View(postBody.AsEnumerable());
                    }

                }

            }
            return View();
        }

        [HttpGet]
        public ActionResult Comments(PostsModel mod)
        {
            IEnumerable<CommentsModel> comments = new List<CommentsModel>();

            int postID = mod.Id;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

                //HTTP GET
                var responseTask = client.GetAsync($"posts/{postID}/comments");

                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<CommentsModel>>();
                    readTask.Wait();

                    comments = readTask.Result;

                }
                else //web api sent error response 
                {
                    //log response status here..

                    comments = Enumerable.Empty<CommentsModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(comments);
        }


        public ActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePost(PostsModel mod)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<PostsModel>("posts", mod);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }

                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            }
            return View();
        }


        public ActionResult DeletePosts(int id)
        {
            PostsModel posts = new PostsModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

                //HTTP GET
                var responseTask = client.GetAsync($"posts/{id}");

                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<PostsModel>();
                    readTask.Wait();

                    posts = readTask.Result;
                   
                }
                else //web api sent error response 
                {
                    //log response status here..

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return View(posts);

        }

        [HttpPost]
        public ActionResult DeletePosts(PostsModel mod)
        {
            int postID = mod.Id;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync($"posts/{postID}");
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Post has been deleted successfully";
                    return RedirectToAction("Index");
                }
            }
            
            return View(mod);
        }

        public void ExportPostsListToCSV()
        {
            StringWriter sw = new StringWriter();

            sw.WriteLine("\"Post ID\",\"User ID\",\"Title\",\"Body\"");

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Exported_Users.csv");
            //Response.ContentType = "text/plain";
            Response.ContentType = "text/csv";

            IEnumerable<PostsModel> posts = new List<PostsModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

                //HTTP GET
                var responseTask = client.GetAsync("posts");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<PostsModel>>();
                    readTask.Wait();

                    posts = readTask.Result;

                    foreach (var line in posts)
                    {
                        sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"",
                                              line.Id,
                                              line.UserId,
                                              line.Title,
                                              line.Body));
                    }

                    Response.Write(sw.ToString());

                }
                else //web api sent error response 
                {
                    //log response status here..

                    posts = Enumerable.Empty<PostsModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            ViewBag.Message = "Posts have been exported successfully";
            Response.End();

        }
    }
}