using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransacaoFinanceira.Application;

public sealed class RetornoTransferencia
{
    public int CorrelationId { get; }
    public bool Sucesso { get; }
    public string Mensagem { get; }

    public RetornoTransferencia(int correlationId, bool sucesso, string mensagem)
    {
        CorrelationId = correlationId;
        Sucesso = sucesso;
        Mensagem = mensagem;
    }

};
