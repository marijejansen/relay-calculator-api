// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordsService.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the RecordsService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RelayCalculator.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using iTextSharp.text.pdf;
    using iTextSharp.text.pdf.parser;
    using RelayCalculator.Services.Models;

    public class RecordsService
    {
        public IEnumerable<RelayRecord> GetRelayRecords()
        {

            return new List<RelayRecord>();
        }

        private string GetPdfContent()
        {
            var fileNameLongCourse = "- overzicht van WMR, EMR en NMR lange baan";
            var reader = new PdfReader($"@{fileNameLongCourse}");

            return "NEE";
        }
    }
}
