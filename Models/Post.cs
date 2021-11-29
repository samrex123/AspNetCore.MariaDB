using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspNetCore.MariaDB.HelpClasses;

namespace AspNetCore.MariaDB.Models
{
    public class Post
    {
        public int? postid { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        [ForeignKey("discussionid")]
        public int discussionid { get; set; }
        public Discussion? Discussion { get; set; }
        [ForeignKey("email")]
        public string? UserMail { get; set; }
        public Post()
        {

        }

        public void SendPost(string email)
        {
            var one = this.postid;
            var two = '"' + this.User + '"';

            var thr = '"' + this.Text + '"';
            var fou = '"' + this.DateTime.ToString() + '"';
            var fiv = this.discussionid;

            var comma = ",";


            string query = "INSERT into POSTS (User, Text, DateTime, discussionid) VALUES (" + two + comma + thr + comma + fou + comma + fiv + ")";

            popmail.SendEmail(email, query);



        }
    }
}
