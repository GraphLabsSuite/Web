using System;
using GraphLabs.DomainModel.Contexts;

// ReSharper disable once CheckNamespace
namespace GraphLabs.DomainModel
{
    public partial class GraphLabsContext :
        INewsContext,
        IUsersContext,
        ISessionsContext,
        IReportsContext,
        ITestsContext,
        ILabWorksContext,
        ITasksContext,
        ISystemContext
    {

    }
}
