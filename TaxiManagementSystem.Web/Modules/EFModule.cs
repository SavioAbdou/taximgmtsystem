using Autofac;
using TaxiManagementSystem.DAL;
using TaxiManagementSystem.DAL.Interfaces;
using TaxiManagementSystem.DAL.UnitOfWork;

namespace TaxiManagementSystem.Web.Modules
{
    public class EFModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new RepositoryModule());

            builder.RegisterType(typeof(ApplicationDbContext)).AsSelf().InstancePerLifetimeScope();

            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork)).InstancePerRequest();
        }
    }
}