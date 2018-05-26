using System.Linq;
using System;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models.Groups;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.Results;
using GraphLabs.Site.Models.ResultsWithTaskInfo;
using GraphLabs.Site.Models.TaskResultsWithActions;
using GraphLabs.Site.Models.TestPool;


namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Teacher | UserRole.Administrator)]
    public class ResultController : GraphLabsFilteringController<GroupModel, Group>
	{
        // TODO: избавиться от UserRepository
        #region Зависимости

        private readonly IUserRepository _userRepository;
        private readonly IEntityBasedModelLoader<TaskResultWithActionsModel, TaskResult> _taskResultWithActionsModelLoader;
        private readonly IEntityBasedModelLoader<ResultModel, Result> _resultModelLoader;
        private readonly IEntityBasedModelLoader<TestPoolModel, TestPool> _testPoolModelLoader;
        private readonly IEntityBasedModelLoader<ResultWithTaskInfoModel, Result> _resultWithTaskInfoModelLoader;
        private readonly IEntityBasedModelLoader<GroupModel, Group> _groupModelLoader;
        private readonly IListModelLoader _listModelLoader;

        #endregion

        public ResultController(
            IUserRepository userRepository,
            IEntityBasedModelLoader<TaskResultWithActionsModel, TaskResult> taskResultWithActionsModelLoader,
            IEntityBasedModelLoader<ResultWithTaskInfoModel, Result> resultWithTaskInfoModelLoader,
            IEntityBasedModelLoader<TestPoolModel, TestPool> testPoolModelLoader,
            IListModelLoader listModelLoader,
            IEntityBasedModelLoader<GroupModel, Group> groupModelLoader,
            IEntityBasedModelLoader<ResultModel, Result> resultModelLoader)
        {
            _userRepository = userRepository;
            _taskResultWithActionsModelLoader = taskResultWithActionsModelLoader;
            _resultWithTaskInfoModelLoader = resultWithTaskInfoModelLoader;
            _testPoolModelLoader = testPoolModelLoader;
            _groupModelLoader = groupModelLoader;
            _listModelLoader = listModelLoader;
            _resultModelLoader = resultModelLoader;
        }

        public override ActionResult Index(string msg)
        {
            var model = _listModelLoader.LoadListModel<GroupListModel, GroupModel>().Filter(FiExpression);
            
            return View((GroupListModel) model);
        }

        public ActionResult StudentList(long id = 0)
        {
            return View(_groupModelLoader.Load(id));
        }

        public ActionResult StudentResult(long id = 0, string ourdatestring = "today")

        {
            var ourdate = DateTime.Today;
            if (!ourdatestring.Equals("today")) ourdate = DateTime.Parse(ourdatestring);
            var student = (Student)_userRepository.GetUserById(id);
           // ViewBag.GroupId = student.Group.Id;
            var model = _listModelLoader.LoadListModel<ResultListModel, ResultModel>()
                .FilterByUser(student.Email)
                 .FilterByDate(ourdate.AddDays((DayOfWeek.Monday - ourdate.DayOfWeek) * (ourdate.DayOfWeek - ourdate.AddDays(-1).DayOfWeek)),
                    ourdate.AddDays(7 + (DayOfWeek.Monday - ourdate.DayOfWeek) * (ourdate.DayOfWeek - ourdate.AddDays(-1).DayOfWeek)));
            return View(model);
        }

        public ActionResult LabWorkResult(long id = 0)
        {
            var model = _resultWithTaskInfoModelLoader.Load(id);
            return View(model);
        }

        public ActionResult TaskResult(long id = 0)
        {
            var model = _taskResultWithActionsModelLoader.Load(id);
            return View(model);
        }

        public ActionResult TestResult(long id = 0)
        {
            var model = _testPoolModelLoader.Load(id);
            return View(model);
        }
    }
}
