using System;

namespace consoleapp
{
    public static class Extensions
    {
        static Extensions()
        {
            var etfs = Cache.GetOrCreate(Crawler.GetETFTickers, TipoAtivo.ETF);
            var fii = Cache.GetOrCreate(Crawler.GetFIITickers, TipoAtivo.FII);

            _getTipoAtivo = (ativo) => ativo.TrimEnd('F') switch
            {
                { } ticker when etfs.ContainsKey(ticker) => TipoAtivo.ETF,
                { } ticker when fii.ContainsKey(ticker) => TipoAtivo.FII,
                _ => TipoAtivo.Acao
            };
        }

        private static readonly Func<string, TipoAtivo> _getTipoAtivo;
        public static TipoAtivo GetTipoAtivo(this string ativo) => _getTipoAtivo(ativo);
    }
}
