// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="IQueryableMethods.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class I Queryable Methods.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.DataTable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Class I Queryable Methods.
    /// </summary>
    public static class IQueryableMethods
    {
        /// <summary>
        /// Datas the table parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <param name="recordsFiltered">The records filtered.</param>
        /// <returns>IQueryable&lt;T&gt;.</returns>
        public static IQueryable<T> DataTableParameters<T>(this IQueryable<T> query, DataTableParameters dataTableParameters, out int recordsFiltered)
        {
            var t = Expression.Parameter(typeof(T), "t");

            var whereCallExpression = query.Expression;

            if (!String.IsNullOrEmpty(dataTableParameters.Search))
            {
                var searches = Regex.Split(dataTableParameters.Search, @"\s+").Where(s => s != string.Empty).ToArray();

                if (searches.Length > 0)
                {
                    var containsMethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });

                    List<Expression> andList = new List<Expression>();

                    foreach (var search in searches.Select(s => s.ToLower()))
                    {
                        List<Expression> orList = new List<Expression>();

                        var searchableColumns = dataTableParameters.SearchableColumns
                            .Select(c =>
                            {
                                var sProp = typeof(T).GetProperties().FirstOrDefault(p => p.Name.ToLower() == c.ToLower());

                                return sProp != null ? sProp.Name : null;
                            })
                            .Where(c => c != null)
                            .ToArray();

                        foreach (var column in searchableColumns)
                        {
                            var pColumn = Expression.Property(t, column);

                            var columnToStringMethodCall = Expression.Call(pColumn, "ToString", Type.EmptyTypes); //avaliar os tipos das colunas - convertendo todas para string
                            var columnToLowerMethodCall = Expression.Call(columnToStringMethodCall, "ToLower", Type.EmptyTypes);
                            var columnContainsMethodCall = Expression.Call(columnToLowerMethodCall, containsMethod, Expression.Constant(search));

                            var piColumn = typeof(T).GetProperty(column);

                            if (!piColumn.PropertyType.IsValueType || piColumn.PropertyType.IsGenericType && piColumn.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                var notNull = Expression.NotEqual(pColumn, Expression.Constant(null, typeof(object)));

                                orList.Add(Expression.AndAlso(notNull, columnContainsMethodCall));
                            }
                            else
                                orList.Add(columnContainsMethodCall);
                        }

                        Expression or = null;

                        if (orList.Count > 1)
                        {
                            if (or == null)
                                or = Expression.OrElse(orList[0], orList[1]);

                            for (int i = 2, l = orList.Count(); i < l; i++)
                                or = Expression.OrElse(or, orList[i]);
                        }
                        else
                            or = orList[0];

                        andList.Add(or);
                    }

                    Expression and = null;

                    if (andList.Count > 1)
                    {
                        if (and == null)
                            and = Expression.AndAlso(andList[0], andList[1]);
                        for (int i = 2, l = andList.Count(); i < l; i++)
                            and = Expression.AndAlso(and, andList[i]);
                    }
                    else
                        and = andList[0];

                    var whereLambda = Expression.Lambda(and, new[] { t });

                    whereCallExpression = Expression.Call(
                        typeof(Queryable),
                        "Where",
                        new Type[] { query.ElementType },
                        query.Expression,
                        whereLambda);
                }
            }

            recordsFiltered = query.Provider.CreateQuery<T>(whereCallExpression).Count();

            Expression orderByCallExpression = whereCallExpression;

            if (dataTableParameters.OrderedColumn == null || string.IsNullOrEmpty(dataTableParameters.OrderedColumn.Column))
                dataTableParameters.OrderedColumn = new OrderedColumn { Column = "Id", Dir = "asc" };

            var oProp = typeof(T).GetProperties().FirstOrDefault(p => p.Name.ToLower() == dataTableParameters.OrderedColumn.Column.ToLower());
            if (oProp != null)
            {
                var orderBy = oProp.Name;
                var orderByProperty = Expression.Property(t, orderBy);
                var orderByToString = Expression.Call(orderByProperty, "ToString", Type.EmptyTypes); //avaliar os tipos das colunas - convertendo todas para string

                Expression orderByToStringMethodCall = null;

                var piOrderBy = typeof(T).GetProperty(orderBy);

                if (!piOrderBy.PropertyType.IsValueType || piOrderBy.PropertyType.IsGenericType && piOrderBy.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var notNull = Expression.NotEqual(orderByProperty, Expression.Constant(null, typeof(object)));

                    orderByToStringMethodCall = Expression.Condition(notNull, orderByToString, Expression.Constant(""));
                }
                else
                    orderByToStringMethodCall = orderByToString;

                var orderByLambda = Expression.Lambda(orderByToStringMethodCall, new[] { t });

                orderByCallExpression = Expression.Call(
                    typeof(Queryable),
                    dataTableParameters.OrderedColumn.Dir.ToLower() == "asc" ? "OrderBy" : "OrderByDescending",
                    new Type[] { query.ElementType, typeof(String) },
                    whereCallExpression,
                    orderByLambda);
            }

            var result = query.Provider.CreateQuery<T>(orderByCallExpression);

            if (dataTableParameters.Start > 0)
                result = result.Skip(dataTableParameters.Start);

            if (dataTableParameters.Length > 0)
                result = result.Take(dataTableParameters.Length);

            return result;
        }
        
    }
}