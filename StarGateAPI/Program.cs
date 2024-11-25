using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;
using StargateAPI.Business.Providers;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// <begin poor coding>
// 11.25.2024 RG, ~ 5am
// All changes related to AWS were lost when web server crashed. Lesson learned. 
// Given the time, I am going to re-create as simply as possible. This is not good coding,
// a much better implementation of this functionality is represented at the following link.
// Time permitting, I would design and implement accordingly.
// https://aws.amazon.com/blogs/modernizing-with-aws/how-to-load-net-configuration-from-aws-secrets-manager/
// The following are stored as plain text for now - not a good practice. Should encode/decode to use.


var secretResponse = await AWSProvider.GetCredentials(
    new BaseCloudRequest
    {
        KeyID = Environment.GetEnvironmentVariable("AWS_KEY_ID"),
        SecretKeyId = Environment.GetEnvironmentVariable("AWS_SECRET_KEY_ID"),
        CredentialsLabel = Environment.GetEnvironmentVariable("AWS_DB_CREDENTIALS_LABEL")
    }
 );

// Build
string connectionString = String.Format(builder.Configuration.GetConnectionString("StarbaseApiDatabase"), secretResponse.Host, secretResponse.DBInstanceIdentifier, secretResponse.UserName, secretResponse.Password);
builder.Services.AddDbContext<StargateContext>(options => 
    options.UseSqlServer(connectionString));

builder.Services.AddMediatR(cfg =>
{
    cfg.AddRequestPreProcessor<CreateAstronautDutyPreProcessor>();
    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


