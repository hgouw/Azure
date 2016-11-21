using System;
using System.Diagnostics;
using System.Xml.Xsl;

namespace Azure.Transform
{
    class Program
    {
        static void Main()
        {
            var watch = Stopwatch.StartNew();

            var transform = new XslTransform();
            transform.Load(@"F:\URJ\breakfast.xsl");
            transform.Transform(@"F:\URJ\breakfast.xml", @"F:\breakfast1.html");

            watch.Stop();
            Console.WriteLine("XslTransform: " +  watch.ElapsedMilliseconds.ToString() + "ms");

            var compiledTransform = new XslCompiledTransform();
            compiledTransform.Load(@"F:\URJ\breakfast.xsl");
            compiledTransform.Transform(@"F:\URJ\breakfast.xml", @"F:\breakfast2.html");

            watch = Stopwatch.StartNew();
            watch.Stop();
            Console.WriteLine("XslCompiledTransform: " + watch.ElapsedMilliseconds.ToString() + "ms");

            Console.ReadLine();
        }
    }
}