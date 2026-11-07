using DN_8.Data;
using DN_8.Models;
using Microsoft.AspNetCore.Mvc;

namespace DN_8.Controllers;

public class InventoryController : Controller
{
    private readonly InventoryRepository _repository;
    public InventoryController(InventoryRepository repository) => _repository = repository;

    public IActionResult Index() => View(_repository.GetAll());

    [HttpGet]
    public IActionResult Create() => View(new InventoryItem());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(InventoryItem item)
    {
        if (!ModelState.IsValid) return View(item);
        _repository.Add(item);
        TempData["Message"] = "Item added successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var item = _repository.GetById(id);
        return item == null ? NotFound() : View(item);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var item = _repository.GetById(id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(InventoryItem item)
    {
        if (!ModelState.IsValid) return View(item);
        if (!_repository.Update(item)) return NotFound();
        TempData["Message"] = "Item updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var item = _repository.GetById(id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (!_repository.Delete(id)) return NotFound();
        TempData["Message"] = "Item deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
