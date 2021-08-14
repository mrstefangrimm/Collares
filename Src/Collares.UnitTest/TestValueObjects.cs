// Copyright (c) 2021 Stefan Grimm. All rights reserved.
// Licensed under the GPL. See LICENSE file in the project root for full license information.
//
using System;

namespace Collares.UnitTest.TestModel {

  class ShoppingItem {
    public string Product { get; set; }
    public decimal Price { get; set; }
  }

  class ShoppingList {
    public string Shopper { get; set; }
    public DateTime CreationTime { get; set; }
  }

  class ShoppingListItemsInfo {
    public decimal TotalPrice { get; set; }
    public decimal AvgPrice { get; set; }
  }
}
