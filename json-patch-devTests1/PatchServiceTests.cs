using Microsoft.VisualStudio.TestTools.UnitTesting;
using json_patch_dev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace json_patch_dev.Tests
{
    [TestClass()]
    public class PatchServiceTests
    {

        [TestMethod()]
        public void CompareTest()
        {
            PatchService patchService = new PatchService();

            var objA = new {
                scalar=1,
                array=new [] {"hello" },
                obj= new { a=1, b=2, c=3, d=new { x=1,y=2 } }
                };

            var objB = new {
                scalar=2,
                array=new [] {"hello","goodbye" },
                obj= new { a=1, b=0, d=new { x=1,y=3 }, e=5 }
                };

            var jsonA = JsonSerializer.Serialize(objA);
            var jsonB = JsonSerializer.Serialize(objB);

            var patch = patchService.Compare(jsonA, jsonB);

            Console.WriteLine(patch);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(patch));
        }

        [TestMethod()]
        public void PatchTest()
        {
            PatchService patchService = new PatchService();

            var objA = new {
                scalar=1,
                array=new [] {"hello" },
                obj= new { a=1, b=2, c=3 }
                };

            var patch = @"[
                {
                ""op"": ""replace"",
                ""path"": ""/scalar"",
                ""value"": 2
                },
                {
                ""op"": ""add"",
                ""path"": ""/array/1"",
                ""value"": ""goodbye""
                },
                {
                ""op"": ""remove"",
                ""path"": ""/obj/c""
                },
                {
                ""op"": ""add"",
                ""path"": ""/obj/d"",
                ""value"": 4
                },
                {
                ""op"": ""replace"",
                ""path"": ""/obj/b"",
                ""value"": 0
                }
            ]";

            var jsonA = JsonSerializer.Serialize(objA);
            //var jsonPatch =  JsonSerializer.Serialize(patch);
            //Console.WriteLine(jsonPatch);

            var jsonB = patchService.Patch(jsonA, patch);

            Console.WriteLine(jsonB);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(jsonB));
        }
    }
}