using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.TaskAward
{
	public interface ITaskAwardService : IDependency
	{
		TaskAwardEntity Create (TaskAwardEntity entity);

		bool Delete(TaskAwardEntity entity);

		TaskAwardEntity Update (TaskAwardEntity entity);

		TaskAwardEntity GetTaskAwardById (int id);

		IQueryable<TaskAwardEntity> GetTaskAwardsByCondition(TaskAwardSearchCondition condition);

		int GetTaskAwardCount (TaskAwardSearchCondition condition);
	}
}