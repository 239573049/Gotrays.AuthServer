var builder = WebApplication.CreateBuilder(args);

// OpenIddict提供与Quartz的原生集成。NET来执行计划任务
// (就像从数据库中删除孤立的授权/令牌)。
builder.Services.AddQuartz(options =>
{
    options.UseSimpleTypeLoader();
    options.UseInMemoryStore();
});

// Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "AuthServerApp", Version = "v1",
                Contact = new OpenApiContact { Name = "AuthServerApp", }
            });
        foreach (var item in Directory.GetFiles(Directory.GetCurrentDirectory(), "*.xml"))
            options.IncludeXmlComments(item, true);
        options.DocInclusionPredicate((docName, action) => true);
    })
    .AddEventBus()
    .AddMasaDbContext<AuthDbContext>(opt =>
    {
        opt.Builder = (provider, optionsBuilder) =>
        {
            optionsBuilder.UseOpenIddict();
            var requiredService = provider.GetRequiredService<IConnectionStringProvider>();
            optionsBuilder.UseSqlite(requiredService.GetConnectionString("DefaultConnection"));
        };
    })
    .AddAutoInject();

builder.Services.AddOpenIddict()

    // 注册OpenIdDict核心组件。
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<AuthDbContext>();
    })

    // 注册OpenIdDict客户端组件。
    .AddClient(options =>
    {
        // 注意:此示例使用代码流，但如果需要，您可以启用其他流。
        options.AllowAuthorizationCodeFlow();
        // 注册用于保护敏感数据（如OpenIddict生成的状态令牌）的签名和加密凭证。
        options.AddDevelopmentEncryptionCertificate()
            .AddDevelopmentSigningCertificate();

        // 注册ASP.NET Core主机并配置特定于ASP.NET Core的选项。
        options.UseAspNetCore()
            .EnableRedirectionEndpointPassthrough();

        // 注册System.Net.Http集成，并使用当前程序集的身份作为更具体的用户代理，这在处理将用户代理用作限制请求方式（例如Reddit）时非常有用。
        options.UseSystemNetHttp()
            .SetProductInformation(typeof(Program).Assembly);

        // 注册Web提供商集成。

        // 注意：为了防止混淆攻击，建议每个提供商使用唯一的重定向终点URI，
        // 除非所有注册的提供商都支持返回包含其URL作为授权响应一部分的特殊"iss"参数。
        // 更多信息请参见https://datatracker.ietf.org/doc/html/draft-ietf-oauth-security-topics#section-4.4。
        options.UseWebProviders()
            .AddGitHub(options =>
            {
                options.SetClientId("c4ade52327b01ddacff3")
                    .SetClientSecret("da6bed851b75e317bf6b2cb67013679d9467c122")
                    .SetRedirectUri("callback/login/github");
            });
    })
// 注册OpenIddict服务器组件。
    .AddServer(options =>
    {
        // 启用授权和令牌端点。
        options.SetAuthorizationEndpointUris("authorize")
            .SetTokenEndpointUris("token");
    
        // 注意：此示例仅使用授权码流，但如果需要支持隐式、密码或客户端凭据，可以启用其他流程。
        options.AllowAuthorizationCodeFlow();
    
        // 注册签名和加密凭证。
        options.AddDevelopmentEncryptionCertificate()
            .AddDevelopmentSigningCertificate();
    
        // 注册ASP.NET Core主机并配置特定于ASP.NET Core的选项。
        //
        // 注意：与其他示例不同，此示例不使用通过传递处理自定义MVC操作中的令牌请求的方式。因此，
        // 令牌请求将由OpenIddict自动处理，并重用从授权码解析出来的身份来生成访问和身份令牌。
        options.UseAspNetCore()
            .EnableAuthorizationEndpointPassthrough()
            .DisableTransportSecurityRequirement();
    })

    // 注册OpenIddict验证组件。
    .AddValidation(options =>
    {
        // 从本地OpenIddict服务器实例导入配置。
        options.UseLocalServer();

        // 注册ASP.NET Core核心.
        options.UseAspNetCore();
    });

builder.Services.AddAuthorization()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

var app = builder.Services.AddServices(builder, option => option.MapHttpMethodsForUnmatched = ["Post"]);

app.UseMasaExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger().UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthServerApp"));

    #region MigrationDb

    await using var scope = app.Services.CreateAsyncScope();
    var context = scope.ServiceProvider.GetService<AuthDbContext>();
    await context.Database.EnsureCreatedAsync();

    var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

    if (await manager.FindByClientIdAsync("console_app") == null)
    {
        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = "console_app",
            RedirectUris =
            {
                new Uri("http://localhost:8914/")
            },
            Permissions =
            {
                OpenIddictConstants.Permissions.Endpoints.Authorization,
                OpenIddictConstants.Permissions.Endpoints.Token,
                OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                OpenIddictConstants.Permissions.ResponseTypes.Code,
                OpenIddictConstants.Permissions.Scopes.Email,
            }
        });
    }

    #endregion
}

app.UseAuthentication();
app.UseAuthorization();

app.MapMethods("/authorize/token", [HttpMethods.Post], async (HttpContext context) =>
{
    var identity = new ClaimsIdentity(authenticationType: "ExternalLogin");
    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));

    return Results.SignIn(new ClaimsPrincipal(identity));
});

// app.MapGet("/authorize", async (HttpContext context) =>
// {
//     // Resolve the claims stored in the cookie created after the GitHub authentication dance.
//     // If the principal cannot be found, trigger a new challenge to redirect the user to GitHub.
//     //
//     // For scenarios where the default authentication handler configured in the ASP.NET Core
//     // authentication options shouldn't be used, a specific scheme can be specified here.
//     var principal = (await context.AuthenticateAsync())?.Principal;
//     if (principal is null)
//     {
//         var properties = new AuthenticationProperties
//         {
//             RedirectUri = context.Request.GetEncodedUrl()
//         };
//
//         return Results.Challenge(properties, [OpenIddictClientAspNetCoreDefaults.AuthenticationScheme]);
//     }
//
//     var identifier = principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
//
//     // Create the claims-based identity that will be used by OpenIddict to generate tokens.
//     var identity = new ClaimsIdentity(
//         authenticationType: TokenValidationParameters.DefaultAuthenticationType,
//         nameType: OpenIddictConstants.Claims.Name,
//         roleType: OpenIddictConstants.Claims.Role);
//
//     // Import a few select claims from the identity stored in the local cookie.
//     identity.AddClaim(new Claim(OpenIddictConstants.Claims.Subject, identifier));
//     identity.AddClaim(
//         new Claim(OpenIddictConstants.Claims.Name, identifier).SetDestinations(OpenIddictConstants.Destinations
//             .AccessToken));
//
//     return Results.SignIn(new ClaimsPrincipal(identity), properties: null,
//         OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
// });

app.Run();