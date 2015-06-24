using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.Task
{
	public interface ITaskService : IDependency
	{
		TaskEntity Create (TaskEntity entity);

		bool Delete(TaskEntity entity);

		TaskEntity Update (TaskEntity entity);

		TaskEntity GetTaskById (int id);

		IQueryable<TaskEntity> GetTasksByCondition(TaskSearchCondition condition);

		int GetTaskCount (TaskSearchCondition condition);
	}
}