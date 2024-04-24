using Microsoft.EntityFrameworkCore;
using TODO.Models;

namespace TODO.Contexts;

public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems { get; set; }
}