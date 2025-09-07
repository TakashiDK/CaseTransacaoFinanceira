using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransacaoFinanceira.Application;

namespace TransacaoFinanceira.Infrastructure;


public sealed class SaidaConsole : IOutput
{
    public void WriteLine(string text) => Console.WriteLine(text);
}
