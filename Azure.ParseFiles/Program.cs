using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
/*
using System.Text;
using System.Xml;
using System.Xml.Linq;
*/
using Azure.DbContexts;

namespace Azure.ParseFiles
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Started parsing the files");
            /*
            if (ParseFiles())
                Console.WriteLine("Successfully parsing the files");
            else
                Console.WriteLine("Unsuccessful in parsing the files");
            */
            var caseDocs = ParseCaseDocs();
            if (caseDocs.Count > 0)
            {
                var repoes = ConvertDocToRepo(caseDocs, "nsw-");
                Console.WriteLine("Successfully parsing the files");
                using (var db = new AzureDbContext())
                {
                    Console.WriteLine("Started saving the data");
                    db.CaseRepoes.AddRange(repoes);
                    db.SaveChanges();
                    Console.WriteLine("Successfully saving the data");
                }
            }
            Console.ReadLine();
        }

/*
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
            Console.WriteLine($"Parsing the file {file}");

            var settings = new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse };
            //var reader = XmlReader.Create(file, settings);
            var xml = File.ReadAllText(file);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            var reader = XmlReader.Create(stream, settings);
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
*/

        private static List<CaseDoc> ParseCaseDocs()
        {
            var caseDocs = new List<CaseDoc>();
            var files = Directory.GetFiles(ConfigurationManager.AppSettings["DataFolder"]);
            Array.Sort(files, StringComparer.InvariantCulture);
            foreach (var file in files)
            {
                if (file.ToLower().EndsWith(".xml", StringComparison.CurrentCulture))
                {
                    var parser = new Parser(file);
                    caseDocs.Add(parser.ParseXml());
                }
            }
            return caseDocs;
        }

        private static List<CaseRepo> ConvertDocToRepo(List<CaseDoc> caseDocs, string prefix)
        {
            var repoes = new List<CaseRepo>();

            foreach (var caseDoc in caseDocs)
            {
                if (caseDoc != null && caseDoc.CaseName != null && caseDoc.CaseName != "")
                {
                    CaseRepo repo = new CaseRepo()
                    {
                        Bcnum = caseDoc.Bcnum,
                        CaseName = caseDoc.CaseName,
                        CourtId = caseDoc.CourtId,
                        CourtName = caseDoc.CourtName,
                        DecisionDate = caseDoc.DecisionDate,
                        FileNameId = prefix + caseDoc.FileNameUsedAsId,
                        FileNo = caseDoc.FileNo,
                        Judges = Stringify(caseDoc.Judges),
                        JudgmentJudges = Stringify(caseDoc.JudgmentJudges),
                        Jurisdiction = caseDoc.Jurisdiction,
                        LegislationRef = Stringify(caseDoc.LegislationRef),
                        Mnc_Casenum = caseDoc.Mnc_Casenum,
                        Mnc_CourtId = caseDoc.Mnc_CourtId,
                        Mnc_Name = caseDoc.Mnc_Name,
                        Mnc_Year = caseDoc.Mnc_Year,
                        Party1 = caseDoc.Party1,
                        Party2 = caseDoc.Party2,
                        Series = caseDoc.Series,
                        Year = caseDoc.Year,
                        CaseOrder = Stringify(caseDoc.CaseOrder)
                    };

                    if (caseDoc.AppearInfoList != null)
                    {
                        foreach (var c in caseDoc.AppearInfoList)
                        {
                            repo.CaseAppearInfoes.Add(new CaseAppearInfo() { Appear_Type = c.Appear_Type.ToString(), FileNameId = caseDoc.FileNameUsedAsId, Party = c.Party, Bar = Stringify(c.Bar), Solicitor = Stringify(c.Solicitor) });
                        }
                    }

                    if (caseDoc.Judgments != null)
                    {
                        foreach (var c in caseDoc.Judgments)
                        {
                            repo.CaseJudgments.Add(new CaseJudgment() { FileNameId = caseDoc.FileNameUsedAsId, JudgmentText = c.Text, JudgmentTitle = c.Title });
                        }
                    }

                    if (caseDoc.CatchwordGrpList != null)
                    {
                        foreach (var c in caseDoc.CatchwordGrpList)
                        {
                            repo.CaseCatchwords.Add(new CaseCatchword() { FileNameId = caseDoc.FileNameUsedAsId, Catchword_Key = c.Catchword_Key, Catchword_KeySum = Stringify(c.Catchword_KeySum) });
                        }
                    }

                    repoes.Add(repo);
                }
                else
                {
                    // Failed to load
                }
            }

            return repoes;
        }

        private static string Stringify(string[] list)
        {
            string temp = "";

            if (list != null)
            {
                foreach (var item in list)
                {
                    temp += item + " | ";
                }
            }
            return temp;
        }
    }
}
