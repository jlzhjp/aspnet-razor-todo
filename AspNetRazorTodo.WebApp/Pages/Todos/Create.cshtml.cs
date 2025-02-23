using AspNetRazorTodo.WebApp.Identity;
using AspNetRazorTodo.WebApp.Models;
using AspNetRazorTodo.WebApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetRazorTodo.WebApp.Pages.Todos;

[Authorize]
public class CreateModel(
    ILogger<CreateModel> logger,
    ITodoRepository todoRepository,
    UserManager<SimpleTodoUser> userManager) : PageModel
{
    [BindProperty]
    public string? Title { get; set; }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (Title is null)
        {
            logger.LogError("`Title` is not set");
            return RedirectToPage("/Error");
        }

        var user = await userManager.GetUserAsync(User) ?? throw new UnauthorizedAccessException();
        var todo = new Todo(Title!, false, user.Id);
        await todoRepository.SaveAsync(todo);
        return RedirectToPage("/Index");
    }
}