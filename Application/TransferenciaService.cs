using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransacaoFinanceira.Domain;

namespace TransacaoFinanceira.Application;

public sealed class TransferenciaService
{
    private readonly IContaRepository _repo;
    private readonly IOutput _saida;
    
    public TransferenciaService(IContaRepository repo, IOutput saida)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _saida = saida ?? throw new ArgumentNullException(nameof(saida));
    }
    
    public RetornoTransferencia Process(int correlationId, long contaOrigem, long contaDestino, decimal valor)
    {
        // Busca/cria contas
        var origem = _repo.GetOrCreate(contaOrigem);
        var destino = _repo.GetOrCreate(contaDestino);
    
    
        if (!origem.Debitar(valor))
        {
            var mensagemCancela = $"Transacao numero {correlationId} foi cancelada por falta de saldo";
            _saida.WriteLine(mensagemCancela);
            return new RetornoTransferencia(correlationId, false, mensagemCancela);
        }
    
    
        destino.Creditar(valor);
    
    
        var mensagemSucesso = $"Transacao numero {correlationId} foi efetivada com sucesso! Novos saldos: Conta Origem:{origem.Saldo} | Conta Destino:{destino.Saldo}";
        _saida.WriteLine(mensagemSucesso);
        return new RetornoTransferencia(correlationId, true, mensagemSucesso);
    }
}
