﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppClient.Models
{
    public class CommentsModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
    }
}