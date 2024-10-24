using ConsultaNumerarios.Data;
using ConsultaNumerarios.Interfaces;
using ConsultaNumerarios.Models;
using ConsultaNumerarios.Repository;
using ConsultaNumerarios.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

internal class Program
{

    #region :: TODO consulta de saldos. ::
    // 1. Get Lista de PAs (done)
    // 2. Get Saldo de Numerários de PAs (done)
    //    a. Saldo Total
    //    b. Saldo dos Terminais
    //    c. Limites Máx e Mín dos Terminais
    //    d. Usuários Responsáveis pelos Terminais
    //    e. Se os saldos estão dentro dos limites 
    //3. Get Saldo de Numerários de PAs por Período (Validar regras) 

    #endregion

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        #region :: Injeção de Dependências :: 
        builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("conexao")));

        builder.Services.AddScoped<IOperacaoRepository, OperacaoRepository>();
        builder.Services.AddScoped<IUnidadeInstituicao, UnidadeInstituicaoRepository>();
        builder.Services.AddScoped<ITerminalService, TerminalService>();
        builder.Services.AddScoped<ITerminalRepository, TerminalRepository>();
        builder.Services.AddScoped<ITipoTerminalRepository, TipoTerminalRepository>();
        builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        #endregion

        //JWT
        var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
        builder.Services.AddSingleton(jwtSettings);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
            };
        });

        //Authorization Policies
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("TIOnly", policy => policy.RequireClaim(ClaimTypes.Role, "TI"));
            options.AddPolicy("AdminOrTI", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador", "TI"));
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        // Swagger com JWT
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement{
            {
                new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
                }
            },new List<string>()
            }});
        });

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("FrontEnd", policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        //    app.UseSwagger();
        //    app.UseSwaggerUI(c =>
        //    {
        //        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        //        c.DefaultModelsExpandDepth(-1); // Disable swagger schema on load
        //    });
        //}

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            c.DefaultModelsExpandDepth(-1); // Disable swagger schema on load
        });

        app.UseHttpsRedirection();
        app.UseCors("FrontEnd");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
