using System.Diagnostics.Contracts;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.StudentActions
{
    class StudentActionModelLoader : AbstractModelLoader<StudentActionModel, StudentAction>
    {
        public StudentActionModelLoader(IEntityQuery query) : base(query) { }

        public override StudentActionModel Load(StudentAction studentAction)
        {
            Guard.IsNotNull(nameof(studentAction), studentAction);

            var model = new StudentActionModel()
            {
                Id = studentAction.Id,
                Description = studentAction.Description,
                Penalty = studentAction.Penalty,
                Time = studentAction.Time
            };

            return model;
        }
    }
}
