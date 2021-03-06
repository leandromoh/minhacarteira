using System;
using System.Collections.Generic;
using System.Linq;

namespace consoleapp
{
    public static partial class Calc
    {
        public static (decimal total, IReadOnlyDictionary<string, decimal>) CalculaPercent(
            IEnumerable<Posicao> pos, Func<Posicao, decimal> selector)
        {
            var total = pos.Sum(selector);
            var dic = pos
                .ToDictionary(x => x.Ativo, x =>
                {
                    var ativoPercent = Regra3(total, selector(x));
                    return Math.Round(ativoPercent, 2);
                });

            return (total, dic);
        }

        public static string Regra3Pretty(decimal x, decimal y, decimal xPercent = 100)
        {
            var d = Regra3(x, y, xPercent) - xPercent;
            var s = d > 0 ? "+" : "-";
            var abs = Math.Abs(d);

            return s + abs;
        }

        public static decimal Regra3(decimal x, decimal y, decimal xPercent = 100) =>
            Math.Round((y * xPercent) / x, 2);
    }
}