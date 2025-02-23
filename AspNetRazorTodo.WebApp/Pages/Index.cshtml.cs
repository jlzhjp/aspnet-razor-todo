using AspNetRazorTodo.WebApp.Identity;
using AspNetRazorTodo.WebApp.Models;
using AspNetRazorTodo.WebApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;

namespace AspNetRazorTodo.WebApp.Pages;

[Authorize]
public class IndexModel(
    ILogger<IndexModel> logger,
    ITodoRepository todoRepository,
    UserManager<SimpleTodoUser> userManager) : PageModel
{
    [BindProperty]
    public string? TodoIdToDelete { get; set; }
    public List<Todo> Todos { get; set; } = [];
    
    public async Task OnGetAsync()
    {
        var user = await userManager.GetUserAsync(User) ?? throw new UnauthorizedAccessException();
        Todos = await todoRepository.FindTodosByUserIdAsync(user.Id);
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
