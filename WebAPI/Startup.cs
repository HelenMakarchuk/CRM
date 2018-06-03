using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public Log log = new Log();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
            });

            services.AddMvc();

            services.AddDbContext<CustomersContext>(options =>
                    options.UseSqlServer(Constants.DBConnectionString));

            services.AddDbContext<DepartmentsContext>(options =>
                    options.UseSqlServer(Constants.DBConnectionString));

            services.AddDbContext<OrdersContext>(options =>
                    options.UseSqlServer(Constants.DBConnectionString));

            services.AddDbContext<PaymentsContext>(options =>
                    options.UseSqlServer(Constants.DBConnectionString));

            services.AddDbContext<UsersContext>(options =>
                    options.UseSqlServer(Constants.DBConnectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //Project will be run on development environment only, so staging and production environments are ignored
            //more information here: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAllOrigins");

            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("CRM WebAPI");
            });
        }
    }
}
