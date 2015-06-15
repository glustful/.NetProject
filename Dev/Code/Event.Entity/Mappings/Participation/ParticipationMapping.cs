using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Event.Entity.Model;

namespace Event.Entity.Mappings.Participation
{
	public class ParticipationMapping : EntityTypeConfiguration<ParticipationEntity>, IMapping
	{
		public ParticipationMapping()
		{
			ToTable("Participation");
			HasKey(c => c.Id);
			Property(c => c.Id).HasColumnType("int").IsOptional();
			HasOptional(c =>c.Crowd);
			Property(c => c.Username).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.UseId).HasColumnType("int");
			Property(c => c.Phone).HasColumnType("varchar").HasMaxLength(11);
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
		}
	}
}