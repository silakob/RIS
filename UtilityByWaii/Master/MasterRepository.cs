using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace OmniWMS.Domain.AggregatesModels.Master
{
    public class MasterRepository<TContext> where TContext : DbContext
    {
        private TContext _context;
        public MasterRepository(TContext context)
        {
            _context = context;
        }
        public T GetMasterData<T>(T model, Expression<Func<T, object>> orderBy = null) where T : class
        {
            try
            {
                Type type = model.GetType();
                PropertyInfo[] properties = type.GetProperties();
                Expression<Func<T, bool>> where = null;
                foreach (var prop in properties)
                {
                    object value = prop.GetValue(model, null);
                    var key = prop.Name;
                    string propType = prop.PropertyType.FullName;
                    ParameterExpression parameter = Expression.Parameter(type, "con");
                    MemberExpression body = Expression.MakeMemberAccess(parameter, prop);
                    Expression left = body;
                    Expression right = Expression.Constant(value);
                    Expression searchExpression = Expression.Equal(left, right);
                    Expression<Func<T, bool>> unaryExpression = null;
                    switch (propType)
                    {
                        case "System.String":
                            if (!String.IsNullOrEmpty((string)value))
                            {
                                unaryExpression = Expression.Lambda<Func<T, bool>>(searchExpression, parameter);
                                where = where == null ? unaryExpression : where.And(unaryExpression);
                            }
                            break;
                        case "System.Guid":
                            if ((Guid)value != Guid.Empty)
                            {
                                unaryExpression = Expression.Lambda<Func<T, bool>>(searchExpression, parameter);
                                where = where == null ? unaryExpression : where.And(unaryExpression);
                            }
                            break;
                        case "System.DateTime":
                            if ((DateTime)value != DateTime.MinValue)
                            {
                                unaryExpression = Expression.Lambda<Func<T, bool>>(searchExpression, parameter);
                                where = where == null ? unaryExpression : where.And(unaryExpression);
                            }
                            break;
                        default:
                            break;
                    }
                }
                IQueryable<T> queryable = where != null ? _context.Set<T>().Where(where) : _context.Set<T>();
                var resultModel = (orderBy != null) ? queryable.OrderBy(orderBy).FirstOrDefault() : queryable.FirstOrDefault();
                return resultModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<T> GetMasterDatas<T>(T model, Expression<Func<T, object>> orderBy = null) where T : class
        {
            try
            {
                Type type = model.GetType();
                PropertyInfo[] properties = type.GetProperties();
                Expression<Func<T, bool>> where = null;
                foreach (var prop in properties)
                {
                    object value = prop.GetValue(model, null);
                    var key = prop.Name;
                    string propType = prop.PropertyType.FullName;
                    ParameterExpression parameter = Expression.Parameter(type, "con");
                    MemberExpression body = Expression.MakeMemberAccess(parameter, prop);
                    Expression left = body;
                    Expression right = Expression.Constant(value);
                    Expression searchExpression = Expression.Equal(left, right);
                    Expression<Func<T, bool>> unaryExpression = null;
                    switch (propType)
                    {
                        case "System.String":
                            if (!String.IsNullOrEmpty((string)value))
                            {
                                unaryExpression = Expression.Lambda<Func<T, bool>>(searchExpression, parameter);
                                where = where == null ? unaryExpression : where.And(unaryExpression);
                            }
                            break;
                        case "System.Guid":
                            if ((Guid)value != Guid.Empty)
                            {
                                unaryExpression = Expression.Lambda<Func<T, bool>>(searchExpression, parameter);
                                where = where == null ? unaryExpression : where.And(unaryExpression);
                            }
                            break;
                        case "System.DateTime":
                            if ((DateTime)value != DateTime.MinValue)
                            {
                                unaryExpression = Expression.Lambda<Func<T, bool>>(searchExpression, parameter);
                                where = where == null ? unaryExpression : where.And(unaryExpression);
                            }
                            break;
                        default:
                            break;
                    }
                }
                IQueryable<T> queryable = where != null ? _context.Set<T>().Where(where) : _context.Set<T>();
                var resultModels = (orderBy != null) ? queryable.OrderBy(orderBy).ToList() : queryable.ToList();
                return resultModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Param => string storeName, OB (Object)
        /// Return => Affected Row (int)
        /// </summary>
        public int CallNonQueryStoreProcedure<T>(string storeName, T @object)
        {
            int affectedRow = 0;
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    List<DbParameter> @params = new List<DbParameter>();
                    Type type = @object.GetType();
                    PropertyInfo[] properties = type.GetProperties();
                    foreach (var prop in properties)
                    {
                        object value = prop.GetValue(@object, null);
                        var key = "@" + prop.Name;
                        string propType = prop.PropertyType.FullName;
                        DbParameter param = command.CreateParameter();
                        switch (propType)
                        {
                            case "System.String":
                                if (!String.IsNullOrEmpty((string)value))
                                {
                                    param.ParameterName = key;
                                    param.DbType = DbType.String;
                                    param.Direction = ParameterDirection.Input;
                                    param.Value = value;
                                    @params.Add(param);
                                }
                                break;
                            case "System.Guid":
                                if ((Guid)value != Guid.Empty)
                                {
                                    param.ParameterName = key;
                                    param.DbType = DbType.Guid;
                                    param.Direction = ParameterDirection.Input;
                                    param.Value = value;
                                    @params.Add(param);
                                }
                                break;
                            case "System.DateTime":
                                if ((DateTime)value != DateTime.MinValue)
                                {
                                    param.ParameterName = key;
                                    param.DbType = DbType.DateTime;
                                    param.Direction = ParameterDirection.Input;
                                    param.Value = value;
                                    @params.Add(param);
                                }
                                break;
                            case "System.Boolean":
                                param.ParameterName = key;
                                param.DbType = DbType.Boolean;
                                param.Direction = ParameterDirection.Input;
                                param.Value = value;
                                @params.Add(param);
                                break;
                            case "System.Decimal":
                                param.ParameterName = key;
                                param.DbType = DbType.Decimal;
                                param.Direction = ParameterDirection.Input;
                                param.Value = value;
                                @params.Add(param);
                                break;
                            case "System.Int32":
                                param.ParameterName = key;
                                param.DbType = DbType.Int32;
                                param.Direction = ParameterDirection.Input;
                                param.Value = value;
                                @params.Add(param);
                                break;
                            default:
                                break;
                        }
                    }
                    command.Parameters.AddRange(@params.ToArray());
                    command.CommandText = storeName;
                    command.CommandType = CommandType.StoredProcedure;
                    _context.Database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {
                        if (result.HasRows)
                        {
                            result.Read();
                            affectedRow = result.GetInt32(0); // x = your sp count value
                        }
                    }
                }

                return affectedRow;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
