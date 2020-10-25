using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace consoleapp
{
    public static class Crawler
    {
        private static int TimeoutInSeconds { get; }

        static Crawler()
        {
            TimeoutInSeconds = int.Parse(Program.Configuration["crawler:timeoutInSeconds"]);
        }

        public static IEnumerable<string> GetFIITickers()
        {
            var address = $"http://www.b3.com.br/pt_br/produtos-e-servicos/negociacao/renda-variavel/fundos-de-investimentos/fii/fiis-listados/";
            var selector = "#ctl00_contentPlaceHolderConteudo_divResultado table tr td:last-child";

            using IWebDriver driver = GetDriver();
            driver.Navigate().GoToUrl(address);

            var iframe = driver.SwitchTo().Frame("bvmf_iframe");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TimeoutInSeconds));

            wait.Until(d => d.FindElements(By.CssSelector(selector)).Count > 0);

            var ativos = iframe.FindElements(By.CssSelector(selector));

            return ativos.Select(x => x.Text + "11").ToArray();
        }

        public static IEnumerable<string> GetETFTickers()
        {
            var address = $"http://www.b3.com.br/pt_br/produtos-e-servicos/negociacao/renda-variavel/etf/renda-variavel/etfs-listados/";
            var selector = "#ctl00_contentPlaceHolderConteudo_etf_pgvETFsRendaVariavel table tr td:last-child";

            using IWebDriver driver = GetDriver();
            driver.Navigate().GoToUrl(address);

            var iframe = driver.SwitchTo().Frame("bvmf_iframe");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TimeoutInSeconds));

            wait.Until(d => d.FindElements(By.CssSelector(selector)).Count > 0);

            var ativos = iframe.FindElements(By.CssSelector(selector));

            return ativos.Select(x => x.Text + "11").ToArray();
        }

        public static IReadOnlyDictionary<string, decimal> GetCotacao(IEnumerable<string> ativos)
        {
            var cellSelector = "div[eid] div[data-ved] span[jscontroller] span[jsname]";

            using IWebDriver driver = GetDriver();
            var dic = new Dictionary<string, decimal>();

            foreach (var ativo in ativos)
            {
                var address = $"http://www.google.com/search?q={ativo}";
                driver.Navigate().GoToUrl(address);

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TimeoutInSeconds));
                var spanCotacao = wait.Until(d => d.FindElement(By.CssSelector(cellSelector)));
                var cotacao = decimal.Parse(spanCotacao.Text.Replace(",", "."));

                dic[ativo] = cotacao;
            }
            return dic;
        }

        public static IWebDriver GetDriver()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var service = ChromeDriverService.CreateDefaultService(path);
            service.HideCommandPromptWindow = true;  

            return new ChromeDriver(service);
        }
    }
}