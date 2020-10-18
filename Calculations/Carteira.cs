using System;
using System.Collections.Generic;
using System.Linq;

namespace consoleapp
{
    public static partial class Calc
    {
        public static Carteira CarteiraMaster(IEnumerable<Carteira> carteiras, string nomeCarteira)
        {
            var aplicadoMaster = carteiras.Sum(x => x.TotalAplicado);
            var patrimonioMaster = carteiras.Sum(x => x.TotalPatrimonio);

            var ativos = carteiras.Select(c => 
            {
                var qtd = c.Ativos.Sum(x => x.Quantidade);

                return new CarteiraAtivo
                {
                    Ativo = c.Nome,
                    Aplicado = c.TotalAplicado,
                    Quantidade = qtd,
                    PrecoMedio = Math.Round(c.TotalAplicado / qtd, 2),
                    Cotacao = null,
                    Patrimonio = c.TotalPatrimonio,
                    PercentValorAplicado = Regra3(aplicadoMaster, c.TotalAplicado),
                    PercentValorPatrimonio = Regra3(patrimonioMaster, c.TotalPatrimonio),
                };
            })
            .ToArray();

            return new Carteira
            {
                Nome = nomeCarteira,
                TotalAplicado = aplicadoMaster,
                TotalPatrimonio = patrimonioMaster,
                Ativos = ativos
            };
        }

        public static Carteira Carteira(IEnumerable<Operacao> ops, string carteira)
        {
            var posicao = PosicaoAtivos(ops);
            var (totalAplicado, per1) = CalculaPercent(posicao, x => x.FinanceiroCompra);

            var cotacao = Crawler.GetCotacao(posicao.Select(x => x.Ativo));
            var (patrimonio, per2) = CalculaPercent(posicao, x => x.Quantidade * cotacao[x.Ativo]);

            var ativos = posicao.Select(x => new CarteiraAtivo
            {
                Ativo = x.Ativo,
                Aplicado = x.FinanceiroCompra,
                PrecoMedio = x.PrecoMedio,
                Quantidade = x.Quantidade,
                Cotacao = cotacao[x.Ativo],
                Patrimonio = x.Quantidade * cotacao[x.Ativo],
                PercentValorAplicado = per1[x.Ativo],
                PercentValorPatrimonio = per2[x.Ativo]
            })
            .ToArray();

            return new Carteira
            {
                Nome = carteira,
                TotalAplicado = totalAplicado,
                TotalPatrimonio = patrimonio,
                Ativos = ativos,
            };
        }
    }
}