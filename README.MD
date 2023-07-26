# Books API

## Requisitos

- .NET 6 SDK
- Visual Studio 2022
- dotnet ef tools 7.0.9
- Docker Desktop

## Como rodar
```bash
export POSTGRES_PASSWORD=<senha do banco>   # pode ser omitido se a variável estiver nas variáveis de ambiente do sistema
docker-compose up -d
```

Na primeira execução, precisaremos inicializar o banco de dados. Para isso é necessário fazer alguns passos adicionais.
Após executar os comandos acima na primeira execução, espere por volta de 30 segundos e execute o comando abaixo.

```bash
dotnet ef database update --connection "User ID=postgres;Password=${POSTGRES_PASSWORD};Host=localhost;Port=5432;Database=books" --project BooksAPI.DataLayer.EF --startup-project BooksAPI
```

**É importante que o comando acima seja executado antes de fazer qualquer requisição a API. Caso alguma requisição seja feita antes,
será necessário reiniciar o container da aplicação.**


## Como usar
A Swagger UI está habilitada para uso e pode ser acessada em http://localhost:8080/swagger. Podem ser feitas requisições diretamente
a API url cURL ou algum outro cliente HTTP.

## Limpeza
Para remover os recursos criados acima, use o comando `docker-compose down -v`. Esse comando deletará também o volume associado 
ao Postgres. Caso deseje apenas para os containers, omita a opção `-v`

