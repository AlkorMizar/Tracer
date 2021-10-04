using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tracer2.TracerAPI.Serializer;
using Tracer2.TracerAPI.Data;

namespace Tracer2.TracerAPI.Serializer.Tests
{
    [TestClass()]
    public class SerializerXmlTests
    {
        [TestMethod()]
        public void SerializeTest()
        {
            string expexted = System.IO.File.ReadAllText(@"D:\Projects\C#\Tracer2\TestXML.txt");

            TraceResult traceResult = new TraceResult();
            MethodInfo info1 = new MethodInfo("MethA", "ClassA", 1);
            MethodInfo info2 = new MethodInfo("MethB", "ClassA", 1);
            MethodInfo info3 = new MethodInfo("MethB", "ClassA", 14);
            MethodInfo info4 = new MethodInfo("MethC", "ClassA", 1);
            MethodInfo info5 = new MethodInfo("MethA", "ClassA", 15);

            traceResult.AddNewMethod(info1, 1, null, 10000);
            traceResult.StopMetod(info1, 1, null, 20000);

            traceResult.AddNewMethod(info1, 2, null, 10000);
            traceResult.AddNewMethod(info2, 2, new MethodInfo[] { info1 }, 10000);
            traceResult.AddNewMethod(info4, 2, new MethodInfo[] { info1, info2 }, 10000);
            traceResult.StopMetod(info4, 2, new MethodInfo[] { info1, info2 }, 90000);
            traceResult.StopMetod(info2, 2, new MethodInfo[] { info1 }, 1110000);
            traceResult.AddNewMethod(info3, 2, new MethodInfo[] { info1 }, 15000);
            traceResult.AddNewMethod(info4, 2, new MethodInfo[] { info1, info3 }, 10000);
            traceResult.StopMetod(info4, 2, new MethodInfo[] { info1, info3 }, 90000);
            traceResult.StopMetod(info3, 2, new MethodInfo[] { info1 }, 215000);
            traceResult.StopMetod(info1, 2, null, 3330000);
            traceResult.AddNewMethod(info5, 2, null, 10000);
            traceResult.StopMetod(info5, 2, null, 120000);

            var ser = SerializerXml.GetInstance();
            string actual = ser.Serialize(traceResult);
        }
    }
}