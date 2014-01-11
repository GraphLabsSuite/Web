using System;
using System.ServiceModel.Activation;
using System.Threading;
using GraphLabs.WcfServices.Data;

namespace GraphLabs.WcfServices
{
    /// <summary> Сервис предоставления данных модулям заданий </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class TasksDataService : ITasksDataService
    {

        /// <summary> Регистрирует начало выполнения задания </summary>
        /// <param name="taskId"> Идентификатор модуля-задания </param>
        /// <param name="sessionGuid"> Идентификатор сессии </param>
        /// <returns> Данные для задания - как правило, исходный граф, или что-то типа того </returns>
        public TaskVariantInfo InitialiseTask(long taskId, Guid sessionGuid)
        {
#if DEBUG
            Thread.Sleep(3000);
#endif
            //using (var _ctx = new GraphLabsContext())
            //{
            //    var sessions = _ctx.Sessions.Where(s => s.Guid == sessionGuid);

            //    if (sessions.Count(c => true) != 1)
            //    {
            //        return null;
            //    }

            //    var session = sessions.Single();

            //    var labWork = _ctx.Results.Where(r => r.Student == session.User && r.)
            //}

            //var guid = new Guid(1,0,0,0,0,0,0,0,0,0,0);
            //if (taskId != guid)
            //{
            //    return new TaskVariantInfo
            //        {
            //            Data = DebugGraphGenerator.GetSerializedGraph(),
            //            Number = "1",
            //            Version = 1
            //        };
            //}
            //else
            //{
            //    return new TaskVariantInfo
            //    {
            //        Data = DebugGraphGenerator.GetSerializedWeightedGraph(),
            //        Number = "1",
            //        Version = 1
            //    };
            //}
            throw new NotImplementedException();
        }

        public TaskVariantInfo FinishTask(string sessionId)
        {
            throw new NotImplementedException();
        }
    }
}
