<div align="center">
  <h1 align="center">WebCrawl</h1>

   <div align="center">
     É um programa de computador que navega pela internet de forma automatizada, coletando informações de páginas da web.
    </div>
</div>

## 📋 <a name="table">Sumary</a>

1. 🤖 [Introdução](#introduction)
2. ⚙️ [Tech Stack](#tech-stack)
3. 🔋 [Recursos](#features)
4. 🤸 [Como usar](#quick-start)
5. 💾 [Variáveis de Ambiente](#envs)
6. 📅 [Versões](#versions)
7. 🤝 [Contribuições](#contributing)
8. 👥 [Autores](#authors)
<br>

## <a name="introduction">🤖 Introdução</a>

Começo da jornada: Um web crawler inicia sua jornada a partir de uma lista de URLs, que são os endereços das páginas da web.

Seguindo os links: Ele acessa essas páginas e identifica todos os links presentes nelas.

Exploração contínua: Em seguida, o crawler segue esses links, explorando novas páginas e repetindo o processo.

Coleta de informações: Ao visitar cada página, o web crawler extrai informações relevantes, como texto, imagens, vídeos e outros tipos de conteúdo.

Indexação dos dados: As informações coletadas são organizadas e armazenadas em um banco de dados, formando um índice que facilita a busca e o acesso aos dados.

<br>

**Para que servem os web crawlers**:
___
+ Mecanismos de busca: Os mecanismos de busca, como o Google, utilizam web crawlers para indexar a internet e construir seus índices de pesquisa.
+ Monitoramento de sites: Web crawlers podem ser usados para monitorar sites em busca de alterações, como novas publicações ou atualizações de conteúdo.
+ Coleta de dados: Empresas e pesquisadores podem utilizar web crawlers para coletar dados da web para análise, estudos de mercado e outras finalidades.
+ Comparação de preços: Sites de comparação de preços utilizam web crawlers para coletar informações sobre produtos e preços em diferentes lojas online.


<br>

**Outros nomes para Webcrawler**:
____
+ Crawler
+ Spider
+ Bot
+ Web robot
+ Indexador automático

  <br>

## <a name="tech-stack">⚙️ Tech Stack</a>

- dotnet 9.0
- Sqlite
  
<br>

## <a name="quick-start">🤸 Como usar</a>

Para iniciar o projeto, siga os seguintes passos em seu dispositivo:

**00 - Pré-requisitos**

Para usar este projeto você deve ter instalado previamente os seguintes pacotes:

- [Git](https://git-scm.com/)
- [dotnet 9.0](https://dotnet.microsoft.com/pt-br/download/dotnet/9.0)
- [Chromium](https://www.chromium.org/getting-involved/download-chromium/)
- [Sqlite](https://www.sqlite.org/download.html) (opcional para visualizar a tabela)
  <br/><br/>

**01 - Clonar o Repositório**

```bash
git clone https://github.com/felipecrovesy/WebCrawler.git
```

**02 - Rodar o Projeto**

```bash
dotnet run WebCrawler.Presentation
```

**03 - Rodar os Tests**

Acesse WebCrawler.UnitTest via terminal

```bash
dotnet test
```

## <a name="constributing">🤝 Contribuições</a>

Contriguições, issues, e novos recursos são vem vindos!

1. Faça um Fork do projeto (<https://github.com/yourname/yourproject/fork>)
2. Crie a sua branch de feature (`git checkout -b feature/fooBar`)
3. Commit suas alterações (`git commit -am 'Add some fooBar'`)
4. Faça um Push para a branch (`git push origin feature/fooBar`)
5. Crie um novo Pull Request


## <a name="authors">👥 Autores</a>

<table style="border-collapse: collapse; table-layout: auto; text-align: left;">

  <tbody>
    <tr>
      <td style="padding: 10px; border: 1px solid #ddd;">
        <img src="https://avatars.githubusercontent.com/u/60819196?v=4" width="60" style="border-radius: 50%; display: block; margin: 0 auto;">
      </td>
      <td style="padding: 10px; border: 1px solid #ddd;">Felipe Crovesy</td>
      <td style="padding: 10px; border: 1px solid #ddd;">
        <a href="https://www.linkedin.com/in/felipe-crovesy-6a299283/" target="_blank">LinkedIn</a> |
        <a href="https://github.com/felipecrovesy" target="_blank">GitHub</a>
      </td>
    </tr>
  </tbody>
</table>
