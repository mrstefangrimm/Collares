// Copyright (c) 2023 Stefan Grimm. All rights reserved.
// Licensed under the GPL. See LICENSE file in the project root for full license information.
//
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Collares.UnitTest.ObjectExtension {

  [TestClass]
  public class ObjectExtTest {

    class Simple {
      public int M1 { get; set; }
      public int M2 { get; internal set; }
      internal int M3 { get; set; }
    }

    class Complex {
      public int M1 { get; set; }
      public int M2 { get; internal set; }
      internal int M3 { get; set; }
      public Simple M4 { get; set; }
    }

    class NotTheSame {
      public string M1 { get; set; }
      public int M2 { get; internal set; }
      internal int M3 { get; set; }
    }

    [TestMethod]
    public void CopyFrom_SimpleToSimpleClass_AllPublicPropertiesAreCopied() {

      // Arrange
      Simple source = new() { M1 = 1, M2 = 2, M3 = 3 };
      Simple target = new() { M1 = 0, M2 = 0, M3 = 0 };

      // Act
      target.CopyFrom(source);

      // Assert
      Assert.AreEqual(1, target.M1);
      Assert.AreEqual(2, target.M2); // Strange
      Assert.AreEqual(0, target.M3);

    }

    [TestMethod]
    public void CopyFrom_SimpleToNotTheSameClass_AllPublicPropertiesWithSameTypeAreCopied() {

      // Arrange
      Simple source = new() { M1 = 1, M2 = 2, M3 = 3 };
      NotTheSame target = new() { M1 = string.Empty, M2 = 0, M3 = 0 };

      // Act
      target.CopyFrom(source);

      // Assert
      Assert.AreEqual(string.Empty, target.M1);
      Assert.AreEqual(2, target.M2);
      Assert.AreEqual(0, target.M3);

    }

    [TestMethod]
    public void CopyFrom_ComplexToComplexClass_AllPublicPropertiesAreCopied() {

      // Arrange
      Simple m4 = new() { M1 = 1, M2 = 2, M3 = 3 };
      Complex source = new() { M1 = 1, M2 = 2, M3 = 3, M4 = m4 };
      Complex target = new() { M1 = 0, M2 = 0, M3 = 0, M4 = new () { M1 = 0, M2 = 0, M3 = 0 } };

      // Act
      target.CopyFrom(source);

      // Assert
      Assert.AreEqual(1, target.M1);
      Assert.AreEqual(2, target.M2);
      Assert.AreEqual(0, target.M3);
      Assert.ReferenceEquals(m4, target.M4);
    }

  }
}
