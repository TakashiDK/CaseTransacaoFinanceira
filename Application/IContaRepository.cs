using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransacaoFinanceira.Domain;

namespace TransacaoFinanceira.Application;

public interface IContaRepository
{
    Conta GetOrCreate(long numeroConta);
    IEnumerable<Conta> All();
}
