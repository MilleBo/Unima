﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using RazorEngine;
using RazorEngine.Templating;
using Encoding = System.Text.Encoding;

namespace Testura.Mutation.Core.Execution.Report.Html
{
    public class HtmlOnlyBodyReportCreator : ReportCreator
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HtmlOnlyBodyReportCreator));

        public HtmlOnlyBodyReportCreator(string savePath)
            : base(savePath)
        {
        }

        private string TemplatePath => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Execution", "Report", "Html", "ReportTemplateOnlyBody.cshtml");

        public override void SaveReport(IList<MutationDocumentResult> mutations, TimeSpan exectutionTime)
        {
            Log.Info("Saving HTML report..");

            if (!mutations.Any(m => m.Survived))
            {
                Log.Info("No mutations to report.");
                return;
            }

            try
            {
                var text = File.ReadAllText(TemplatePath);
                var renderedText = Engine.Razor.RunCompile(text, "report", null, mutations.Where(m => m.Survived));

                File.WriteAllText(SavePath, renderedText, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Log.Error("Failed to save html report", ex);
                throw;
            }

            Log.Info("Html report saved successfully.");
        }
    }
}
