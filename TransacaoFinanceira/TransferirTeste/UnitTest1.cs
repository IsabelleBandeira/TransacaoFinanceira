using System.Diagnostics;
using TransacaoFinanceira;

namespace TransferirTeste;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestSaldoOrigemMaiorOuIgualQueValor()
    {
        //Deveria retornar true

        // Arrange
        int correlationId = 1;
        //string datetime = "07/06/2024 10:19:22";
        uint contaOrigem = 938485762;
        uint contaDestino = 2147483649;
        decimal valor = 50;
        decimal expectedSaldoOrigem = 130;
        decimal expectedSaldoDestino = 50;

        //Transacao transacao = new Transacao(correlationId, datetime, contaOrigem, contaDestino, valor);
        ExecutarTransacaoFinanceira executor = new ExecutarTransacaoFinanceira();
        AcessoDados acesso = new AcessoDados();

         // Act
        executor.Transferir(correlationId, contaOrigem, contaDestino, valor);

        // Assert
        ContasSaldo actualOrigem = acesso.GetSaldo<ContasSaldo>(contaOrigem);
        ContasSaldo actualDestino = acesso.GetSaldo<ContasSaldo>(contaDestino);
        Assert.AreEqual(expectedSaldoOrigem, actualOrigem.Saldo, "Account not debited correctly");
        Assert.AreEqual(expectedSaldoDestino, actualDestino.Saldo, "Account not debited correctly");
    }

    [TestMethod]
    public void  TestSaldoOrigemMenorQueValor() {
        //Deveria retornar falso

        // Arrange
        int correlationId = 1;
        //string datetime = "07/06/2024 10:19:22";
        uint contaOrigem = 938485762;
        uint contaDestino = 2147483649;
        decimal valor = 250;
        decimal expectedSaldoOrigem = -70;
        decimal expectedSaldoDestino = 250;

        //Transacao transacao = new Transacao(correlationId, datetime, contaOrigem, contaDestino, valor);
        ExecutarTransacaoFinanceira executor = new ExecutarTransacaoFinanceira();

         // Act
        executor.Transferir(correlationId, contaOrigem, contaDestino, valor);

        // Assert
        AcessoDados acesso = new AcessoDados();
        ContasSaldo actualOrigem = acesso.GetSaldo<ContasSaldo>(contaOrigem);
        ContasSaldo actualDestino = acesso.GetSaldo<ContasSaldo>(contaDestino);
        Assert.AreEqual(expectedSaldoOrigem, actualOrigem.Saldo, "Account not debited correctly");
        Assert.AreEqual(expectedSaldoDestino, actualDestino.Saldo, "Account not debited correctly");
    }
}