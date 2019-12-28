﻿using System.Collections.Generic;
using System.Windows.Controls;
using Testura.Mutation.Core;

namespace Testura.Mutation.Sections.MutationDocumentsExecutionResult
{
    /// <summary>
    /// Interaction logic for FailedToCompileMutationDocumentsView.xaml
    /// </summary>
    public partial class FailedToCompileMutationDocumentsView : TabItem
    {
        public FailedToCompileMutationDocumentsView(IEnumerable<MutationDocumentResult> mutantsFailedToCompile)
        {
            InitializeComponent();

            var viewModel = DataContext as FailedToCompileMutationDocumentsViewModel;
            viewModel.InitializeMutants(mutantsFailedToCompile);
        }
    }
}
