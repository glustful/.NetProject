using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.Entity.Model.Service;
using YooPoon.Core.Data;
using Zerg.Common.Data;

namespace Community.Entity.Mappings.Service
{
    public class ServiceMapping : EntityTypeConfiguration<ServiceEntity>, IZergMapping
    {
        public ServiceMapping()
        {
            ToTable("Service");
            HasKey(c => c.Id);
            Property(c => c.AddTime).HasColumnType("datetime").IsOptional();
            Property(c => c.AddUser).HasColumnType("int").IsOptional();
            Property(c => c.Class).HasColumnType("varchar").HasMaxLength(50).IsOptional();
            Property(c => c.Name).HasColumnType("varchar").HasMaxLength(50).IsOptional();
            //Property(c => c.ServiceId).HasColumnType("int").IsOptional();
            Property(c => c.Link).HasColumnType("varchar").HasMaxLength(50).IsOptional();
            Property(c => c.UpTime).HasColumnType("datetime").IsOptional();
            Property(c => c.UpUser).HasColumnType("int").IsOptional();
        }
    }
}
