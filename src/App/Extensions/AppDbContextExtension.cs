using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Npgsql.NameTranslation;

namespace App.Extensions;

public static class AppDbContextExtension
{
    public static void AddGlobalHasQueryFilterForBaseTypeEntities<T>(this ModelBuilder builder, Expression<Func<T, bool>> expression)
    {
        var entities = builder.Model
        .GetEntityTypes()
        .Where(e => e.ClrType.BaseType == typeof(T))
        .Select(e => e.ClrType);
        foreach (var entity in entities)
        {
            var newParam = Expression.Parameter(entity);
            var newbody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
            builder.Entity(entity).HasQueryFilter(Expression.Lambda(newbody, newParam));
        }
    }

    public static void ConvertToSnakeCase(this ModelBuilder builder)
    {
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            entity.SetTableName(NpgsqlSnakeCaseNameTranslator.ConvertToSnakeCase(entity.GetTableName()));

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(NpgsqlSnakeCaseNameTranslator.ConvertToSnakeCase(property.GetColumnName()));
            }

            foreach (var key in entity.GetKeys())
            {
                key.SetName(NpgsqlSnakeCaseNameTranslator.ConvertToSnakeCase(key.GetName()));
            }

            foreach (var foreignKey in entity.GetForeignKeys())
            {
                foreignKey.SetConstraintName(NpgsqlSnakeCaseNameTranslator.ConvertToSnakeCase(foreignKey.GetConstraintName()));
            }

            foreach (var index in entity.GetIndexes())
            {
                //index.SetName(NpgsqlSnakeCaseNameTranslator.ConvertToSnakeCase(index.GetName()));
                index.SetDatabaseName(NpgsqlSnakeCaseNameTranslator.ConvertToSnakeCase(index.Name));
            }
        }
    }
}
