# minhacarteira

Calcula posição e dados da carteira com base em um arquivo [TSV](https://en.wikipedia.org/wiki/Tab-separated_values) contendo operacoes realizadas (compra e venda).  

Agrupa as operações em carteiras com base no tipo do ativo (FII, ETF, Ação) e para cada uma dessas carteira calcula-se:
- quantidade de cada ativo
- preco medio de cada ativo
- valor aplicado em cada ativo (quantidade x preco medio)
- valor patrimonial de cada ativo (quantidade x cotação)
- rentabilidade de cada ativo (valor aplicado x valor patrimonial)
- total aplicado da carteira (soma do valor aplicado de cada ativo)
- total patrimonial da carteira (soma do valor patrimonial de cada ativo)
- porcentagem do valor aplicado de cada ativo em relação ao total aplicado da carteira
- porcentagem do valor patrimonial de cada ativo em relação ao total patrimonial da carteira

Por ultimo apresenta o consolidado de cada carteira na forma de carteira de renda variavel, apresentando:
- total aplicado
- total patrimonial 
- rentabilidade total (total aplicado x total patrimonial)
- quantidade de papeis em cada carteira
- porcentagem do valor aplicado de cada carteira em relação ao total aplicado
- porcentagem do valor patrimonial de cada carteira em relação ao total patrimonial


**Disclaimer: O aplicativo não é um consultor de investimentos. Os dados e as informações não constituem uma recomendação para comprar ou vender títulos financeiros.  O aplicativo não faz declarações sobre a conveniência ou a adequação de qualquer investimento. Todos os dados e informações são fornecidos "no estado em que se encontram" sem qualquer tipo de garantia, somente para fins de informação pessoal, e não de negociações ou recomendações. Consulte seu agente ou representante financeiro para verificar os preços antes de executar qualquer negociação.**
