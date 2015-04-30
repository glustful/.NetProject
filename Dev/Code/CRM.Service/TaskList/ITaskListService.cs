using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.TaskList
{
	public interface ITaskListService : IDependency
	{
		TaskListEntity Create (TaskListEntity entity);

		bool Delete(TaskListEntity entity);

		TaskListEntity Update (TaskListEntity entity);

		TaskListEntity GetTaskListById (int id);

		IQueryable<TaskListEntity> GetTaskListsByCondition(TaskListSearchCondition condition);

		int GetTaskListCount (TaskListSearchCondition condition);
	}
}