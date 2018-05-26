using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Models.LabExecution
{
    internal sealed class TestExecutionModelLoader
    {
        public TestListEntryModel Load(Result currentResult, DomainModel.TestPoolEntry testPoolEntry)
        {
            var resultForEntry = currentResult
                .AbstractResultEntries
                .OfType<TestResult>()
                .SingleOrDefault(result => result.TestPoolEntry.Id == testPoolEntry.Id);

            var taskState = resultForEntry?.Status == ExecutionStatus.Complete
                ? TaskExecutionState.Solved
                : TaskExecutionState.New;

            return new TestListEntryModel()
            {
                QuestionId = testPoolEntry.Id,
                TestName = testPoolEntry.TestQuestion.Question,
                State = taskState
            };
        }
    }
}
