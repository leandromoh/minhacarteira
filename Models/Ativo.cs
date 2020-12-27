using System;

namespace consoleapp
{
    public struct Ativo
    {
        public string Ticker { get; set; }
        public TipoAtivo Tipo { get; set; }

        public static implicit operator string(Ativo x) => x.Ticker;
    }

    public enum TipoAtivo
    {
        Acao = 0,
        ETF = 1,
        BDR = 2,
        FII = 3,
        RendaFixa = 4,
        Poupanca = 5,
        Outro = 6,
    }
}
