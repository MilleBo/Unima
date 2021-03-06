﻿using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Testura.Mutation.Core
{
    public class MutationDocument
    {
        private readonly Document _orginalDocument;

        public MutationDocument(Document orginalDocument, MutationDocumentDetails mutationDetails)
        {
            MutationDetails = mutationDetails;
            Id = Path.GetRandomFileName();
            FileName = orginalDocument?.Name;
            FilePath = orginalDocument?.FilePath;
            ProjectName = orginalDocument?.Project.Name;
            _orginalDocument = orginalDocument;
        }

        public string Id { get; }

        public string FileName { get; }

        public string FilePath { get; set; }

        public string ProjectName { get; set; }

        public MutationDocumentDetails MutationDetails { get; }

        public string MutationName => $"Proj: {ProjectName}, File: {FileName}({MutationDetails.Location.Where} - {MutationDetails.Location.Line})";

        public async Task<Document> CreateMutatedDocumentAsync()
        {
            var editor = await DocumentEditor.CreateAsync(_orginalDocument);
            editor.ReplaceNode(MutationDetails.Original, MutationDetails.Mutation);
            return _orginalDocument.WithText(editor.GetChangedDocument().GetSyntaxRootAsync().Result.GetText());
        }
    }
}