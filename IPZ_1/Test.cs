using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace IPZ_1
{
	[Serializable]
	public class Test
	{
		public string Name { get; set; }
		public int Number { get; set; }

		public Test() { }
		public Test(string name, int number) {  Name = name; Number = number; }

		public static void ToFile(Test args, string FileName) 
		{
			if (FileName == string.Empty || args == null)
				throw new ArgumentNullException();

			using(var file = new System.IO.StreamWriter(FileName)) 
			{
				file.WriteLine("{0}; {1}",
					args.Name, args.Number);
			}
		}

		public static void SerializeJson<T> (string fileName, IEnumerable<T> data)
		{
			var json = new DataContractJsonSerializer(data.GetType());

			using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
			{
				json.WriteObject(file, data);
			}
		}


	}
}
