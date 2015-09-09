using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using YooPoon.Core.Autofac;
using YooPoon.Data.EntityFramework;

namespace Zerg.Common.Data
{
    public class CMSDbContext : EfDbContext
    {
        public CMSDbContext() { }
        public CMSDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typeFinder = new AppDomainTypeFinder();
            var typesToRegister = typeFinder.FindClassesOfType(typeof(IZergMapping), false)
                .Where(type => !String.IsNullOrEmpty(type.Namespace) && type.Namespace.StartsWith("CMS"))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            base.BaseOnModelCreate(modelBuilder);
        }
    }

    public class CRMDbContext : EfDbContext
    {
        public CRMDbContext() { }
        public CRMDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typeFinder = new AppDomainTypeFinder();
            var typesToRegister = typeFinder.FindClassesOfType(typeof(IZergMapping), false)
                .Where(type => !String.IsNullOrEmpty(type.Namespace) && type.Namespace.StartsWith("CRM"))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            base.BaseOnModelCreate(modelBuilder);
        }
    }

    public class TradingDbContext : EfDbContext
    {
        public TradingDbContext() { }
        public TradingDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typeFinder = new AppDomainTypeFinder();
            var typesToRegister = typeFinder.FindClassesOfType(typeof(IZergMapping), false)
                .Where(type => !String.IsNullOrEmpty(type.Namespace) && type.Namespace.StartsWith("Trading"))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            base.BaseOnModelCreate(modelBuilder);
        }
    }

    public class EventDbContext : EfDbContext
    {
        public EventDbContext() { }
        public EventDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typeFinder = new AppDomainTypeFinder();
            var typesToRegister = typeFinder.FindClassesOfType(typeof(IZergMapping), false)
                .Where(type => !String.IsNullOrEmpty(type.Namespace) && type.Namespace.StartsWith("Event"))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            base.BaseOnModelCreate(modelBuilder);
        }
    }

    public class CommunityDbContext : EfDbContext
    {
        public CommunityDbContext() { }
        public CommunityDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typeFinder = new AppDomainTypeFinder();
            var typesToRegister = typeFinder.FindClassesOfType(typeof(IZergMapping), false)
                .Where(type => !String.IsNullOrEmpty(type.Namespace) && type.Namespace.StartsWith("Community"))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            base.BaseOnModelCreate(modelBuilder);
        }
    }
}
