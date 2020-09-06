using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static consoleapp.Calc;

namespace consoleapp
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            var ops = ParserOperacao.ParseTSV(@"C:\Users\leandro\Desktop\se.txt");
            var gruposAtivo = ops.GroupBy(op => GetTipoAtivo(op.Ativo));

            foreach (var grupo in gruposAtivo)
            {
                Printt(grupo, grupo.Key.ToString());
            }

            Console.Read();
        }

        static void Printt(IEnumerable<Operacao> ops, string carteira)
        {
            var posicao = PosicaoAtivos(ops);
            var (totalAplicado, per1) = CalculaPercent(posicao, x => x.FinanceiroCompra);

            var cotacao = Crawler.GetCotacao(posicao.Select(x => x.Ativo));
            var (patrimonio, per2) = CalculaPercent(posicao, x => x.Quantidade * cotacao[x.Ativo]);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n");
            Console.WriteLine($"{"carteira",20}: {carteira,10}");
            Console.WriteLine($"{"qtd ativos",20}: {posicao.Length,10}");
            Console.WriteLine($"{"total aplicado",20}: {totalAplicado.ToString("C"),10}");
            Console.WriteLine($"{"total patrimonio",20}: {patrimonio.ToString("C"),10}");
            Console.WriteLine($"{"% rentabilidade",20}: {Regra3Pretty(totalAplicado, patrimonio),10}");
            Console.WriteLine();

            var colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
            colors = colors.Where(x => x != ConsoleColor.Black).ToArray();
            var i = 0;

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

            foreach (var x in posicao)
            {
                Console.ForegroundColor = colors[++i % colors.Length];

                Console.WriteLine(
                    $"{x.Ativo,10}\t" +
                    $"{x.FinanceiroCompra,10}\t" +
                    $"{x.PrecoMedio,10}\t" +
                    $"{x.Quantidade,10}\t" +
                    $"{cotacao[x.Ativo],10}\t" +
                    $"{x.Quantidade * cotacao[x.Ativo],10}\t" +
                    $"{Regra3Pretty(x.PrecoMedio, cotacao[x.Ativo]),10}\t" +
                    $"{per1[x.Ativo],10}\t" +
                    $"{per2[x.Ativo],10}");
            }
        }

        public static TipoAtivo GetTipoAtivo(string ativo)
        {
            var etfs = Cache.GetOrCreate(Crawler.GetETFTickers, TipoAtivo.ETF);
            var fii = Cache.GetOrCreate(Crawler.GetFIITickers, TipoAtivo.FII);

            return ativo.TrimEnd('F') switch
            {
                { } ticker when etfs.ContainsKey(ticker) => TipoAtivo.ETF,
                { } ticker when fii.ContainsKey(ticker) => TipoAtivo.FII,
                _ => TipoAtivo.Acao
            };
        }
    }
}
