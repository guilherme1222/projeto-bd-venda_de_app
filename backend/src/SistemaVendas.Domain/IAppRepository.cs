using SistemaVendas.Domain;

namespace SistemaVendas.Domain;

public interface IAppRepository
{
    Task<IReadOnlyList<Desenvolvedor>> GetAllDesenvolvedoresAsync();
    Task<Desenvolvedor?> GetDesenvolvedorByIdAsync(string cpf);
    Task CreateDesenvolvedorAsync(Desenvolvedor desenvolvedor);
    Task UpdateDesenvolvedorAsync(Desenvolvedor desenvolvedor);
    Task DeleteDesenvolvedorAsync(string cpf);

    Task<IReadOnlyList<Aplicativo>> GetAllAplicativosAsync();
    Task<Aplicativo?> GetAplicativoByIdAsync(string idApp);
    Task CreateAplicativoAsync(Aplicativo aplicativo);
    Task UpdateAplicativoAsync(Aplicativo aplicativo);
    Task DeleteAplicativoAsync(string idApp);

    Task<IReadOnlyList<Usuario>> GetAllUsuariosAsync();
    Task<Usuario?> GetUsuarioByIdAsync(string idUsuario);
    Task CreateUsuarioAsync(Usuario usuario);
    Task UpdateUsuarioAsync(Usuario usuario);
    Task DeleteUsuarioAsync(string idUsuario);

    Task<IReadOnlyList<Cartao>> GetAllCartoesAsync();
    Task<Cartao?> GetCartaoByIdAsync(string numero);
    Task CreateCartaoAsync(Cartao cartao);
    Task UpdateCartaoAsync(Cartao cartao);
    Task DeleteCartaoAsync(string numero);

    Task<IReadOnlyList<Dispositivo>> GetAllDispositivosAsync();
    Task<Dispositivo?> GetDispositivoByIdAsync(string idDispositivo);
    Task CreateDispositivoAsync(Dispositivo dispositivo);
    Task UpdateDispositivoAsync(Dispositivo dispositivo);
    Task DeleteDispositivoAsync(string idDispositivo);

    Task<IReadOnlyList<Compra>> GetAllComprasAsync();
    Task<Compra?> GetCompraByIdAsync(string idCompra);
    Task CreateCompraAsync(Compra compra);
    Task UpdateCompraAsync(Compra compra);
    Task DeleteCompraAsync(string idCompra);

    Task<IReadOnlyList<Instalado>> GetAllInstaladosAsync();
    Task<Instalado?> GetInstaladoByIdAsync(string idInstalacao);
    Task CreateInstaladoAsync(Instalado instalado);
    Task UpdateInstaladoAsync(Instalado instalado);
    Task DeleteInstaladoAsync(string idInstalacao);

    Task<IReadOnlyList<Avaliacao>> GetAllAvaliacoesAsync();
    Task<Avaliacao?> GetAvaliacaoByIdAsync(string idUsuario, string idApp);
    Task CreateAvaliacaoAsync(Avaliacao avaliacao);
    Task UpdateAvaliacaoAsync(Avaliacao avaliacao);
    Task DeleteAvaliacaoAsync(string idUsuario, string idApp);
}
