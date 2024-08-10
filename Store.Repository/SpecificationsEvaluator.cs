﻿using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Core.Specifications;

namespace Store.Repository
{
    internal static class SpecificationsEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> specs)
        {
            var query = inputQuery;

            if(specs.Criteria is not null)
                query = query.Where(specs.Criteria);

            if(specs.OrderBy is not null) 
                query = query.OrderBy(specs.OrderBy);
            else if(specs.OrderByDesc is not null)
                query= query.OrderByDescending(specs.OrderByDesc);

            if (specs.IsPaginationEnabled)
                query = query.Skip(specs.Skip).Take(specs.Take);

            query = specs.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            return query;
        }
    }
}
