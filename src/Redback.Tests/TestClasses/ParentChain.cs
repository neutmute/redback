using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests.TestClasses
{
    public class ParentChain
    {
        #region Properties
        public ParentChain Child { get; set; }

        public string Name { get; set; }
        #endregion

        #region Constructors
        public ParentChain(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region Static Methods
        public static ParentChain GetGrandFatherSample()
        {
            var greatGrandfather = new ParentChain("greatGrandfather");
            var grandfather = new ParentChain("grandfather");
            var father = new ParentChain("father");
            var son = new ParentChain("son");
            
            greatGrandfather.Child = grandfather;
            grandfather.Child = father;
            father.Child = son;

            return greatGrandfather;
        }
        #endregion
    }
}
