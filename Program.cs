using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace consoleapp
{
    public static class Program
    {
        static IReadOnlyDictionary<string, Ativo> Get(Func<IEnumerable<string>> func, TipoAtivo tipo)
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(dir, $"{tipo}.json");
            IEnumerable<string> ativos;

            if (File.Exists(path))
            {
                var content = new StreamReader(path).ReadToEnd();
                ativos = JsonSerializer.Deserialize<string[]>(content);
            }
            else
            {
                ativos = func();
                var content = JsonSerializer.Serialize(ativos);
                File.WriteAllText(path, content);
            }

            return ativos
                    .Select(x => new Ativo { Ticker = x, Tipo = tipo })
                    .ToDictionary(x => x.Ticker);
        }



        static async Task Main(string[] args)
        {
            var etfs = Get(Crawler.GetETFTickers, TipoAtivo.ETF);
            var fii = Get(Crawler.GetFIITickers, TipoAtivo.FII);
            var ops = ParserOperacao.ParseTSV(@"C:\Users\leandro\Desktop\se.txt");
            var grupos = ops.GroupBy(op => GetTipo(op.Ativo));

            foreach (var grupo in grupos)
            {
                Printt(grupo, grupo.Key.ToString());
            }

            Console.Read();

            TipoAtivo GetTipo(string ativo) =>
                ativo.TrimEnd('F') switch
                {
                    { } a when etfs.ContainsKey(a) => TipoAtivo.ETF,
                    { } a when fii.ContainsKey(a) => TipoAtivo.FII,
                    _ => TipoAtivo.Acao
                };
        }

        static void Printt(IEnumerable<Operacao> ops, string carteira)
        {
            var posicao = CalculaPosicao(ops);
            var (totalAplicado, per1) = CalculaPercent(posicao, x => x.FinanceiroCompra);

            var cotacao = Crawler.GetCotacao(posicao.Select(x => x.Ativo));
            var (patrimonio, per2) = CalculaPercent(posicao, x => x.Quantidade * cotacao[x.Ativo]);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n");
            Console.WriteLine($"{"carteira",20}: {carteira,10}");
            Console.WriteLine($"{"total aplicado",20}: {totalAplicado.ToString("C"),10}");
            Console.WriteLine($"{"total patrimonio",20}: {patrimonio.ToString("C"),10}");
            Console.WriteLine($"{"% rentabilidade",20}: {Regra3(totalAplicado, patrimonio),10}");
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
                    $"{Regra3(x.PrecoMedio, cotacao[x.Ativo]),10}\t" +
                    $"{per1[x.Ativo],10}\t" +
                    $"{per2[x.Ativo],10}");
            }
        }

        static (decimal total, IReadOnlyDictionary<string, decimal>) CalculaPercent(Posicao[] pos, Func<Posicao, decimal> selector)
        {
            var total = pos.Sum(selector);
            var dic = pos
                .ToDictionary(x => x.Ativo, x =>
                {
                    var ativoPercent = Regra3(total, 100, selector(x));
                    return Math.Round(ativoPercent, 2);
                });

            return (total, dic);
        }

        static string Regra3(decimal x, decimal y)
        {
            var d = Regra3(x, 100, y) - 100;
            var s = d > 0 ? "+" : "-";
            var abs = Math.Abs(d);
            abs = Math.Round(abs, 2);

            return s + abs;
        }

        static decimal Regra3(decimal x, decimal xPercent, decimal y) => (y * xPercent) / x;

        // https://www.controlacao.com.br/blog/como-e-calculado-o-preco-medio-da-sua-carteira
        static Posicao[] CalculaPosicao(IEnumerable<Operacao> operacoes)
        {
            var resultado = operacoes.GroupBy(x => x.Ativo, (ativo, ops) =>
            {
                var (medio, qtd) = ops.Aggregate((medio: 0M, qtd: 0), (acc, op) =>
                {
                    if (op.QuantidadeVenda > 0)
                        return (acc.medio, acc.qtd - op.QuantidadeVenda);

                    var x = acc.medio * acc.qtd;
                    var y = op.Preco * op.QuantidadeCompra;
                    var novaQtd = acc.qtd + op.QuantidadeCompra;
                    var novoMedio = (x + y) / novaQtd;

                    return (novoMedio, novaQtd);
                });

                return new Posicao
                {
                    Ativo = ativo,
                    Quantidade = qtd,
                    PrecoMedio = Math.Round(medio, 2)
                };
            })
            .Where(x => x.Quantidade != 0)
            .ToArray();

            return resultado;
        }
    }
}
