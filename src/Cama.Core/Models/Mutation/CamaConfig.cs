﻿using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Cama.Core.Models.Mutation
{
    public class CamaConfig
    {
        public string ProjectName { get; set; }

        [JsonIgnore]
        public string ProjectPath { get; set; }

        public string SolutionPath { get; set; }

        public string TestProjectName { get; set; }

        public IList<string> MutationProjectNames { get; set; }

        public string ProjectBinPath => Path.Combine(Path.GetDirectoryName(ProjectPath), "bin");
    }
}
