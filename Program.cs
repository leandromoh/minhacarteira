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

            var gruposAtivo = ops.GroupBy(op => op.Ativo.GetTipoAtivo());
            var carteiras = gruposAtivo
                .Select(g => Carteira(g, g.Key.ToString()))
                .ToArray();

            var carteiraRV = CarteiraMaster(carteiras, "RV");

            carteiras.Prepend(carteiraRV).SaveAsHTML(fileName);
        }
    }
}
