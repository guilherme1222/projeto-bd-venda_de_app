using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using SistemaVendas.Domain;

namespace SistemaVendas.Infrastructure;

public class OracleAppRepository : IAppRepository
{
    private readonly string _connectionString;

    public OracleAppRepository(IConfiguration configuration)
    {
        var oracleSettings = configuration.GetSection("OracleSettings");
        var walletPath = oracleSettings.GetValue<string>("WalletPath") ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(walletPath))
        {
            OracleConfiguration.WalletLocation = walletPath;
            OracleConfiguration.SqlNetWalletOverride = true;
            Environment.SetEnvironmentVariable("TNS_ADMIN", walletPath);
        }

        var connectionString = configuration.GetConnectionString("OracleDb");
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            _connectionString = connectionString;
            return;
        }

        var userId = oracleSettings.GetValue<string>("UserId") ?? string.Empty;
        var password = oracleSettings.GetValue<string>("Password") ?? string.Empty;
        var dataSource = oracleSettings.GetValue<string>("DataSource") ?? string.Empty;

        if (string.IsNullOrWhiteSpace(walletPath))
        {
            throw new InvalidOperationException("A configuração do Oracle exige OracleSettings:WalletPath no appsettings.json.");
        }

        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException("A configuração do Oracle exige OracleSettings:UserId e OracleSettings:Password no appsettings.json.");
        }

        if (string.IsNullOrWhiteSpace(dataSource))
        {
            throw new InvalidOperationException("A configuração do Oracle exige OracleSettings:DataSource no appsettings.json.");
        }

        _connectionString = $"User Id={userId};Password={password};Data Source={dataSource}";
    }

    private OracleConnection GetConnection() => new(_connectionString);

    private static void AddParameter(OracleCommand cmd, string name, object? value, OracleDbType type) =>
        cmd.Parameters.Add(name, type).Value = value ?? DBNull.Value;

    public async Task<IReadOnlyList<Desenvolvedor>> GetAllDesenvolvedoresAsync()
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("SELECT cpf, nome, especialidade, agencia, conta, pix FROM desenvolvedor ORDER BY cpf", conn);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        var lista = new List<Desenvolvedor>();
        while (await reader.ReadAsync())
        {
            lista.Add(new Desenvolvedor
            {
                Cpf = reader.GetString(0),
                Nome = reader.GetString(1),
                Especialidade = reader.GetString(2),
                Agencia = reader.GetString(3),
                Conta = reader.GetString(4),
                Pix = reader.GetString(5)
            });
        }
        return lista;
    }

    public async Task<Desenvolvedor?> GetDesenvolvedorByIdAsync(string cpf)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("SELECT cpf, nome, especialidade, agencia, conta, pix FROM desenvolvedor WHERE cpf = :cpf", conn);
        AddParameter(cmd, ":cpf", cpf, OracleDbType.Varchar2);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Desenvolvedor
            {
                Cpf = reader.GetString(0),
                Nome = reader.GetString(1),
                Especialidade = reader.GetString(2),
                Agencia = reader.GetString(3),
                Conta = reader.GetString(4),
                Pix = reader.GetString(5)
            };
        }
        return null;
    }

    public async Task CreateDesenvolvedorAsync(Desenvolvedor desenvolvedor)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("INSERT INTO desenvolvedor (cpf, nome, especialidade, agencia, conta, pix) VALUES (:cpf, :nome, :especialidade, :agencia, :conta, :pix)", conn);
        AddParameter(cmd, ":cpf", desenvolvedor.Cpf, OracleDbType.Varchar2);
        AddParameter(cmd, ":nome", desenvolvedor.Nome, OracleDbType.Varchar2);
        AddParameter(cmd, ":especialidade", desenvolvedor.Especialidade, OracleDbType.Varchar2);
        AddParameter(cmd, ":agencia", desenvolvedor.Agencia, OracleDbType.Varchar2);
        AddParameter(cmd, ":conta", desenvolvedor.Conta, OracleDbType.Varchar2);
        AddParameter(cmd, ":pix", desenvolvedor.Pix, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateDesenvolvedorAsync(Desenvolvedor desenvolvedor)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("UPDATE desenvolvedor SET nome = :nome, especialidade = :especialidade, agencia = :agencia, conta = :conta, pix = :pix WHERE cpf = :cpf", conn);
        AddParameter(cmd, ":nome", desenvolvedor.Nome, OracleDbType.Varchar2);
        AddParameter(cmd, ":especialidade", desenvolvedor.Especialidade, OracleDbType.Varchar2);
        AddParameter(cmd, ":agencia", desenvolvedor.Agencia, OracleDbType.Varchar2);
        AddParameter(cmd, ":conta", desenvolvedor.Conta, OracleDbType.Varchar2);
        AddParameter(cmd, ":pix", desenvolvedor.Pix, OracleDbType.Varchar2);
        AddParameter(cmd, ":cpf", desenvolvedor.Cpf, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteDesenvolvedorAsync(string cpf)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("DELETE FROM desenvolvedor WHERE cpf = :cpf", conn);
        AddParameter(cmd, ":cpf", cpf, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<IReadOnlyList<Aplicativo>> GetAllAplicativosAsync()
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("SELECT id_app, cpf, nome, data_lancamento, tamanho, downloads, genero, preco, versao, status FROM aplicativo ORDER BY id_app", conn);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        var lista = new List<Aplicativo>();
        while (await reader.ReadAsync())
        {
            lista.Add(new Aplicativo
            {
                IdApp = reader.GetString(0),
                Cpf = reader.GetString(1),
                Nome = reader.GetString(2),
                DataLancamento = reader.GetDateTime(3),
                Tamanho = reader.GetDecimal(4),
                Downloads = reader.GetInt32(5),
                Genero = reader.GetString(6),
                Preco = reader.GetDecimal(7),
                Versao = reader.GetString(8),
                Status = reader.GetString(9)
            });
        }
        return lista;
    }

    public async Task<Aplicativo?> GetAplicativoByIdAsync(string idApp)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("SELECT id_app, cpf, nome, data_lancamento, tamanho, downloads, genero, preco, versao, status FROM aplicativo WHERE id_app = :id_app", conn);
        AddParameter(cmd, ":id_app", idApp, OracleDbType.Varchar2);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Aplicativo
            {
                IdApp = reader.GetString(0),
                Cpf = reader.GetString(1),
                Nome = reader.GetString(2),
                DataLancamento = reader.GetDateTime(3),
                Tamanho = reader.GetDecimal(4),
                Downloads = reader.GetInt32(5),
                Genero = reader.GetString(6),
                Preco = reader.GetDecimal(7),
                Versao = reader.GetString(8),
                Status = reader.GetString(9)
            };
        }
        return null;
    }

    public async Task CreateAplicativoAsync(Aplicativo aplicativo)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("INSERT INTO aplicativo (id_app, cpf, nome, data_lancamento, tamanho, downloads, genero, preco, versao, status) VALUES (:id_app, :cpf, :nome, :data_lancamento, :tamanho, :downloads, :genero, :preco, :versao, :status)", conn);
        AddParameter(cmd, ":id_app", aplicativo.IdApp, OracleDbType.Varchar2);
        AddParameter(cmd, ":cpf", aplicativo.Cpf, OracleDbType.Varchar2);
        AddParameter(cmd, ":nome", aplicativo.Nome, OracleDbType.Varchar2);
        AddParameter(cmd, ":data_lancamento", aplicativo.DataLancamento, OracleDbType.Date);
        AddParameter(cmd, ":tamanho", aplicativo.Tamanho, OracleDbType.Decimal);
        AddParameter(cmd, ":downloads", aplicativo.Downloads, OracleDbType.Int32);
        AddParameter(cmd, ":genero", aplicativo.Genero, OracleDbType.Varchar2);
        AddParameter(cmd, ":preco", aplicativo.Preco, OracleDbType.Decimal);
        AddParameter(cmd, ":versao", aplicativo.Versao, OracleDbType.Varchar2);
        AddParameter(cmd, ":status", aplicativo.Status, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateAplicativoAsync(Aplicativo aplicativo)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("UPDATE aplicativo SET cpf = :cpf, nome = :nome, data_lancamento = :data_lancamento, tamanho = :tamanho, downloads = :downloads, genero = :genero, preco = :preco, versao = :versao, status = :status WHERE id_app = :id_app", conn);
        AddParameter(cmd, ":cpf", aplicativo.Cpf, OracleDbType.Varchar2);
        AddParameter(cmd, ":nome", aplicativo.Nome, OracleDbType.Varchar2);
        AddParameter(cmd, ":data_lancamento", aplicativo.DataLancamento, OracleDbType.Date);
        AddParameter(cmd, ":tamanho", aplicativo.Tamanho, OracleDbType.Decimal);
        AddParameter(cmd, ":downloads", aplicativo.Downloads, OracleDbType.Int32);
        AddParameter(cmd, ":genero", aplicativo.Genero, OracleDbType.Varchar2);
        AddParameter(cmd, ":preco", aplicativo.Preco, OracleDbType.Decimal);
        AddParameter(cmd, ":versao", aplicativo.Versao, OracleDbType.Varchar2);
        AddParameter(cmd, ":status", aplicativo.Status, OracleDbType.Varchar2);
        AddParameter(cmd, ":id_app", aplicativo.IdApp, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAplicativoAsync(string idApp)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("DELETE FROM aplicativo WHERE id_app = :id_app", conn);
        AddParameter(cmd, ":id_app", idApp, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<IReadOnlyList<Usuario>> GetAllUsuariosAsync()
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("SELECT id_usuario, login, preferencias FROM usuario ORDER BY id_usuario", conn);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        var lista = new List<Usuario>();
        while (await reader.ReadAsync())
        {
            lista.Add(new Usuario { IdUsuario = reader.GetString(0), Login = reader.GetString(1), Preferencias = reader.GetString(2) });
        }
        return lista;
    }

    public async Task<Usuario?> GetUsuarioByIdAsync(string idUsuario)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("SELECT id_usuario, login, preferencias FROM usuario WHERE id_usuario = :id_usuario", conn);
        AddParameter(cmd, ":id_usuario", idUsuario, OracleDbType.Varchar2);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Usuario { IdUsuario = reader.GetString(0), Login = reader.GetString(1), Preferencias = reader.GetString(2) };
        }
        return null;
    }

    public async Task CreateUsuarioAsync(Usuario usuario)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("INSERT INTO usuario (id_usuario, login, preferencias) VALUES (:id_usuario, :login, :preferencias)", conn);
        AddParameter(cmd, ":id_usuario", usuario.IdUsuario, OracleDbType.Varchar2);
        AddParameter(cmd, ":login", usuario.Login, OracleDbType.Varchar2);
        AddParameter(cmd, ":preferencias", usuario.Preferencias, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateUsuarioAsync(Usuario usuario)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("UPDATE usuario SET login = :login, preferencias = :preferencias WHERE id_usuario = :id_usuario", conn);
        AddParameter(cmd, ":login", usuario.Login, OracleDbType.Varchar2);
        AddParameter(cmd, ":preferencias", usuario.Preferencias, OracleDbType.Varchar2);
        AddParameter(cmd, ":id_usuario", usuario.IdUsuario, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteUsuarioAsync(string idUsuario)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("DELETE FROM usuario WHERE id_usuario = :id_usuario", conn);
        AddParameter(cmd, ":id_usuario", idUsuario, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<IReadOnlyList<Cartao>> GetAllCartoesAsync()
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("SELECT numero, id_usuario, validade, bandeira, tipo_cartao FROM cartao ORDER BY numero", conn);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        var lista = new List<Cartao>();
        while (await reader.ReadAsync())
        {
            lista.Add(new Cartao { Numero = reader.GetString(0), IdUsuario = reader.GetString(1), Validade = reader.GetDateTime(2), Bandeira = reader.GetString(3), TipoCartao = reader.GetString(4) });
        }
        return lista;
    }

    public async Task<Cartao?> GetCartaoByIdAsync(string numero)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("SELECT numero, id_usuario, validade, bandeira, tipo_cartao FROM cartao WHERE numero = :numero", conn);
        AddParameter(cmd, ":numero", numero, OracleDbType.Varchar2);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Cartao { Numero = reader.GetString(0), IdUsuario = reader.GetString(1), Validade = reader.GetDateTime(2), Bandeira = reader.GetString(3), TipoCartao = reader.GetString(4) };
        }
        return null;
    }

    public async Task CreateCartaoAsync(Cartao cartao)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("INSERT INTO cartao (numero, id_usuario, validade, bandeira, tipo_cartao) VALUES (:numero, :id_usuario, :validade, :bandeira, :tipo_cartao)", conn);
        AddParameter(cmd, ":numero", cartao.Numero, OracleDbType.Varchar2);
        AddParameter(cmd, ":id_usuario", cartao.IdUsuario, OracleDbType.Varchar2);
        AddParameter(cmd, ":validade", cartao.Validade, OracleDbType.Date);
        AddParameter(cmd, ":bandeira", cartao.Bandeira, OracleDbType.Varchar2);
        AddParameter(cmd, ":tipo_cartao", cartao.TipoCartao, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateCartaoAsync(Cartao cartao)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("UPDATE cartao SET id_usuario = :id_usuario, validade = :validade, bandeira = :bandeira, tipo_cartao = :tipo_cartao WHERE numero = :numero", conn);
        AddParameter(cmd, ":id_usuario", cartao.IdUsuario, OracleDbType.Varchar2);
        AddParameter(cmd, ":validade", cartao.Validade, OracleDbType.Date);
        AddParameter(cmd, ":bandeira", cartao.Bandeira, OracleDbType.Varchar2);
        AddParameter(cmd, ":tipo_cartao", cartao.TipoCartao, OracleDbType.Varchar2);
        AddParameter(cmd, ":numero", cartao.Numero, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteCartaoAsync(string numero)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("DELETE FROM cartao WHERE numero = :numero", conn);
        AddParameter(cmd, ":numero", numero, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<IReadOnlyList<Dispositivo>> GetAllDispositivosAsync()
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("SELECT id_dispositivo, id_usuario, modelo, tipo_dispositivo FROM dispositivo ORDER BY id_dispositivo", conn);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        var lista = new List<Dispositivo>();
        while (await reader.ReadAsync())
        {
            lista.Add(new Dispositivo { IdDispositivo = reader.GetString(0), IdUsuario = reader.GetString(1), Modelo = reader.GetString(2), TipoDispositivo = reader.GetString(3) });
        }
        return lista;
    }

    public async Task<Dispositivo?> GetDispositivoByIdAsync(string idDispositivo)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("SELECT id_dispositivo, id_usuario, modelo, tipo_dispositivo FROM dispositivo WHERE id_dispositivo = :id_dispositivo", conn);
        AddParameter(cmd, ":id_dispositivo", idDispositivo, OracleDbType.Varchar2);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Dispositivo { IdDispositivo = reader.GetString(0), IdUsuario = reader.GetString(1), Modelo = reader.GetString(2), TipoDispositivo = reader.GetString(3) };
        }
        return null;
    }

    public async Task CreateDispositivoAsync(Dispositivo dispositivo)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("INSERT INTO dispositivo (id_dispositivo, id_usuario, modelo, tipo_dispositivo) VALUES (:id_dispositivo, :id_usuario, :modelo, :tipo_dispositivo)", conn);
        AddParameter(cmd, ":id_dispositivo", dispositivo.IdDispositivo, OracleDbType.Varchar2);
        AddParameter(cmd, ":id_usuario", dispositivo.IdUsuario, OracleDbType.Varchar2);
        AddParameter(cmd, ":modelo", dispositivo.Modelo, OracleDbType.Varchar2);
        AddParameter(cmd, ":tipo_dispositivo", dispositivo.TipoDispositivo, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateDispositivoAsync(Dispositivo dispositivo)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("UPDATE dispositivo SET id_usuario = :id_usuario, modelo = :modelo, tipo_dispositivo = :tipo_dispositivo WHERE id_dispositivo = :id_dispositivo", conn);
        AddParameter(cmd, ":id_usuario", dispositivo.IdUsuario, OracleDbType.Varchar2);
        AddParameter(cmd, ":modelo", dispositivo.Modelo, OracleDbType.Varchar2);
        AddParameter(cmd, ":tipo_dispositivo", dispositivo.TipoDispositivo, OracleDbType.Varchar2);
        AddParameter(cmd, ":id_dispositivo", dispositivo.IdDispositivo, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteDispositivoAsync(string idDispositivo)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("DELETE FROM dispositivo WHERE id_dispositivo = :id_dispositivo", conn);
        AddParameter(cmd, ":id_dispositivo", idDispositivo, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<IReadOnlyList<Compra>> GetAllComprasAsync()
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("SELECT id_compra, id_usuario, id_app, data_compra, preco_pago, forma_pagamento FROM compra ORDER BY id_compra", conn);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        var lista = new List<Compra>();
        while (await reader.ReadAsync())
        {
            lista.Add(new Compra { IdCompra = reader.GetString(0), IdUsuario = reader.GetString(1), IdApp = reader.GetString(2), DataCompra = reader.GetDateTime(3), PrecoPago = reader.GetDecimal(4), FormaPagamento = reader.GetString(5) });
        }
        return lista;
    }

    public async Task<Compra?> GetCompraByIdAsync(string idCompra)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("SELECT id_compra, id_usuario, id_app, data_compra, preco_pago, forma_pagamento FROM compra WHERE id_compra = :id_compra", conn);
        AddParameter(cmd, ":id_compra", idCompra, OracleDbType.Varchar2);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Compra { IdCompra = reader.GetString(0), IdUsuario = reader.GetString(1), IdApp = reader.GetString(2), DataCompra = reader.GetDateTime(3), PrecoPago = reader.GetDecimal(4), FormaPagamento = reader.GetString(5) };
        }
        return null;
    }

    public async Task CreateCompraAsync(Compra compra)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("INSERT INTO compra (id_compra, id_usuario, id_app, data_compra, preco_pago, forma_pagamento) VALUES (:id_compra, :id_usuario, :id_app, :data_compra, :preco_pago, :forma_pagamento)", conn);
        AddParameter(cmd, ":id_compra", compra.IdCompra, OracleDbType.Varchar2);
        AddParameter(cmd, ":id_usuario", compra.IdUsuario, OracleDbType.Varchar2);
        AddParameter(cmd, ":id_app", compra.IdApp, OracleDbType.Varchar2);
        AddParameter(cmd, ":data_compra", compra.DataCompra, OracleDbType.Date);
        AddParameter(cmd, ":preco_pago", compra.PrecoPago, OracleDbType.Decimal);
        AddParameter(cmd, ":forma_pagamento", compra.FormaPagamento, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateCompraAsync(Compra compra)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("UPDATE compra SET id_usuario = :id_usuario, id_app = :id_app, data_compra = :data_compra, preco_pago = :preco_pago, forma_pagamento = :forma_pagamento WHERE id_compra = :id_compra", conn);
        AddParameter(cmd, ":id_usuario", compra.IdUsuario, OracleDbType.Varchar2);
        AddParameter(cmd, ":id_app", compra.IdApp, OracleDbType.Varchar2);
        AddParameter(cmd, ":data_compra", compra.DataCompra, OracleDbType.Date);
        AddParameter(cmd, ":preco_pago", compra.PrecoPago, OracleDbType.Decimal);
        AddParameter(cmd, ":forma_pagamento", compra.FormaPagamento, OracleDbType.Varchar2);
        AddParameter(cmd, ":id_compra", compra.IdCompra, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteCompraAsync(string idCompra)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("DELETE FROM compra WHERE id_compra = :id_compra", conn);
        AddParameter(cmd, ":id_compra", idCompra, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<IReadOnlyList<Instalado>> GetAllInstaladosAsync()
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("SELECT id_instalacao, id_usuario, id_app, id_dispositivo, data_instalacao FROM instalado ORDER BY id_instalacao", conn);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        var lista = new List<Instalado>();
        while (await reader.ReadAsync())
        {
            lista.Add(new Instalado { IdInstalacao = reader.GetString(0), IdUsuario = reader.GetString(1), IdApp = reader.GetString(2), IdDispositivo = reader.GetString(3), DataInstalacao = reader.GetDateTime(4) });
        }
        return lista;
    }

    public async Task<Instalado?> GetInstaladoByIdAsync(string idInstalacao)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("SELECT id_instalacao, id_usuario, id_app, id_dispositivo, data_instalacao FROM instalado WHERE id_instalacao = :id_instalacao", conn);
        AddParameter(cmd, ":id_instalacao", idInstalacao, OracleDbType.Varchar2);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Instalado { IdInstalacao = reader.GetString(0), IdUsuario = reader.GetString(1), IdApp = reader.GetString(2), IdDispositivo = reader.GetString(3), DataInstalacao = reader.GetDateTime(4) };
        }
        return null;
    }

    public async Task CreateInstaladoAsync(Instalado instalado)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("INSERT INTO instalado (id_instalacao, id_usuario, id_app, id_dispositivo, data_instalacao) VALUES (:id_instalacao, :id_usuario, :id_app, :id_dispositivo, :data_instalacao)", conn);
        AddParameter(cmd, ":id_instalacao", instalado.IdInstalacao, OracleDbType.Varchar2);
        AddParameter(cmd, ":id_usuario", instalado.IdUsuario, OracleDbType.Varchar2);
        AddParameter(cmd, ":id_app", instalado.IdApp, OracleDbType.Varchar2);
        AddParameter(cmd, ":id_dispositivo", instalado.IdDispositivo, OracleDbType.Varchar2);
        AddParameter(cmd, ":data_instalacao", instalado.DataInstalacao, OracleDbType.Date);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateInstaladoAsync(Instalado instalado)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("UPDATE instalado SET id_usuario = :id_usuario, id_app = :id_app, id_dispositivo = :id_dispositivo, data_instalacao = :data_instalacao WHERE id_instalacao = :id_instalacao", conn);
        AddParameter(cmd, ":id_usuario", instalado.IdUsuario, OracleDbType.Varchar2);
        AddParameter(cmd, ":id_app", instalado.IdApp, OracleDbType.Varchar2);
        AddParameter(cmd, ":id_dispositivo", instalado.IdDispositivo, OracleDbType.Varchar2);
        AddParameter(cmd, ":data_instalacao", instalado.DataInstalacao, OracleDbType.Date);
        AddParameter(cmd, ":id_instalacao", instalado.IdInstalacao, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteInstaladoAsync(string idInstalacao)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("DELETE FROM instalado WHERE id_instalacao = :id_instalacao", conn);
        AddParameter(cmd, ":id_instalacao", idInstalacao, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<IReadOnlyList<Avaliacao>> GetAllAvaliacoesAsync()
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("SELECT id_usuario, id_app, nota, comentario FROM avaliacao ORDER BY id_usuario, id_app", conn);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        var lista = new List<Avaliacao>();
        while (await reader.ReadAsync())
        {
            lista.Add(new Avaliacao { IdUsuario = reader.GetString(0), IdApp = reader.GetString(1), Nota = reader.GetInt32(2), Comentario = reader.IsDBNull(3) ? string.Empty : reader.GetString(3) });
        }
        return lista;
    }

    public async Task<Avaliacao?> GetAvaliacaoByIdAsync(string idUsuario, string idApp)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("SELECT id_usuario, id_app, nota, comentario FROM avaliacao WHERE id_usuario = :id_usuario AND id_app = :id_app", conn);
        AddParameter(cmd, ":id_usuario", idUsuario, OracleDbType.Varchar2);
        AddParameter(cmd, ":id_app", idApp, OracleDbType.Varchar2);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Avaliacao { IdUsuario = reader.GetString(0), IdApp = reader.GetString(1), Nota = reader.GetInt32(2), Comentario = reader.IsDBNull(3) ? string.Empty : reader.GetString(3) };
        }
        return null;
    }

    public async Task CreateAvaliacaoAsync(Avaliacao avaliacao)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("INSERT INTO avaliacao (id_usuario, id_app, nota, comentario) VALUES (:id_usuario, :id_app, :nota, :comentario)", conn);
        AddParameter(cmd, ":id_usuario", avaliacao.IdUsuario, OracleDbType.Varchar2);
        AddParameter(cmd, ":id_app", avaliacao.IdApp, OracleDbType.Varchar2);
        AddParameter(cmd, ":nota", avaliacao.Nota, OracleDbType.Int32);
        AddParameter(cmd, ":comentario", avaliacao.Comentario, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateAvaliacaoAsync(Avaliacao avaliacao)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("UPDATE avaliacao SET nota = :nota, comentario = :comentario WHERE id_usuario = :id_usuario AND id_app = :id_app", conn);
        AddParameter(cmd, ":nota", avaliacao.Nota, OracleDbType.Int32);
        AddParameter(cmd, ":comentario", avaliacao.Comentario, OracleDbType.Varchar2);
        AddParameter(cmd, ":id_usuario", avaliacao.IdUsuario, OracleDbType.Varchar2);
        AddParameter(cmd, ":id_app", avaliacao.IdApp, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAvaliacaoAsync(string idUsuario, string idApp)
    {
        using var conn = GetConnection();
        using var cmd = new OracleCommand("DELETE FROM avaliacao WHERE id_usuario = :id_usuario AND id_app = :id_app", conn);
        AddParameter(cmd, ":id_usuario", idUsuario, OracleDbType.Varchar2);
        AddParameter(cmd, ":id_app", idApp, OracleDbType.Varchar2);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}
