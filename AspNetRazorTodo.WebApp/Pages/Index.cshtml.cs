using AspNetRazorTodo.WebApp.Models;
using AspNetRazorTodo.WebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop.Implementation;
using MongoDB.Bson;

namespace AspNetRazorTodo.WebApp.Pages;

public class IndexModel(ILogger<IndexModel> logger, ITodoRepository todoRepository) : PageModel
{
    [BindProperty]
    public string? TodoIdToDelete { get; set; }
    public List<Todo> Todos { get; set; } = [];
    
    public async Task OnGetAsync()
    {
        Todos = await todoRepository.FindTodosAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (TodoIdToDelete is null)
        {
            logger.LogError("`TodoIdToDelete` not set");
            return BadRequest();
        }

        if (!ObjectId.TryParse(TodoIdToDelete, out var id))
        {
            logger.LogError("`TodoIdToDelete` not a valid `ObjectId`");
            return BadRequest();
        }

        var deletedCount = await todoRepository.DeleteAsync(id);

        if (deletedCount != 0) return RedirectToPage();
        
        logger.LogError("`Todo` with id {todoId} not found", TodoIdToDelete);
        return NotFound();
    }
}
