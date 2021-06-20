using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppClient.Models
{
    public class PostsModel
    {
      
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Title { get; set; }

        [StringLength (399)]
        public string Body { get; set; }
    }
}