using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Core;
using GraphLabs.Site.Core.OperationContext;

namespace GraphLabs.Dal.Ef
{
    /// <summary> Контекст бизнес-операции </summary>
    sealed class OperationContextImpl : IOperationContext<IGraphLabsContext>
    {
        private readonly IChangesTracker _changesTracker;
        private readonly IGraphLabsContext _dataContext;

        /// <summary> Контекст доступа к данным </summary>
        public IGraphLabsContext DataContext
        {
            get
            {
                return _dataContext;
            }
        }

        public OperationContextImpl(IGraphLabsContext context, IChangesTracker changesTracker)
        {
            _changesTracker = changesTracker;
            _dataContext = context;
        }

        private bool _isComplete = false;

        /// <summary> Сохранить изменения (завершает операцию) </summary>
        public void Complete()
        {
            //TODO
            _changesTracker.SaveChanges();
        }

        public void Dispose()
        {
            //TODO
        }
    }
}