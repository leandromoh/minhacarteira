using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace consoleapp
{
    public static class WriterHTML
    {
        public static void SaveAsHTML(this IEnumerable<Carteira> carteiras, string destinationPath)
        {
            var pageTitle = Path.GetFileNameWithoutExtension(destinationPath);
            var pageContent = carteiras.GetHTML(pageTitle);

            File.WriteAllText(destinationPath, pageContent);
        }

        private static string GetHTML(this IEnumerable<Carteira> carteiras, string pageTitle = "Page Title")
        {
            return $@"
            <!DOCTYPE html>
            <html>
                <head>
                    <title>{pageTitle}</title>
                    <script src=""https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.2/Chart.min.js""></script>
                    <script src=""https://cdn.jsdelivr.net/npm/chartjs-plugin-datalabels@0.4.0/dist/chartjs-plugin-datalabels.min.js""></script>
                    {GetStyle()}
                </head>
                <body>
                    {string.Join("\n", carteiras.Select(GetTable))}
                </body>
            </html>
            ";
        }

        private static string GetChart(Carteira o)
        {
            var id = Guid.NewGuid();
            var culture = CultureInfo.InvariantCulture;
            var values = string.Join(", ", o.Ativos.Select(x => x.PercentValorPatrimonio.ToString(culture)));
            var names = string.Join(", ", o.Ativos.Select(x => $"'{x.Ativo}'"));

            return $@" 
            <div class=""chart-container"">
                <canvas id=""{id}""></canvas>
            </div>
            <script>
            (function(){{
                var ctx = document.getElementById('{id}').getContext('2d');
                var chart = new Chart(ctx, {{
                    type: 'pie',
                    data: {{
                        datasets: [{{
                            label: 'Colors',
                            data: [{values}],
                            backgroundColor: [
                                '#0074D9', '#FF4136', '#2ECC40', '#FF851B', 
                                '#7FDBFF', '#B10DC9', '#FFDC00', '#001f3f', 
                                '#39CCCC', '#01FF70', '#85144b', '#F012BE', 
                                '#3D9970', '#111111', '#AAAAAA']
                        }}],
                        labels: [{names}]
                    }},
                    options: {{
                        responsive:true,
                        title:{{
                            display: true,
                            text: ""% Patrimonio""
                        }},
                        plugins: {{
                            datalabels: {{
                                formatter: (value, ctx) =>  value + '%',
                                color: '#fff',
                            }}
                        }}
                    }}
                }});
            }})();
            </script>
            ";
        }

        private static string GetStyle()
        {
            return @"
                <style>
                    table {
                        font-family: Arial, Helvetica, sans-serif;
                        border-collapse: collapse;
                        width: 100%;
                    }

                    td, th {
                        border: 1px solid #ddd;
                        padding: 8px;
                    }

                    tr:nth-child(even){
                        background-color: #f2f2f2;
                    }

                    tr:hover {
                        background-color: #ddd;
                    }

                    th {
                        padding-top: 12px;
                        padding-bottom: 12px;
                        text-align: left;
                        background-color: #4CAF50;
                        color: white;
                    }

                    .chart-container {
                        margin-left: 10%;
                        width: 80%;
                        margin-bottom: 5%; 
                    }
                </style>
                ";
        }

        private static string GetSummaryTable(Carteira o)
        {
            return $@"
                <table>
                    <tr>
                        <th>Carteira</th>
                        <th>Qtd Ativos</th>
                        <th>Total Aplicado</th>
                        <th>Total Patrimonio</th>
                        <th>Total Lucro</th>
                        <th>% Rentabilidade</th>
                    </tr>
                    <tr>
                        <td>{o.Nome}</td>
                        <td>{o.QtdAtivos}</td>
                        <td>{o.TotalAplicado:C}</td>
                        <td>{o.TotalPatrimonio:C}</td>
                        <td>{o.TotalLucro:C}</td>
                        <td>{o.Rentabilidade}</td>
                    </tr>
                </table>";
        }

        private static string GetTable(Carteira o)
        {
            return GetSummaryTable(o) + $@"
                <table>
                    <tr>
                        <th>Ativo</th>
                        <th>Aplicado</th>
                        <th>Pre. Medio</th>
                        <th>Qtd</th>
                        <th>Cotacao</th>
                        <th>Patrimonio</th>
                        <th>% Rentab</th>
                        <th>% val aplicado</th>
                        <th>% patrimonio</th>
                    </tr>
                    {string.Join("\n", o.Ativos.Select(GetRow))}
                </table> {GetChart(o)}";
        }

        private static string GetRow(CarteiraAtivo p)
        {
            return $@"
                <tr>
                    <td>{p.Ativo}</td>
                    <td>{p.Aplicado:C}</td>
                    <td>{p.PrecoMedio:C}</td>
                    <td>{p.Quantidade}</td>
                    <td>{p.Cotacao:C}</td>
                    <td>{p.Patrimonio:C}</td>
                    <td>{p.PercentRentab}</td>
                    <td>{p.PercentValorAplicado}</td>
                    <td>{p.PercentValorPatrimonio}</td>
                </tr>
                ";
        }
    }
}