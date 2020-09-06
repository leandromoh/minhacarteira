using System.Collections.Generic;
using System.Linq;
using static consoleapp.Calc;

namespace consoleapp
{
    public struct Carteira
    {
        public string Nome;
        public decimal TotalAplicado;
        public decimal TotalPatrimonio;
        public IEnumerable<CarteiraAtivo> Ativos;

        public int QtdAtivos => Ativos.Count();
        public string Rentabilidade => Regra3Pretty(TotalAplicado, TotalPatrimonio); 
    }

    public struct CarteiraAtivo
    {
        public string Ativo;
        public decimal Aplicado;
        public decimal PrecoMedio;
        public int Quantidade;
        public decimal? Cotacao;
        public decimal Patrimonio;

        public string PercentRentab => Regra3Pretty(Aplicado, Patrimonio); 
        public decimal PercentValorAplicado;
        public decimal PercentValorPatrimonio;
    }
}