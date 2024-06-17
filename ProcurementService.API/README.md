# ProcurementService.API

> Бальные танцы интерфейс серверной части

### Проект работает на:

1. net8.0
2. Entity Framework
3. SQL Server (MS SQL)

### Команды добавление миграции

add-migration Create_DataBase -context ApplicationDbContext -o DAL/Migrations

> [!TIP] 
> Проект использует паттерны: <br />
> [Unit of Work](https://bool.dev/blog/detail/unit-of-work-patterny-obektno-relyatsionnoy-logiki-poeaa) <br />
> [Repository](https://bool.dev/blog/detail/pattern-repozitoriy-poeaa) <br />
