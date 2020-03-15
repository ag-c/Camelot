using System;
using Camelot.Behaviors.Implementations;
using Camelot.Behaviors.Interfaces;
using Camelot.Factories.Implementations;
using Camelot.Factories.Interfaces;
using Camelot.FileSystemWatcherWrapper.Implementations;
using Camelot.FileSystemWatcherWrapper.Interfaces;
using Camelot.Providers.Implementations;
using Camelot.Providers.Interfaces;
using Camelot.Services.Implementations;
using Camelot.Services.Interfaces;
using Camelot.Services.Operations.Implementations;
using Camelot.Services.Operations.Interfaces;
using Camelot.TaskPool.Interfaces;
using Camelot.ViewModels;
using Camelot.ViewModels.MainWindow;
using Splat;

namespace Camelot
{
    public static class Bootstrapper
    {
        public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            RegisterServices(services, resolver);
            RegisterViewModels(services, resolver);
        }

        private static void RegisterServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            services.RegisterLazySingleton<IFileService>(() => new FileService());
            services.RegisterLazySingleton<IFileSystemWatcherWrapperFactory>(() => new FileSystemWatcherWrapperFactory());
            services.RegisterLazySingleton<ITaskPool>(() => new TaskPool.Implementations.TaskPool(Environment.ProcessorCount));
            services.Register<IOperationsFactory>(() => new OperationsFactory(
                resolver.GetService<ITaskPool>()));
            services.RegisterLazySingleton<IFileSystemWatchingService>(() => new FileSystemWatchingService(
                resolver.GetService<IFileSystemWatcherWrapperFactory>()));
            services.RegisterLazySingleton<IFilesSelectionService>(() => new FilesSelectionService());
            services.RegisterLazySingleton<IDirectoryService>(() => new DirectoryService());
            services.RegisterLazySingleton<IFileOpeningService>(() => new FileOpeningService());
            services.RegisterLazySingleton(() => new FileOpeningBehavior(
                resolver.GetService<IFileOpeningService>()));
            services.RegisterLazySingleton(() => new DirectoryOpeningBehavior(
                resolver.GetService<IMainWindowViewModelProvider>()));
        }

        private static void RegisterViewModels(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            services.RegisterLazySingleton<IFileViewModelFactory>(() => new FileViewModelFactory(
                resolver.GetService<FileOpeningBehavior>(),
                resolver.GetService<DirectoryOpeningBehavior>()));
            services.Register(() => new OperationsViewModel(
                resolver.GetService<IFilesSelectionService>(),
                resolver.GetService<IOperationsFactory>()));
            services.Register(() => new FilesPanelViewModel(
                resolver.GetService<IFileService>(),
                resolver.GetService<IDirectoryService>(),
                resolver.GetService<IFilesSelectionService>(),
                resolver.GetService<IFileViewModelFactory>()
                ));
            services.RegisterLazySingleton<IMainWindowViewModelProvider>(
                () => new MainWindowViewModelProvider());
            services.RegisterLazySingleton(() => new MainWindowViewModel(
                resolver.GetService<OperationsViewModel>(),
                resolver.GetService<FilesPanelViewModel>(),
                resolver.GetService<FilesPanelViewModel>()));
        }
    }
}