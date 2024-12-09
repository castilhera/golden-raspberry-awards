# Golden Raspberry Awards API

API RESTful que possibilita:

- a leitura da lista de indicados e vencedores da categoria *Pior Filme* do Golden Raspberry Awards.

- obter o produtor com maior intervalo entre dois prêmios consecutivos, e o que obteve dois prêmios mais rápido.

##  Requisitos do Sistema

- **.NET 8 ou superior** instalado.

##  Instalação

Para instalar e executar o projeto:

**Baixar e compilar:**

1. Clone o repositório golden-raspberry-awards:
```sh
git clone https://github.com/castilhera/golden-raspberry-awards
```

2. Navegue até a pasta do projeto:
```sh
cd golden-raspberry-awards\src\GoldenRaspberryAwards.Api
```

3. Instale as dependências do projeto:

```sh
dotnet restore
```

##  Executar

Para executar a API, na pasta do projeto, utilize o seguinte comando:

```sh
dotnet run
```

## Testes

### Testes de Integração

Para o teste de integração, mantenha a aplicação em execução.

1. Navegue até o diretório do projeto de testes:

```sh
cd golden-raspberry-awards\tests\GoldenRaspberryAwards.Api.IntegrationTests
```

2. Execute o teste, utilizando o comando:

```sh
dotnet test
```