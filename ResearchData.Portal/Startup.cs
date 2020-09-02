using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ResearchData.Portal.Data.Contextos;
using ResearchData.Portal.Data.Repositorio.Admin;
using ResearchData.Portal.Data.Repositorio.Analises;
using ResearchData.Portal.Data.Repositorio.Caracteristicas;
using ResearchData.Portal.Data.Repositorio.Colaborador;
using ResearchData.Portal.Data.Repositorio.Comentarios;
using ResearchData.Portal.Data.Repositorio.Experimentos;
using ResearchData.Portal.Data.Repositorio.Grupos;
using ResearchData.Portal.Data.Repositorio.Medicoes;
using ResearchData.Portal.Data.Repositorio.Projetos;
using ResearchData.Portal.Data.Repositorio.Sujeitos;
using ResearchData.Portal.GerenciamentoUsuario.PerfisDeUsuario;
using ResearchData.Portal.GerenciamentoUsuario;
using ResearchData.Portal.Servicos.Email;
using ResearchData.Portal.Servicos.Logger;
using ResearchData.Portal.Models.Exportacao;
using System.Data.SqlClient;

namespace ResearchData.Portal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public RDContextoLog _contextoLog;

        public void ConfigureServices(IServiceCollection services)
        {
            #region ConfigCabecalho

            services.AddCors();

            #endregion


            #region CookieseRecursosDeCulture

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddLocalization(opts =>
            {
                opts.ResourcesPath = "Recurso";
            });

            services.AddMvc()
                .AddViewLocalization(
                    opts => { opts.ResourcesPath = "Recurso"; })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var linguasSuportadas = new List<CultureInfo> {
                    new CultureInfo("pt-BR"),
                    new CultureInfo("en-US")
                };

                opts.DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR");
                opts.SupportedCultures = linguasSuportadas;
                opts.SupportedUICultures = linguasSuportadas;
            });

            #endregion


            #region ConfiguracaoDeBase

            var connection = Configuration["ConnectionStrings:DefaultConnection"];

            //Utilizando SqlServer
            // services.AddDbContext<RDContextoDaAplicacao>(options =>
            //    options.UseSqlServer(connection)
            // );

            //Utilizando SqLite
            services.AddDbContext<RDContextoDaAplicacao>(options => options
                        .UseSqlite(connection, builder => builder
                        .MigrationsAssembly(typeof(Startup).Assembly.FullName)));

            //Log Sql Server
            // services.AddTransient<RDContextoLog>().AddDbContext<RDContextoLog>(options =>
            // options.UseSqlServer(connection));

            services.AddTransient<RDContextoLog>().AddDbContext<RDContextoLog>(options =>
            options.UseSqlite(connection));


            #endregion


            #region ConfiguracaoUsuario
            services.AddIdentity<UsuarioAplicacao, IdentityRole>()
                                                       .AddEntityFrameworkStores<RDContextoDaAplicacao>()
                                                       .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
            });

            services.Configure<DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromHours(3));

            services.AddAuthentication()
                .AddGoogle(options =>
                 {
                     IConfigurationSection googleAuthNSection =
                     Configuration.GetSection("Authentication:Google");
                     //Caso queira utilizar pelo secrect manager
                     //options.ClientId = googleAuthNSection["Authentication:Google:ClientId"];
                     //options.ClientSecret = googleAuthNSection["Authentication:Google:ClientSecret"];
                     options.ClientId = Configuration["Authentication:Google:ClientId"];
                     options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                 });

            #endregion


            #region ServiceEmail

            services.AddTransient<IEmailSender, EnviarEmail>();
            services.Configure<AutorizacaoEmail>(Configuration);

            #endregion


            #region repositorios

            services.AddTransient<IProjetoRepositorio, ProjetoRepositorio>();
            services.AddTransient<IAnaliseRepositorio, AnaliseRepositorio>();
            services.AddTransient<ICaracteristicaRepositorio, CaracteristicaRepositorio>();
            services.AddTransient<ISujeitoRepositorio, SujeitoRepositorio>();
            services.AddTransient<IGrupoRepositorio, GrupoRepositorio>();
            services.AddTransient<IMedicaoRepositorio, MedicaoRepositorio>();
            services.AddTransient<IExperimentoRepositorio, ExperimentoRepositorio>();
            services.AddTransient<IComentarioRepositorio, ComentarioRepositorio>();
            services.AddTransient<IAdminRepositorio, AdminRepositorio>();
            services.AddTransient<IColaboradorRepositorio, ColaboradorRepositorio>();
            services.AddTransient<IExportarCsv, ExportarCsv>();

            #endregion


            #region LogConfig


            _contextoLog = services.BuildServiceProvider()
                      .GetService<RDContextoLog>();

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            app.UseCsp(opts => opts
                .BlockAllMixedContent()
            );

            #region Configpolici
            var policyCollection = new HeaderPolicyCollection()
        .AddFeaturePolicy(builder =>
        {
            builder.AddAccelerometer()
                .Self()
                .For("http://testUrl.com");

            builder.AddAmbientLightSensor()
                .Self();


            builder.AddAutoplay()
                .Self();

            builder.AddCamera()
                .None();

            builder.AddEncryptedMedia()
                .Self();

            builder.AddFullscreen()
                .All();

            builder.AddGeolocation()
                .None();

            builder.AddGyroscope()
                .None();

            builder.AddMagnetometer()
                .None();

            builder.AddMicrophone()
                .None();

            builder.AddMidi()
                .None();

            builder.AddPayment()
                .None();

            builder.AddPictureInPicture()
                .None();

            builder.AddSpeaker()
                .None();

            builder.AddSyncXHR()
                .None();

            builder.AddUsb()
                .None();

            builder.AddVR()
                .None();
        });

            #endregion

            app.UseSecurityHeaders(policyCollection);


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/500");
                app.UseStatusCodePagesWithReExecute("/Erros/TratarCodigoDeErro/{0}");
            }


            app.UseHsts(hsts => hsts.MaxAge(365).IncludeSubdomains());
            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.UnsafeUrl());
            app.UseXXssProtection(options1 => options1.EnabledWithBlockMode());
            app.UseXfo(options2 => options2.Deny());


            app.Use(async (context, next) =>
            {
                await next.Invoke();
            });



            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);




            #region InciarRolesDoSistemaEPrimeiroAdministrador

            InitializeDatabase(app);


            CriarRoles(serviceProvider).Wait();
            CriarAdministrador(serviceProvider).Wait();

            #endregion


            #region RotaPadrao


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Painel}/{action=Index}/{Id?}");
            });


            #endregion

            #region LogConfigure

            var LogCustomizado = new CustomLoggerProvider(
               new CustonLoggerProviderConfiguration { LogLevel = LogLevel.Information })
            {
                Context = _contextoLog
            };

            loggerFactory.AddProvider(LogCustomizado);


            #endregion
        }

        #region definicaoRoles
        private static async Task CriarRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<UsuarioAplicacao>>();

            string[] rolesNames = { PerfisPadroes.ADMINISTRADOR, PerfisPadroes.USUARIO, PerfisPadroes.CRIARCATEGORIA, PerfisPadroes.CRIARTEMPLETE };
            IdentityResult result;
            foreach (var namesRole in rolesNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(namesRole);
                if (!roleExist)
                {
                    result = await roleManager.CreateAsync(new IdentityRole(namesRole));
                }
            }

        }
        #endregion

        #region definirAdmin
        private async Task CriarAdministrador(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<UsuarioAplicacao>>();

            var administradorEmail = Configuration["RDADMIN:ADMINISTRADOR-EMAIL"];
            var administradorSenha = Configuration["RDADMIN:ADMINISTRADOR-SENHA"];

            var ExisteAdmin = await userManager.FindByEmailAsync(administradorEmail);

            if (ExisteAdmin != null)
            {
                return;
            }

            var admin = new UsuarioAplicacao()
            {
                UserName = administradorEmail,
                Email = administradorEmail,
                EmailConfirmed = true,
                Nome = "Administrador do Sistema",
                DataCadastro = DateTime.Now,
                DataUltimoAcesso = DateTime.Now
            };

            var result = await userManager.CreateAsync(admin, administradorSenha);



            await userManager.AddToRoleAsync(admin, PerfisPadroes.ADMINISTRADOR);

        }

        #endregion


        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                try
                {
                    scope.ServiceProvider.GetRequiredService<RDContextoDaAplicacao>().Database.Migrate();
                    scope.ServiceProvider.GetRequiredService<RDContextoLog>().Database.Migrate();
                }
                catch (SqlException exception) when (exception.Number == 1801) { }

            }
        }
    }
}
