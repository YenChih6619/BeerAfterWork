using Microsoft.AspNetCore.SignalR;
using System.Text;
using Web.Components;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

//Bootstrap

//配置繁體中文
/**********************************************************************/
builder.Services.AddBootstrapBlazor(null, options =>
{
    // 忽略文化信息丢失日志
    options.IgnoreLocalizerMissing = true;

    // 设置 RESX 格式多语言资源文件 如 Program.{CultureName}.resx
    options.ResourceManagerStringLocalizerType = typeof(Program);

    // 设置 Json 格式嵌入式资源文件
    options.AdditionalJsonAssemblies = new[] { typeof(Web.Components.App).Assembly };

    // 设置 Json 物理路径文件
    options.AdditionalJsonFiles = new string[]
    {
        @"D:\zh-TW.json"
    };
});
/**********************************************************************/


// 增加 SignalR 服务数据传输大小限制配置
builder.Services.Configure<HubOptions>(option => option.MaximumReceiveMessageSize = null);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseResponseCompression();
}

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
