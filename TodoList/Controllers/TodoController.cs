using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.DTOs;
using TodoList.Model;

namespace TodoList.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {

        private readonly DataContext _context;
        public TodoController(DataContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<List<TodoItem>>> GetTodos()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return await _context.TodoItems.Where(t => t.UserId == userId).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> AddTodo([FromBody] TodoCreateDto dto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var todo = new TodoItem
            {
                Title = dto.Title,
                Description = dto.Description,
                IsComplete = dto.IsComplete,
                UserId = userId
            };

            _context.TodoItems.Add(todo);
            await _context.SaveChangesAsync();

            return Ok(todo);
        } 

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTodo(int id, [FromBody] TodoCreateDto dto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var todo = await _context.TodoItems.FindAsync(id);

            if (todo == null || todo.UserId != userId)
                return Unauthorized();

            todo.Title = dto.Title;
            todo.Description = dto.Description;
            todo.IsComplete = dto.IsComplete;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodo(int id)
        {
            var todo = await _context.TodoItems.FindAsync(id);
            if (todo == null || todo.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value)) return Unauthorized();

            _context.TodoItems.Remove(todo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
