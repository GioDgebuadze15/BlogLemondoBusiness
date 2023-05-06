namespace Blog.Data.Forms;

public class CreatePostForm
{
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime DateOfPublish { get; set; }
}