﻿using System;

namespace Cama.Core.Exceptions
{
    public class MutationDocumentException : Exception
    {
        public MutationDocumentException(string message)
            : base(message)
        {
        }

        public MutationDocumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
