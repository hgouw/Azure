using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Azure.DbContexts
{
    public class AzureDbContext : DbContext
    {
        public DbSet<CaseAppearInfo> CaseAppearInfoes { get; set; }
        public DbSet<CaseCatchword> CaseCatchwords { get; set; }
        public DbSet<CaseJudgment> CaseJudgments { get; set; }
        public DbSet<CaseRepo> CaseRepoes { get; set; }
    }

    public class CaseAppearInfo
    {
        [Key, Column(Order = 1)]
        public int Id { get; set; }
        [ForeignKey("CaseRepo")]
        public string FileNameId { get; set; }
        public string Appear_Type { get; set; }
        public string Party { get; set; }
        public string Bar { get; set; }
        public string Solicitor { get; set; }

        public virtual CaseRepo CaseRepo { get; set; }
    }

    public class CaseCatchword
    {
        [Key, Column(Order = 1)]
        public int Id { get; set; }
        [ForeignKey("CaseRepo"), Column(Order = 2)]
        public string FileNameId { get; set; }
        public string Catchword_Key { get; set; }
        public string Catchword_KeySum { get; set; }

        public virtual CaseRepo CaseRepo { get; set; }
    }

    public class CaseJudgment
    {
        [Key, Column(Order = 1)]
        public int Id { get; set; }
        [ForeignKey("CaseRepo"), Column(Order = 2)]
        public string FileNameId { get; set; }
        public string JudgmentTitle { get; set; }
        public string JudgmentText { get; set; }

        public virtual CaseRepo CaseRepo { get; set; }
    }

    public class CaseRepo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CaseRepo()
        {
            this.CaseAppearInfoes = new HashSet<CaseAppearInfo>();
            this.CaseCatchwords = new HashSet<CaseCatchword>();
            this.CaseJudgments = new HashSet<CaseJudgment>();
        }

        [Key, Column(Order = 1)]
        public string FileNameId { get; set; }
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
        public Nullable<System.DateTime> DecisionDate { get; set; }
        public string Judges { get; set; }
        public string JudgmentJudges { get; set; }
        public string LegislationRef { get; set; }
        public string CaseOrder { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<CaseAppearInfo> CaseAppearInfoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<CaseCatchword> CaseCatchwords { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<CaseJudgment> CaseJudgments { get; set; }
    }
}