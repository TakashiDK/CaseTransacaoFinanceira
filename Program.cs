using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


//Obs: Voce é livre para implementar na linguagem de sua preferência, desde que respeite as funcionalidades e saídas existentes, além de aplicar os conceitos solicitados.

namespace TransacaoFinanceira
{
    class Program
    {

        static void Main(string[] args)
        {
            var cultura = CultureInfo.GetCultureInfo("pt-BR");

            var TRANSACOES = new[] { 
                new {correlation_id= 1,datetime="09/09/2023 14:15:00", conta_origem= 938485762L, conta_destino= 2147483649L, VALOR= 150m},
                new {correlation_id= 2,datetime="09/09/2023 14:15:05", conta_origem= 2147483649L, conta_destino= 210385733L, VALOR= 149m},
                new {correlation_id= 3,datetime="09/09/2023 14:15:29", conta_origem= 347586970L, conta_destino= 238596054L, VALOR= 1100m},
                new {correlation_id= 4,datetime="09/09/2023 14:17:00", conta_origem= 675869708L, conta_destino= 210385733L, VALOR= 5300m},
                new {correlation_id= 5,datetime="09/09/2023 14:18:00", conta_origem= 238596054L, conta_destino= 674038564L, VALOR= 1489m},
                new {correlation_id= 6,datetime="09/09/2023 14:18:20", conta_origem= 573659065L, conta_destino= 563856300L, VALOR= 49m},
                new {correlation_id= 7,datetime="09/09/2023 14:19:00", conta_origem= 938485762L, conta_destino= 2147483649L, VALOR= 44m},
                new {correlation_id= 8,datetime="09/09/2023 14:19:01", conta_origem= 573659065L, conta_destino= 675869708L, VALOR= 150m},
            };

            var ordem = TRANSACOES
                .Select(t => new
                {
                    t.correlation_id,
                    t.conta_origem,
                    t.conta_destino,
                    t.VALOR,
                    dt = DateTime.ParseExact(t.datetime, "dd/MM/yyyy HH:mm:ss", cultura)
                })
                .OrderBy(t => t.dt)
                .ThenBy(t => t.correlation_id)
                .ToList();

            var executor = new executarTransacaoFinanceira();

            foreach (var t in ordem)
            {
                executor.transferir(t.correlation_id, t.conta_origem, t.conta_destino, t.VALOR);
            }

            Console.WriteLine("\nSaldos finais:");
            foreach (var s in executor.ListarSaldosOrdenados())
            {
                Console.WriteLine($"{s.conta}: {s.saldo}");
            }
        }
    }

    class executarTransacaoFinanceira: acessoDados
    {
        public void transferir(int correlation_id, long conta_origem, long conta_destino, decimal valor)
        {
            var conta_saldo_origem = getSaldo(conta_origem);

            if (conta_saldo_origem.saldo < valor)
            {
                Console.WriteLine("Transacao numero {0} foi cancelada por falta de saldo", correlation_id);
                return;
            }

            var conta_saldo_destino = getSaldo(conta_destino);

            conta_saldo_origem.saldo -= valor;
            conta_saldo_destino.saldo += valor;

            Console.WriteLine("Transação numero {0} foi efetivada com sucesso! Novos saldos: Conta Origem:{1} | Conta Destino: {2}",correlation_id, conta_saldo_origem.saldo, conta_saldo_destino.saldo);
        }
    }
    class contas_saldo
    {
        public contas_saldo(long conta, decimal valor)
        {
            this.conta = conta;
            this.saldo = valor;
        }
        public long conta { get; set; }
        public decimal saldo { get; set; }
    }
    class acessoDados
    {
        private readonly List<contas_saldo> TABELA_SALDOS;

        public acessoDados()
        {
            TABELA_SALDOS = new List<contas_saldo>
            {
                new contas_saldo(938485762L, 180m),
                new contas_saldo(347586970L, 1200m),
                new contas_saldo(2147483649L, 0m),
                new contas_saldo(675869708L, 4900m),
                new contas_saldo(238596054L, 478m),
                new contas_saldo(573659065L, 787m),
                new contas_saldo(210385733L, 10m),
                new contas_saldo(674038564L, 400m),
                new contas_saldo(563856300L, 1200m)
            };
        }
        public contas_saldo getSaldo(long id)
        {
            var saldo = TABELA_SALDOS.Find(x => x.conta == id);
            if (saldo == null)
            {
                saldo = new contas_saldo(id, 0m);
                TABELA_SALDOS.Add(saldo);
            }
            return saldo;
        }

        public IEnumerable<contas_saldo> ListarSaldosOrdenados()
            => TABELA_SALDOS.OrderBy(x => x.conta);
    }
}
