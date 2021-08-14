// Copyright (c) 2021 Stefan Grimm. All rights reserved.
// Licensed under the GPL. See LICENSE file in the project root for full license information.
//
using Collares.Base;
using Collares.UnitTest.TestModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Collares.UnitTest.Collection {
  using ShoppingListItemsResponse = WebApiCollectionResponse<ShoppingItemResponse, ShoppingItem>;
  class ShoppingItemResponse : WebApiResourceResponse<ShoppingItem> {
  }
  class CollectionResponseNotRecommended : WebApiCollectionResponseBase<ShoppingItemResponse, ShoppingItem> {
  }
  class CollectionResponseTestWrong : WebApiCollectionResponseBase<ShoppingItem, ShoppingItem> {
  }

  [TestClass]
  public class WebApiCollectionTest {

    [TestMethod]
    public void TestMethod1() {

      const string hrefPost = "/api/shoppinglists/4/items";

      var item1Response = new ShoppingItemResponse() {
        Id = 1
      };
      item1Response.Data.Product = "apples";
      item1Response.Data.Price = 3.49m;
      item1Response.AddHref(HrefType.Delete, hrefPost + "/1");

      var item2Response = new ShoppingItemResponse() {
        Id = 2
      };
      item2Response.Data.Product = "pears";
      item2Response.Data.Price = 2.99m;
      item2Response.AddHref(HrefType.Delete, hrefPost + "/2");

      var resp1 = new ShoppingListItemsResponse();
      resp1.Data.Add(item1Response);
      resp1.Data.Add(item2Response);
      resp1.AddHref(HrefType.Post, hrefPost);

      var resp2 = new CollectionResponseNotRecommended();
      resp2.Data = new List<ShoppingItemResponse>();
      resp2.Data.Add(item1Response);
      resp2.Data.Add(item2Response);
      resp2.AddHref(HrefType.Post, hrefPost);

      var resp3 = new CollectionResponseTestWrong();
      //resp3.Data.Add(item1);
      //resp3.Data.Add(item2);
      resp3.AddHref(HrefType.Post, hrefPost);

      string jsonResp1 = JsonSerializer.Serialize(resp1);
      string jsonResp2 = JsonSerializer.Serialize(resp2);

      var desResp1 = JsonSerializer.Deserialize<ShoppingListItemsResponse>(jsonResp1);
      var desResp2 = JsonSerializer.Deserialize<CollectionResponseNotRecommended>(jsonResp2);

      Assert.AreEqual(2, desResp1.Data.Count);
      Assert.AreEqual(2, desResp2.Data.Count);
      
      Assert.AreEqual("apples", desResp1.Data.First().Data.Product);
      Assert.AreEqual("apples", desResp2.Data.First().Data.Product);
      Assert.AreEqual("pears", desResp1.Data.Last().Data.Product);
      Assert.AreEqual("pears", desResp2.Data.Last().Data.Product);

      Assert.AreEqual(hrefPost, desResp1.Hrefs["post"]);
      Assert.AreEqual(hrefPost, desResp2.Hrefs["post"]);
    }
  }
}
