using System;
using System.Collections.Generic;
using System.Linq;

namespace consoleapp
{
    public static partial class Calc
    {
        public static Posicao[] PosicaoAtivos(IEnumerable<Operacao> operacoes)
        {
            return operacoes.GroupBy(x => x.Ativo.TrimEnd('F'), (ativo, ops) =>
            {
                var (medio, qtd) = PrecoMedio(ops.OrderBy(x => x.DtNegociacao));

                return new Posicao
                {
                    Ativo = ativo,
                    Quantidade = qtd,
                    PrecoMedio = Math.Round(medio, 2)
                };
            })
            .Where(x => x.Quantidade != 0)
            .ToArray();
        }

        // https://www.controlacao.com.br/blog/como-e-calculado-o-preco-medio-da-sua-carteira
        public static (decimal pMedio, int qtd) PrecoMedio(IEnumerable<Operacao> operacoes)
        {
            return operacoes.Aggregate((medio: 0M, qtd: 0), (acc, op) =>
            {
                if (op.QuantidadeVenda > 0)
                    return (acc.medio, acc.qtd - op.QuantidadeVenda);

                var x = acc.medio * acc.qtd;
                var y = op.Preco * op.QuantidadeCompra;
                var novaQtd = acc.qtd + op.QuantidadeCompra;
                var novoMedio = (x + y) / novaQtd;

                return (novoMedio, novaQtd);
            });
        }
    }
}