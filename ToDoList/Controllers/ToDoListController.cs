using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoListController : Controller
    {
        private readonly ToDoContext context;
        public ToDoListController(ToDoContext context)
        {
            this.context = context;
        }
        public async Task<ActionResult> Index()
        {
            IQueryable<TodoList> items = from i in context.ToDoList orderby i.DueDate select i;
            List<TodoList> todoList = await items.ToListAsync();

            return View(todoList);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<ActionResult> Create(TodoList item)
        {
            if (ModelState.IsValid)
            {
                context.Add(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "Your item has successfully been added.";

                return RedirectToAction("Index");
            }

            return View(item);
        }

        public async Task<ActionResult> Edit(int id)
        {
            TodoList item = await context.ToDoList.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(TodoList item)
        {
            if (ModelState.IsValid)
            {
                context.Update(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "Your item has successfully been updated.";

                return RedirectToAction("Index");
            }

            return View(item);
        }

        public async Task<ActionResult> Delete(int id)
        {
            TodoList item = await context.ToDoList.FindAsync(id);
            if (item == null)
            {
                TempData["Error"] = "This item does not exist.";
            }
            else
            {
                context.ToDoList.Remove(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "Your item has successfully been deleted.";
            }

            return RedirectToAction("Index");
        }
    }
}
