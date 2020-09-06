namespace consoleapp
{
    public struct Posicao
    {
        public string Ativo;
        public decimal PrecoMedio;
        public int Quantidade;

        public decimal FinanceiroCompra => PrecoMedio * Quantidade;

        public override string ToString() => $"{Quantidade} {Ativo} {PrecoMedio}";
    }
}