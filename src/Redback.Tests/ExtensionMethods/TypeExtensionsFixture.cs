using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Redback.ExtensionMethods;

namespace Redback.Tests
{
    public class TypeExtensions : Fixture
    {
        [TestCase(typeof(int?), "Nullable<Int32>")]
        [TestCase(typeof(int), "Int32")]
        public void Test(Type testType, string  expectedText)
        {
            Assert.AreEqual(expectedText, testType.ToGenericTypeString());
        }
    }
}
