using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.TaskType
{
	public interface ITaskTypeService : IDependency
	{
		TaskTypeEntity Create (TaskTypeEntity entity);

		bool Delete(TaskTypeEntity entity);

		TaskTypeEntity Update (TaskTypeEntity entity);

		TaskTypeEntity GetTaskTypeById (int id);

		IQueryable<TaskTypeEntity> GetTaskTypesByCondition(TaskTypeSearchCondition condition);

		int GetTaskTypeCount (TaskTypeSearchCondition condition);
	}
}