using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using TransacaoFinanceira.Application;
using TransacaoFinanceira.Domain;
using TransacaoFinanceira.Infrastructure;


//Obs: Voce é livre para implementar na linguagem de sua preferência, desde que respeite as funcionalidades e saídas existentes, além de aplicar os conceitos solicitados.

namespace TransacaoFinanceira;
public static class Program
{
    public static void Main(string[] args)
    { 
        var cultura = CultureInfo.GetCultureInfo("pt-BR");
    
    
        // Seed de contas existentes
        var seedAccounts = new[]
        {
            new Conta(938485762L, 180m),
            new Conta(347586970L, 1200m),
            new Conta(2147483649L, 0m),
            new Conta(675869708L, 4900m),
            new Conta(238596054L, 478m),
            new Conta(573659065L, 787m),
            new Conta(210385733L, 10m),
            new Conta(674038564L, 400m),
            new Conta(563856300L, 1200m)
        };
    
    
        // Dataset de transações (mantendo as saídas originais)
        var TRANSACOES = new[]
        {
            new { correlation_id = 1, datetime = "09/09/2023 14:15:00", conta_origem = 938485762L, conta_destino = 2147483649L, VALOR = 150m },
            new { correlation_id = 2, datetime = "09/09/2023 14:15:05", conta_origem = 2147483649L, conta_destino = 210385733L, VALOR = 149m },
            new { correlation_id = 3, datetime = "09/09/2023 14:15:29", conta_origem = 347586970L, conta_destino = 238596054L, VALOR = 1100m },
            new { correlation_id = 4, datetime = "09/09/2023 14:17:00", conta_origem = 675869708L, conta_destino = 210385733L, VALOR = 5300m },
            new { correlation_id = 5, datetime = "09/09/2023 14:18:00", conta_origem = 238596054L, conta_destino = 674038564L, VALOR = 1489m },
            new { correlation_id = 6, datetime = "09/09/2023 14:18:20", conta_origem = 573659065L, conta_destino = 563856300L, VALOR = 49m },
            new { correlation_id = 7, datetime = "09/09/2023 14:19:00", conta_origem = 938485762L, conta_destino = 2147483649L, VALOR = 44m },
            new { correlation_id = 8, datetime = "09/09/2023 14:19:01", conta_origem = 573659065L, conta_destino = 675869708L, VALOR = 150m },
        };
    
    
        // Ordena cronologicamente (e desempata por correlation_id) e processa SEQUENCIALMENTE
        var ordered = TRANSACOES
            .Select(t => new {
                t.correlation_id,
                t.conta_origem,
                t.conta_destino,
                t.VALOR,
                dt = DateTime.ParseExact(t.datetime, "dd/MM/yyyy HH:mm:ss", cultura)
            })
            .OrderBy(t => t.dt)
            .ThenBy(t => t.correlation_id)
            .ToList();
    
        IContaRepository repo = new InMemoryAccountRepository(seedAccounts);
        IOutput saida = new SaidaConsole();
        var service = new TransferenciaService(repo, saida);
    
    
        foreach (var t in ordered)
        {
            service.Process(t.correlation_id, t.conta_origem, t.conta_destino, t.VALOR);
        }
    
    
        //Mostrar saldos finais
        Console.WriteLine("\nSaldos finais:");
        foreach (var acc in repo.All().OrderBy(a => a.NumeroConta))
        {
            Console.WriteLine($"{acc.NumeroConta}: {acc.Saldo}");
        }
    }
}