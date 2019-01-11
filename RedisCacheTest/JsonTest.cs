using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace RedisCacheTest
{
    [TestClass]
    public class JsonTest
    {
        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        [TestMethod]
        public void JsonEmpty()
        {
            var jsonString = JsonConvert.SerializeObject(new TestObject()
            {
                Name = string.Empty
            }, serializerSettings);

            Console.WriteLine(jsonString);
        }

        class TestObject
        {
            public string Name { get; set; }
            public string Password { get; set; }
        }
    }
}
