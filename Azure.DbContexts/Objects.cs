using System;
using System.Collections.Generic;

namespace Azure.DbContexts
{
    public class CaseDoc
    {
        public string FileNameUsedAsId { get; set; }
        public string Series { get; set; }
        public string Year { get; set; }
        public string Bcnum { get; set; }
        public string Jurisdiction { get; set; }

        public string CaseName { get; set; }
        public string Party1 { get; set; }
        public string Party2 { get; set; }

        public string CourtId { get; set; }
        public string CourtName { get; set; }

        public string Mnc_Year { get; set; }
        public string Mnc_CourtId { get; set; }
        public string Mnc_Casenum { get; set; }
        public string Mnc_Name { get; set; }

        public string FileNo { get; set; }
        public string[] Judges { get; set; }
        public DateTime DecisionDate { get; set; }

        public List<CatchwordGrp> CatchwordGrpList { get; set; }

        public string[] JudgmentJudges { get; set; }
        public List<Judgment> Judgments { get; set; }

        public string[] CaseOrder { get; set; }

        public List<AppearInfo> AppearInfoList { get; set; }
        //public string[] AppearInfo { get; set; }

        public string[] LegislationRef { get; set; }
    }

    public class CatchwordGrp
    {
        public string Catchword_Key { get; set; }
        public string[] Catchword_KeySum { get; set; }

        public override string ToString()
        {
            string ans = Catchword_Key;
            foreach (var c in Catchword_KeySum)
                ans += c + " | ";
            return ans;
        }
    }

    public class Judgment
    {
        public string Title { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return Title + " | " + Text + " | ";
        }
    }

    public class AppearInfo
    {
        public AppearType Appear_Type { get; set; }
        public string Party { get; set; }
        public string[] Bar { get; set; }
        public string[] Solicitor { get; set; }

        public enum AppearType { COUNSEL, SOLICITOR }

        public override string ToString()
        {
            string ans = Appear_Type + " | " + Party + " | ";
            foreach (var c in Bar)
                ans += c + " | ";
            foreach (var c in Solicitor)
                ans += c + " | ";
            return ans;
        }
    }
}