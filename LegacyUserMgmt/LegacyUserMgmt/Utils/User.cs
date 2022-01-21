using System;

namespace LegacyUserMgmt
{
    public class User
    {
        public UserType Type { get; set; }
        public string Id { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int PostsWritten { get; set; }
        public int CommentsWritten { get; set; }
        public int Rating { get; set; }
    }
}
