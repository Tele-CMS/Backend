//using Audit.SqlServer.Providers;
using Elmah.Io.AspNetCore;
using HC.Common.Options;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Service.Automapper;
using HC.Patient.Service.Users.Interfaces;
using HC.Patient.Service.Users;
using HC.Patient.Web.Filters;
using HC.Patient.Web.Hubs;
using HC.Repositories;
using HC.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
//using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
 using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace HC.Patient.Web
{
    public partial class Startup
    {
        private const string DefaultCorsPolicyName = "http://75.126.168.31:6044";
        public IConfigurationRoot Configuration { get; }
        private static int organizationID = 0;
        private Organization organization = new Organization();
        public IWebHostEnvironment HostingEnvironment { get; }
        public Startup(IConfiguration configuration)
        {
            AutomapperConfiguration.Configure();
            //env.ConfigureNLog("nlog.config");
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            //    .AddEnvironmentVariables();
            //string sAppPath = env.ContentRootPath; //Application Base Path
            //string swwwRootPath = env.WebRootPath;  //wwwroot folder path
            //Configuration = builder.Build();
            Configuration = (IConfigurationRoot)configuration;
            organizationID = Configuration.GetValue<Int32>("OrganizationID");
        }


        private const string SecretKey = "needtogetthisfromenvironment";
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));


        //This method gets called by the runtime.Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)//,HCPatientContext context)
        {
            //organization = context.Organization.Where(p => p.Id == organizationID).FirstOrDefault();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AuthorizedUser",
                      policy =>
                        policy.Requirements.Add(new AuthorizationFilter("HealthCare")));
                    //policy => policy.RequireClaim("HealthCare", "IAmAuthorized"));

                });
            //services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);
            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            services.AddSingleton<IAuthorizationHandler, AuthorizeHandler>();
            //services.AddSingleton<IUserService, UserService>();
            services.AddTransient<IUserService, UserService>();
            services.AddSingleton<IConfiguration>(Configuration);

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });
            //services.AddTransient<ValidateMimeMultipartContentFilter>();
            services.AddSignalR();
            services.AddScoped<LogFilter>();
            // Add framework services.
            services.AddControllers(options =>
            {
                    //options.Filters.Add(new AuthorizeFilter("AuthorizedUser"));
                }).AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            //Master Context from App Settings
            services.AddDbContext<HCMasterContext>(option => { option.UseSqlServer(Configuration.GetConnectionString("HCMaster"), b => b.MigrationsAssembly("HC.Patient.Web")); });

            //Organization Context first time initial from appsettings 
            services.AddDbContext<HCOrganizationContext>(option => { option.UseSqlServer(Configuration.GetConnectionString("HCOrganization"), b => b.MigrationsAssembly("HC.Patient.Web")); });

            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddSingleton(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            // Add all Transient dependencies
            services = BuildUnityContainer.RegisterAddTransient(services);

            //Configure CORS for angular2 UI
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(DefaultCorsPolicyName, builder => builder.AllowAnyOrigin()
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials());
            //});
            services.AddCors(
               options => options.AddPolicy(DefaultCorsPolicyName,
                   builder =>
                   {
                       builder
                            //.AllowAnyOrigin()
                           .SetIsOriginAllowed(origin => true) // allow any origin;
                           .AllowCredentials()
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .WithExposedHeaders("Access-Control-Expose-Headers", "Content-Disposition");
                          
                   }));
            services.AddSignalR(options => { options.HandshakeTimeout = TimeSpan.FromMinutes(5); options.KeepAliveInterval = TimeSpan.FromMinutes(5); options.EnableDetailedErrors = true; });
            // Register the Swagger generator, defining one or more Swagger documents

            //services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new Info
            //     {
            //         Title = "Health Care Product",
            //         Version = "v2",
            //         Description = "APIs",
            //         TermsOfService = "None",
            //         Contact = new Contact
            //         {
            //             Name = "HealthCare",
            //             Email = string.Empty,
            //             Url = ""
            //         },
            //         License = new License
            //         {
            //             Name = "Use under LICX",
            //             Url = "https://example.com/license"
            //         }
            //     });
            //     c.IgnoreObsoleteProperties();
            //     //c.IncludeXmlComments(GetXmlCommentsPath());
            //     c.DescribeAllEnumsAsStrings();
            //     c.OperationFilter<FileUploadOperation>(); //Register File Upload Operation Filter
            //                                               // Set the comments path for the Swagger JSON and UI.
            //     var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //     var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //     c.IncludeXmlComments(xmlPath);
            // });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Health Care Product",
                    Version = "v2",
                    Description = "APIs",
                });
                c.IgnoreObsoleteProperties();
                    //c.IncludeXmlComments(GetXmlCommentsPath());
                    //c.DescribeAllEnumsAsStrings();
                    //c.OperationFilter<FileUploadOperation>(); //Register File Upload Operation Filter
                    // Set the comments path for the Swagger JSON and UI.
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddScoped<LogFilter>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfileConfiguration());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            //_iUserService.AddIpAddress();
            /////////commented need for future refrence
            //////First register a custom made db context provider
            ////services.AddTransient<ApplicationDbContextFactory>();
            //////Then use implementation factory to get the one you need            
            ////services.AddTransient<HCOrganizationContext>(serviceProvider =>
            ////{
            ////    var context = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            ////    //var userHeader = context.Request.Headers["loggedInUser"];
            ////    //if (string.IsNullOrEmpty(userHeader)) return new GuestControlPanel();
            ////    //if ("admin" == userHeader) return new AdminControlPanel();
            ////    return serviceProvider.GetService<ApplicationDbContextFactory>().CreateApplicationDbContext(context);
            ////});
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, HCOrganizationContext context, IUserService userService)//, 
        {
            DefaultFilesOptions DefaultFile = new DefaultFilesOptions();
            DefaultFile.DefaultFileNames.Clear();
            DefaultFile.DefaultFileNames.Add("Index.html");
            app.UseDefaultFiles(DefaultFile);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            //app.UseStaticFiles();
            //app.UseRouting();

            loggerFactory.ConfigureNLog("nlog.config");
            //following code is used for access the Images
            if (!Directory.Exists("Images"))
                Directory.CreateDirectory("Images");
            if (!Directory.Exists("Documents"))
                Directory.CreateDirectory("Documents");

            app.UseStaticFiles(new StaticFileOptions
            {

                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Images")),
                RequestPath = "/Images"
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Documents")),
                RequestPath = "/Documents"
            });
            if (!Directory.Exists("Chat/Files"))
                Directory.CreateDirectory("Chat/Files");
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Chat")),
                RequestPath = new PathString("/Chat")
            });
            ////////



            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
            app.UseElmahIo("6e46539834624195b66edb223f370836", new Guid("d6251932-8823-4afe-b0ba-0ef3fac938fb"));
            //app.UseJwtBearerAuthentication();
            //new JwtIssuerOptions
            //{
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = true,
            //    TokenValidationParameters = tokenValidationParameters
            //});

            //loggerFactory.AddConsole(Microsoft.Extensions.Logging.LogLevel.Trace);
            LoggerFactory.Create(builder => builder.AddConsole());

            //loggerFactory.AddDebug();

            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            //});


            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            app.UseWebSockets();

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(webSocketOptions);

            app.Use((context, next) =>
            {
                if (context.Request.Path.Value.StartsWith("////"))
                {
                    context.Request.Path = new PathString(context.Request.Path.Value.Replace("////", "/"));
                }

                if (context.Request.Path.Value.StartsWith("///"))
                {
                    context.Request.Path = new PathString(context.Request.Path.Value.Replace("///", "/"));
                }

                if (context.Request.Path.Value.StartsWith("//"))
                {
                    context.Request.Path = new PathString(context.Request.Path.Value.Replace("//", "/"));
                }

                return next();
            });

            app.Use(async (HttpContext, next) =>
            {
                if (HttpContext.Request.Path == "/ws")
                {
                    if (HttpContext.WebSockets.IsWebSocketRequest)
                    {
                        WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                        await Echo(HttpContext, webSocket);
                    }
                    else
                    {
                        HttpContext.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }
            });
            app.UseExceptionHandler(
            builder =>
            {
                builder.Run(
                  async dbcontext =>
                  {
                      dbcontext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                      dbcontext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                      var error = dbcontext.Features.Get<IExceptionHandlerFeature>();
                      if (error != null)
                      {
                              // context.Response.AddApplicationError(error.Error.Message);
                              await dbcontext.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                      }
                  });
            });
            app.UseCors(DefaultCorsPolicyName); //Enable CORS!
                                                //app.UseSignalR(route =>
                                                //{
                                                //    route.MapHub<ChatHub>("/chathub");
                                                //});
                                                //app.UseMvc(routes =>
                                                //{
                                                //    routes.MapRoute(
                                                //        name: "defaultWithArea",
                                                //        template: "{area}/{controller=Home}/{action=Index}/{id?}");

            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Account}/{action=Index}/{id?}");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapControllerRoute("defaultWithArea", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Account}/{action=Index}/{id?}");
            });

            //app.UseJsonApi();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();


            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "My API V1");
                    //c.ShowRequestHeaders();
                    //c.ShowJsonEditor();
                    //c.DocExpansion("none");
                });
            context.Database.EnsureCreated();
            //ConfigureAuth(app);
            loggerFactory.AddNLog();
            //add NLog.Web
            app.AddNLogWeb();
            //
            var configDir = "C:\\Logs";

            if (configDir != string.Empty)
            {
                var logEventInfo = LogEventInfo.CreateNullEvent();


                foreach (FileTarget target in LogManager.Configuration.AllTargets.Where(t => t is FileTarget))
                {
                    var filename = target.FileName.Render(logEventInfo).Replace("'", "");
                    target.FileName = Path.Combine(configDir, filename);
                }

                LogManager.ReconfigExistingLoggers();
            }
            //organization = context.Organization.Where(p => p.Id == organizationID).FirstOrDefault();
            LogManager.Configuration.Variables["connectionString"] = Configuration.GetConnectionString("HCOrganization");

            LogManager.Configuration.Variables["configDir"] = "C:\\Logs";
            LogManager.ConfigurationReloaded += updateConfig;
            //app.UseMvc();
            //userService.AddIpAddress();
        }



        private void updateConfig(object sender, LoggingConfigurationReloadedEventArgs e)
        {
            LogManager.Configuration.Variables["connectionString"] = Configuration.GetConnectionString("HCOrganization");
            LogManager.Configuration.Variables["configDir"] = "C:\\Logs";
        }

        private async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
    //    public Startup(IConfiguration configuration)
    //    {
    //        Configuration = configuration;
    //    }

    //    public IConfiguration Configuration { get; }

    //    // This method gets called by the runtime. Use this method to add services to the container.
    //    public void ConfigureServices(IServiceCollection services)
    //    {
    //        services.AddControllers();
    //        services.AddMvc(options =>
    //        {
    //            options.EnableEndpointRouting = false;
    //        });
    //    }

    //    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    //    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    //    {
    //        if (env.IsDevelopment())
    //        {
    //            app.UseDeveloperExceptionPage();
    //        }
    //        app.UseStaticFiles();
    //        app.UseHttpsRedirection();

    //        app.UseRouting();

    //        app.UseAuthorization();

    //        //app.UseEndpoints(endpoints =>
    //        //{
    //        //    endpoints.MapControllers();
    //        //});
    //        app.UseMvcWithDefaultRoute();
    //    }
    //}
}
