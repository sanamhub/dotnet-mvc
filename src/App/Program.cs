using App.Configurations;

var builder = WebApplication.CreateBuilder(args);
builder.AddServicesToContainer();

var app = builder.Build();
app.ConfigureHttpRequestPipeline();

app.Run();
