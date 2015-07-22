using System;
using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.BrokerWithdraw
{
    public class BrokerWithdrawMapping : EntityTypeConfiguration<BrokerWithdrawEntity>, Zerg.Common.Data.IZergMapping
    {
        public BrokerWithdrawMapping()
        {
            ToTable("BrokerWithdraw");
            HasKey(c => c.Id);

            HasOptional(c => c.Broker);
            HasOptional(c => c.BankCard);
            HasOptional(c => c.AccAccountantId);
            Property(c => c.WithdrawTime).HasColumnType("datetime");
            Property(c => c.WithdrawTotalNum).HasColumnType("decimal");
            Property(c => c.Adduser).HasColumnType("int");
            Property(c => c.Addtime).HasColumnType("datetime");
            Property(c => c.Upuser).HasColumnType("int");
            Property(c => c.Uptime).HasColumnType("datetime");
            Property(c => c.State).HasColumnType("int");
            Property(c => c.WithdrawDesc).HasColumnType("varchar").HasMaxLength(30);
            Property(c => c.BankSn).HasColumnType("varchar").HasMaxLength(30);
        }
    }
}
