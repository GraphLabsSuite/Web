//using System.Linq;
//using GraphLabs.DomainModel;
//using GraphLabs.Site.Models.Infrastructure;

//namespace GraphLabs.Site.Models.DemoLab
//{
//    /// <summary> Модель списка групп </summary>
//    public sealed class DemoLabListModel
//    {
//        private readonly IEntityQuery _query;
//        private readonly IEntityBasedModelLoader<DemoLabModel, Group> _modelLoader;

//        /// <summary> Модель списка групп </summary>
//        public GroupListModel(IEntityQuery query, IEntityBasedModelLoader<DemoLabModel, Group> modelLoader)
//        {
//            _query = query;
//            _modelLoader = modelLoader;
//        }

//        /// <summary> Загружает группы </summary>
//        protected override GroupModel[] LoadItems()
//        {
//            return _query.OfEntities<Group>()
//                .ToArray()
//                .Select(_modelLoader.Load)
//                .ToArray();
//        }
//    }
//}