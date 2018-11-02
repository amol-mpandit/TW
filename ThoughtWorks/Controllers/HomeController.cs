using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ThoughtWorks.Controllers
{
    public class HomeController : Controller
    {
        public string Uri = "https://http-hunt.thoughtworks-labs.net/challenge/";
        
        public void Get()
        {
            string uri = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri + "input");
            request.Headers.Add("userid", "MFUksCI1q");
            

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
                if (stream != null)
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        var question = reader.ReadToEnd();
                        var result = Process(question);
                        Post(result);
                    }
        }

        public string[] Process(string input)
        {
            var json = JsonConvert.DeserializeObject<Question>(input);
            var hiddenTools = json.HiddenTools;
            var tools = json.Tools;
            var foundTools = new List<string>();
            foreach (var word in tools)
            {
                bool notFound = true;
                foreach (char charater in word)
                {
                    if (!hiddenTools.Contains(charater.ToString()))
                    {
                        notFound = true;
                        break;
                    }
                }

                if (!notFound)
                {
                    foundTools.Add(word);
                }
            }
            return foundTools.ToArray();

        }

        public void Post(string[] data)
        {
            
            var foundTools = new FoundTools
            {
                Tools = data
            };
            var toolsFound = JsonConvert.SerializeObject(foundTools);
            byte[] dataBytes = Encoding.UTF8.GetBytes(toolsFound);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Uri + "output");
            request.ContentLength = dataBytes.Length;
            request.ContentType = "application/json; charset=utf-8";
            request.Method = "POST";
            request.Headers.Add("userid", "MFUksCI1q");

            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(dataBytes, 0, dataBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
                if (stream != null)
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        var result = reader.ReadToEnd();
                    }
        }
    }

    public class Question
    {
        public string HiddenTools { get; set; }
        public string[] Tools { get; set; }
    }

    public class FoundTools
    {
        public string[] Tools { get; set; }
    }
}

