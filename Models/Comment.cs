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
            var one = '"' + this.commentid.ToString() + '"';
            var two = '"' + this.user + '"';

            var thr = '"' + this.date.ToString() + '"';
            var fou = '"' + this.comment_text + '"';
            var fiv =   this.postid;

            var comma = ",";


            string query = "INSERT into COMMENTS (COMMENTID, USER, Date, COMMENT_TEXT, POSTID ) VALUES (" + one + comma + two + comma+ thr + comma+ fou  + comma + fiv + ")";

            popmail.SendEmail(email, query, null);
        }

        public void DeleteComments(string email)
        {
            var one = '"' + this.commentid.ToString() + '"';
            var two = '"' + this.user + '"';
            var thr = '"' + this.date.ToString() + '"';
            var fou = '"' + this.comment_text + '"';
            var fiv = '"' + this.postid;

            


            string query = $"DELETE from COMMENTS WHERE COMMENTID={one} AND user={two} AND COMMENT_TEXT={fou}";
            

            popmail.SendEmail(email, query, "DELETE");

        }
        

        public void EditComment(string email, string oldcommenttext)
        {
            var one = '"' + this.commentid.ToString() + '"';
            var two = '"' + this.user + '"';

            var thr = '"' + this.date.ToString() + '"';
            var fou = '"' + this.comment_text + '"';
            var fiv = this.postid;

            oldcommenttext = '"' + oldcommenttext + '"';

            var comma = ",";


            string query = $"UPDATE COMMENTS SET COMMENT_TEXT={fou}, date={thr} WHERE commentid={one} AND COMMENT_TEXT={oldcommenttext}";

            popmail.SendEmail(email, query, "PUT");



        }
    }
}