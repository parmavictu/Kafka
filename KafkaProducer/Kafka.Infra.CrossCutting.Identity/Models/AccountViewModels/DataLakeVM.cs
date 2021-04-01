using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kafka.Infra.CrossCutting.Identity.Models.AccountViewModels
{
    public class DataLakeVM
    {
        public string Proposal { get; set; }
        public string Cpf { get; set; }
        public int Alucod { get; set; }
        public int Espcod { get; set; }
        public int SchoolYear { get; set; }
        public string Unitiy { get; set; }
        public DateTime ProposalDate { get; set; }
        public string ResultProposal { get; set; }
        public DateTime AnalistDate { get; set; }
        public string Line { get; set; }
        public string Decision { get; set; }
        public string Login { get; set; }
        public string UserName { get; set; }
        public DateTime DeadlineAnalyze { get; set; }
        public string DocIdentiyStudent { get; set; }
        public string DocCpfStudent { get; set; }
        public string DocStudentResidenceCertificate { get; set; }
        public string DocIdentiyGuarantee { get; set; }
        public string DocCpfGuarantee { get; set; }
        public string DocGuaranteeResidenceCertificate { get; set; }
        public string DocProofIncomeGuarantee { get; set; }
        public string DocCpfSpouseGuarantee { get; set; }
        public string SponsoredLinksPep { get; set; }
        public string DocSponsoredLinks { get; set; }
        public string DocDocumentAdditional1 { get; set; }
        public string DocDocumentAdditional2 { get; set; }
        public string DocDocumentAdditional3 { get; set; }

    }
}