






using System;
using System.Collections.Generic;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using Trading.Entity.Model;

namespace Trading.Service.ParameterValue
{
    public class ParameterValueService : IParameterValueService
    {
        private readonly Zerg.Common.Data.ITradingRepository<ParameterValueEntity> _parametervalueRepository;
        private readonly ILog _log;

        public ParameterValueService(Zerg.Common.Data.ITradingRepository<ParameterValueEntity> parametervalueRepository, ILog log)
        {
            _parametervalueRepository = parametervalueRepository;
            _log = log;
        }

        public ParameterValueEntity Create(ParameterValueEntity entity)
        {
            try
            {
                _parametervalueRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public bool Delete(ParameterValueEntity entity)
        {
            try
            {
                _parametervalueRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return false;
            }
        }

        public ParameterValueEntity Update(ParameterValueEntity entity)
        {
            try
            {
                _parametervalueRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public ParameterValueEntity GetParameterValueById(int id)
        {
            try
            {
                return _parametervalueRepository.GetById(id);
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }
        public IQueryable<ParameterValueEntity> GetParameterValuesByCondition(ParameterValueSearchCondition condition)
        {
            var query = _parametervalueRepository.Table;
            try
            {

                if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Addtime >= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Addtime < condition.AddtimeEnd.Value);
                }


                if (condition.UpdtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Updtime >= condition.UpdtimeBegin.Value);
                }
                if (condition.UpdtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Updtime < condition.UpdtimeEnd.Value);
                }


                if (!string.IsNullOrEmpty(condition.Parametervalue))
                {
                    query = query.Where(q => q.Parametervalue.Contains(condition.Parametervalue));
                }



                if (!string.IsNullOrEmpty(condition.Adduser))
                {
                    query = query.Where(q => q.Adduser.Contains(condition.Adduser));
                }



                if (!string.IsNullOrEmpty(condition.Upduser))
                {
                    query = query.Where(q => q.Upduser.Contains(condition.Upduser));
                }



                if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }


                if (condition.Parameters != null && condition.Parameters.Any())
                {
                    query = query.Where(q => condition.Parameters.Contains(q.Parameter));
                }


                if (condition.Sorts != null && condition.Sorts.Any())
                {
                    query = query.Where(q => condition.Sorts.Contains(q.Sort));
                }




                if (condition.OrderBy.HasValue)
                {
                    switch (condition.OrderBy.Value)
                    {

                        case EnumParameterValueSearchOrderBy.OrderById:
                            query = condition.IsDescending ? query.OrderByDescending(q => q.Id) : query.OrderBy(q => q.Id);
                            break;

                    }

                }

                else
                {
                    query = query.OrderBy(q => q.Id);
                }

                if (condition.Page.HasValue && condition.PageCount.HasValue)
                {
                    query = query.Skip((condition.Page.Value - 1) * condition.PageCount.Value).Take(condition.PageCount.Value);
                }
                return query;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        /// <summary>
        /// 获取参数下所有参数值；
        /// </summary>
        /// <param name="parameterId"></param>
        /// <returns></returns>
        public IQueryable<ParameterValueEntity> GetParameterValuesByParameter(int parameterId)
        {
            var query = _parametervalueRepository.Table;
            try
            {
                query = query.Where(q => q.Parameter.Id == parameterId);
                return query.OrderBy(q => q.Id);
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }
        public int GetParameterValueCount(ParameterValueSearchCondition condition)
        {
            var query = _parametervalueRepository.Table;
            try
            {

                if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Addtime >= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Addtime < condition.AddtimeEnd.Value);
                }


                if (condition.UpdtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Updtime >= condition.UpdtimeBegin.Value);
                }
                if (condition.UpdtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Updtime < condition.UpdtimeEnd.Value);
                }


                if (!string.IsNullOrEmpty(condition.Parametervalue))
                {
                    query = query.Where(q => q.Parametervalue.Contains(condition.Parametervalue));
                }



                if (!string.IsNullOrEmpty(condition.Adduser))
                {
                    query = query.Where(q => q.Adduser.Contains(condition.Adduser));
                }



                if (!string.IsNullOrEmpty(condition.Upduser))
                {
                    query = query.Where(q => q.Upduser.Contains(condition.Upduser));
                }



                if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }


                if (condition.Parameters != null && condition.Parameters.Any())
                {
                    query = query.Where(q => condition.Parameters.Contains(q.Parameter));
                }


                if (condition.Sorts != null && condition.Sorts.Any())
                {
                    query = query.Where(q => condition.Sorts.Contains(q.Sort));
                }



                return query.Count();
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return -1;
            }
        }
    }
}