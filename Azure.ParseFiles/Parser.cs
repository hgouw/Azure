using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Azure.ParseFiles
{
    public class Parser
    {
        private readonly string FilePath;

        public Parser()
        {
        }

        public Parser(string filePath)
        {
            FilePath = filePath;
            FixXml();
        }

        private void FixXml()
        {
            string text = File.ReadAllText(FilePath);

            //string sep = "<!DOCTYPE caseml PUBLIC \" -//Butterworths//DTD XML CASE REPORTS VER 1.0//EN\">";
            string sep = "<caseml";

            //string correction = "<?xml version=\"1.0\" encoding=\"utf-8\"?><!DOCTYPE caseml PUBLIC \"-//Butterworths//DTD XML CASE REPORTS VER 1.0//EN\" \"caseml.dtd\">";
            string correction = "<?xml version=\"1.0\" encoding=\"utf-8\"?><!DOCTYPE caseml PUBLIC \" -//Butterworths//DTD XML CASE REPORTS VER 1.0//EN\" \"file:////F:/URJ/dtd/caseml.dtd\">";

            string[] split = text.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);

            string ans = "";

            if (split.Length == 2)
            {
                ans = correction + sep + split[1];
                File.WriteAllText(FilePath, ans);
            }
        }

        public CaseDoc ParseXml()
        {
            string sep = "|";
            CaseDoc casedoc = new CaseDoc();

            XmlReaderSettings settings = new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse };

            XmlReader reader = XmlReader.Create(FilePath, settings);

            XDocument doc = null;

            string[] tlist = FilePath.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
            casedoc.FileNameUsedAsId = tlist[tlist.Length - 1];

            try
            {
                doc = XDocument.Load(reader);
            }
            catch (Exception ex)
            {
            }

            if (doc != null)
            {
                XElement elCase = doc.Element("caseml").Element("case");

                if (elCase != null)
                {
                    if (elCase.Attribute("series") != null)
                        casedoc.Series = elCase.Attribute("series").Value;

                    if (elCase.Attribute("year") != null)
                        casedoc.Year = elCase.Attribute("year").Value;

                    if (elCase.Attribute("bcnum") != null)
                        casedoc.Bcnum = elCase.Attribute("bcnum").Value;

                    if (elCase.Attribute("jur") != null)
                        casedoc.Jurisdiction = elCase.Attribute("jur").Value;

                    casedoc.CaseName = elCase.Element("headnote").Element("caseinfo").Element("casename").Value;

                    if (casedoc.CaseName.Contains(" v "))
                    {
                        string[] split = casedoc.CaseName.Split(new string[] { " v " }, StringSplitOptions.RemoveEmptyEntries);
                        casedoc.Party1 = split[0];// casedoc.CaseName.Split(new string[] { " v " }, StringSplitOptions.RemoveEmptyEntries)[0];

                        if (split.Length == 2)
                            casedoc.Party2 = split[1];//casedoc.CaseName.Split(new string[] { " v " }, StringSplitOptions.RemoveEmptyEntries)[1];
                        else
                            casedoc.Party2 = "";
                    }
                    else
                    {
                        casedoc.Party1 = casedoc.CaseName;
                        casedoc.Party2 = "";
                    }

                    casedoc.CourtId = elCase.Element("headnote").Element("caseinfo").Element("courtinfo").Element("court").Attribute("courtid").Value;
                    casedoc.CourtName = elCase.Element("headnote").Element("caseinfo").Element("courtinfo").Element("court").Value;

                    IEnumerable<XElement> elJudges = elCase.Element("headnote").Element("caseinfo").Element("courtinfo").Elements("coram");

                    var temp = "";

                    foreach (var item in elJudges)
                    {
                        if (item.Element("judge") != null)
                            temp += item.Element("judge").Value;
                    }

                    casedoc.Judges = temp.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);

                    temp = "";

                    if (elCase.Element("headnote").Element("caseinfo").Element("courtinfo").Element("fileno") != null)
                        casedoc.FileNo = elCase.Element("headnote").Element("caseinfo").Element("courtinfo").Element("fileno").Value;

                    XElement elMnc = elCase.Element("headnote").Element("caseinfo").Element("courtinfo").Element("mnc");

                    if (elMnc != null)
                    {
                        casedoc.Mnc_Name = elMnc.Attribute("name").Value;
                        casedoc.Mnc_CourtId = elMnc.Attribute("courtid").Value;
                        casedoc.Mnc_Casenum = elMnc.Attribute("casenum").Value;
                        casedoc.Mnc_Year = elMnc.Attribute("year").Value;
                    }

                    try
                    {
                        int d_day = int.Parse(elCase.Element("headnote").Element("caseinfo").Element("courtinfo").Element("dates").Element("decdate").Attribute("day").Value);
                        int d_mon = int.Parse(elCase.Element("headnote").Element("caseinfo").Element("courtinfo").Element("dates").Element("decdate").Attribute("month").Value);
                        int d_yr = int.Parse(elCase.Element("headnote").Element("caseinfo").Element("courtinfo").Element("dates").Element("decdate").Attribute("year").Value);
                        casedoc.DecisionDate = new DateTime(d_yr, d_mon, d_day);
                    }
                    catch (Exception ex)
                    {
                    }

                    if (elCase.Element("headnote").Element("catchwordgrp") != null)
                    {
                        IEnumerable<XElement> catchwordgrpList = elCase.Element("headnote").Element("catchwordgrp").Elements("catchwords");

                        casedoc.CatchwordGrpList = new List<CatchwordGrp>();

                        if (catchwordgrpList != null)
                        {
                            foreach (var item in catchwordgrpList)
                            {
                                CatchwordGrp grp = new CatchwordGrp();
                                grp.Catchword_Key = item.Element("key").Value;

                                var keysumlist = item.Elements("keysum");
                                temp = "";
                                foreach (var c1 in keysumlist)
                                {
                                    temp += c1.Value + sep;
                                }
                                grp.Catchword_KeySum = temp.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);

                                casedoc.CatchwordGrpList.Add(grp);
                            }
                        }
                    }

                    XElement elCaseOrder = elCase.Element("judgmentgrp").Element("caseorder");

                    if (elCaseOrder != null)
                    {
                        IEnumerable<XElement> caseOrderList = null;

                        temp = "";
                        if (elCaseOrder.Element("list") != null)
                        {
                            caseOrderList = elCaseOrder.Element("list").Elements("li").Descendants("p");

                            foreach (var item in caseOrderList)
                            {
                                temp += item.Value + sep; //item.Element("text").Value + sep; 
                            }
                        }

                        else if (elCaseOrder.Element("list") == null)
                        {
                            caseOrderList = elCaseOrder.Elements("p");
                            foreach (var item in caseOrderList)
                            {
                                temp += item.Value + sep;
                            }
                        }

                        casedoc.CaseOrder = temp.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);
                    }

                    XElement elAppearInfo = elCase.Element("judgmentgrp").Element("appearinfo");

                    casedoc.AppearInfoList = new List<AppearInfo>();

                    if (elAppearInfo != null)
                    {
                        IEnumerable<XElement> appearList = elAppearInfo.Elements("appear");

                        foreach (var item in appearList)
                        {
                            AppearInfo ai = new AppearInfo();

                            if (item.Value.StartsWith("Counsel", StringComparison.CurrentCulture))
                                ai.Appear_Type = AppearInfo.AppearType.COUNSEL;

                            if (item.Value.StartsWith("Solicitor", StringComparison.CurrentCulture))
                                ai.Appear_Type = AppearInfo.AppearType.SOLICITOR;

                            if (item.Element("party") != null)
                                ai.Party = item.Element("party").Value;

                            IEnumerable<XElement> barList = item.Elements("bar");
                            IEnumerable<XElement> solList = item.Elements("sol");

                            temp = "";
                            if (barList != null)
                            {
                                foreach (var item2 in barList)
                                    temp += item2.Value + sep;
                            }
                            ai.Bar = temp.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);

                            temp = "";
                            if (solList != null)
                            {
                                foreach (var item2 in solList)
                                    temp += item2.Value + sep;
                            }
                            ai.Solicitor = temp.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);

                            casedoc.AppearInfoList.Add(ai);
                        }
                    }

                    XElement elJudgmentgrp = elCase.Element("judgmentgrp").Element("judgment");

                    if (elJudgmentgrp != null)
                    {
                        IEnumerable<XElement> judgeList = elJudgmentgrp.Element("coram").Elements("judge");

                        temp = "";
                        foreach (var item in judgeList)
                            temp += item.Value + sep;

                        casedoc.JudgmentJudges = temp.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);

                        IEnumerable<XElement> pgrpList = elJudgmentgrp.Elements("pgrp");

                        casedoc.Judgments = new List<Judgment>();

                        foreach (var item in pgrpList)
                        {
                            Judgment j = new Judgment();
                            j.Title = item.Element("title").Value;

                            item.Element("title").Remove();
                            j.Text = item.Value;

                            casedoc.Judgments.Add(j);
                        }
                    }

                    XElement elHeld = elCase.Element("headnote").Element("held");

                    if (elHeld != null)
                    {
                        if (elHeld.Element("referleg") != null && elHeld.Element("referleg").Elements("legref") != null)
                        {
                            IEnumerable<XElement> legList = elHeld.Element("referleg").Elements("legref");
                            temp = "";
                            foreach (var item in legList)
                            {
                                if (item.Element("legname") != null)
                                    temp += item.Element("legname").Value + sep;
                            }
                        }
                    }

                    casedoc.LegislationRef = temp.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);
                }
            }

            return casedoc;
        }
    }
}