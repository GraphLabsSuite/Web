using System;
using System.Linq;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Models.LabExecution
{
    internal sealed class TestExecutionModelLoader
    {
        public TestListEntryModel Load(Result currentResult, DomainModel.TestQuestion question)
        {
            var resultForEntry = currentResult
                .AbstractResultEntries
                .OfType<TestResult>()
                .SingleOrDefault(result => result.TestQuestion.Id == question.Id);

            var taskState = resultForEntry?.Status == ExecutionStatus.Complete
                ? TaskExecutionState.Solved
                : TaskExecutionState.New;

            return new TestListEntryModel()
            {
                QuestionId = question.Id,
                TestName = question.Question,
                State = taskState
            };
        }
    }
}
