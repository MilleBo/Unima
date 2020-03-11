﻿using System;
using Testura.Mutation.Core.Creator.Mutators.BinaryExpressionMutators;

namespace Testura.Mutation.Core.Creator.Mutators
{
    public static class MutationOperatorFactory
    {
        public static IMutator GetMutationOperator(MutationOperators mutationOperators)
        {
            switch (mutationOperators)
            {
                case MutationOperators.ConditionalBoundary:
                    return new ConditionalBoundaryMutator();

                case MutationOperators.NegateCondtional:
                    return new NegateConditionalMutator();

                case MutationOperators.Math:
                    return new MathMutator();

                case MutationOperators.NegateTypeCompability:
                    return new NegateTypeCompabilityMutator();

                case MutationOperators.Increment:
                    return new IncrementsMutator();

                case MutationOperators.ReturnValue:
                    return new ReturnValueMutator();

                case MutationOperators.MethodCall:
                    return new MethodCallMutator();

                default:
                    throw new ArgumentOutOfRangeException(nameof(mutationOperators), mutationOperators, null);
            }
        }
    }
}
