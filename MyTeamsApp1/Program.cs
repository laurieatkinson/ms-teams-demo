using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.TeamsFx.SimpleAuth;
using MyTeamsApp1;
using MyTeamsApp1.Interop.TeamsSDK;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddTeamsFxSimpleAuth(builder.Configuration);
builder.Services.AddTeamsFx(builder.Configuration.GetSection("TeamsFx"));
builder.Services.AddScoped<MicrosoftTeams>();

builder.Services.AddControllers();
builder.Services.AddHttpClient("WebClient", client => client.Timeout = TimeSpan.FromSeconds(600));
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapControllers();
});

app.Run();

