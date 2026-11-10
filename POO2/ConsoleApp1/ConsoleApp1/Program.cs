using NLog;

var logger = LogManager.GetCurrentClassLogger();

logger.Info("Início do programa");

try
{
    DemonstrarNiveisDeLog(logger);
    DemonstrarConversao(logger);
    DemonstrarConta(logger);
    DemonstrarTransferencia(logger);
}
catch (Exception ex)
{
    logger.Fatal(ex, "Falha crítica no fluxo principal");
}
finally
{
    logger.Info("Fim do programa");
    LogManager.Shutdown();
}

static void DemonstrarNiveisDeLog(Logger logger)
{
    logger.Trace("Detalhe técnico");
    logger.Debug("Valor da variável X: {0}", 10);
    logger.Info("Usuário logado");
    logger.Warn("Tentativa inválida registrada para demonstração");
    logger.Error("Erro de demonstração sem exceção");
    logger.Fatal("Falha crítica de demonstração registrada");
}

static void DemonstrarConversao(Logger logger)
{
    try
    {
        var numero = int.Parse("abc");
        logger.Info("Número convertido: {0}", numero);
    }
    catch (Exception ex)
    {
        logger.Error(ex, "Erro ao converter valor para inteiro");
    }
}

static void DemonstrarConta(Logger logger)
{
    var conta = new Conta(100);

    logger.Info("Saldo inicial da conta: {0}", conta.Saldo);

    try
    {
        logger.Info("Tentando sacar {0}", 200);
        conta.Sacar(200);
        logger.Info("Saque realizado. Saldo atual: {0}", conta.Saldo);
    }
    catch (InvalidOperationException ex)
    {
        logger.Error(ex, "Erro ao realizar saque por saldo insuficiente");
    }

    try
    {
        logger.Info("Tentando sacar valor inválido");
        conta.Sacar(-10);
    }
    catch (ArgumentException ex)
    {
        logger.Warn(ex, "Tentativa de saque com valor inválido");
    }

    logger.Info("Depositando {0}", 300);
    conta.Depositar(300);
    logger.Info("Depósito concluído. Saldo atual: {0}", conta.Saldo);

    logger.Info("Tentando sacar {0}", 150);
    conta.Sacar(150);
    logger.Info("Saque concluído. Saldo atual: {0}", conta.Saldo);
}

static void DemonstrarTransferencia(Logger logger)
{
    var origem = new Conta(500);
    var destino = new Conta(100);

    try
    {
        logger.Info("Iniciando transferência de {0} da conta origem para a conta destino", 200);
        Transferir(origem, destino, 200);
        logger.Info("Transferência concluída. Origem: {0}; destino: {1}", origem.Saldo, destino.Saldo);
    }
    catch (Exception ex)
    {
        logger.Error(ex, "Erro ao realizar transferência");
    }
}

static void Transferir(Conta origem, Conta destino, decimal valor)
{
    origem.Sacar(valor);
    destino.Depositar(valor);
}

public class Conta
{
    public decimal Saldo { get; private set; }

    public Conta(decimal saldoInicial)
    {
        if (saldoInicial < 0)
        {
            throw new ArgumentException("O saldo inicial não pode ser negativo.", nameof(saldoInicial));
        }

        Saldo = saldoInicial;
    }

    public void Depositar(decimal valor)
    {
        if (valor <= 0)
        {
            throw new ArgumentException("O valor do depósito deve ser maior que zero.", nameof(valor));
        }

        Saldo += valor;
    }

    public void Sacar(decimal valor)
    {
        if (valor <= 0)
        {
            throw new ArgumentException("O valor do saque deve ser maior que zero.", nameof(valor));
        }

        if (Saldo < valor)
        {
            throw new InvalidOperationException("Saldo insuficiente.");
        }

        Saldo -= valor;
    }
}
