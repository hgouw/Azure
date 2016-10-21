using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Azure.ParseFiles
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Started parsing the files");
            if (ParseFiles())
                Console.WriteLine("Successfully parsing the files");
            else
                Console.WriteLine("Unsuccessful in parsing the files");
            Console.ReadLine();
        }

        private static bool ParseFiles()
        {
            var ok = true;
            var files = Directory.GetFiles(ConfigurationManager.AppSettings["DataFolder"]);
            Array.Sort(files, StringComparer.InvariantCulture);
            var caseDocs = new List<CaseDoc>();
            foreach (var file in files)
            {
                if (file.ToLower().EndsWith(".xml", StringComparison.CurrentCulture))
                    if (!ParseXml(file))
                        ok = false;
            }
            return ok;
        }

        private static bool ParseXml(string file)
        {
            XmlReaderSettings settings = new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse };
            XmlReader reader = XmlReader.Create(file, settings);
            XDocument doc = null;
            try
            {
                doc = XDocument.Load(reader);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
