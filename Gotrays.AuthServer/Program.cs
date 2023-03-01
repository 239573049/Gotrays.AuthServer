using System;
using System.IO;
using System.Net.Http;
using Dantooine.Server;
using Dantooine.Server.Data;
using Dantooine.Server.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenIddict.Abstractions;
using Quartz;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;


var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddRazorPages();

services.AddServerSideBlazor()
    .AddHubOptions(options =>
    {
        options.HandshakeTimeout = TimeSpan.FromSeconds(600000); // 获取或设置服务器用于客户端传入握手请求超时的间隔
        options.ClientTimeoutInterval = TimeSpan.FromSeconds(600000); // 长时间未发送消息断开
        options.KeepAliveInterval = TimeSpan.FromSeconds(1000); // 获取或设置串行器用于向连接的客户端发送keep alive ping的间隔
        options.EnableDetailedErrors = false; // 获取或设置一个值，该值指示是否将详细错误消息发送到客户端。详细的错误消息包括来自服务器上抛出的异常的详细信息。
        options.MaximumParallelInvocationsPerClient = 1; // 默认情况下，客户端一次只允许调用一个Hub方法。更改此属性将允许客户端在排队之前同时调用多个方法。
        options.MaximumReceiveMessageSize = 1024 * 1024; // 获取或设置单个传入集线器消息的最大消息大小。默认值是32KB。
        options.StreamBufferCapacity = 100; // 获取或设置客户端上传流的最大缓冲区大小
    });

services.AddMasaBlazor();

services.AddControllersWithViews();
services.AddRazorPages();
services.AddServerSideBlazor();

services.AddDbContext<ApplicationDbContext>(options =>
{
    // Configure the context to use sqlite.
    options.UseSqlite($"Filename={Path.Combine(Path.GetTempPath(), "openiddict-dantooine-server.sqlite3")}");

    // Register the entity sets needed by OpenIddict.
    // Note: use the generic overload if you need
    // to replace the default OpenIddict entities.
    options.UseOpenIddict();
});

services.AddDatabaseDeveloperPageExceptionFilter();

// Register the Identity services.
services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

// OpenIddict offers native integration with Quartz.NET to perform scheduled tasks
// (like pruning orphaned authorizations/tokens from the database) at regular intervals.
services.AddQuartz(options =>
{
    options.UseMicrosoftDependencyInjectionJobFactory();
    options.UseSimpleTypeLoader();
    options.UseInMemoryStore();
});

// Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

services.AddOpenIddict()

    // Register the OpenIddict core components.
    .AddCore(options =>
    {
        // Configure OpenIddict to use the Entity Framework Core stores and models.
        // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
        options.UseEntityFrameworkCore()
            .UseDbContext<ApplicationDbContext>();

        // Enable Quartz.NET integration.
        options.UseQuartz();
    })

    // Register the OpenIddict server components.
    .AddServer(options =>
    {
        // Enable the authorization, logout, token and userinfo endpoints.
        options.SetAuthorizationEndpointUris("connect/authorize")
            .SetLogoutEndpointUris("connect/logout")
            .SetIntrospectionEndpointUris("connect/introspect")
            .SetTokenEndpointUris("connect/token")
            .SetUserinfoEndpointUris("connect/userinfo")
            .SetVerificationEndpointUris("connect/verify");

        // Mark the "email", "profile" and "roles" scopes as supported scopes.
        options.RegisterScopes(OpenIddictConstants.Permissions.Scopes.Email,
            OpenIddictConstants.Permissions.Scopes.Profile, OpenIddictConstants.Permissions.Scopes.Roles);

        // Note: this sample only uses the authorization code flow but you can enable
        // the other flows if you need to support implicit, password or client credentials.
        options.AllowAuthorizationCodeFlow();

        // Register the signing and encryption credentials.
        options.AddDevelopmentEncryptionCertificate()
            .AddDevelopmentSigningCertificate();

        // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
        options.UseAspNetCore()
            .EnableAuthorizationEndpointPassthrough()
            .EnableLogoutEndpointPassthrough()
            .EnableTokenEndpointPassthrough()
            .EnableUserinfoEndpointPassthrough()
            .EnableStatusCodePagesIntegration();
    })
    .AddValidation(options =>
    {
        // Import the configuration from the local OpenIddict server instance.
        options.UseLocalServer();

        // Register the ASP.NET Core host.
        options.UseAspNetCore();
    });

services.AddHostedService<Worker>();

services.AddScoped<HttpClient>();

services.AddTransient<IEmailSender, EmailSender>();

services.AddSwaggerGen(delegate(SwaggerGenOptions option)
{
    option.SwaggerDoc("v1.0", new OpenApiInfo
    {
        Version = "v1.0", // 版本
        Title = "Gotrays 授权中心", // 标题
        Description = "Gotrays 授权中心服务", // 描述 
        Contact = new OpenApiContact
        {
            Name = "token", // 作者
            Email = "239573049@qq.com", // 邮箱
            Url = new Uri("http://blog.tokengo.top") // 可以放Github地址
        }
    });

    // 加载xml文档 显示Swagger的注释
    string[] files = Directory.GetFiles(AppContext.BaseDirectory, "*.xml"); //获取api文档 
    string[] array = files;
    foreach (string filePath in array)
    {
        option.IncludeXmlComments(filePath, includeControllerXmlComments: true);
    }

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });

    // 添加Authorization的输入框
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "Please enter into field the word 'Bearer' followed by a space and the JWT value,Format: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
});
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseStatusCodePagesWithReExecute("~/error");
    //app.UseExceptionHandler("~/error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Video Web Api");
    c.DocExpansion(DocExpansion.None);
    c.DefaultModelsExpandDepth(-1);
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
});

app.MapBlazorHub(options =>
{
    options.ApplicationMaxBufferSize = 1024 * 1024 * 1024;
    options.TransportMaxBufferSize = 1024 * 1024 * 1024;
});

app.MapFallbackToPage("/_Host");

await app.RunAsync();