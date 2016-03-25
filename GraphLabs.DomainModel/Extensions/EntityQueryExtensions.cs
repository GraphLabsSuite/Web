using GraphLabs.DomainModel.Infrastructure;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> ���������� ��� GraphLabsContext </summary>
    public static class EntityQueryExtensions
    {
        /// <summary> ����� �������� (������, ���� �� �����) </summary>
        /// <exception cref="EntityNotFoundException">�������� � �������� ������ �� �������</exception>
        [NotNull]
        public static TEntity Get<TEntity>(this IEntityQuery query, params object[] keyValues)
            where TEntity : AbstractEntity
        {
            var entity = query.Find<TEntity>();
            if (entity == null)
                throw new EntityNotFoundException(typeof(TEntity), keyValues);

            return entity;
        }
    }
}