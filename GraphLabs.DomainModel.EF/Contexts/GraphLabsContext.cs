using System;
using GraphLabs.DomainModel.EF.Contexts;

// ReSharper disable once CheckNamespace
namespace GraphLabs.DomainModel.EF
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
