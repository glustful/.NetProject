using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CMS.Entity.Model;
using Zerg.Common.Data;

namespace CMS.Entity.Mappings.Channel
{
	public class ChannelMapping : EntityTypeConfiguration<ChannelEntity>, IZergMapping
	{
		public ChannelMapping()
		{
			ToTable("Channel");
			HasKey(c => c.Id);
			Property(c => c.Name).HasColumnType("varchar");
			Property(c => c.Status).HasColumnType("int");
			HasOptional(c =>c.Parent);
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.UpdUser).HasColumnType("int").IsOptional();
			Property(c => c.UpdTime).HasColumnType("datetime").IsOptional();
		    HasMany(c => c.Contents).WithRequired(c => c.Channel);
		}
	}
}