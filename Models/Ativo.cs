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
        FII = 2,
        BDR = 3,
        Outro = 4,
    }
}
