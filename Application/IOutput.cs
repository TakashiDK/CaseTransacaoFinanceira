using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransacaoFinanceira.Application;

public interface IOutput
{
    void WriteLine(string message);
}
