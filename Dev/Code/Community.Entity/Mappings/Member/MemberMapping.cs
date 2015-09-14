using System.Data.Entity.ModelConfiguration;
using Community.Entity.Model.Member;
using Zerg.Common.Data;

namespace Community.Entity.Mappings.Member
{
	public class MemberMapping : EntityTypeConfiguration<MemberEntity>, IZergMapping
	{
		public MemberMapping()
		{
			ToTable("Member");
			HasKey(c => c.Id);
			
			Property(c => c.RealName).HasColumnType("varchar").HasMaxLength(50).IsOptional();
			Property(c => c.IdentityNo).HasColumnType("varchar").HasMaxLength(20).IsOptional();
			Property(c => c.Gender).HasColumnType("int").IsOptional();
			Property(c => c.Phone).HasColumnType("varchar").HasMaxLength(20).IsOptional();
			Property(c => c.Icq).HasColumnType("varchar").HasMaxLength(20).IsOptional();
			Property(c => c.PostNo).HasColumnType("varchar").HasMaxLength(20).IsOptional();
			Property(c => c.Thumbnail).HasColumnType("varchar").HasMaxLength(255).IsOptional();
			Property(c => c.AccountNumber).HasColumnType("decimal").IsOptional();
			Property(c => c.Points).HasColumnType("decimal").IsOptional();
			Property(c => c.Level).HasColumnType("int").IsOptional();
			Property(c => c.AddTime).HasColumnType("datetime").IsOptional();
			Property(c => c.UpdUser).HasColumnType("int").IsOptional();
			Property(c => c.UpdTime).HasColumnType("datetime").IsOptional();
		    Property(c => c.UserId).HasColumnType("int").IsRequired();

		    HasMany(c => c.Address).WithRequired(c=>c.Member);
		}
	}
}