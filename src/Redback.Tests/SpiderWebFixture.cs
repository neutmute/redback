using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Redback.Tests.TestClasses;

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

            sut.Traverse(data, delegate(TraverseResult result)
            {
                if (resultCount == 1)
                {
                    Assert.That(result.Target.Value is List<int>);    
                }
                resultCount++;
            });
        }
    }
}
