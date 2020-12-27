using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace consoleapp
{
    public static class Extensions
    {
        private static readonly Regex numberAtEnd;

        static Extensions()
        {
            numberAtEnd = new Regex(@"\d+$", RegexOptions.Compiled);

            var etfs = Cache.GetOrCreate(Crawler.GetETFTickers, TipoAtivo.ETF);
            var fii = Cache.GetOrCreate(Crawler.GetFIITickers, TipoAtivo.FII);

            var rendafixa = new[] 
            {
                "LCI", "LCA", "CDB", 
                "CRI", "CRA", "Debenture",
                "IPCA", "CDI", "SELIC", "Tesouro" 
            };

            var rendafixaRegex = rendafixa
                .Select(produto =>
                    new Regex($@"(^|\s){produto}(\s|$)", RegexOptions.IgnoreCase))
                .ToArray();

            var poupancaRegex = new Regex(@"(^|\s)poupan.a(\s|$)", RegexOptions.IgnoreCase);

            _getTipoAtivo = (ativo) =>
            {
                var ticker = ativo.TrimEnd('F');

                if (ticker.GetNumeroTicker() is int n)
                {
                    if (n >= 3 && n <= 6)
                        return TipoAtivo.Acao;

                    if (n >= 32 && n <= 35)
                        return TipoAtivo.BDR;
                }

                if (rendafixaRegex.Any(regexProduto => regexProduto.IsMatch(ticker)))
                    return TipoAtivo.RendaFixa;

                if (poupancaRegex.IsMatch(ticker))
                    return TipoAtivo.Poupanca;

                if (etfs.ContainsKey(ticker))
                    return TipoAtivo.ETF;

                if (fii.ContainsKey(ticker))
                    return TipoAtivo.FII;

                return TipoAtivo.Outro;
            };
        }

        private static readonly Func<string, TipoAtivo> _getTipoAtivo;
        public static TipoAtivo GetTipoAtivo(this string ativo) => _getTipoAtivo(ativo);

        public static int? GetNumeroTicker(this string ativo) =>
            numberAtEnd.Match(ativo) is var match && match.Success
            ? int.Parse(match.Value)
            : (int?)null;
    }
}
