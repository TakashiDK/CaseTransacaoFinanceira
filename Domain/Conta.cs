using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransacaoFinanceira.Domain;

public sealed class Conta
{
    public long NumeroConta { get; }
    public decimal Saldo { get; private set; }


    public Conta(long numeroConta, decimal saldo)
    {
        NumeroConta = numeroConta;
        Saldo = saldo;
    }


    public bool Debitar(decimal valor)
    {
        if (valor <= 0m) return false;
        if (Saldo < valor) return false;
        Saldo -= valor;
        return true;
    }


    public void Creditar(decimal valor)
    {
        if (valor <= 0m) return; // ignorar créditos inválidos
        Saldo += valor;
    }
}
