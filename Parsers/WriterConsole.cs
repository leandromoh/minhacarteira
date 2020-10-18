using System;
using System.Collections.Generic;
using System.Linq;

namespace consoleapp
{
    public static class WriterConsole
    {
        public static void ConsoleWrite(this IEnumerable<Carteira> carteiras)
        {
            foreach(var c in carteiras)
                c.ConsoleWrite();
        }

        public static void ConsoleWrite(this Carteira o)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n");
            Console.WriteLine($"{"carteira",20}: {o.Nome,11}");
            Console.WriteLine($"{"qtd ativos",20}: {o.QtdAtivos,11}");
            Console.WriteLine($"{"total aplicado",20}: {o.TotalAplicado.ToString("C"),11}");
            Console.WriteLine($"{"total patrimonio",20}: {o.TotalPatrimonio.ToString("C"),11}");
            Console.WriteLine($"{"% rentabilidade",20}: {o.Rentabilidade,11}");
            Console.WriteLine();
            Console.WriteLine(
                $"{"Ativo",10}\t" +
                $"{"Aplicado",10}\t" +
                $"{"Pre. Medio",10}\t" +
                $"{"Qtd",10}\t" +
                $"{"Cotacao",10}\t" +
                $"{"Patrimonio",10}\t" +
                $"{"% Rentab",10}\t" +
                $"{"% val aplicado",10}\t" +
                $"{"% patrimonio",10}" + 
                "\n");

            var colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
            colors = colors.Where(x => x != ConsoleColor.Black).ToArray();
            var i = 0;

            foreach (var p in o.Ativos)
            {
                Console.ForegroundColor = colors[++i % colors.Length];

                Console.WriteLine(
                    $"{p.Ativo,10}\t" +
                    $"{p.Aplicado,10}\t" +
                    $"{p.PrecoMedio,10}\t" +
                    $"{p.Quantidade,10}\t" +
                    $"{p.Cotacao,10}\t" +
                    $"{p.Patrimonio,10}\t" +
                    $"{p.PercentRentab,10}\t" +
                    $"{p.PercentValorAplicado,10}\t" +
                    $"{p.PercentValorPatrimonio,10}");
            }
        }
    }
}