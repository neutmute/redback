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

        [Test]
        public void ChainRecursion()
        {
            Func<ParentLink, string, string> getParentTree = null;

            getParentTree = (p, t) =>
            {
                if (p == null)
                {
                    return string.Empty;
                }
                t = getParentTree(p.Request.Parent, t);
                return t + "." + p.MemberInfo.Name;
            };

            var sb = new StringBuilder();
            var destinationVar = "dest";
            var sourceVar = "src";

            TraversePointCallback callBack = t =>
            {
                var commentPrefix = string.Empty;

                if (t.Target.HitType == Enumerations.Member)
                {
                    var parentText = getParentTree(t.Parent, string.Empty);

                    if (t.Target.Type.IsEnum)
                    {
                        var destTypeName = $"Domain.Origination.Schema.{t.Target.Type.Name}";
                        sb.AppendLine(
                            $"{destinationVar}{parentText}.{t.Target.Name} = ({destTypeName}) Enum.Parse(typeof({destTypeName}), {sourceVar}{parentText}.{t.Target.Name}.ToString());");
                        return;
                    }

                    if (t.Target.Type != typeof(string))
                    {
                        commentPrefix = $"//Can't convert! {t.Target.Type.Name} !!\t";
                        return;
                    }

                    sb.AppendLine($"{commentPrefix}{destinationVar}{parentText}.{t.Target.Name} = {sourceVar}{parentText}.{t.Target.Name}; // {t.Target.Value}");
                }
            };

            var sut = new SpiderWeb();
            var data = ParentChain.GetGrandFatherSample();
            sut.Traverse(data, callBack);

            //Console.WriteLine(sb.ToString());

            Assert.AreEqual(@"dest.Child.Child.Child.Name = src.Child.Child.Child.Name; // son
dest.Child.Child.Name = src.Child.Child.Name; // father
dest.Child.Name = src.Child.Name; // grandfather
dest.Name = src.Name; // greatGrandfather
".Replace("\r\n", "\n"), sb.ToString().Replace("\r\n", "\n"));
        }
    }
}
