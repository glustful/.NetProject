using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.TaskTag
{
	public class TaskTagMapping : EntityTypeConfiguration<TaskTagEntity>, Zerg.Common.Data.IZergMapping
	{
		public TaskTagMapping()
		{
			ToTable("TaskTag");
			HasKey(c => c.Id);
			 
			Property(c => c.Name).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.Describe).HasColumnType("varchar").HasMaxLength(256).IsOptional();
			Property(c => c.Value).HasColumnType("varchar").HasMaxLength(50);
		}
	}
}