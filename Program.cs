using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using static consoleapp.Calc;

namespace consoleapp
{
    public static class Program
    {
        public static IConfigurationRoot Configuration { get; }

        static Program()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            // https://stackoverflow.com/a/11696363
            // https://docs.microsoft.com/pt-br/dotnet/api/system.globalization.numberformatinfo.currencynegativepattern
            var culture = (CultureInfo) CultureInfo.CurrentCulture.Clone();
            culture.NumberFormat.CurrencyNegativePattern = 1;
            CultureInfo.CurrentCulture = culture;
        }

        static void Main(string[] args)
        {
            var culture = new CultureInfo("pt-BR");
            var operationsFilePath = Configuration["input:operationsFilePath"];
            var reportDirectoryPath = Configuration["output:reportDirectoryPath"];
            var reportFilePath =  Path.Combine(reportDirectoryPath, 
                                        $"minhacarteira.{DateTime.Now:yyyy.MM.dd.HH.mm.ss}.html");

            var ops = ParserOperacao.ParseCSV(operationsFilePath, culture);

            var gruposAtivo = ops.GroupBy(op => op.Ativo.GetTipoAtivo());
            var carteiras = gruposAtivo
                .Select(g => Carteira(g, g.Key.ToString()))
                .ToArray();

            var carteiraRV = CarteiraMaster(carteiras, "RV");

            carteiras.Prepend(carteiraRV).SaveAsHTML(reportFilePath);
        }
    }
}
