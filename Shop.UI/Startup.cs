using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shop.Application.UsersAdmin;
using Shop.Database;
using Shop.UI.Middleware;
using Stripe;

namespace Shop.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(Configuration["DefaultConnection"]));

            services.AddIdentity<IdentityUser, IdentityRole>(options => 
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDBContext>();

            services.ConfigureApplicationCookie(options => 
            {
                options.LoginPath = "/Accounts/Login";
            });

            services.AddAuthorization(options =>             
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
                //options.AddPolicy("Manager", policy => policy.RequireClaim("Role", "Manager"));
                options.AddPolicy("Manager", policy 
                    => policy.RequireAssertion(context => 
                    context.User.HasClaim("Role", "Manager")
                    || context.User.HasClaim("Role", "Admin")));
            });

            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizeFolder("/Admin");
                options.Conventions.AuthorizePage("/Admin/ConfigureUsers", "Admin");
            })
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.Name = "Cart";
                options.Cookie.MaxAge = TimeSpan.FromMinutes(20);
            });

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddTransient<CreateUser>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseHttpContextItemsMiddleware();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseAuthentication();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers(); //+
                endpoints.MapRazorPages();
            });

            app.UseMvcWithDefaultRoute();
        }
    }
}
