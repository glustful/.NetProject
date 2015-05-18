using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.TaskPunishment
{
	public interface ITaskPunishmentService : IDependency
	{
		TaskPunishmentEntity Create (TaskPunishmentEntity entity);

		bool Delete(TaskPunishmentEntity entity);

		TaskPunishmentEntity Update (TaskPunishmentEntity entity);

		TaskPunishmentEntity GetTaskPunishmentById (int id);

		IQueryable<TaskPunishmentEntity> GetTaskPunishmentsByCondition(TaskPunishmentSearchCondition condition);

		int GetTaskPunishmentCount (TaskPunishmentSearchCondition condition);
	}
}