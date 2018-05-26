using System.Collections.Generic;
using GraphLabs.Dal.Ef;
using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.DomainModel
{
    /// <summary> Действие при выполнении ЛР </summary>
    public partial class AbstractStudentAction 
    {
        /// <summary> Валидация </summary>
        public override IEnumerable<EntityValidationError> OnEntityValidating()
        {

            if (Penalty < 0 || Penalty > 100)
                yield return new EntityValidationError(nameof(Penalty), ValidationErrors.Action_OnEntityValidating_Штрафной_балл_может_находиться_только_в_диапазоне_от_0_до_100_);
        }
    }

    /// <summary> Действие при выполнении ЛР </summary>
    public partial class StudentAction
    {
        /// <summary> Валидация </summary>
        public override IEnumerable<EntityValidationError> OnEntityValidating()
        {
            if (string.IsNullOrWhiteSpace(Description))
                yield return new EntityValidationError(nameof(Description), ValidationErrors.Action_OnEntityValidating_Необходимо_указать_описание_действия_);

        }
    }
}
