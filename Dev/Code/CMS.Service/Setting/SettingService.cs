using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CMS.Entity.Model;
using Zerg.Common.Data;

namespace CMS.Service.Setting
{
	public class SettingService : ISettingService
	{
		private readonly ICMSRepository<SettingEntity> _settingRepository;
		private readonly ILog _log;

		public SettingService(ICMSRepository<SettingEntity> settingRepository,ILog log)
		{
			_settingRepository = settingRepository;
			_log = log;
		}
		
		public SettingEntity Create (SettingEntity entity)
		{
			try
            {
                _settingRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(SettingEntity entity)
		{
			try
            {
                _settingRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public SettingEntity Update (SettingEntity entity)
		{
			try
            {
                _settingRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public SettingEntity GetSettingById (int id)
		{
			try
            {
                return _settingRepository.GetById(id); ;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<SettingEntity> GetSettingsByCondition(SettingSearchCondition condition)
		{
			var query = _settingRepository.Table;
			try
			{
				if (string.IsNullOrEmpty(condition.Key))
                {
                    query = query.Where(q => q.Key == condition.Key);
                }
				if (string.IsNullOrEmpty(condition.Value))
                {
                    query = query.Where(q => q.Value == condition.Value);
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumSettingSearchOrderBy.OrderById:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
							break;
                    }
					
				}
				else
				{
					query = query.OrderBy(q=>q.Id);
				}

				if (condition.Page.HasValue && condition.PageCount.HasValue)
                {
                    query = query.Skip((condition.Page.Value - 1)*condition.PageCount.Value).Take(condition.PageCount.Value);
                }
				return query;
			}
			catch(Exception e)
			{
				_log.Error(e,"数据库操作出错");
                return null;
			}
		}

		public int GetSettingCount (SettingSearchCondition condition)
		{
			var query = _settingRepository.Table;
			try
			{
				if (string.IsNullOrEmpty(condition.Key))
                {
                    query = query.Where(q => q.Key == condition.Key);
                }
				if (string.IsNullOrEmpty(condition.Value))
                {
                    query = query.Where(q => q.Value == condition.Value);
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				return query.Count();
			}
			catch(Exception e)
			{
				_log.Error(e,"数据库操作出错");
                return -1;
			}
		}
	}
}