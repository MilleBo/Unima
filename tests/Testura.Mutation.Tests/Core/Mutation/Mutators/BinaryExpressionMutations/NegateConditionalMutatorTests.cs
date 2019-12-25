﻿using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Testura.Mutation.Core.Creator.Mutators.BinaryExpressionMutators;

namespace Testura.Mutation.Tests.Core.Mutation.Mutators.BinaryExpressionMutations
{
    [TestFixture]
    public class NegateConditionalMutatorTests
    {
        [TestCase("==", "!=")]
        [TestCase("!=", "==")]
        [TestCase("<", ">")]
        [TestCase("<=", ">")]
        [TestCase(">", "<")]
        [TestCase(">=", "<")]
        public void ConditionalTests(string preMutation, string postMutation)
        {
            var tree = SyntaxFactory.ParseSyntaxTree($"classC{{publicvoidDo(){{if(i{preMutation}1)}}");
            var root = tree.GetRoot();

            var ifConditionalMutationOperator = new NegateConditionalMutator();
            var doc = ifConditionalMutationOperator.GetMutatedDocument(root, null);

           Assert.AreEqual($"i {postMutation} 1", doc[0].MutationDetails.Mutation.ToString());
        }
    }
}
