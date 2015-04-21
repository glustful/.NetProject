using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CMS.Entity.Model;

namespace CMS.Entity.Mappings.Advertisement
{
	public class AdvertisementMapping : EntityTypeConfiguration<AdvertisementEntity>, IMapping
	{
		public AdvertisementMapping()
		{
			ToTable("Advertisement");
			HasKey(c => c.Id);
			Property(c => c.Id).HasColumnType("int");
			Property(c => c.Title).HasColumnType("varchar");
			Property(c => c.Detail).HasColumnType("varchar").IsOptional();
			Property(c => c.Continue).HasColumnType("datetime");
			HasOptional(c =>c.Content);
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.UpdUser).HasColumnType("int").IsOptional();
			Property(c => c.UpdTime).HasColumnType("datetime").IsOptional();
		}
	}
}