using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;


namespace IPZ_1
{
    [Serializable]
    public class Logs
	{
        public string Action { get; set; }
        public string UserName { get; set; }
        public string Time { get; set; }
		
		public Logs(string time, string userName, string action) { Time = time; UserName = userName; Action = action; }


    }


    public static class Serialize
    {
        public static void SerializeJson<T>(string fileName, List<T> data)
        {
            var json = new DataContractJsonSerializer(data.GetType());

            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                json.WriteObject(file, data);
            }
        }

        public static List<T> DeserializeJson<T>(string fileName, Type type)
        {
            if (!File.Exists(fileName))
                return null;

            var json = new DataContractJsonSerializer(type);

            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                return json.ReadObject(file) as List<T>;
            }
        }

        public static void AddLogAction<T>(T log, string fileName, Type type)
        {
            var logsList = DeserializeJson<T>(fileName, type);
            if (logsList != null)
            {
                logsList.Add(log);
                SerializeJson<T>(fileName, logsList);
            }
        }

    }
}
