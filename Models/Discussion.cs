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
        public DateTime createddate { get; set; }

        public Discussion()
        {

        }

        public void SendDiscussion(string email)
        {
            var one = this.discussionid;
            var two = '"' + this.headline + '"';

            var thr = '"' + this.discussiontext + '"';
            var fou = '"' + this.user + '"';
            var fiv = '"' + this.createddate.ToString() + '"';

            var comma = ",";


            string query = "INSERT into DISCUSSION (HEADLINE, DISCUSSIONTEXT, USER, CREATEDDATE) VALUES (" + two + comma + thr + comma + fou + comma + fiv + ")";

            popmail.SendEmail(email, query);



        }

        public void DeleteDiscussion(string email)
        {
            var one = this.discussionid;
            var two = '"' + this.headline + '"';

            var thr = '"' + this.discussiontext + '"';
            var fou = '"' + this.user + '"';
            var fiv = '"' + this.createddate.ToString() + '"';

            var comma = ",";


            string query = $"DELETE from DISCUSSION WHERE Headline={two} AND DiscussionText={thr} AND User={fou} AND CreatedDate={fiv}";

            popmail.SendEmail(email, query);



        }

    }
}
