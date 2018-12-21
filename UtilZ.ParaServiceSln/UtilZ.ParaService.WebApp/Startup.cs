using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using UtilZ.ParaService.WebApp.Models;

namespace UtilZ.ParaService.WebApp
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

            //js跨域
            services.AddCors(options =>
            {
                options.AddPolicy(WebAppConstant.CorsPolicy,
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            // services.AddAuthentication(x =>
            // {
            //     x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //     x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            // })
            //.AddJwtBearer(o =>
            //{
            //    // 若不设置Authority，就必须指定MetadataAddress
            //    o.Authority = "https://oidc.faasx.com/";//此值只能为这个值，否则无法登陆
            //    // 默认为Authority+".well-known/openid-configuration"
            //    //o.MetadataAddress = "https://oidc.faasx.com/.well-known/openid-configuration";
            //    o.RequireHttpsMetadata = false;
            //    o.Audience = "api";

            //    o.Events = new JwtBearerEvents()
            //    {
            //        OnMessageReceived = context =>
            //        {
            //            const string accessToken = "access_token";
            //            context.Token = context.Request.Query[accessToken];
            //            if (string.IsNullOrWhiteSpace(context.Token))
            //            {
            //                context.Token = context.Request.Headers[accessToken];
            //            }

            //            return Task.CompletedTask;
            //        }
            //    };
            //    o.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        NameClaimType = JwtClaimTypes.Name,
            //        RoleClaimType = JwtClaimTypes.Role,

            //        // 用于适配本地模拟Token
            //        ValidateIssuer = false,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(WebAppConstant.Secret))
            //        /***********************************TokenValidationParameters的参数默认值***********************************/
            //        // RequireSignedTokens = true,
            //        // SaveSigninToken = false,
            //        // ValidateActor = false,
            //        // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
            //        // ValidateAudience = true,
            //        // ValidateIssuer = true, 
            //        // ValidateIssuerSigningKey = false,
            //        // 是否要求Token的Claims中必须包含Expires
            //        // RequireExpirationTime = true,
            //        // 允许的服务器时间偏移量
            //        // ClockSkew = TimeSpan.FromSeconds(300),
            //        // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
            //        // ValidateLifetime = true
            //    };
            //});

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
                app.UseHsts();
            }

            //js跨域
            app.UseCors(WebAppConstant.CorsPolicy);

            //默认文件
            DefaultFilesOptions defaultFiles = new DefaultFilesOptions();
            defaultFiles.DefaultFileNames.Clear();
            defaultFiles.DefaultFileNames.Add("/htmls/login.html");
            app.UseDefaultFiles(defaultFiles);
            app.UseStaticFiles();

            //app.UseHttpsRedirection();
            app.UseCookiePolicy();
            //app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
