﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Testura.Mutation.VsExtension.Sections.MutationExplorer;
using Testura.Mutation.VsExtension.Services;
using Task = System.Threading.Tasks.Task;

namespace Testura.Mutation.VsExtension.Sections.Selects
{
    internal sealed class SelectProjectFileCommand
    {
        public const int CommandId = 256;
        public static readonly Guid CommandSet = new Guid("4dd019e1-138b-43cd-b2d9-686d466ef14c");

        private readonly AsyncPackage _package;
        private readonly MutationFilterItemCreatorService _mutationFilterItemCreatorService;

        private SelectProjectFileCommand(
            AsyncPackage package,
            OleMenuCommandService commandService,
            MutationFilterItemCreatorService mutationFilterItemCreatorService)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            _mutationFilterItemCreatorService = mutationFilterItemCreatorService;
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static SelectProjectFileCommand Instance { get; private set; }

        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider => _package;

        public static async Task InitializeAsync(AsyncPackage package, MutationFilterItemCreatorService mutationFilterItemCreatorService)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new SelectProjectFileCommand(package, commandService, mutationFilterItemCreatorService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            _package.JoinableTaskFactory.RunAsync(async () =>
            {
                try
                {
                    await _package.JoinableTaskFactory.SwitchToMainThreadAsync();

                    var dte = await ServiceProvider.GetServiceAsync(typeof(DTE)) as DTE;

                    if (dte == null)
                    {
                        throw new NotSupportedException("Cannot create tool window");
                    }

                    var selectedItems = dte.SelectedItems;

                    if (selectedItems != null)
                    {
                        var files = new List<string>();

                        foreach (SelectedItem selectedItem in selectedItems)
                        {
                            if (selectedItem.ProjectItem is ProjectItem projectItem)
                            {
                                files.Add(projectItem.Document.FullName);
                            }
                        }

                        var window =
                            await _package.FindToolWindowAsync(typeof(MutationExplorerWindow), 0, true, _package.DisposalToken) as MutationExplorerWindow;
                        if (window?.Frame == null)
                        {
                            throw new NotSupportedException("Cannot create tool window");
                        }

                        var windowFrame = (IVsWindowFrame)window.Frame;
                        Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());

                        window.InitializeWindow(_mutationFilterItemCreatorService.CreateFilterFromFilePaths(files));
                    }
                }
                catch (Exception ex)
                {
                    var o = ex.Message;
                }
            });
        }
    }
}