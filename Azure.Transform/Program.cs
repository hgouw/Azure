using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace Azure.Transform
{
    class Program
    {
        static void Main()
        {
            var watch = Stopwatch.StartNew();

            watch.Start();
            var transform = new XslTransform();
            transform.Load(@"F:\URJ\breakfast.xsl");
            transform.Transform(@"F:\URJ\breakfast.xml", @"F:\breakfast1.html");
            watch.Stop();
            Console.WriteLine("XslTransform: " + watch.ElapsedMilliseconds.ToString() + "ms");

            watch.Start();
            var compiledTransform = new XslCompiledTransform();
            compiledTransform.Load(@"F:\URJ\breakfast.xsl");
            compiledTransform.Transform(@"F:\URJ\breakfast.xml", @"F:\breakfast2.html");
            watch.Stop();
            Console.WriteLine("XslCompiledTransform: " + watch.ElapsedMilliseconds.ToString() + "ms");

            watch.Start();
            compiledTransform.Load(@"F:\URJ\breakfast.xsl");
            var sb = new StringBuilder();
            var xmlWriter = XmlWriter.Create(sb);
            compiledTransform.Transform(@"F:\URJ\breakfast.xml", xmlWriter);
            File.WriteAllText(@"F:\breakfast3.html", sb.ToString());
            watch.Stop();
            Console.WriteLine("XslCompiledTransform: " + watch.ElapsedMilliseconds.ToString() + "ms");

            Console.ReadLine();
        }
    }
}