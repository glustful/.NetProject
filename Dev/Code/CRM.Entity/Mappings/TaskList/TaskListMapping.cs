using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.TaskList
{
	public class TaskListMapping : EntityTypeConfiguration<TaskListEntity>, IMapping
	{
		public TaskListMapping()
		{
			ToTable("TaskList");
			HasKey(c => c.Id);
			 
			HasRequired(c =>c.Task);
			HasRequired(c =>c.Broker);
			Property(c => c.Taskschedule).HasColumnType("varchar").HasMaxLength(50);
		}
	}
}