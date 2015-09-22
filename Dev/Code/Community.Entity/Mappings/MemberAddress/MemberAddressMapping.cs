using System.Data.Entity.ModelConfiguration;
using Community.Entity.Model.MemberAddress;
using Zerg.Common.Data;

namespace Community.Entity.Mappings.MemberAddress
{
	public class MemberAddressMapping : EntityTypeConfiguration<MemberAddressEntity>, IZergMapping
	{
		public MemberAddressMapping()
		{
			ToTable("MemberAddress");
			HasKey(c => c.Id);
			HasRequired(c =>c.Member).WithMany(c=>c.Address);
            HasOptional(c => c.Area);
            //todo:在正式版本后改为required
			Property(c => c.Address).HasColumnType("varchar").HasMaxLength(30).IsOptional();
			Property(c => c.Zip).HasColumnType("varchar").HasMaxLength(10).IsOptional();
			Property(c => c.Linkman).HasColumnType("varchar").HasMaxLength(30).IsOptional();
			Property(c => c.Tel).HasColumnType("varchar").HasMaxLength(15).IsOptional();
			Property(c => c.Adduser).HasColumnType("int").IsOptional();
			Property(c => c.Addtime).HasColumnType("datetime").IsOptional();
			Property(c => c.Upduser).HasColumnType("int").IsOptional();
			Property(c => c.Updtime).HasColumnType("datetime").IsOptional();
		    Property(c => c.IsDefault).HasColumnType("bit").IsOptional();
		}
	}
}