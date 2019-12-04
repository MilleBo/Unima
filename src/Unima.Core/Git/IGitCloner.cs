﻿using System.Threading.Tasks;

namespace Unima.Core.Git
{
    public interface IGitCloner
    {
        Task CloneSolutionAsync(
            string repositoryUrl,
            string branch,
            string outputPath,
            string username,
            string password);
    }
}
