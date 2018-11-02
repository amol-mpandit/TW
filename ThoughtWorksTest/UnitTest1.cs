using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ThoughtWorks.Controllers;

namespace ThoughtWorksTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestFoundTools()
        {
            var input = @"{""hiddenTools"":""opekandifehgujnsr"",""tools"":[""knife"",""guns"",""rope"",""xy"",""and"",""gjkn""]}";

            var result = Process(input);
            var expected = new [] { "knife", "guns","and" };
            CollectionAssert.AreEqual(result,expected);
        }

        public string[] Process(string input)
        {
            var json = JsonConvert.DeserializeObject<Question>(input);
            var hiddenTools = json.HiddenTools;
            var tools = json.Tools;
            var foundTools = new List<string>();
            foreach (var word in tools)
            {
                var found = false;
                var index = 0;
                foreach (var charater in word)
                {
                    if (hiddenTools.LastIndexOf(charater, index) > index)
                    {
                        found = true;
                        index = hiddenTools.IndexOf(charater);
                    }
                    else
                    {
                        found = false;
                    }
                }

                if (found)
                {
                    foundTools.Add(word);
                }
            }
            return foundTools.ToArray();

        }
    }
}
