using System;

namespace consoleapp
{
    public struct Operacao
    {
        public DateTime DtNegociacao;
        public int Conta;
        public string Ativo;
        public decimal Preco;
        public int QuantidadeCompra;
        public int QuantidadeVenda;
        public decimal FinanceiroCompra => Preco * QuantidadeCompra;
        public decimal FinanceiroVenda => Preco * QuantidadeVenda;

        public override string ToString() => Ativo;
    }
}