using System.Diagnostics;
using AspNetRazorTodo.WebApp.Models;
using AspNetRazorTodo.WebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;

namespace AspNetRazorTodo.WebApp.Pages.Todos;

public class EditModel(ILogger<EditModel> logger, ITodoRepository todoRepository) : PageModel
{
    [BindProperty]
    public string? Title { get; set; }

    private bool? _isCompleted;

    [BindProperty]
    public bool IsCompleted
    {
        get => _isCompleted ?? false;
        set => _isCompleted = value;
    }

    private record FindTodoResult();
    private record TodoFound(Todo Todo) : FindTodoResult;
    private record InvalidId() : FindTodoResult;
    private record TodoNotFound() : FindTodoResult;
    
    private async Task<FindTodoResult> FindTodoByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId)) return new InvalidId();
        var todo = await todoRepository.FindTodoByIdAsync(objectId);
        return todo is null ? new TodoNotFound() : new TodoFound(todo);
    }
    
    public async Task<IActionResult> OnGetAsync(string id)
    {
        switch (await FindTodoByIdAsync(id))
        {
            case InvalidId:
                logger.LogError("Invalid `ObjectId` {id}", id);
                return BadRequest();
            case TodoNotFound:
                logger.LogError("Todo with id {id} not found", id);
                return NotFound();
            case TodoFound({ } todo):
                Title = todo.Title;
                IsCompleted = todo.IsCompleted;
                return Page();
        }

        throw new InvalidOperationException("Unknown subclass of `FindTodoResult`");
    }

    public async Task<IActionResult> OnPostAsync(string id)
    {
        if (Title is null) return BadRequest("Missing field `Title`");
        if (_isCompleted is null) return BadRequest("Missing field `IsCompleted`");
        
        switch (await FindTodoByIdAsync(id))
        {
            case InvalidId:
                logger.LogError("Invalid `ObjectId` {id}", id);
                return BadRequest();
            case TodoNotFound:
                logger.LogError("Todo with id {id} not found", id);
                return NotFound();
            case TodoFound({ } todo):
                todo.Title = Title!;
                todo.IsCompleted = IsCompleted;
                await todoRepository.SaveAsync(todo);
                return RedirectToPage("/Index");
        }

        throw new InvalidOperationException("Unknown subclass of `FindTodoResult`");
    }
}