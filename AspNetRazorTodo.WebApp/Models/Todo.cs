using MongoDB.Bson;

namespace AspNetRazorTodo.WebApp.Models;
public class Todo
{
    private Todo() { }

    public Todo(string title, bool isCompleted)
    {
        Title = title;
        IsCompleted = isCompleted;
    }

    public ObjectId Id { get; private set; } = ObjectId.GenerateNewId();

    public string Title { get; private set; } = null!;

    public bool IsCompleted { get; private set; }
}
