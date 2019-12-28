﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Prism.Mvvm;
using Testura.Mutation.Core;

namespace Testura.Mutation.Sections.MutationDocumentsExecutionResult
{
    public class FailedToCompileMutationDocumentsViewModel : BindableBase, INotifyPropertyChanged
    {
        public FailedToCompileMutationDocumentsViewModel()
        {
            MutantsFailedToCompile = new ObservableCollection<MutationDocumentResult>();
        }

        public ObservableCollection<MutationDocumentResult> MutantsFailedToCompile { get; set; }

        public void InitializeMutants(IEnumerable<MutationDocumentResult> mutantsFailedToCompile)
        {
            MutantsFailedToCompile = new ObservableCollection<MutationDocumentResult>(mutantsFailedToCompile);
        }
    }
}
