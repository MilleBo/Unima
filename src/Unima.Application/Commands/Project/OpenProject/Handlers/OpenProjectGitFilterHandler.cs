﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Anotar.Log4Net;
using Unima.Application.Models;
using Unima.Core.Config;
using Unima.Core.Creator.Filter;

namespace Unima.Application.Commands.Project.OpenProject.Handlers
{
    public class OpenProjectGitFilterHandler : OpenProjectHandler
    {
        private readonly MutationDocumentFilterItemGitDiffCreator _diffCreator;

        public OpenProjectGitFilterHandler(MutationDocumentFilterItemGitDiffCreator diffCreator)
        {
            _diffCreator = diffCreator;
        }

        public override Task HandleAsync(UnimaFileConfig fileConfig, UnimaConfig applicationConfig)
        {
            if (fileConfig.Git != null && fileConfig.Git.GenerateFilterFromDiffWithMaster)
            {
                LogTo.Info("Creating filter items from git diff with master");

                var filterItems = _diffCreator.GetFilterItemsFromDiff(Path.GetDirectoryName(fileConfig.SolutionPath), string.Empty);

                if (applicationConfig.Filter == null)
                {
                    applicationConfig.Filter = new MutationDocumentFilter { FilterItems = new List<MutationDocumentFilterItem>() };
                }

                applicationConfig.Filter.FilterItems.AddRange(filterItems);
            }

            return base.HandleAsync(fileConfig, applicationConfig);
        }
    }
}