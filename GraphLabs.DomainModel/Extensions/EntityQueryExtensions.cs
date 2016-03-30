using GraphLabs.DomainModel.Infrastructure;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.Extensions
{
    /// <summary> ���������� ��� GraphLabsContext </summary>
    public static class EntityQueryExtensions
    {
        /// <summary> ����� �������� (������, ���� �� �����) </summary>
        /// <exception cref="EntityNotFoundException">�������� � �������� ������ �� �������</exception>
        [NotNull]
        public static TEntity Get<TEntity>(this IEntityQuery query, object keyValue)
            where TEntity : AbstractEntity
        {
            var entity = query.Find<TEntity>(keyValue);
            if (entity == null)
                throw new EntityNotFoundException(typeof(TEntity), new [] { keyValue });

            return entity;
        }

        /// <summary> ����� �������� (null, ���� �� �����) </summary>
        [CanBeNull]
        public static TEntity Find<TEntity>(this IEntityQuery query, object keyValue)
            where TEntity : AbstractEntity
        {
            return query.Find<TEntity>(new [] {keyValue});
        }
    }
}