using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrowserHistoryGatherer.UnitTest {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {
            var jObject = new JObject();
            
            jObject["prop"] = "3123";
            jObject["prop2"] = new JArray("Prop2", "3123");
            
            var childJObject = new JObject();
            
            var textWriter = new StreamWriter("1.json");
            var jsonWriter =  new JsonTextWriter(textWriter);
            
            jObject.WriteTo(jsonWriter);
            jsonWriter.Close();
            textWriter.Dispose();
        }
    }
}
