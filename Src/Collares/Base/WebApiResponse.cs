// Copyright (c) 2020-2021 Stefan Grimm. All rights reserved.
// Licensed under the GPL. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;

namespace Collares.Base {

  public interface WebApiResponse {
    string Type { get; }
    IDictionary<string, string> Hrefs { get; }
  }
}
