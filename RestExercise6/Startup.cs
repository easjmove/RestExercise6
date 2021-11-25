using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RestExercise6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestExercise6
{
    public class Startup
    {
        public const string AllowAllPolicyName = "allowAll";
        public const string AllowOnlyGetMethodPolicyName = "allowOnlyGetMethod";
        public const string AllowOnlyZealandOriginPolicyName = "allowOnlyZealandOrigin";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            //Added swagger to the project
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestExercise6", Version = "v1", 
                    Contact = new OpenApiContact() { Name = "Morten Vestergaard", Email = "move@zealand.dk" }
                });
            });

            //Having a policy that allows all
            services.AddCors(options => options.AddPolicy(AllowAllPolicyName,
                builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));

            //A policy that only allows GET, but everything else
            //We don't use this in this application, this is just an example
            services.AddCors(options => options.AddPolicy(AllowOnlyGetMethodPolicyName,
                builder => builder.AllowAnyOrigin()
                .WithMethods("GET")
                .AllowAnyHeader()));

            //A policy that only allow requests coming from zealand.dk
            services.AddCors(options => options.AddPolicy(AllowOnlyZealandOriginPolicyName,
                builder => builder.WithOrigins("https://zealand.dk")
                .AllowAnyMethod()
                .AllowAnyHeader()));

            //Telling the Service that it should be using a DbContext
            //This line is commented out in this solution, as it will use the InMemory SqlServer instead
            //services.AddDbContext<ItemContext>(opt => opt.UseSqlServer(Secrets.ConnectionString));

            //This is only used here so the project is able to run without an Azure Database
            //The data stored here is not permanent
            services.AddDbContext<ItemContext>(opt => opt.UseInMemoryDatabase("Items"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //moved the following 2 line outside the if (env.IsDevelopment()) statement, in order for it to be accessible everywhere
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestExercise6 v1"));

            app.UseRouting();

            //What the default policy should be
            app.UseCors("allowAll");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
