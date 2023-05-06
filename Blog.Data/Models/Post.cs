using System.ComponentModel.DataAnnotations;

namespace Blog.Data.Models;

public class Post : BaseModel<int>
{
    [Required] public string Title { get; set; }
    [Required] public string Body { get; set; }
    [Required] public DateTime PublishDate { get; set; }
}