# Distribuidor de Lucros

Este software foi desenvolvido utilizando .NET Core e é responsável por calcular e repartir os lucros da empresa entre os funcionarios, com base em uma regra pre-estabelecida a qual define pesos por área, tempo de empresa e faixa salarial, de forma a determinar quanto cada funcionario, previamente cadastrado, deve receber. 

A partir deste software, é possível cadastrar e descadastrar os funcionários e em conjunto com a informação do montante disponível para repartir entre os funcionários, é possível obter quanto cada um receberá.

Todos estes processos são executados via chamadas a API's REST. 


## Pre requistos

[Microsoft .NETCore 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)

O restante das bibliotecas externas utilizadas por este projeto podem ser obtidas via nuget. A restauração destes pacotes é feita automaticamente ao executar os comandos `dotnet build` e `dotnet run`. 

## Execucao

Ao executar a aplicacao, será exibida via browser a interface do usuário do Swagger, contendo a documentação de todas as API's disponíveis no projeto, conforme imagem abaixo:

## Uso

Esta aplicação possui três endPoints os quais são necessários para o gerenciamento dos funcionários da empresa e a geração de um JSON contendo o resultado da distribuição dos lucros.

[POST] /DistribuidorLucros/api/CadastraFuncionarios - Esta API realiza o cadastro de um ou mais funcionários a partir do schema abaixo:

```json
[
  {
    "matricula": "string",
    "nome": "string",
    "area": "string",
    "cargo": "string",
    "salario_bruto": "string",
    "data_de_admissao": "2020-12-16T01:17:02.369Z"
  }
]
```

[POST] /DistribuidorLucros/api//DistribuidorLucros/api/DescadastraFuncionarios - Esta API descadastra um ou mais funcionários previamente cadastrados no sistema a partir do schema abaixo:

```json
[
  {
    "matricula": "string",
    "nome": "string",
    "area": "string",
    "cargo": "string",
    "salario_bruto": "string",
    "data_de_admissao": "2020-12-16T01:17:02.369Z"
  }
]
```

[GET] /DistribuidorLucros/api/DistribuiParticipacao - Esta chamada de API recebe como parâmetro uma string contendo o montante (ex: "R$ 5.812.891,20") que será dividido entre os funcionários cadastrados e retorna o resultado desta divisão de acordo com o schema abaixo:

```json
{
  "participacoes": [
    {
      "matricula": "string",
      "nome": "string",
      "valor_da_participacao": "string"
    }
  ],
  "total_de_funcionarios": 0,
  "total_distribuido": "string",
  "total_disponibilizado": "string",
  "saldo_total_disponibilizado": "string"
}
```

Caso haja algum erro durante o processamento das requisições será retornado erro 500 e, por exemplo, a mensagem abaixo:

```json
{
	"error":"Input string was not in a correct format."
}
```

Caso, se trate de um erro conhecido e tratado pela aplicacao, sera retornado o erro 400, conforme exemplo abaixo:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "traceId": "|e191cd56-428a4076811904b4.",
  "errors": {
    "[0].matricula": [
      "The matricula field is required."
    ]
  }
}
```

## Testes

Para executar os testes unitarios da aplicacao, escolha Teste no menu do Visual Studio, Windows e, em seguida, Gerenciador de Testes.

Você pode executar todos os testes na solução, todos os testes em um grupo ou um conjunto de testes que você selecionar. Realize uma destas ações:

- Para executar todos os testes de uma solução, escolha o ícone Executar Todos.
- Para executar todos os testes de um grupo padrão, escolha o ícone Executar e, em seguida, escolha o grupo no menu.
- Selecione os testes individuais que deseja executar, abra o menu do clique com o botão direito para o teste selecionado e, em seguida, escolha Executar Testes Selecionados.
