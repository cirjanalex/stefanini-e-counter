using System;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Hosting;
using DocumentFormat.OpenXml.Wordprocessing;
using stefanini_e_counter.Models;
using System.Collections.Generic;

namespace stefanini_e_counter.Logic
{

    public interface IDocumentProcessor {
        string CreateDocument(string user, string reason);
    }

    public class DocumentProcessor: IDocumentProcessor
    {
        public const string ReferenceDocName = "Template Adeverinta Medic";

        private readonly IWebHostEnvironment _env;

        public DocumentProcessor(IWebHostEnvironment env)
        {
            _env = env;
        }
        
        public string CreateDocument(string user, string reason) {

            var refFileInfo = _env.ContentRootFileProvider.GetFileInfo($"Resources/{ReferenceDocName}.docx");
            if ( !refFileInfo.Exists )
                return null;//NotFound("reference certificate not available");

            string guid = Guid.NewGuid().ToString();
            var newFilePath = _env.ContentRootFileProvider.GetFileInfo($"Resources/{guid}.docx").PhysicalPath;

            new System.IO.FileInfo(refFileInfo.PhysicalPath).CopyTo(newFilePath);

            // https://docs.microsoft.com/en-us/office/open-xml/how-to-change-text-in-a-table-in-a-word-processing-document
            // https://stackoverflow.com/questions/18316873/replace-text-in-word-document-using-open-xml
            using (WordprocessingDocument doc = WordprocessingDocument.Open(newFilePath, true)) 
            { 
                Dictionary<DocumentFieldsEnum,string> data;
                switch(user.ToLower()) {
                    case "alexandra ungureanu":
                        data = DocumentData.DataForAlexandraUngureanu(reason);
                        break;
                    case "dan magirescu":
                        data = DocumentData.DataForDanMagirescu(reason);
                        break;
                    default:
                        data = DocumentData.NewData(user, reason);
                        break;
                }
                foreach(var pair in data) {
                    ReplaceText(doc, DocumentData.DocumentFields[pair.Key], pair.Value);
                }
            }

            return guid;
        }
        private void ReplaceText(WordprocessingDocument doc, string original, string change) {
            var document = doc.MainDocumentPart.Document;
            foreach (var text in document.Descendants<Text>())
            {
                if (text.Text.Contains(original))
                {
                    text.Text = text.Text.Replace(original, change);
                }
            } 
        }

        private void ReplaceTextOld(WordprocessingDocument doc, string original, string change) {
            var body = doc.MainDocumentPart.Document.Body;
            var paras = body.Elements<Paragraph>();

            foreach (var para in paras)
            {
                foreach (var run in para.Elements<Run>())
                {
                    foreach (var text in run.Elements<Text>())
                    {
                        if (text.Text.Contains(original))
                        {
                            text.Text = text.Text.Replace(original, change);
                        }
                    }
                }
            }
        }        
    }
}