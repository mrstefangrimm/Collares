// Copyright (c) 2020 Stefan Grimm. All rights reserved.
// Licensed under the GPL. See LICENSE file in the project root for full license information.
//
using System.Linq;
using System.Reflection;

namespace Collares {

  public static class ObjectExt {
    public static T1 CopyFrom<T1, T2>(this T1 destination, T2 source) where T1 : class where T2 : class {

      PropertyInfo[] srcFields = source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
      PropertyInfo[] destFields = destination.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);

      foreach (var property in srcFields) {
        var dest = destFields.FirstOrDefault(x => x.Name == property.Name);
        if (dest != null && dest.CanWrite) {
          dest.SetValue(destination, property.GetValue(source, null), null);
        }
      }

      return destination;
    }
  }
}
