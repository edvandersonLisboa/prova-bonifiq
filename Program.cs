using Microsoft.EntityFrameworkCore;
using ProvaPub.Repository;
using ProvaPub.Repository.Interfaces;
using ProvaPub.Services;
using ProvaPub.Services.Factories;
using ProvaPub.Services.Factories.Interfaces;
using ProvaPub.Services.Interfaces;
using ProvaPub.Services.Payments;
using ProvaPub.Shared.Repository.Base;
using ProvaPub.Shared.SystemDate;
using ProvaPub.Shared.SystemDate.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<TestDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("ctx")), ServiceLifetime.Scoped);
builder.Services.AddScoped<Random>();
//builder.Services.AddScoped<TestDbContext>();
builder.Services.AddScoped<RandomService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CustomerService>();

//services  pagaments
builder.Services.AddScoped<IPaymentService,CreditCard>();
builder.Services.AddScoped<IPaymentService, Pix>();
builder.Services.AddScoped<IPaymentService, PayPal>();

builder.Services.AddScoped<IRandomNumberRepository, RandomNumberRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentServiceFactory, PaymentServiceFactory>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();
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
