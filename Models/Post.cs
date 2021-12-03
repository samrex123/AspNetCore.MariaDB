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
        //[ForeignKey("email")]
        //public string? UserMail { get; set; }
        public Post()
        {

        }

        public void SendPost(string email)
        {
            var one = '"' + this.postid.ToString() + '"';
            var two = '"' + this.User + '"';

            var thr = '"' + this.Text + '"';
            var fou = '"' + this.DateTime.ToString() + '"';
            var fiv = this.discussionid;

            var comma = ",";


            string query = "INSERT into POSTS (POSTID, User, Text, DateTime, discussionid) VALUES ("+ one + comma + two + comma + thr + comma + fou + comma + fiv + ")";

            popmail.SendEmail(email, query, null);



        }

        public void EditPost(string email, string oldtext)
        {
            var one = '"' + this.postid.ToString()+'"';
            var two = '"' + this.User + '"';

            var thr = '"' + this.Text + '"';
            var fou = '"' + this.DateTime.ToString() + '"';
            var fiv = this.discussionid;


            oldtext = '"' + oldtext + '"';
            var comma = ",";


            string query = $"UPDATE POSTS SET TEXT={thr} WHERE postid={one} AND User={two} AND Text={oldtext}";





            popmail.SendEmail(email, query, "PUT");



        }

        public void DeletePost(string email)
        {
            var one = '"'+this.postid.ToString()+'"';
            var two = '"' + this.User + '"';

            var thr = '"' + this.Text + '"';
            var fou = '"' + this.DateTime.ToString() + '"';
            var fiv = this.discussionid;

            var comma = ",";


            string query = $"DELETE from POSTS WHERE POSTID={one} AND User={two} AND Text={thr}";

            popmail.SendEmail(email, query, "DELETE");



        }
    }
}
