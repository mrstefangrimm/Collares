// Copyright (c) 2020-2021 Stefan Grimm. All rights reserved.
// Licensed under the GPL. See LICENSE file in the project root for full license information.
//
using Collares.Base;
using System.Collections.Generic;

namespace Collares {

  public abstract class WebApiResourceResponse<TDATA> : WebApiResourceResponseBase<TDATA> where TDATA : new() {
    public override string Type => WebApiResponseType.Resource.ToString();
    public override TDATA Data { get; set; } = new TDATA();
  }

  public sealed class WebApiCollectionResponse<TRESPONSE, TDATA> : WebApiCollectionResponseBase<TRESPONSE, TDATA> where TRESPONSE : WebApiResourceResponse<TDATA> where TDATA : new() {
    public override ICollection<TRESPONSE> Data { get; set; } = new List<TRESPONSE>();
  }

  public sealed class WebApiInfoResponse<TDATA> : WebApiInfoResponseBase<TDATA> where TDATA : new() {
    public override string Type => WebApiResponseType.Info.ToString();
    public override TDATA Data { get; set; } = new TDATA();
  }

}
