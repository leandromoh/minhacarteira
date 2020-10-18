using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static consoleapp.Calc;

namespace consoleapp
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            var culture = new CultureInfo("pt-BR");
            var ops = ParserOperacao.ParseCSV(@"C:\Users\xxxxxx\Desktop\xxxxxx.txt", culture);
            var fileName = @$"C:\Users\xxxxxx\Desktop\carteira.{DateTime.Now:yyyy.MM.dd.HH.mm.ss}.html";

            var gruposAtivo = ops.GroupBy(op => GetTipoAtivo(op.Ativo));
            var carteiras = gruposAtivo
                .Select(g => Carteira(g, g.Key.ToString()))
                .ToArray();

            var carteiraRV = CarteiraMaster(carteiras);

            carteiras.Prepend(carteiraRV).SaveAsHTML(fileName);
        }

        public static readonly Func<string, TipoAtivo> GetTipoAtivo = BuildGetTipoAtivo();
        private static Func<string, TipoAtivo> BuildGetTipoAtivo()
        {
            var etfs = Cache.GetOrCreate(Crawler.GetETFTickers, TipoAtivo.ETF);
            var fii = Cache.GetOrCreate(Crawler.GetFIITickers, TipoAtivo.FII);

            return (ativo) => ativo.TrimEnd('F') switch
            {
                { } ticker when etfs.ContainsKey(ticker) => TipoAtivo.ETF,
                { } ticker when fii.ContainsKey(ticker) => TipoAtivo.FII,
                _ => TipoAtivo.Acao
            };
        }
    }
}
