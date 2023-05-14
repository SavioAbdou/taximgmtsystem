using System.Reflection;
using Autofac;
using TaxiManagementSystem.DAL.Interfaces;
using TaxiManagementSystem.DAL.Repository;

namespace TaxiManagementSystem.Web.Modules
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("TaxiManagementSystem.DAL"))
                .Where(x => x.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType(typeof(UserManager)).As(typeof(IUserManager)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(RoleManager)).As(typeof(IRoleManager)).InstancePerLifetimeScope();
        }
    }
}