using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BitRent.Data;
using BitRent.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BitRentDb"));
});
builder.Services.AddScoped<IAppUser, AppUserRepository>();
builder.Services.AddScoped<ITransaction, TransactionRepository>();
builder.Services.AddScoped<ICustomer, CustomerRepository>();
builder.Services.AddScoped<IOwner, OwnerRepository>();
builder.Services.AddScoped<IProperty, PropertyRepository>();

builder.Services.AddControllers().AddNewtonsoftJson(o =>
{
    o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    o.UseCamelCasing(true);
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtBitRent:Issuer"],//domain.com
        ValidAudience = builder.Configuration["JwtBitRent:Audience"],//domain.com
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtBitRent:Key"]!))
    };
});

builder.Services.AddCors(policy =>
{
    policy.AddPolicy("corsPolicy", o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()
    .WithExposedHeaders("Bearer"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s => {
    s.CustomSchemaIds(c => c.FullName);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseSession();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("corsPolicy");
app.UseRouting();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
