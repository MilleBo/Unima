﻿using System.Collections.Generic;

namespace Testura.Mutation.Core.Loggers
{
    public interface IMutationRunLogger
    {
        void LogBeforeRun(IList<MutationDocument> mutationDocuments);

        void LogBeforeMutation(MutationDocument mutationDocument);

        void LogAfterMutation(MutationDocument mutationDocument, List<MutationDocumentResult> results, int mutationsRemainingCount);
    }
}
