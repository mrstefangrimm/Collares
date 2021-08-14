// Copyright (c) 2020-2021 Stefan Grimm. All rights reserved.
// Licensed under the GPL. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;

namespace Collares.Base {

  public abstract class WebApiResponseBase<TDATA> : WebApiResponse {
    public abstract string Type { get; }
    public abstract TDATA Data { get; set; }
    public IDictionary<string, string> Hrefs { get; set; } = new Dictionary<string, string>();
  }

  public class WebApiResourceResponseBase<TDATA> : WebApiResponseBase<TDATA> {
    public override string Type => WebApiResponseType.Resource.ToString();
    public override TDATA Data { get; set; }

    public long Id { get; set; }
  }

  public class WebApiCollectionResponseBase<TRESPONSE, TDATA> : WebApiResponseBase<ICollection<TRESPONSE>> {
    public override string Type => WebApiResponseType.Collection.ToString();
    public override ICollection<TRESPONSE> Data { get; set; }
  }

  public class WebApiInfoResponseBase<TDATA> : WebApiResponseBase<TDATA> {
    public override string Type => WebApiResponseType.Info.ToString();
    public override TDATA Data { get; set; }
  }

}
