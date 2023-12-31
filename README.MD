# Books API

## Requisitos

- .NET 6 SDK
- Visual Studio 2022
- dotnet ef tools 7.0.9
- Docker Desktop

## Como rodar
```bash
export POSTGRES_PASSWORD=<senha do banco>   # pode ser omitido se a vari�vel estiver nas vari�veis de ambiente do sistema
docker-compose up -d
```

Na primeira execu��o, precisaremos inicializar o banco de dados. Para isso � necess�rio fazer alguns passos adicionais.
Ap�s executar os comandos acima na primeira execu��o, espere por volta de 30 segundos e execute o comando abaixo.

```bash
dotnet ef database update --connection "User ID=postgres;Password=${POSTGRES_PASSWORD};Host=localhost;Port=5432;Database=books" --project BooksAPI.DataLayer.EF --startup-project BooksAPI
```

**� importante que o comando acima seja executado antes de fazer qualquer requisi��o a API. Caso alguma requisi��o seja feita antes,
ser� necess�rio reiniciar o container da aplica��o.**


## Como usar
A Swagger UI est� habilitada para uso e pode ser acessada em http://localhost:8080/swagger. Podem ser feitas requisi��es diretamente
a API url cURL ou algum outro cliente HTTP.

## Limpeza
Para remover os recursos criados acima, use o comando `docker-compose down -v`. Esse comando deletar� tamb�m o volume associado 
ao Postgres. Caso deseje apenas para os containers, omita a op��o `-v`

