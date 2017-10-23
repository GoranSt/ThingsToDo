using SimpleInjector;
using SimpleInjector.Integration.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThingsToDo.BL.Interfaces;
using ThingsToDo.BL.Services;

namespace ThingsToDo.Dependency
{
    public static class SimpleInjectorBindings
    {
        public static Container GetContainer()
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            // Register your types, for instance:

            container.Register<ITasksService, TasksService>(Lifestyle.Scoped);
            container.Register<ICategoriesService, CategoriesService>(Lifestyle.Scoped);
            container.Register<IStatisticsService, StatisticsService>(Lifestyle.Scoped);

            return container;
        }

    }
}
