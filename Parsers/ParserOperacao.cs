using System;
using System.Linq;
using System.IO;
using System.Globalization;

namespace consoleapp
{
    public static class ParserOperacao
    {
        public static Operacao[] ParseCSV(string filePath, CultureInfo culture)
        {
            var lines = File.ReadAllLines(filePath);

            var ops = lines
                        .Skip(1) // header
                        .TakeWhile(s => !string.IsNullOrWhiteSpace(s))
                        .Select(line => ParseLine(line, culture))
                        .OrderBy(x => x.Ativo)
                        .ToArray();

            return ops;
        }

        public static Operacao ParseLine(string line, CultureInfo culture)
        {
            var columns = line.Replace(';', '\t').Split("\t");
            var op = new Operacao();

            op.DtNegociacao = DateTime.Parse(columns[0], culture);
            op.Conta = int.Parse(columns[1], culture);
            op.Ativo = columns[2].Replace("\"", string.Empty).ToUpper();
            op.Preco = decimal.Parse(columns[3], culture);
            op.QuantidadeCompra = int.Parse(columns[4], culture);
            op.QuantidadeVenda = int.Parse(columns[5], culture);

            if (op.QuantidadeCompra > 0
             && op.QuantidadeVenda > 0)
            {
                throw new InvalidOperationException("não pode comprar e vender na mesma operação");
            }

            return op;
        }
    }
}