using System.ComponentModel.DataAnnotations;

namespace Blog.Data.Models;

public class BaseModel<TKey>
{
    [Key]
    public TKey Id { get; set; }
}