using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using starTEDSystem.DAL;
using starTEDSystem.BLL;
#endregion


namespace starTEDSystem
{
    public static class StartupExtensions
    {
        public static void AddBackendDependencies(this IServiceCollection services,
    Action<DbContextOptionsBuilder> options)
        {
            //the first parameter of your method referes to the class you are attempting to 
            //   extend
            //syntac for the firat parameter:  this theclassbeingextend parametername

            //any additinal arguments exsiting on the callin statement follow the 
            //   first parameter separated by commas


            //add the context class of your application library (DAL) to the service
            //  collection
            //pass in the connection string options
            services.AddDbContext<StarTEDContext>(options);




            //add any business logic layer class to the service collection so oour
            //  web app has access to the methods within the BLL class

            //the argument for the AddTransient is called a factory
            //basically what you are adding is a localize method
            services.AddTransient<ProgramServices>((serviceProvider) =>
            {
                //get the dbcontext class
                var context = serviceProvider.GetRequiredService<StarTEDContext>();
                //create an instance of WestWindServices supplying the reference to
                //   the context class
                return new ProgramServices(context);
            });


        }


    }
}
