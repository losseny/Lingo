using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Application.Common.Interfaces;
using FluentValidation.AspNetCore;
using Infrastructure.Persistence;
using lingo.Filters;
using lingo.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace lingo;


public static class ConfigureServices
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
	{ 
		if (environment.IsDevelopment())
			services.AddDatabaseDeveloperPageExceptionFilter();

		services.AddSingleton<ICurrentUserService, CurrentUserService>();
		services.AddSingleton<IRequestService, RequestService>();

		services.AddHttpContextAccessor();

		services.AddHealthChecks()
			.AddDbContextCheck<ApplicationDbContext>();

		services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
				options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			})
			.AddMvcLocalization();
		services.AddFluentValidationClientsideAdapters();

		services.AddRazorPages();

		// Customise default API behaviour
		services.Configure<ApiBehaviorOptions>(options =>
			options.SuppressModelStateInvalidFilter = true);

		// Configure swagger API Docs
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo
			{
				Version = "v1",
				Title = environment.EnvironmentName
			});
			options.AddSecurityDefinition("Bearer",
				new OpenApiSecurityScheme
				{
					Description = "Example: 'Bearer {your JWT token}'",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});
			options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
						Scheme = SecuritySchemeType.OAuth2.ToString(),
						Name = "Bearer",
						In = ParameterLocation.Header
					},
					Array.Empty<string>()
				}
			});
			options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
			options.CustomOperationIds(api => $"{api.ActionDescriptor.RouteValues["action"]}");
		});

		// Configure JWT authentication
		services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = configuration["Authentication:OAuth2:Issuer"],
					ValidAudience = configuration["Authentication:OAuth2:Audience"],
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:OAuth2:SecurityKey"]!))
				};
			});

		return services;
	}
}