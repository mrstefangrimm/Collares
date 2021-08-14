// Copyright (c) 2020 Stefan Grimm. All rights reserved.
// Licensed under the GPL. See LICENSE file in the project root for full license information.
//
using Collares.Base;

namespace Collares {

  public enum PredefinedWebApiResponseType {
    Resource,
    Collection,
    Info
  }

  public class WebApiResponseType {
    public static Val2Type<PredefinedWebApiResponseType> Resource = new Val2Type<PredefinedWebApiResponseType>(PredefinedWebApiResponseType.Resource);
    public static Val2Type<PredefinedWebApiResponseType> Collection = new Val2Type<PredefinedWebApiResponseType>(PredefinedWebApiResponseType.Collection);
    public static Val2Type<PredefinedWebApiResponseType> Info = new Val2Type<PredefinedWebApiResponseType>(PredefinedWebApiResponseType.Info);
  }

  public enum GenericHrefType {
    Self,
    Get,
    Post,
    Put,
    Patch,
    Delete
  }
  
  public class HrefType {
    public static Val2Type<GenericHrefType> Self = new Val2Type<GenericHrefType>(GenericHrefType.Self);
    public static Val2Type<GenericHrefType> Get = new Val2Type<GenericHrefType>(GenericHrefType.Get);
    public static Val2Type<GenericHrefType> Post = new Val2Type<GenericHrefType>(GenericHrefType.Post);
    public static Val2Type<GenericHrefType> Put = new Val2Type<GenericHrefType>(GenericHrefType.Put);
    public static Val2Type<GenericHrefType> Patch = new Val2Type<GenericHrefType>(GenericHrefType.Patch);
    public static Val2Type<GenericHrefType> Delete = new Val2Type<GenericHrefType>(GenericHrefType.Delete);
  }

  public static class WebApiUtil {
    public static void AddHref<THREF>(this WebApiResponse target, Val2Type<THREF> type, string value) {
      target.Hrefs.Add(type.ToString().ToLower(), value?.ToLower());
    }
  }

  public class Val2Type<T> {
    public Val2Type(T val) {
      Val = val;
    }
    public override string ToString() {
      return Val.ToString();
    }
    public T Val { get; }
  }

}