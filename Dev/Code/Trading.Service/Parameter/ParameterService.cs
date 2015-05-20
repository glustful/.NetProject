






using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using Trading.Entity.Model;

namespace Trading.Service.Parameter
{
    public class ParameterService : IParameterService
    {
        private readonly Zerg.Common.Data.ITradingRepository<ParameterEntity> _parameterRepository;
        private readonly ILog _log;

        public ParameterService(Zerg.Common.Data.ITradingRepository<ParameterEntity> parameterRepository, ILog log)
        {
            _parameterRepository = parameterRepository;
            _log = log;
        }

        public ParameterEntity Create(ParameterEntity entity)
        {
            try
            {
                _parameterRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public bool Delete(ParameterEntity entity)
        {
            try
            {
                _parameterRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return false;
            }
        }

        public ParameterEntity Update(ParameterEntity entity)
        {
            try
            {
                _parameterRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public ParameterEntity GetParameterById(int id)
        {
            try
            {
                return _parameterRepository.GetById(id);
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }
        public IQueryable<ParameterEntity> GetParameterEntitysByClassifyId(int classifyId)
        {
            var query = _parameterRepository.Table;
            try
            {
                query = query.Where(q => q.Classify.Id == classifyId);
                return query.OrderBy(q => q.Id);
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }

        }
        public IQueryable<ParameterEntity> GetParametersByCondition(ParameterSearchCondition condition)
        {
            var query = _parameterRepository.Table;
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


                if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(q => q.Name.Contains(condition.Name));
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


                if (condition.Classifys != null && condition.Classifys.Any())
                {
                    query = query.Where(q => condition.Classifys.Contains(q.Classify));
                }


                if (condition.Sorts != null && condition.Sorts.Any())
                {
                    query = query.Where(q => condition.Sorts.Contains(q.Sort));
                }




                if (condition.OrderBy.HasValue)
                {
                    switch (condition.OrderBy.Value)
                    {

                        case EnumParameterSearchOrderBy.OrderById:
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

        public int GetParameterCount(ParameterSearchCondition condition)
        {
            var query = _parameterRepository.Table;
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


                if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(q => q.Name.Contains(condition.Name));
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


                if (condition.Classifys != null && condition.Classifys.Any())
                {
                    query = query.Where(q => condition.Classifys.Contains(q.Classify));
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