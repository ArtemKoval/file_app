using System.IO;
using Domain.Commands;
using Domain.FileSystem;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NFS;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<
                IRenameCommand<RenameResult, bool, RenameState>,
                RenameCommand>();
            services.AddScoped<
                IDownloadFileCommand<FileDownloadResult, Stream, DownloadFileState>,
                DownloadFileCommand>();
            services.AddScoped<
                IRemoveCommand<RemoveResult, bool, RemoveState>,
                RemoveCommand>();
            services.AddScoped<
                IUploadFileCommand<FileUploadResult, bool, UploadFileState>,
                UploadFileCommand>();
            services.AddScoped<
                IGetFolderSizeCommand<GetFolderSizeResult, long, GetFolderSizeState>,
                GetFolderSizeCommand>();
            services.AddScoped<
                IFileSystem,
                PhysicalFileSystem>();
            services.AddScoped<
                IFileSystemService,
                FileSystemService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

//            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}