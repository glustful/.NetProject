using System;
using System.Linq;
using Community.Entity.Model.Category;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Community.Service.Category
{
	public class CategoryService : ICategoryService
	{
		private readonly ICommunityRepository<CategoryEntity> _categoryRepository;
		private readonly ILog _log;

		public CategoryService(ICommunityRepository<CategoryEntity> categoryRepository,ILog log)
		{
			_categoryRepository = categoryRepository;
			_log = log;
		}
		
		public CategoryEntity Create (CategoryEntity entity)
		{
			try
            {
                _categoryRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(CategoryEntity entity)
		{
			try
            {
                _categoryRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public CategoryEntity Update (CategoryEntity entity)
		{
			try
            {
                _categoryRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public CategoryEntity GetCategoryById (int id)
		{
			try
            {
                return _categoryRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}
      
        public IQueryable<CategoryEntity> GetCategorysBySuperFather(int father)
        {
            var query = _categoryRepository.Table;
            try
            {
                if(father==0)
                {
                    query = query.Where(q => q.Father.Id == null);
                }
                else 
                { 
                query = query.Where(q => q.Father.Id == father);
                }
                return query.OrderBy(q => q.Id);
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }
		public IQueryable<CategoryEntity> GetCategorysByCondition(CategorySearchCondition condition)
		{
			var query = _categoryRepository.Table;
			try
			{
				if (condition.AddTimeBegin.HasValue)
                {
                    query = query.Where(q => q.AddTime>= condition.AddTimeBegin.Value);
                }
                if (condition.AddTimeEnd.HasValue)
                {
                    query = query.Where(q => q.AddTime < condition.AddTimeEnd.Value);
                }
				if (condition.UpdTimeBegin.HasValue)
                {
                    query = query.Where(q => q.UpdTime>= condition.UpdTimeBegin.Value);
                }
                if (condition.UpdTimeEnd.HasValue)
                {
                    query = query.Where(q => q.UpdTime < condition.UpdTimeEnd.Value);
                }
				if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(q => q.Name.Contains(condition.Name));
                }
				if (!string.IsNullOrEmpty(condition.AddUser))
                {
                    query = query.Where(q => q.AddUser == int.Parse(condition.AddUser));
                }
				if (!string.IsNullOrEmpty(condition.UpdUser))
                {
                    query = query.Where(q => q.UpdUser == int.Parse(condition.UpdUser));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Sorts != null && condition.Sorts.Any())
                {
                    query = query.Where(q => condition.Sorts.Contains(q.Sort));
                }

				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumCategorySearchOrderBy.OrderById:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
							break;
						case EnumCategorySearchOrderBy.OrderByAddTime:
							query = condition.IsDescending?query.OrderByDescending(q=>q.AddTime):query.OrderBy(q=>q.AddTime);
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

		public int GetCategoryCount (CategorySearchCondition condition)
		{
			var query = _categoryRepository.Table;
			try
			{
				if (condition.AddTimeBegin.HasValue)
                {
                    query = query.Where(q => q.AddTime>= condition.AddTimeBegin.Value);
                }
                if (condition.AddTimeEnd.HasValue)
                {
                    query = query.Where(q => q.AddTime < condition.AddTimeEnd.Value);
                }
				if (condition.UpdTimeBegin.HasValue)
                {
                    query = query.Where(q => q.UpdTime>= condition.UpdTimeBegin.Value);
                }
                if (condition.UpdTimeEnd.HasValue)
                {
                    query = query.Where(q => q.UpdTime < condition.UpdTimeEnd.Value);
                }
				if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(q => q.Name.Contains(condition.Name));
                }
				if (!string.IsNullOrEmpty(condition.AddUser))
                {
                    query = query.Where(q => q.AddUser==int.Parse(condition.AddUser));
                }
				if (!string.IsNullOrEmpty(condition.UpdUser))
                {
                    query = query.Where(q => q.UpdUser==int.Parse(condition.UpdUser));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Sorts != null && condition.Sorts.Any())
                {
                    query = query.Where(q => condition.Sorts.Contains(q.Sort));
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