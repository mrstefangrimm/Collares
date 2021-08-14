# Collares
**Coll**ections **a**nd **res**ources library for REST APIs.



What is the project about? Collares is a Web API class library for ASP.NET Core to build a restful API with .NET/C#.  This document describes the idea behind it.



Collares is used in the project [OteaMate](https://github.com/mrstefangrimm/opteamate) which has a open [API](https://opteamate.dynv6.net/swagger/index.html) and is open source. Collares is written in C# and uses .NET 5.0.



Is Collares for you? Collares is for Hobbyist and small projects.



Motivation, why the project exists? The project started from one single class that defined a typed response type for a REST service. It is not an overwhelming library and is useful for small projects with a separation of domain and API classes.

```C#
// This is an illustration and not how it is currently implemented
public class WebApiResponseBase<TDATA> where TDATA : new() {
  public string Type { get; set; }
  public TDATA Data { get; set; } = new TDATA();
  public IDictionary<string, string> Hrefs { get; set; } = new Dictionary<string, string>();
}
```



Features: TBD.



## Web API Response Types

By definition, the response of a Web API should not change its form. With Collares, the response types 'Collection' and 'Info' have a sealed form whereas the type 'Resource' is extendable.

| Property |                                                      |
| -------- | ---------------------------------------------------- |
| Type     | is always either 'Collection', 'Resource' or 'Info'. |
| HRefs    | is a dictionary of possible actions.                 |
| Id       | is the unique resource id.                           |
| Data     | is the payload respectively  a value object.         |



![Logic-Composite](./Res/Logic-Composite.jpg)





Web API Response Types and Data Payload Objects

### Type Resource Response

A resource has a unique Id. A resource can be directly accessed and patched or deleted.

#### Example

Return the shopping list with Id "4".
Note that the Data is not the items collection but a value object describing the shopping list.

Query: `GET http://../shoppinglists/4`
Output:

```json
{ "Type":"Resource",
  "Id":4,
  "Data":
  { "Shopper":"S. Hopper",
    "CreationTime":"2021-01-03T15:59:57.9366648+01:00"
  },
  "Hrefs":{"delete":"/api/shoppinglists/4"},
  "Items":
  { "Type":"Collection",
    "Data":
    [
      { "Type":"Resource",
        "Id":1,
        "Data":{"Product":"apples","Price":3.49},
        "Hrefs":{}
      }, 
      { "Type":"Resource",
        "Id":2,
        "Data":{"Product":"pears","Price":2.99},
        "Hrefs":{}
      }
    ],
    "Hrefs":{}
  }  
}
```


### Type Collection Response

A collection hasn't an Id. Data is a collection of Resource-Responses.

#### Example

Return all the items of the shopping list with Id "4".

Query: `GET http://../shoppinglists/4/items`
Output:

```json
{ "Type":"Collection",
  "Data":
  [
    { "Type":"Resource",
      "Id":1,
      "Data":{"Product":"apples","Price":3.49},
      "Hrefs":{"delete":"/api/shoppinglists/4/items/1"}
    },
    { "Type":"Resource",
      "Id":2,
      "Data":{"Product":"pears","Price":2.99},
      "Hrefs":{"delete":"/api/shoppinglists/4/items/2"}
    }
  ],
  "Hrefs":{"post":"/api/shoppinglists/4/items"}
}
```

### Type Info Response

An info response hasn't an Id. Data is a value object. The HRef "post" of the info object indicates that items can be added.

#### Example 1

Return the info with the total price of the shopping list with Id "4".

Query: `GET http://../shoppinglists/4/items/info`
Output:

```json
{ "Type":"Info",
  "Data":
  { "TotalPrice":6.48,
    "AvgPrice":3.24
  },
  "Hrefs":{}

}
```

#### Example 2

Return the information if shopping list can be added. 
The query `GET http://../shoppinglists` would return the whole Database. The solution is an info response that  just returns the information you need:

Query: `GET http://../shoppinglists/info`
Output:

```json
{ "Type":"Info",
  "Data":{},
  "Hrefs":{"post":"/api/shoppinglists/info"}
}
```



## How to use in the ApiController

Given a ASP.NET Core Web Application that manages shopping lists.

- `curl GET GET http://../api/shoppinglists | json`
  Returns all the stored shopping lists in the system (can be thousands).
- `curl GET GET http://../api/shoppinglists/1 | json`
  Gets the shopping list with some information about the list and a collection of items on the list.
- `curl GET GET http://../api/shoppinglists/info | json`
  Gets information on the controller, namely if adding new shopping lists is supported
- `curl GET GET http://../api/shoppinglists/4/items/info | json`
  Gets information on the items on the lists, e.g. the total price.



```c#
using Collares;

using ShoppingListsInfoResponse = WebApiInfoResponse<object>;
using ShoppingListItemsInfoResponse = WebApiInfoResponse<ItemsInfo>;

[Route("api/[controller]")]
[ApiController]
public class ShoppingListsController : ControllerBase {
  [HttpGet("info")]
  public IActionResult GetShoppingListsInfo() {
    var response = new ShoppingListsInfoResponse();
    // PostEvent
    response.AddHref(HrefType.Post, "api/shoppinglists");
    return Ok(response);
  }
  [HttpGet]
  public IActionResult GetShoppingLists() {
    if (authorized) {
        // ... the point here is that an info response is needed.
    }
    return Unauthorized();    
  }   
  [HttpGet("{id}")]
  public IActionResult GetShoppingList(long id) {
    var dbObj = _context.Events.Find(id);
    if (dbObj == null) { return NotFound(); }
    _context.Entry(dbObj).Collection(e => e.Items).Load();
      
    var response = new EventResponse() { Id = dbObj.Id };
    response.Data.CopyFrom(dbObj);
    foreach (var itemDbObj in dbObj.Items) {
      var itemResponse = new RegistrationResponse { Id = itemDbObj.Id };
      response.Data.CopyFrom(dbo);
      response.Items.Data.Add(itemResponse);
    }

    var evtDto = CreateEventResponse(evtDbo);
    return Ok(response);
  }

  [HttpGet("{id}/items/info")]
  public async Task<IActionResult> GetShoppingListItemsInfo(long id) {
    var dbObj = _context.ShoppingLists.Find(id);
    if (dbObj == null) { return NotFound();
    _context.Entry(dbObj).Collection(e => e.Items).Load();
      
    var response = new ShoppingListItemsInfoResponse();
    response.Data.TotalPrice = CalculateTotal(dbObj);
    response.Data.AvgPrice = CalculateAverage(dbObj);
    if (listIsOpen) {
      response.AddHref(HrefType.Post, $"api/shoppinglists/{id}/items");
    }
    return Ok(response);
  }

}
```



## License

<a rel="license" href="http://creativecommons.org/licenses/by-sa/4.0/"><img alt="Creative Commons License" style="border-width:0" src="https://i.creativecommons.org/l/by-sa/4.0/88x31.png" /></a>

This work is licensed under the Creative Commons Attribution-ShareAlike 4.0 International License. To view a copy of this license, visit http://creativecommons.org/licenses/by-sa/4.0/ or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA.





