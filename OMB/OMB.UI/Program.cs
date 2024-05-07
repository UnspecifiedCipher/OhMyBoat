using OMB.UI;
using OMB.UI.Components;
using OMB.Aplication.UserUseCases;
using OMB.Aplication.ShipPostUseCases;
using OMB.Aplication.VehiclePostUseCases;
using OMB.Aplication.VehicleUseCases;
using OMB.Aplication.ShipUseCases;
using OMB.Repositories;
using OMB.Aplication.Interfaces;
//Cosas para la sesión
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

using(OMBContext context = new OMBContext()){
    context.Database.EnsureCreated();
}
Testing.Initialize();

// ACA ESTA LO QUE AGREGAMOS NOSOTROS

//ESTO ES PARA AGREGAR EL SERVICIO DE MIDDLEWARE DE SESIÓN

builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".OMB.Session"; // Cambia el nombre de la cookie según tus preferencias
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Establece el tiempo de expiración de la sesión
});


builder.Services.AddTransient<addUserUseCase>();
builder.Services.AddTransient<deleteUserUseCase>();
builder.Services.AddTransient<modifyUserUseCase>();
builder.Services.AddTransient<userListUseCase>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//si no agregas todos los scoped aunque no los uses no funca

builder.Services.AddTransient<addVehicleUseCase>();
builder.Services.AddTransient<deleteVehicleUseCase>();
builder.Services.AddTransient<modifyVehicleUseCase>();
builder.Services.AddTransient<vehicleListUseCase>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();

builder.Services.AddTransient<addVehiclePostUseCase>();
builder.Services.AddTransient<deleteVehiclePostUseCase>();
builder.Services.AddTransient<modifyVehiclePostUseCase>();
builder.Services.AddTransient<vehiclePostListUseCase>();
builder.Services.AddScoped<IVehiclePostRepository, VehiclePostRepository>();

builder.Services.AddTransient<addShipUseCase>();
builder.Services.AddTransient<deleteShipUseCase>();
builder.Services.AddTransient<modifyShipUseCase>();
builder.Services.AddTransient<shipListUseCase>();
builder.Services.AddScoped<IShipRepository, ShipRepository>();

builder.Services.AddTransient<addShipPostUseCase>();
builder.Services.AddTransient<deleteShipPostUseCase>();
builder.Services.AddTransient<modifyShipPostUseCase>();
builder.Services.AddTransient<shipPostListUseCase>();
builder.Services.AddScoped<IShipPostRepository, ShipPostRepository>();

// FIN DE LO QUE AGREGAMOS NOSOTROS

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


//ESTO ES PARA HABILITAR EL MIDDLEWARE DE SESIÓN
app.UseSession();



app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
