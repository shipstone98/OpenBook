using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Shipstone.OpenBook.Converters;

namespace Shipstone.OpenBook.Models
{
    [JsonConverter(typeof (PostConverter))]
    public class Post
    {
        [Required]
        [StringLength(Internals._PostContentMaxLength, MinimumLength = 1)]
        public String Content { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Created at")]
        public DateTime Created => this.CreatedUtc.ToLocalTime();
        
        [DataType(DataType.DateTime)]
        [Display(Name = "Created at")]
        public DateTime CreatedUtc { get; set; }

        public virtual User Creator { get; set; }
        public int CreatorId { get; set; }
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Modified at")]
        public DateTime Modified => this.ModifiedUtc.ToLocalTime();
        
        [DataType(DataType.DateTime)]
        [Display(Name = "Modified at")]
        public DateTime ModifiedUtc { get; set; }
    }
}
