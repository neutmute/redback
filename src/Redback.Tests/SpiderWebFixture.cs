using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Redback.Tests.TestClasses;
using UnitTests.TestClasses;

namespace Redback.Tests
{
    public class SpiderWebFixture : Fixture
    {
        [Test]
        public void List()
        {
            var sut = new SpiderWeb();
            var data = new MonthArray();
            var resultCount = 0;

            sut.Traverse(data, result =>
            {
                if (resultCount == 1)
                {
                    Assert.That(result.Target.Value is List<int>);
                }
                resultCount++;
            });
        }

        /// <summary>
        /// Not sure why it seems to recruse back out...needs investigation
        /// </summary>
        [Test]
        public void Chain()
        {
            var data = ParentChain.GetGrandFatherSample(); 
            var sut = new SpiderWeb();
            var sb = new StringBuilder();
            sut.Traverse(data, result => sb.AppendFormat("{0}|", result.Target.Value));

            Console.WriteLine(sb.ToString());
            Assert.AreEqual("greatGrandfather|grandfather|father|son|||son|son|father|father|grandfather|grandfather|greatGrandfather|", sb.ToString());
        }
    }
}
