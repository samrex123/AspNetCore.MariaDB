using AspNetCore.MariaDB.HelpClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.MariaDB.Models
{
    public class Discussion 
    {
        public int? discussionid { get; set; }
        public string headline { get; set; }
        public string discussiontext { get; set; }
        public string user { get; set; }
        public DateTime createddate { get; set; } = DateTime.UtcNow;

        public Discussion()
        {

        }

        public void SendDiscussion(string email)
        {
            var one = '"' + this.discussionid.ToString() + '"';
            var two = '"' + this.headline + '"';

            var thr = '"' + this.discussiontext + '"';
            var fou = '"' + this.user + '"';
            var fiv = '"' + this.createddate.ToString() + '"';

            var comma = ",";


            string query = "INSERT into DISCUSSION (DISCUSSIONID, HEADLINE, DISCUSSIONTEXT, USER, CREATEDDATE) VALUES (" + one + comma + two + comma + thr + comma + fou + comma + fiv + ")";

            popmail.SendEmail(email, query, null);



        }

        public void EditDiscussion(string email, string oldtext)
        {
            var one = '"' + this.discussionid.ToString() + '"';
            var two = '"' + this.headline + '"';
            var thr = '"' + this.discussiontext + '"';
            var fou = '"' + this.user + '"';
            var fiv = '"' + this.createddate.ToString() + '"';

            oldtext = '"' + oldtext + '"';

            string query = $"UPDATE DISCUSSION SET DISCUSSIONTEXT={thr}, HEADLINE={two} " +
                $"WHERE discussionid={one} AND DISCUSSIONTEXT={oldtext} AND USER={fou}";

            popmail.SendEmail(email, query, "PUT");
        }

        public void DeleteDiscussion(string email)
        {
            var one = '"' + this.discussionid.ToString() + '"';
            var two = '"' + this.headline + '"';
            var thr = '"' + this.discussiontext + '"';
            var fou = '"' + this.user + '"';
            var fiv = '"' + this.createddate.ToString() + '"';

            string query = $"DELETE from DISCUSSION WHERE DISCUSSIONID={one} AND Headline={two} AND DiscussionText={thr} AND User={fou}";

            popmail.SendEmail(email, query, "DELETE");
        }
    }
}
