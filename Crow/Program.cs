using Autofac;
using Crow.Api.Compiler;
using Crow.Api.Configs;
using Crow.Api.Dependencies;
using Crow.Api.Repositories;
using Crow.Commands;
using Crow.Compiler;
using Crow.Configs;
using Crow.Dependencies;
using Crow.Repositories;
using Raven;

namespace Crow
{
    class Program
    {
        static void Main(string[] args)
        {
            IContainer container = null;
        
            var builder = new ContainerBuilder();
            
            builder.RegisterType<CommandManager>()
                    .As<ICommandManager>()
                    .SingleInstance();
            builder.RegisterType<ConfigManager>()
                    .As<IConfigManager>()
                    .SingleInstance();
            builder.RegisterType<CompilerManager>()
                    .As<ICompilerManager>()
                    .SingleInstance();
            builder.RegisterType<DependencyManager>()
                    .As<IDependencyManager>()
                    .SingleInstance();
            builder.RegisterType<RepositoryManager>()
                    .As<IRepositoryManager>()
                    .SingleInstance();
            
            builder.RegisterType<Crow>()
                    .AsSelf();

            builder.RegisterType<HjsonConfigSerializer>()
                    .As<IConfigSerializer>();
            builder.RegisterType<HjsonConfigDeserializer>()
                    .As<IConfigDeserializer>();
            
            builder.RegisterType<InteractiveCommand>()
                    .AsSelf();
            builder.RegisterType<BuildCommand>()
                    .AsSelf();
            
            builder.Register(c => container)
                    .AsSelf();
            builder.RegisterBuildCallback(c => container = c);

            container = builder.Build();

            var crow = container.Resolve<Crow>();
            
            crow.Initialize();
            crow.Start(args);
        }
    }
}