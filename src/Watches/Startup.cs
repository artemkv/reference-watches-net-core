﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Watches.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Watches.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Watches
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WatchesDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(BadRequestExceptionFilter));
            }).AddXmlSerializerFormatters(); // Allows Xml formatting as well as Json, using content negotiation
            services.AddScoped<IWatchRepository, WatchRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IApiConfiguration, ApiConfiguration>();
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
