using AspNetRazorTodo.WebApp.Models;
using AspNetRazorTodo.WebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetRazorTodo.WebApp.Pages.Todos;

public class CreateModel(ILogger<CreateModel> logger, ITodoRepository todoRepository) : PageModel
{
    [BindProperty]
    public string? Title { get; set; }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (Title == null)
        {
            logger.LogError("`Title` is not set");
            return RedirectToPage("/Error");
        }

        var todo = new Todo(Title!, false);
        await todoRepository.SaveAsync(todo);
        return RedirectToPage("/Index");
    }
}