namespace SistemaVendas.Domain;

public class Desenvolvedor
{
    public string Cpf { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Especialidade { get; set; } = string.Empty;
    public string Agencia { get; set; } = string.Empty;
    public string Conta { get; set; } = string.Empty;
    public string Pix { get; set; } = string.Empty;
}

public class Aplicativo
{
    public string IdApp { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public DateTime DataLancamento { get; set; }
    public decimal Tamanho { get; set; }
    public int Downloads { get; set; }
    public string Genero { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string Versao { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class Usuario
{
    public string IdUsuario { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Preferencias { get; set; } = string.Empty;
}

public class Cartao
{
    public string Numero { get; set; } = string.Empty;
    public string IdUsuario { get; set; } = string.Empty;
    public DateTime Validade { get; set; }
    public string Bandeira { get; set; } = string.Empty;
    public string TipoCartao { get; set; } = string.Empty;
}

public class Dispositivo
{
    public string IdDispositivo { get; set; } = string.Empty;
    public string IdUsuario { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string TipoDispositivo { get; set; } = string.Empty;
}

public class Compra
{
    public string IdCompra { get; set; } = string.Empty;
    public string IdUsuario { get; set; } = string.Empty;
    public string IdApp { get; set; } = string.Empty;
    public DateTime DataCompra { get; set; }
    public decimal PrecoPago { get; set; }
    public string FormaPagamento { get; set; } = string.Empty;
}

public class Instalado
{
    public string IdInstalacao { get; set; } = string.Empty;
    public string IdUsuario { get; set; } = string.Empty;
    public string IdApp { get; set; } = string.Empty;
    public string IdDispositivo { get; set; } = string.Empty;
    public DateTime DataInstalacao { get; set; }
}

public class Avaliacao
{
    public string IdUsuario { get; set; } = string.Empty;
    public string IdApp { get; set; } = string.Empty;
    public int Nota { get; set; }
    public string Comentario { get; set; } = string.Empty;
}
