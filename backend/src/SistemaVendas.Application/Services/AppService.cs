using SistemaVendas.Domain;

namespace SistemaVendas.Application.Services;

public class AppService
{
    private readonly IAppRepository _repository;

    public AppService(IAppRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<Desenvolvedor>> GetAllDesenvolvedoresAsync() => _repository.GetAllDesenvolvedoresAsync();
    public Task<Desenvolvedor?> GetDesenvolvedorByIdAsync(string cpf) => _repository.GetDesenvolvedorByIdAsync(cpf);
    public Task CreateDesenvolvedorAsync(Desenvolvedor desenvolvedor) => _repository.CreateDesenvolvedorAsync(desenvolvedor);
    public Task UpdateDesenvolvedorAsync(Desenvolvedor desenvolvedor) => _repository.UpdateDesenvolvedorAsync(desenvolvedor);
    public Task DeleteDesenvolvedorAsync(string cpf) => _repository.DeleteDesenvolvedorAsync(cpf);

    public Task<IReadOnlyList<Aplicativo>> GetAllAplicativosAsync() => _repository.GetAllAplicativosAsync();
    public Task<Aplicativo?> GetAplicativoByIdAsync(string idApp) => _repository.GetAplicativoByIdAsync(idApp);
    public Task CreateAplicativoAsync(Aplicativo aplicativo) => _repository.CreateAplicativoAsync(aplicativo);
    public Task UpdateAplicativoAsync(Aplicativo aplicativo) => _repository.UpdateAplicativoAsync(aplicativo);
    public Task DeleteAplicativoAsync(string idApp) => _repository.DeleteAplicativoAsync(idApp);

    public Task<IReadOnlyList<Usuario>> GetAllUsuariosAsync() => _repository.GetAllUsuariosAsync();
    public Task<Usuario?> GetUsuarioByIdAsync(string idUsuario) => _repository.GetUsuarioByIdAsync(idUsuario);
    public Task CreateUsuarioAsync(Usuario usuario) => _repository.CreateUsuarioAsync(usuario);
    public Task UpdateUsuarioAsync(Usuario usuario) => _repository.UpdateUsuarioAsync(usuario);
    public Task DeleteUsuarioAsync(string idUsuario) => _repository.DeleteUsuarioAsync(idUsuario);

    public Task<IReadOnlyList<Cartao>> GetAllCartoesAsync() => _repository.GetAllCartoesAsync();
    public Task<Cartao?> GetCartaoByIdAsync(string numero) => _repository.GetCartaoByIdAsync(numero);
    public Task CreateCartaoAsync(Cartao cartao) => _repository.CreateCartaoAsync(cartao);
    public Task UpdateCartaoAsync(Cartao cartao) => _repository.UpdateCartaoAsync(cartao);
    public Task DeleteCartaoAsync(string numero) => _repository.DeleteCartaoAsync(numero);

    public Task<IReadOnlyList<Dispositivo>> GetAllDispositivosAsync() => _repository.GetAllDispositivosAsync();
    public Task<Dispositivo?> GetDispositivoByIdAsync(string idDispositivo) => _repository.GetDispositivoByIdAsync(idDispositivo);
    public Task CreateDispositivoAsync(Dispositivo dispositivo) => _repository.CreateDispositivoAsync(dispositivo);
    public Task UpdateDispositivoAsync(Dispositivo dispositivo) => _repository.UpdateDispositivoAsync(dispositivo);
    public Task DeleteDispositivoAsync(string idDispositivo) => _repository.DeleteDispositivoAsync(idDispositivo);

    public Task<IReadOnlyList<Compra>> GetAllComprasAsync() => _repository.GetAllComprasAsync();
    public Task<Compra?> GetCompraByIdAsync(string idCompra) => _repository.GetCompraByIdAsync(idCompra);
    public Task CreateCompraAsync(Compra compra) => _repository.CreateCompraAsync(compra);
    public Task UpdateCompraAsync(Compra compra) => _repository.UpdateCompraAsync(compra);
    public Task DeleteCompraAsync(string idCompra) => _repository.DeleteCompraAsync(idCompra);

    public Task<IReadOnlyList<Instalado>> GetAllInstaladosAsync() => _repository.GetAllInstaladosAsync();
    public Task<Instalado?> GetInstaladoByIdAsync(string idInstalacao) => _repository.GetInstaladoByIdAsync(idInstalacao);
    public Task CreateInstaladoAsync(Instalado instalado) => _repository.CreateInstaladoAsync(instalado);
    public Task UpdateInstaladoAsync(Instalado instalado) => _repository.UpdateInstaladoAsync(instalado);
    public Task DeleteInstaladoAsync(string idInstalacao) => _repository.DeleteInstaladoAsync(idInstalacao);

    public Task<IReadOnlyList<Avaliacao>> GetAllAvaliacoesAsync() => _repository.GetAllAvaliacoesAsync();
    public Task<Avaliacao?> GetAvaliacaoByIdAsync(string idUsuario, string idApp) => _repository.GetAvaliacaoByIdAsync(idUsuario, idApp);
    public Task CreateAvaliacaoAsync(Avaliacao avaliacao) => _repository.CreateAvaliacaoAsync(avaliacao);
    public Task UpdateAvaliacaoAsync(Avaliacao avaliacao) => _repository.UpdateAvaliacaoAsync(avaliacao);
    public Task DeleteAvaliacaoAsync(string idUsuario, string idApp) => _repository.DeleteAvaliacaoAsync(idUsuario, idApp);
}
