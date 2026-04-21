# CSV to SQL Insert Generator

Ferramenta em **.NET** para gerar comandos `INSERT` SQL a partir de um arquivo **CSV**.

O objetivo é facilitar a importação de dados em banco de dados convertendo automaticamente os valores do CSV para o formato SQL correto.

---

# Como funciona

A aplicação:

1. Lê um arquivo **CSV**
2. Usa uma **configuração de colunas**
3. Converte os valores conforme o **tipo definido**
4. Gera comandos **INSERT INTO**

---

# Estrutura do CSV

O arquivo CSV deve usar **`;` como separador** e conter um **header com os nomes das colunas**.

Exemplo:

```
Name;Age;Birth;Graduation;Salary
Fernando;30;1994-05-10 08:30:00;2018-12-20;5500.50
Maria;25;1999-01-15 10:00:00;2022-07-01;4200.75
```

---

# Configuração das colunas

As colunas são configuradas através de um dicionário onde:

* **Key** → nome da coluna
* **Value** → tipo do banco ou valor customizado

Exemplo:

```csharp
var columns = new Dictionary<string, ColumnDefinition>
{
    { "Name", new ColumnDefinition { DatabaseType = DbType.String } },
    { "Age", new ColumnDefinition { DatabaseType = DbType.Int16 } },
    { "Birth", new ColumnDefinition { DatabaseType = DbType.DateTime } },
    { "Graduation", new ColumnDefinition { DatabaseType = DbType.Date } },
    { "Salary", new ColumnDefinition { DatabaseType = DbType.Decimal } },
    { "Creation", new ColumnDefinition {
        CustomSqlType = "DateTimeNow",
        CustomValue = "CURRENT_TIMESTAMP"
    }}
};
```

---

# Exemplo de uso

```csharp
var service = new SqlGeneratorService();

var sql = service.GenerateInsertFromCsv(
    "MyTable",
    columns,
    "./files",
    "data.csv"
);

Console.WriteLine(sql);
```

---

# SQL gerado

Exemplo de saída:

```sql
INSERT INTO MyTable (Name, Age, Birth, Graduation, Salary)
VALUES ('Fernando', 30, '1994-05-10 08:30:00', '2018-12-20', 5500.50);

INSERT INTO MyTable (Name, Age, Birth, Graduation, Salary)
VALUES ('Maria', 25, '1999-01-15 10:00:00', '2022-07-01', 4200.75);
```

---

# Tipos suportados

A aplicação utiliza o enum `DbType` para definir como cada valor será convertido.

```csharp
public enum DbType
{
    AnsiString = 0,
    Binary = 1,
    Byte = 2,
    Boolean = 3,
    Currency = 4,
    Date = 5,
    DateTime = 6,
    Decimal = 7,
    Double = 8,
    Guid = 9,
    Int16 = 10,
    Int32 = 11,
    Int64 = 12,
    Object = 13,
    SByte = 14,
    Single = 15,
    String = 16,
    Time = 17,
    UInt16 = 18,
    UInt32 = 19,
    UInt64 = 20,
    VarNumeric = 21,
    AnsiStringFixedLength = 22,
    StringFixedLength = 23,
    Xml = 25,
    DateTime2 = 26,
    DateTimeOffset = 27
}
```

---

# Valores customizados

Também é possível definir **valores SQL customizados** que não vêm do CSV.

Exemplo:

```csharp
new ColumnDefinition
{
    CustomSqlType = "DateTimeNow",
    CustomValue = "CURRENT_TIMESTAMP"
}
```

Isso gera:

```sql
CURRENT_TIMESTAMP
```

---
