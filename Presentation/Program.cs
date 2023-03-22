using Application;
using Infrastructure;
using lingo;

// Maakt een pre-config van WebApplicationBuilder om waarschinlijk een een container/context op te bouwen
var builder = WebApplication.CreateBuilder(args); 

// Add services to the container. The Dependency Injection. Defines the services your application needs and pre-configures the services.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentationServices(builder.Configuration, builder.Environment);

var app = builder.Build();

//app.InitialiseAndSeedDatabase(); // if you want to seed data into db. So start database with data already in it

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // idk
    app.UseMigrationsEndPoint(); // idk
}
else
{
    app.UseExceptionHandler("/Error"); // sets an route for handling exceptions
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

string[] supportedCultures = { "en" }; // configures the context/container to specific local/country a culture is used for number and date formats

app.UseRequestLocalization(options =>
{
    options.SetDefaultCulture(supportedCultures.First()); // adds default local/country
    options.AddSupportedCultures(supportedCultures); // adds all local/country that the application needs to support or supports?
    options.AddSupportedUICultures(supportedCultures); // Chooses which ?culture resources? is going to be loaded by the app
});

app.UseHttpsRedirection(); // Converts HTTP request to HTTPS request
app.UseStaticFiles(); // Allows the app to use static files like HTML, CSS, images, and JavaScript;

app.UseRouting(); // Allows the app to use request routing like api/v1/games

app.UseAuthentication(); // To define the user and see if user exist
app.UseAuthorization(); // To define what the Authenticated user is allowed to do in the app

app.Run(); // Starts the application and blocks here until Thread is stopped