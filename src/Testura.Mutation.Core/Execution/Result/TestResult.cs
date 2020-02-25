﻿using System;

namespace Testura.Mutation.Core.Execution.Result
{
    [Serializable]
    public class TestResult
    {
        /// <summary>
        /// Gets or sets the full name of this test.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the short name of this test.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the error text of this test.
        /// </summary>
        public string InnerText { get; set; }
    }
}
