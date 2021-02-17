using System;
using Newtonsoft.Json;

namespace CoOwnershipManager.Data
{

    public class Post
    {
        public Post()
        {
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PostedAt { get; set; }

        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public int BuildingId { get; set; }
        
        [JsonIgnore]
        public Building Building { get; set; }

    }
}
