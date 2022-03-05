// Stefan Grimm, 2022
// Released to public domain
// Please do not use this as a template for your Web service. This is example code to illustrate Collares.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Collares.WebApiExample.Controllers {

  using ShoppinglistItemsResponse
    = WebApiCollectionResponse<ShoppinglistItemResponse, ShoppinglistItem>;
  class ShoppinglistItemResponse : WebApiResourceResponse<ShoppinglistItem> { }

  /// <summary>
  /// Entity class that would be stored in/managed by the entity framework.
  /// </summary>
  class PeristentShoppinglistItem : ShoppinglistItem {
    public long Id { get; set; }
  }

  [ApiController]
  [Route("[controller]")]
  public class ShoppinglistController : ControllerBase {
    private static List<PeristentShoppinglistItem> _dbItems = new() {
        new PeristentShoppinglistItem { Id = 1, Product = "apples", Price = 3.49m },
        new PeristentShoppinglistItem { Id = 2, Product = "pears",  Price = 2.99m }
      };
     
    private readonly ILogger<ShoppinglistController> _logger;

    public ShoppinglistController(ILogger<ShoppinglistController> logger) {
      _logger = logger;
    }

    /// <summary>Gets a list of items.</summary>
    [HttpGet("rawitems")]
    public IActionResult GetRawItems() {
      // Return the items
      return Ok(_dbItems);
    }

    /// <summary>Gets a list of items.</summary>
    [HttpGet("items")]
    public IActionResult GetItems() {
      // Create the response
      ShoppinglistItemsResponse response = new();
      response.AddHref(HrefType.Post, $"api/shoppinglist/items");
      foreach (var dbItem in _dbItems) {
        ShoppinglistItemResponse itemResponse = new() { Id = dbItem.Id };
        itemResponse.Data.CopyFrom(dbItem);
        itemResponse.AddHref(
          HrefType.Delete, $"api/shoppinglist/items/{itemResponse.Id}");
        itemResponse.AddHref(
          HrefType.Patch, $"api/shoppinglist/items/{itemResponse.Id}");
        response.Data.Add(itemResponse);
      }
      return Ok(response);
    }

    [HttpGet("items/info")]
    public IActionResult GetItemsInfo() {
      // Create the response
      WebApiInfoResponse<ShoppinglistInfo> response = new() { Data = new ShoppinglistInfo() { NumberOfItems = _dbItems.Count } };
      response.AddHref(HrefType.Post, $"api/shoppinglist/items");
      response.AddHref(HrefType.Get, $"api/shoppinglist/items");
      return Ok(response);
    }

    [HttpGet("items/{id}")]
    public IActionResult GetItem(long id) {
      var dbItem = _dbItems.FirstOrDefault(x => x.Id == id);
      if (dbItem == null) {
        return NotFound();
      }
      // Create the response
      ShoppinglistItemResponse response = new() { Id = id };
      response.Data.CopyFrom(dbItem);
      // Notification to the application that it can delete or patch this resource
      response.AddHref(HrefType.Delete, $"api/shoppinglist/items/{response.Id}");
      response.AddHref(HrefType.Patch, $"api/shoppinglist/items/{response.Id}");
      return Ok(response);
    }

    [HttpPost("items/{id}")]
    public IActionResult PostItem(long id, ShoppinglistItem item) {
      if (_dbItems.FirstOrDefault(x => x.Id == id) != null) {
        return Conflict();
      }
      PeristentShoppinglistItem dbItem = new() { Id = id };
      dbItem.CopyFrom(item);
      _dbItems.Add(dbItem);
      return Ok();
    }

    [HttpDelete("items/{id}")]
    public IActionResult DeleteItem(long id) {
      var dbItem = _dbItems.FirstOrDefault(x => x.Id == id);
      if (dbItem == null) {
        return NotFound();
      }
      _dbItems.Remove(dbItem);
      return Ok();
    }

    [HttpPatch("items/{id}")]
    public IActionResult PatchItem(long id, ShoppinglistItem item) {
      var dbItem = _dbItems.FirstOrDefault(x => x.Id == id);
      if (dbItem == null) {
        return NotFound();
      }
      dbItem.CopyFrom(item);
      return Ok();
    }

    class ShoppinglistInfo {
      public string Name => "Shoppinglist Controller";
      public long NumberOfItems { get; set; }
    }
  }
}
