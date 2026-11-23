using Microsoft.Extensions.FileProviders;
using SistemaVendas.Application.Services;
using SistemaVendas.Domain;
using SistemaVendas.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5150");
builder.Services.AddSingleton<IAppRepository, OracleAppRepository>();
builder.Services.AddSingleton<AppService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("frontend");

var frontendPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "..", "..", "frontend"));
if (Directory.Exists(frontendPath))
{
    app.UseDefaultFiles(new DefaultFilesOptions
    {
        FileProvider = new PhysicalFileProvider(frontendPath),
        DefaultFileNames = new[] { "index.html" }
    });
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(frontendPath)
    });
}

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

var api = app.MapGroup("/api");

api.MapGet("/desenvolvedores", async (AppService service) => await service.GetAllDesenvolvedoresAsync());
api.MapGet("/desenvolvedores/{cpf}", async (string cpf, AppService service) =>
{
    var result = await service.GetDesenvolvedorByIdAsync(cpf);
    return result is null ? Results.NotFound() : Results.Ok(result);
});
api.MapPost("/desenvolvedores", async (Desenvolvedor desenvolvedor, AppService service) =>
{
    await service.CreateDesenvolvedorAsync(desenvolvedor);
    return Results.Created($"/api/desenvolvedores/{desenvolvedor.Cpf}", desenvolvedor);
});
api.MapPut("/desenvolvedores", async (Desenvolvedor desenvolvedor, AppService service) =>
{
    await service.UpdateDesenvolvedorAsync(desenvolvedor);
    return Results.Ok(desenvolvedor);
});
api.MapDelete("/desenvolvedores/{cpf}", async (string cpf, AppService service) =>
{
    await service.DeleteDesenvolvedorAsync(cpf);
    return Results.NoContent();
});

api.MapGet("/aplicativos", async (AppService service) => await service.GetAllAplicativosAsync());
api.MapGet("/aplicativos/{idApp}", async (string idApp, AppService service) =>
{
    var result = await service.GetAplicativoByIdAsync(idApp);
    return result is null ? Results.NotFound() : Results.Ok(result);
});
api.MapPost("/aplicativos", async (Aplicativo aplicativo, AppService service) =>
{
    await service.CreateAplicativoAsync(aplicativo);
    return Results.Created($"/api/aplicativos/{aplicativo.IdApp}", aplicativo);
});
api.MapPut("/aplicativos", async (Aplicativo aplicativo, AppService service) =>
{
    await service.UpdateAplicativoAsync(aplicativo);
    return Results.Ok(aplicativo);
});
api.MapDelete("/aplicativos/{idApp}", async (string idApp, AppService service) =>
{
    await service.DeleteAplicativoAsync(idApp);
    return Results.NoContent();
});

api.MapGet("/usuarios", async (AppService service) => await service.GetAllUsuariosAsync());
api.MapGet("/usuarios/{idUsuario}", async (string idUsuario, AppService service) =>
{
    var result = await service.GetUsuarioByIdAsync(idUsuario);
    return result is null ? Results.NotFound() : Results.Ok(result);
});
api.MapPost("/usuarios", async (Usuario usuario, AppService service) =>
{
    await service.CreateUsuarioAsync(usuario);
    return Results.Created($"/api/usuarios/{usuario.IdUsuario}", usuario);
});
api.MapPut("/usuarios", async (Usuario usuario, AppService service) =>
{
    await service.UpdateUsuarioAsync(usuario);
    return Results.Ok(usuario);
});
api.MapDelete("/usuarios/{idUsuario}", async (string idUsuario, AppService service) =>
{
    await service.DeleteUsuarioAsync(idUsuario);
    return Results.NoContent();
});

api.MapGet("/cartoes", async (AppService service) => await service.GetAllCartoesAsync());
api.MapGet("/cartoes/{numero}", async (string numero, AppService service) =>
{
    var result = await service.GetCartaoByIdAsync(numero);
    return result is null ? Results.NotFound() : Results.Ok(result);
});
api.MapPost("/cartoes", async (Cartao cartao, AppService service) =>
{
    await service.CreateCartaoAsync(cartao);
    return Results.Created($"/api/cartoes/{cartao.Numero}", cartao);
});
api.MapPut("/cartoes", async (Cartao cartao, AppService service) =>
{
    await service.UpdateCartaoAsync(cartao);
    return Results.Ok(cartao);
});
api.MapDelete("/cartoes/{numero}", async (string numero, AppService service) =>
{
    await service.DeleteCartaoAsync(numero);
    return Results.NoContent();
});

api.MapGet("/dispositivos", async (AppService service) => await service.GetAllDispositivosAsync());
api.MapGet("/dispositivos/{idDispositivo}", async (string idDispositivo, AppService service) =>
{
    var result = await service.GetDispositivoByIdAsync(idDispositivo);
    return result is null ? Results.NotFound() : Results.Ok(result);
});
api.MapPost("/dispositivos", async (Dispositivo dispositivo, AppService service) =>
{
    await service.CreateDispositivoAsync(dispositivo);
    return Results.Created($"/api/dispositivos/{dispositivo.IdDispositivo}", dispositivo);
});
api.MapPut("/dispositivos", async (Dispositivo dispositivo, AppService service) =>
{
    await service.UpdateDispositivoAsync(dispositivo);
    return Results.Ok(dispositivo);
});
api.MapDelete("/dispositivos/{idDispositivo}", async (string idDispositivo, AppService service) =>
{
    await service.DeleteDispositivoAsync(idDispositivo);
    return Results.NoContent();
});

api.MapGet("/compras", async (AppService service) => await service.GetAllComprasAsync());
api.MapGet("/compras/{idCompra}", async (string idCompra, AppService service) =>
{
    var result = await service.GetCompraByIdAsync(idCompra);
    return result is null ? Results.NotFound() : Results.Ok(result);
});
api.MapPost("/compras", async (Compra compra, AppService service) =>
{
    await service.CreateCompraAsync(compra);
    return Results.Created($"/api/compras/{compra.IdCompra}", compra);
});
api.MapPut("/compras", async (Compra compra, AppService service) =>
{
    await service.UpdateCompraAsync(compra);
    return Results.Ok(compra);
});
api.MapDelete("/compras/{idCompra}", async (string idCompra, AppService service) =>
{
    await service.DeleteCompraAsync(idCompra);
    return Results.NoContent();
});

api.MapGet("/instalados", async (AppService service) => await service.GetAllInstaladosAsync());
api.MapGet("/instalados/{idInstalacao}", async (string idInstalacao, AppService service) =>
{
    var result = await service.GetInstaladoByIdAsync(idInstalacao);
    return result is null ? Results.NotFound() : Results.Ok(result);
});
api.MapPost("/instalados", async (Instalado instalado, AppService service) =>
{
    await service.CreateInstaladoAsync(instalado);
    return Results.Created($"/api/instalados/{instalado.IdInstalacao}", instalado);
});
api.MapPut("/instalados", async (Instalado instalado, AppService service) =>
{
    await service.UpdateInstaladoAsync(instalado);
    return Results.Ok(instalado);
});
api.MapDelete("/instalados/{idInstalacao}", async (string idInstalacao, AppService service) =>
{
    await service.DeleteInstaladoAsync(idInstalacao);
    return Results.NoContent();
});

api.MapGet("/avaliacoes", async (AppService service) => await service.GetAllAvaliacoesAsync());
api.MapGet("/avaliacoes/{idUsuario}/{idApp}", async (string idUsuario, string idApp, AppService service) =>
{
    var result = await service.GetAvaliacaoByIdAsync(idUsuario, idApp);
    return result is null ? Results.NotFound() : Results.Ok(result);
});
api.MapPost("/avaliacoes", async (Avaliacao avaliacao, AppService service) =>
{
    await service.CreateAvaliacaoAsync(avaliacao);
    return Results.Created($"/api/avaliacoes/{avaliacao.IdUsuario}/{avaliacao.IdApp}", avaliacao);
});
api.MapPut("/avaliacoes", async (Avaliacao avaliacao, AppService service) =>
{
    await service.UpdateAvaliacaoAsync(avaliacao);
    return Results.Ok(avaliacao);
});
api.MapDelete("/avaliacoes/{idUsuario}/{idApp}", async (string idUsuario, string idApp, AppService service) =>
{
    await service.DeleteAvaliacaoAsync(idUsuario, idApp);
    return Results.NoContent();
});

app.Run();
