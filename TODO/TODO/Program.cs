using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TODO.Contexts;
using TODO.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/todo", async (TodoContext context) =>
{
    return Results.Ok(await context.TodoItems
        .Select(item => new TodoItemDTO(item))
        .ToListAsync());
});

app.MapGet("/todo/{id}", async ([FromRoute] long id, TodoContext context) =>
{
    var foundedItem = await context.TodoItems.FindAsync(id);

    return foundedItem is null 
        ? Results.NotFound() 
        : Results.Ok(new TodoItemDTO(foundedItem));
}).WithName("GetItemById");

app.MapPost("/todo", async ([FromBody] TodoItemDTO item, TodoContext context) =>
{
    var todoItem = new TodoItem
    {
        IsComplete = item.IsComplete,
        Name = item.Name
    };

    await context.TodoItems.AddAsync(todoItem);
    await context.SaveChangesAsync();
    
    return Results.CreatedAtRoute(
        "GetItemById",
        new { id = todoItem.Id });
});

app.MapPut("/todo/{id}", async ([FromRoute] long id, [FromBody] TodoItemDTO item, TodoContext context) =>
{
    if (id != item.Id)
        return Results.BadRequest();
    
    var foundedItem = await context.TodoItems.FindAsync(id);

    if (foundedItem is null)
        return Results.NotFound();
    
    foundedItem.Name = item.Name;
    foundedItem.IsComplete = item.IsComplete;
    
    try
    {
        await context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        return Results.NotFound();
    }
    
    return Results.NoContent();
});

app.MapDelete("/todo/{id}", async ([FromRoute] long id, TodoContext context) =>
{
    var foundedItem = await context.TodoItems.FindAsync(id);

    if (foundedItem is null)
        return Results.NotFound();
    
    context.TodoItems.Remove(foundedItem);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();
