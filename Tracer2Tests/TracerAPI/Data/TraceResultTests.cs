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
    public class TraceResultTests
    {
        TraceResult traceResult;
        MethodInfo info1, info2;

        [TestInitialize]
        public void Initialize()
        {
            traceResult = new TraceResult();
            info1 = new MethodInfo("1", "1", 1);
            info2 = new MethodInfo("2", "2", 2);
            GetThreadTest_General_ThreadWithRightId();
        }

        [TestMethod()]
        public void AddNewMethodTest_AddNodeToNewThread()
        {
            traceResult.AddNewMethod(info1, 12, null, 10000);

            ThreadNode thread = traceResult.GetThread(12);
            Assert.IsTrue(thread != null &&
                           thread.GetMethod(new MethodInfo[] { info1 }) != null);
        }

        [TestMethod()]
        public void AddNewMethodTest_AddNodeToExistingThread()
        {
            AddNewMethodTest_AddNodeToNewThread();
            ThreadNode thread = traceResult.GetThread(12);

            traceResult.AddNewMethod(info2, 12, new MethodInfo[] { info1 }, 10000);

            Assert.IsTrue(thread != null &&
                          thread.GetMethod(new MethodInfo[] { info1, info2 }) != null);
        }

        [TestMethod()]
        public void GetThreadTest_General_ThreadWithRightId()
        {
            AddNewMethodTest_AddNodeToNewThread();

            ThreadNode thread = traceResult.GetThread(12);

            Assert.IsTrue(thread.Id == 12);
        }

        [TestMethod()]
        public void StopMetodTest_General_DeactivatedNode()
        {
            bool result;
            AddNewMethodTest_AddNodeToNewThread();
            ThreadNode thread = traceResult.GetThread(12);

            traceResult.StopMetod(info1, 12, null, 20000);

            result = thread != null && !thread.GetMethod(new MethodInfo[] { info1 }).IsActive;
            Assert.IsTrue(result);
        }
    }
}