using FluentValidation;
using foroLIS_backend.DTOs;
using foroLIS_backend.DTOs.FileDto;
using foroLIS_backend.DTOs.PostDtos;
using foroLIS_backend.DTOs.SurveyDtos;
using foroLIS_backend.Extensions;
using foroLIS_backend.Infrastructure.Context;
using foroLIS_backend.Models;
using foroLIS_backend.Repository;
using foroLIS_backend.Services;
using foroLIS_backend.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");


if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("The connectionString enviroment is not set");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<Users, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

    //services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICurrentUserService,CurrentService>();
builder.Services.AddScoped<ICommonService<PostDto, Guid, PostInsertDto, PostUpdateDto>, PostService>();
builder.Services.AddScoped<ISurveyService<SurveyDto, SurveyInsertDto, UserFieldSurveyDto, UserFieldInsertSurveyDto>,SurveyService>();
builder.Services.AddScoped<FileService>();

//validators
builder.Services.AddScoped<IValidator<PostInsertDto>, PostInsertValidator>();
builder.Services.AddScoped<IValidator<PostUpdateDto>, PostUpdateValidator>();
builder.Services.AddScoped<IValidator<SurveyInsertDto>, SurveyInsertValidator>();
builder.Services.AddScoped<IValidator<UserRegisterRequestDto>, UserRegisterRequestValidator>();
builder.Services.AddScoped<IValidator<IFormFile>,FileValidator>();

// repository
builder.Services.AddScoped<IRepository<Post,PostDto>, PostRepository>();
builder.Services.AddScoped<ISurveyRepository<Survey, FieldSurvey, UserFieldSurvey, FieldSurveyDto>, SurveyRepository>();
builder.Services.AddScoped<IFileRepository<MediaFile>, FileRepository>();

builder.Services.AddScoped<GoogleService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<GoogleService>(c =>
{
    c.BaseAddress = new Uri("https://www.googleapis.com/oauth2/v1/userinfo");
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Auth", Version = "v1", Description = "Services to Authenticate user" });


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter a valid token in the following format: {your token here} do not add the word 'Bearer' before it."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});



builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.ConfigureCors();

var app = builder.Build();


app.UseCors("CorsPolicy");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "FilesUploaded")),
    RequestPath = "/files"
});

app.Run();
