using AspNetCore.MariaDB.HelpClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.MariaDB.Models
{
    public class Comment
    {
        [Key]
        public int? commentid { get; set; }

        public string user { get; set; }

        public DateTime date { get; set; } = DateTime.Now;
        public string comment_text { get; set; }
        [ForeignKey("postid")]
        public int postid { get; set; }
        public Post? post { get; set; }

        public Comment()
        {

        }

        public void SendComments(string email)
        {
            var one = this.commentid;
            var two = '"' + this.user + '"';

            var thr = '"' + this.date.ToString() + '"';
            var fou = '"' + this.comment_text + '"';
            var fiv =   this.postid;

            var comma = ",";


            string query = "INSERT into COMMENTS (USER, Date, COMMENT_TEXT, POSTID ) VALUES (" + two + comma+ thr + comma+ fou  + comma + fiv + ")";

            popmail.SendEmail(email, query);
        }

        public void DeleteComments(string email)
        {
            var one = this.commentid;
            var two = '"' + this.user + '"';
            var thr = '"' + this.date.ToString() + '"';
            var fou = '"' + this.comment_text + '"';
            var fiv = '"' + this.postid;

            


            string query = $"DELETE from COMMENTS WHERE USER={two} AND DATE={thr} AND COMMENT_TEXT={fou}";

            popmail.SendEmail(email, query);

        }
        

        public void EditComment(string email)
        {
            var one = this.commentid;
            var two = '"' + this.user + '"';

            var thr = '"' + this.date.ToString() + '"';
            var fou = '"' + this.comment_text + '"';
            var fiv = this.postid;

            var comma = ",";


            string query = $"UPDATE COMMENTS SET COMMENT_TEXT={fou} WHERE User={two} AND DATE={thr}";

            popmail.SendEmail(email, query);



        }
    }
}