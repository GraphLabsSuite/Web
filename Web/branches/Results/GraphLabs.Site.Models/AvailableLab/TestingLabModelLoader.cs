using System.Diagnostics.Contracts;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.AvailableLab
{
    /// <summary> ��������� ������� ����������� ������������ ����� </summary>
    sealed class TestingLabModelLoader : AbstractModelLoader<TestingLabModel, AbstractLabSchedule>
    {
        /// <summary> ��������� ������� ����������� ������������ ����� </summary>
        public TestingLabModelLoader(IEntityQuery query) : base(query) { }

        /// <summary> ��������� �� ��������-��������� </summary>
        public override TestingLabModel Load(AbstractLabSchedule sch)
        {
            Contract.Requires(sch != null);
            Contract.Requires(sch.Mode == LabExecutionMode.TestMode);

            var model = new TestingLabModel
            {
                Id = sch.Id,
                Name = sch.LabWork.Name,
                AvailableTill = sch.DateTill,
            };

            return model;
        }
    }
}