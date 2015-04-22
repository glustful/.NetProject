using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.TaskTag
{
	public interface ITaskTagService : IDependency
	{
		TaskTagEntity Create (TaskTagEntity entity);

		bool Delete(TaskTagEntity entity);

		TaskTagEntity Update (TaskTagEntity entity);

		TaskTagEntity GetTaskTagById (int id);

		IQueryable<TaskTagEntity> GetTaskTagsByCondition(TaskTagSearchCondition condition);

		int GetTaskTagCount (TaskTagSearchCondition condition);
	}
}