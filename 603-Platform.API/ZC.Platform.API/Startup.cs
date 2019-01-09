using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySugar;

namespace ZC.Platform.API
{
    public class Startup
    {
        //为StartUp.cs添加属性
        public static ILoggerRepository repository { get; set; }
        public static  ILog log;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            //log4net
            repository = LogManager.CreateRepository("NETCoreRepository");
            //指定配置文件
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            log = LogManager.GetLogger(repository.Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }
        
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            DbContext.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
