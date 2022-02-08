using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration _config; //access appsettings.json values
        public Startup(IConfiguration config)
        {
            _config = config;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(
                _config.GetConnectionString("EmployeeDbConnection")
                ));
            /* AddDbContextPool check if there are instance in the pool and return it
            without create new instance */
            services.AddIdentity<ApplicationUser, IdentityRole>(options=> {
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 3;
            }).AddEntityFrameworkStores<AppDbContext>();
            // adding Mvc Service to use its middleware
            // AddXmlSerializerFormatters to allow XML Format Response

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });

            services.AddMvc(config => {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build(); // make authorization globally
                config.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters(); 

            // dependency Injection Inject IEmployeeRepository Service In All Controllers
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
            //services.AddScoped<IEmployeeRepository, MockEmployeeRepository>();
            // AddScoped with every scope http request create instance, have same instance for the same scope /home
            //services.AddTransiant<IEmployeeRepository, MockEmployeeRepository>();
            // AddTransiant adding new instance for any request
            // AddSingletone generate single instance for all requests

            // Adding Claim Policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy",
                    policy => policy.RequireClaim("Delete Role"));
                options.AddPolicy("AdminRolePolicy",
                    policy => policy.RequireRole("Admin"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            /*
             what happen if we navigate to /foo/bar and this route doese not registered?
             1-first we change env to Production so will go to else block request doese not have status code yet
             2- then will pass it to next middleware SaticFiles which wil pass the request with next middleware MVC
             3- will not ound controller and view then will return respose with status code 404 so will fllow back middleware cycle
             4- pass response to the prev middleware with status code 404 
             4- the prev middleware which is statisFiles will pass the response to the prev middleware 
             5- then will navigate to the Error/404
             so if we use UseStatusCodePagesWithRedirects the request will be redirected to Error/404 and
             the status code of /foo/bar will be "302" means resource is changed (redirected) and make another request to Error/404
             with status 200
             if we use UseStatusCodePagesWithReExecute will ReExecute the Http pipeline with Error/404 means
             will pass Error/404 request to next middleware
             and to next untill MVC then will return View but the View Will be Returned at the same route /foo/bar not Redirected 
             and status code will be 404
             */
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");//if error occured like throw exception so navigate to /Error
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            //DefaultFilesOptions df = new DefaultFilesOptions();
            //df.DefaultFileNames.Clear();
            //df.DefaultFileNames.Add("htmlpage.html");
            // UseDefaultFiles must be register before UseStaticFiles
            //app.UseDefaultFiles(); //then will preview htmlpage.html in the first request empty request

            //FileServerOptions fs = new FileServerOptions();
            //fs.DefaultFilesOptions.DefaultFileNames.Clear();
            //fs.DefaultFilesOptions.DefaultFileNames.Add("htmlpage.html");
            //app.UseFileServer(fs); // combine functionality of UseDefaultFiles and UseStaticFiles
            // Middlewares Runt With this Order

            app.UseStaticFiles(); // middleware to request for static file in the wwwroot folder


            app.UseAuthentication();

            //following template: '{controller=Home}/{action=Index}/{id?}'

            //app.UseMvcWithDefaultRoute(); // middleware to adding MVC with default route home/index for / route

            //middleware for adding mvc only
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });


            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World 2!");
            //});
        }
    }
}
