using System.Data.Entity;
using System.Reflection.Emit;
using ThingsToDo.Models.Entity;

namespace ThingsToDo.DAL
{
    public class ThingsToDoAppContext : DbContext
    {


        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public ThingsToDoAppContext() : base("name=ThingsToDoAppContext")
        {
        }

        public System.Data.Entity.DbSet<ThingsToDo.Models.Entity.Tasks> Tasks { get; set; }

        public System.Data.Entity.DbSet<ThingsToDo.Models.Entity.RemovedTasks> RemovedTasks { get; set; }

        public System.Data.Entity.DbSet<ThingsToDo.Models.Entity.Categories> Categories { get; set; }
    }
}
