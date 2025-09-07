using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TransacaoFinanceira.Application;
using TransacaoFinanceira.Domain;

namespace TransacaoFinanceira.Infrastructure;


public sealed class InMemoryAccountRepository : IContaRepository
{
    private readonly ConcurrentDictionary<long, Conta> _contas = new();


    public InMemoryAccountRepository(IEnumerable<Conta> seed)
    {
        foreach (var a in seed)
            _contas[a.NumeroConta] = a;
    }


    public Conta GetOrCreate(long number)
    {
        return _contas.GetOrAdd(number, n => new Conta(n, 0m));
    }


    public IEnumerable<Conta> All() => _contas.Values;
}
