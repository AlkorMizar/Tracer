using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tracer2.TracerAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer2.TracerAPI.Data.Tests
{
    [TestClass()]
    public class MethodNodeTests
    {
        MethodNode method;

        [TestInitialize]
        public void Initialyze()
        {
            method = new MethodNode(new MethodInfo("a", "A", 1), 100000);
        }

        [TestMethod()]
        public void ShouldSerializeInnerMethodsTest_EmtyInnerMethods_ResultFalse()
        {
            bool result = method.ShouldSerializeInnerMethods();
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void ShouldSerializeInnerMethodsTest_HasInnerMethods_ResultTrue()
        {
            method.AddInnerMethod(new MethodNode(new MethodInfo("b", "C", 3)));

            bool result = method.ShouldSerializeInnerMethods();
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void AddInnerMethodTest_AddOneNode_AddedeNotNull()
        {
            MethodInfo info = new MethodInfo("b", "C", 3);
            MethodNode addedNode;

            method.AddInnerMethod(new MethodNode(info));
            addedNode = method.GetInnerMethod(info);

            Assert.IsNotNull(addedNode);
        }

        [TestMethod()]
        public void AddInnerMethodTest_AddSeveralNode_AddedeNotNull()
        {
            MethodInfo[] infos = { new MethodInfo("b", "C", 3), new MethodInfo("d", "C", 33),
                                  new MethodInfo("bs", "Cs", 23), new MethodInfo("dd", "Cd", 145)  };
            bool isNotNull = true;
            foreach (var info in infos)
            {
                method.AddInnerMethod(new MethodNode(info));
            }
            foreach (var info in infos)
            {
                isNotNull &= method.GetInnerMethod(info) != null;
            }

            Assert.IsTrue(isNotNull);
        }

        [TestMethod()]
        public void StopTest()
        {
            long expected = 10;
            method.Stop(200000);

            Assert.AreEqual(expected, method.Time);
        }

        [TestMethod()]
        public void GetInnerMethodTest()
        {
            MethodInfo info = new MethodInfo("b", "C", 3);
            bool result;
            method.AddInnerMethod(new MethodNode(info));
            
            result = method.GetInnerMethod(info).IsThatMethod(info);

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsThatMethodTest_SameData_ResultTrue()
        {
            bool result = method.IsThatMethod(new MethodInfo("a", "A", 1));
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsThatMethodTest_DiferentData_ResultFalse()
        {
            bool result = method.IsThatMethod(new MethodInfo("A", "A", 1));
            Assert.IsFalse(result);
        }
    }
}