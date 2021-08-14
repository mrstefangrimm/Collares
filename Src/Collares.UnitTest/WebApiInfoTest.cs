// Copyright (c) 2021 Stefan Grimm. All rights reserved.
// Licensed under the GPL. See LICENSE file in the project root for full license information.
//
using Collares.Base;
using Collares.UnitTest.TestModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;


namespace Collares.UnitTest.Info {
  using ShoppingListInfoResponse = WebApiInfoResponse<object>;
  using ShoppingListItemsInfoResponse = WebApiInfoResponse<ShoppingListItemsInfo>;

  class InfoResponseNotRecommended : WebApiInfoResponseBase<ShoppingListItemsInfo> {
  }
  class InfoResponseNotRecommendedWithoutData : WebApiInfoResponseBase<object> {
  }
  
  [TestClass]
  public class WebApiInfoTest {
    [TestMethod]
    public void TestMethod1() {

      var resp1 = new ShoppingListItemsInfoResponse();
      resp1.Data.TotalPrice = 6.48m;
      resp1.Data.AvgPrice = 3.24m;

      var resp2 = new ShoppingListInfoResponse();

      var resp3 = new InfoResponseNotRecommended() {
        Data = new ShoppingListItemsInfo { TotalPrice = 6.48m, AvgPrice = 3.24m }
      };

      var resp4 = new InfoResponseNotRecommendedWithoutData();

      string jsonResp1 = JsonSerializer.Serialize(resp1);
      string jsonResp2 = JsonSerializer.Serialize(resp2);
      string jsonResp3 = JsonSerializer.Serialize(resp3);
      string jsonResp4 = JsonSerializer.Serialize(resp4);

      var desResp1 = JsonSerializer.Deserialize<ShoppingListItemsInfoResponse>(jsonResp1);
      var desResp2 = JsonSerializer.Deserialize<ShoppingListInfoResponse>(jsonResp2);
      var desResp3 = JsonSerializer.Deserialize<InfoResponseNotRecommended>(jsonResp3);
      var desResp4 = JsonSerializer.Deserialize<InfoResponseNotRecommendedWithoutData>(jsonResp4);

      Assert.AreEqual(6.48m, desResp1.Data.TotalPrice);
      Assert.AreEqual(3.24m, desResp1.Data.AvgPrice);
      Assert.IsNotNull(desResp2.Data);
      Assert.AreEqual(6.48m, desResp3.Data.TotalPrice);
      Assert.AreEqual(3.24m, desResp3.Data.AvgPrice);
      Assert.IsNull(desResp4.Data);
    }

    [TestMethod]
    public void TestMethod2() {

      const string hrefPost = "/api/shoppinglists/info";

      var resp2 = new ShoppingListInfoResponse();
      resp2.AddHref(HrefType.Post, hrefPost);
     
      string jsonResp2 = JsonSerializer.Serialize(resp2);

      var desResp2 = JsonSerializer.Deserialize<ShoppingListInfoResponse>(jsonResp2);

      Assert.IsNotNull(desResp2.Data);

      Assert.AreEqual(hrefPost, desResp2.Hrefs["post"]);
    }

  }
}
