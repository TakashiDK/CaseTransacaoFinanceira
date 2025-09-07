using System.Collections.Concurrent;
using System.Security.Principal;
using TransacaoFinanceira.Application;
using TransacaoFinanceira.Domain;
using TransacaoFinanceira.Infrastructure;
using Xunit;

public class TransferServiceTests
{
    private sealed class BufferOutput : IOutput
    {
        public readonly ConcurrentQueue<string> Lines = new();
        public void WriteLine(string text) => Lines.Enqueue(text);
    }

    [Fact]
    public void TransfereQuandoHaSaldo()
    {
        var repo = new InMemoryAccountRepository(new[] {
            new Conta(1, 100m), new Conta(2, 0m)
        });
        var outp = new BufferOutput();
        var svc = new TransferenciaService(repo, outp);

        var ret = svc.Process(1, 1, 2, 40m);

        Assert.True(ret.Sucesso);
        Assert.Contains("efetivada com sucesso", ret.Mensagem);
        Assert.Equal(60m, repo.GetOrCreate(1).Saldo);
        Assert.Equal(40m, repo.GetOrCreate(2).Saldo);
    }

    [Fact]
    public void CancelaQuandoSaldoInsuficiente()
    {
        var repo = new InMemoryAccountRepository(new[] {
            new Conta(1, 10m), new Conta(2, 0m)
        });
        var outp = new BufferOutput();
        var svc = new TransferenciaService(repo, outp);

        var ret = svc.Process(99, 1, 2, 50m);

        Assert.False(ret.Sucesso);
        Assert.Equal("Transacao numero 99 foi cancelada por falta de saldo", ret.Mensagem);
        Assert.Equal(10m, repo.GetOrCreate(1).Saldo);
        Assert.Equal(0m, repo.GetOrCreate(2).Saldo);
    }
}
