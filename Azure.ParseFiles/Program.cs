using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Azure.ParseFiles
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Started parsing files");
            ParseFiles();
            Console.WriteLine("Successfully parsing files");
            Console.ReadLine();
        }

        private static void ParseFiles()
        {           
            var files = Directory.GetFiles(ConfigurationManager.AppSettings["DataFolder"]);
            Array.Sort(files, StringComparer.InvariantCulture);
            var caseDocs = new List<CaseDoc>();
            foreach (var file in files)
            {
                if (file.ToLower().EndsWith(".xml", StringComparison.CurrentCulture))
                {
                    var parser = new Parser(file);
                    caseDocs.Add(parser.ParseXml());
                }
            }
        }
    }
}
