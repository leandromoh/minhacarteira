using System;
using System.Linq;
using System.IO;

namespace consoleapp
{
    public static class ParserOperacao
    {
        public static Operacao[] ParseTSV(string filePath)
        {
            var lines = File.ReadAllLines(filePath);

            var ops = lines
                        .Skip(1) // header
                        .TakeWhile(s => !string.IsNullOrWhiteSpace(s))
                        .Select(ParseLine)
                        .OrderBy(x => x.Ativo)
                        .ToArray();

            return ops;
        }

        public static Operacao ParseLine(string line)
        {
            var columns = line.Split("\t");
            var op = new Operacao();

            op.DtNegociacao = DateTime.ParseExact(columns[0], "d/M/yyyy", null);
            op.Conta = int.Parse(columns[1]);
            op.Ativo = columns[2];
            op.Preco = decimal.Parse(columns[3]);
            op.QuantidadeCompra = int.Parse(columns[4]);
            op.QuantidadeVenda = int.Parse(columns[5]);

            return op;
        }
    }
}