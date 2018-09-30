﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using Cama.Core;
using Cama.Core.Creator.Mutators;
using Cama.Extensions;
using Cama.Models;
using Cama.Service;
using Cama.Service.Commands;
using Cama.Service.Commands.Mutation.CreateMutations;
using Cama.Tabs;
using Prism.Commands;
using Prism.Mvvm;

namespace Cama.Sections.MutationDocumentsOverview
{
    public class MutationDocumentsOverviewViewModel : BindableBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IMutationModuleTabOpener _tabOpener;
        private readonly ILoadingDisplayer _loadingDisplayer;
        private CamaConfig _config;

        public MutationDocumentsOverviewViewModel(ICommandDispatcher commandDispatcher, IMutationModuleTabOpener tabOpener, ILoadingDisplayer loadingDisplayer)
        {
            _commandDispatcher = commandDispatcher;
            _tabOpener = tabOpener;
            _loadingDisplayer = loadingDisplayer;
            Documents = new ObservableCollection<DocumentRowModel>();
            CreateDocumentsCommand = new DelegateCommand(CreateDocuments);
            FileSelectedCommand = new DelegateCommand<DocumentRowModel>(DocumentSelected);
            RunAllMutationsCommand = new DelegateCommand(RunAllMutations);
            MutationOperatorGridItems = new ObservableCollection<MutationOperatorGridItem>(Enum
                .GetValues(typeof(MutationOperators)).Cast<MutationOperators>().Select(m =>
                    new MutationOperatorGridItem
                    {
                        IsSelected = true,
                        MutationOperator = m,
                        Description = m.GetValue()
                    }));
        }

        public DelegateCommand CreateDocumentsCommand { get; set; }

        public DelegateCommand RunAllMutationsCommand { get; set; }

        public DelegateCommand<DocumentRowModel> FileSelectedCommand { get; set; }

        public ObservableCollection<DocumentRowModel> Documents { get; set; }

        public ObservableCollection<MutationOperatorGridItem> MutationOperatorGridItems { get; set; }

        public bool IsMutationDocumentsLoaded => Documents.Any();

        private async void CreateDocuments()
        {
            _loadingDisplayer.ShowLoading("Creating mutation documents..");
            var settings = MutationOperatorGridItems.Where(m => m.IsSelected).Select(m => m.MutationOperator);
            var mutationDocuments = await _commandDispatcher.ExecuteCommandAsync(new CreateMutationsCommand(_config, settings.Select(MutationOperatorFactory.GetMutationOperator).ToList()));
            var fileNames = mutationDocuments.Select(r => r.FileName).Distinct();

            foreach (var fileName in fileNames)
            {
                Documents.Add(new DocumentRowModel { MFile = new FileMutationsModel(fileName, mutationDocuments.Where(m => m.FileName == fileName).ToList() )});
            }

            _loadingDisplayer.HideLoading();
        }

        public void Initialize(CamaConfig config)
        {
            _config = config;
        }

        private void RunAllMutations()
        {
            _tabOpener.OpenTestRunTab(Documents.SelectMany(d => d.MFile.MutationDocuments).ToList(), _config);
        }

        private void DocumentSelected(DocumentRowModel documentRow)
        {
            _tabOpener.OpenFileDetailsTab(documentRow.MFile, _config);
        }
    }
}