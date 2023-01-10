using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Batch_Rename
{
    public class JsonUtils
    {
        public static JsonSerializerOptions _options =
            new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        public static string Convert2Json(object obj)
        {
            var options = new JsonSerializerOptions(_options)
            {
                WriteIndented = true
            };
            string reuslt = System.Text.Json.JsonSerializer.Serialize(obj, options);
            return reuslt;
        }
        public static void WriteJson(object obj, string fileName)
        {
            var jsonString = Convert2Json(obj);
            File.WriteAllText(fileName, jsonString);
        }

        public static object LoadJson(string filename)
        {
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            using (StreamReader r = new StreamReader(filename))
            {
                string json = r.ReadToEnd();

                items = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
            }
            return items;
        }

    }
}
