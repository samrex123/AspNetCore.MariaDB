using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.MariaDB.Models.Dto
{
    public class DiscussionDTO
    {
        public int? discussionid { get; set; }
        public string headline { get; set; }
        public string discussiontext { get; set; }
        public string user { get; set; }
        public DateTime createddate { get; set; }

        public List<Post> posts { get; set; }

        public DiscussionDTO(Discussion discussion, List<Post> posts)
        {
            this.discussionid = discussion.discussionid;
            this.headline = discussion.headline;
            this.discussiontext = discussion.discussiontext;
            this.user = discussion.user;
            this.createddate = discussion.createddate;

            this.posts = posts;
        }

    }
}
