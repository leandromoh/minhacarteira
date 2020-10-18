using System;
using System.Collections.Generic;
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
                    {GetStyle()}
                </head>
                <body>
                    {string.Join("\n", carteiras.Select(GetTable))}
                </body>
            </html>
            ";
        }

        private static string GetStyle()
        {
            return @"
                <style>
                    hr {
                        margin-bottom: 5%; 
                    }

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
                        <td>{(o.TotalPatrimonio - o.TotalAplicado):C}</td>
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
                </table> <hr/>";
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