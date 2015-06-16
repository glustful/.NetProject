using System.Data.Entity.ModelConfiguration;
using Event.Entity.Model;
using Zerg.Common.Data;

namespace Event.Entity.Mappings.Participation
{
	public class ParticipationMapping : EntityTypeConfiguration<ParticipationEntity>, IZergMapping
	{
		public ParticipationMapping()
		{
			ToTable("Participation");
			HasKey(c => c.Id);

			HasRequired(c =>c.Crowd).WithOptional();
			Property(c => c.Username).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Phone).HasColumnType("varchar").HasMaxLength(11);
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}