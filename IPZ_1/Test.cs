using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text.Json;

namespace IPZ_1
{
	[Serializable]
	public class Test
	{
		public string Name { get; set; }
		public string Action { get; set; }

		public Test() { }
		public Test(string name, string action) {  Name = name; Action = action; }


		public static void SerializeJson<T> (string fileName, List<T> data)
        {
            var json = new DataContractJsonSerializer(data.GetType());

            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                json.WriteObject(file, data);
            }
        }

        public static List<T> DeserializeJson<T>(string fileName, Type result)
        {
            if(!File.Exists(fileName))
                return null;

            var json = new DataContractJsonSerializer(result);

            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                return json.ReadObject(file) as List<T>;
            }
        }


    }
}
