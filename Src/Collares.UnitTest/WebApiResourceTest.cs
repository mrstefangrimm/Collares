// Copyright (c) 2021 Stefan Grimm. All rights reserved.
// Licensed under the GPL. See LICENSE file in the project root for full license information.
//
using Collares.Base;
using Collares.UnitTest.TestModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Collares.UnitTest.Resource {
  using ResourceResponseNotRecommended = WebApiResourceResponseBase<ShoppingList>;
  using ShoppingListItems = WebApiCollectionResponse<ShoppingItemResponse, ShoppingItem>;
  class ShoppingItemResponse : WebApiResourceResponse<ShoppingItem> {
  }
  class ShoppingListResponse : WebApiResourceResponse<ShoppingList> {
    public ShoppingListItems Items { get; set; } = new ShoppingListItems();
  }
  class ResourceResponseNotRecommended2 : WebApiResourceResponseBase<ShoppingList> {
    public ShoppingListItems Items { get; set; }
  }
  class ResourceResponseWrong : WebApiResourceResponseBase<string> {
    public ICollection<ShoppingItem> Items { get; set; }
  }

  [TestClass]
  public class WebApiResourceTest {
    [TestMethod]
    public void TestMethod1() {

      const string hrefDelete = "/api/shoppinglists/4";

      var item1Response = new ShoppingItemResponse { Id = 1 };
      item1Response.Data.Product = "apples";
      item1Response.Data.Price = 3.49m;
      var item2Response = new ShoppingItemResponse { Id = 2 };
      item2Response.Data.Product = "pears";
      item2Response.Data.Price = 2.99m;

      var resp1 = new ResourceResponseNotRecommended() { 
        Id = 1,
        Data = new ShoppingList() { Shopper = "S. Hopper" }
      };
      //resp1.Items.

      var resp2 = new ShoppingListResponse();
      resp2.Id = 4;
      resp2.Data.Shopper = "S. Hopper";
      resp2.Data.CreationTime = DateTime.Now;
      resp2.Items.Data.Add(item1Response);
      resp2.Items.Data.Add(item2Response);
      resp2.AddHref(HrefType.Delete, hrefDelete);

      var resp3 = new ResourceResponseNotRecommended2() {
        Id = 3,
        Data = new ShoppingList() { Shopper = "S. Hopper" },
        Items = new ShoppingListItems()
      };
      resp3.Items.Data.Add(item1Response);
      resp3.Items.Data.Add(item2Response);

      var resp4 = new ResourceResponseWrong() {
        Id = 3,
        Data = "S. Hopper",
        Items = new List<ShoppingItem>()
      };
      resp4.Items.Add(new ShoppingItem { Product = "apples", Price = 3.49m });
      resp4.Items.Add(new ShoppingItem { Product = "pears", Price = 2.99m });

      string jsonResp1 = JsonSerializer.Serialize(resp1);
      string jsonResp2 = JsonSerializer.Serialize(resp2);
      string jsonResp3 = JsonSerializer.Serialize(resp3);

      var desResp1 = JsonSerializer.Deserialize<ResourceResponseNotRecommended>(jsonResp1);
      var desResp2 = JsonSerializer.Deserialize<ShoppingListResponse>(jsonResp2);
      var desResp3 = JsonSerializer.Deserialize<ResourceResponseNotRecommended2>(jsonResp3);

      Assert.AreEqual("S. Hopper", desResp1.Data.Shopper);
      Assert.AreEqual("S. Hopper", desResp2.Data.Shopper);
      Assert.AreEqual("S. Hopper", desResp3.Data.Shopper);
      Assert.AreEqual("apples", desResp2.Items.Data.First().Data.Product);
      Assert.AreEqual("apples", desResp3.Items.Data.First().Data.Product);
      Assert.AreEqual("pears", desResp2.Items.Data.Last().Data.Product);
      Assert.AreEqual("pears", desResp3.Items.Data.Last().Data.Product);

    }
  }
}
