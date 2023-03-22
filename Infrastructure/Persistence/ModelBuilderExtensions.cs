using System.Reflection;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence;

internal static class ModelBuilderExtensions
{
    /// <summary>
    /// Save enum values as string in database so changes to the enum do not conflict with existing data in the database.
    /// </summary>
    public static ModelBuilder AddEnumStringConversions(this ModelBuilder builder)
    {
        // Add converters for Enums
        foreach (var property in builder.Model.GetEntityTypes().SelectMany(type => type.GetProperties().Where(property => (Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType).IsEnum)))
        {
            var type = typeof(EnumToStringConverter<>).MakeGenericType((Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType));
            var converter = Activator.CreateInstance(type, new ConverterMappingHints()) as ValueConverter;

            property.SetValueConverter(converter);
        }

        return builder;
    }

    /// <summary>
    /// Add query filters for all entities of type <see cref="BaseSoftDeletableEntity"/> so they are filtered out in queries when deleted.
    /// </summary>
    [Obsolete("This overwrites all previously applied query filters.")]
    public static ModelBuilder AddSoftDeleteQueryFilters(this ModelBuilder builder)
    {
        foreach (var type in builder.Model.GetEntityTypes().Where(type => type.ClrType.IsSubclassOf(typeof(BaseSoftDeletableEntity))).Select(entityType => entityType.ClrType))
        {
            MethodInfo method = typeof(ModelBuilderExtensions).GetMethod(nameof(ApplySoftDeleteQueryFilter), BindingFlags.NonPublic | BindingFlags.Static)!;
            MethodInfo generic = method.MakeGenericMethod(type);

            generic.Invoke(typeof(ModelBuilderExtensions), new object?[] { builder });
        }

        return builder;
    }

    private static ModelBuilder ApplySoftDeleteQueryFilter<TEntity>(this ModelBuilder builder) where TEntity : BaseSoftDeletableEntity
    {
        builder.Entity<TEntity>().HasQueryFilter(entity => entity.DeletedAt == null);

        return builder;
    }
}