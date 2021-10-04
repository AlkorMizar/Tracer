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
    public class ThreadNodeTests
    {
        ThreadNode thread;
        MethodInfo info1,info2;

        [TestInitialize]
        public void Initialize()
        {
            thread = new ThreadNode(11, 0);
            info1 = new MethodInfo("1", "1", 1);
            info2 = new MethodInfo("2", "2", 2);
            GetMethodTest_SerchForNodeInRoot_NotNullNode();
            GetMethodTest_SerchForNodeInnerMethod_NotNullNode();
        }

        //add node on first level of thread(directly in root)
        // then find it
        [TestMethod()]
        public void StartNewMethodTest_AddOne_NotNullElemInStruct()
        {
            thread.StartNewMethod(info1, null, 10000);

            MethodNode method = thread.GetMethod(new MethodInfo[] { info1 });
            Assert.IsTrue(method != null && method.IsThatMethod(info1)) ;
        }

        //add node with existing path
        // then find it
        [TestMethod()]
        public void StartNewMethodTest_AddOnInnerLevelExistingPath_NotNullElemInStructResultTrue()
        {
            StartNewMethodTest_AddOne_NotNullElemInStruct();
            thread.StartNewMethod(info2, new MethodInfo[] { info1 }, 10000);

            thread.StartNewMethod(info1, new MethodInfo[] { info1,info2 }, 10000);

            MethodNode method = thread.GetMethod(new MethodInfo[] { info1, info2, info1 });
            Assert.IsTrue(method != null && method.IsThatMethod(info1));

        }

        //add node with no existing path
        // then find it
        [TestMethod()]
        public void StartNewMethodTest_AddOnInnerLevelNoExistingPath_NotNullElemsInStruct()
        {
            bool result = false;

            thread.StartNewMethod(info2, new MethodInfo[] { info1}, 10000);

            MethodNode method1 = thread.GetMethod(new MethodInfo[] { info1, info2});
            MethodNode method2 = thread.GetMethod(new MethodInfo[] { info1 });
            result = method1 != null && method2 != null &&
                   method1.IsThatMethod(info2) && method2.IsThatMethod(info1); 
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void StopMethodTest_StopNodeInRoot_DeactivateNode()
        {
            StartNewMethodTest_AddOne_NotNullElemInStruct();

            thread.StopMethod(info1, null, 10000);

            Assert.IsFalse(thread.GetMethod(new MethodInfo[] { info1 }).IsActive);
        }

        public void StopMethodTest_StopNodeInnnerMethod_DeactivateNode()
        {
            StartNewMethodTest_AddOnInnerLevelNoExistingPath_NotNullElemsInStruct();

            thread.StopMethod(info2, new MethodInfo[] { info1 }, 10000);

            Assert.IsTrue(thread.GetMethod(new MethodInfo[] { info1,info2 }).IsActive);
        }

        [TestMethod()]
        public void GetMethodTest_SerchForNodeInRoot_NotNullNode()
        {
            bool result = false;

            thread.StartNewMethod(info1, null, 10000);

            result = thread.GetMethod(new MethodInfo[] { info1 }).IsThatMethod(info1);
            Assert.IsTrue(result);
        }

        public void GetMethodTest_SerchForNodeInnerMethod_NotNullNode()
        {
            bool result = false;
            thread.StartNewMethod(info2, new MethodInfo[] { info1 }, 10000);

            result = thread.GetMethod(new MethodInfo[] { info1,info2 }).IsThatMethod(info2);
            Assert.IsTrue(result);
        }
    }
}