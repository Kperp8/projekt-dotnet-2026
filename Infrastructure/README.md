# Infrastructure

## Rola projektu

Warstwa infrastrukturalna – implementacje wszystkiego, co dotyczy zewnętrznych technologii: bazy danych, poczty e-mail, systemu plików, generowania PDF, itp.

Zna projekty `Domain` i `Application` (implementuje ich interfejsy). Nie jest znana przez `Web` bezpośrednio – jest rejestrowana przez Dependency Injection w `Web/Program.cs`.

## Co tutaj trafia

| Element | Przykłady |
|---|---|
| `DbContext` (EF Core) | `AppDbContext` – konfiguracja połączenia, `DbSet<>` dla każdej encji |
| Konfiguracje EF | `PatientConfiguration`, `VisitConfiguration` – Fluent API, indeksy, relacje |
| Migracje EF Core | generowane przez `dotnet ef migrations add` |
| Implementacje repozytoriów | `PatientRepository : IPatientRepository` – zapytania EF |
| Implementacje usług zewnętrznych | `EmailService : IEmailService`, `PdfService : IPdfService`, `FileStorageService` |
| Konfiguracja NLog | `nlog.config` lub konfiguracja przez kod |
| Seed danych | klasa seedująca dane testowe do bazy (`DataSeeder`) |

## Struktura folderów (propozycja)

```
Infrastructure/
├── Data/
│   ├── AppDbContext.cs
│   ├── Configurations/
│   │   ├── PatientConfiguration.cs
│   │   └── VisitConfiguration.cs
│   ├── Migrations/
│   └── Seeding/
├── Repositories/
├── Services/
│   ├── EmailService.cs
│   ├── PdfService.cs
│   └── FileStorageService.cs
└── ...
```

## Czego tutaj nie ma

- Żadnej logiki biznesowej (reguły, walidacje domenowe, przepływ danych).
- Żadnych kontrolerów ani endpointów API.
- Żadnych DTO ani mapperów (te są w `Application`).

## Ważne: migracje EF Core

Migracje generujesz zawsze z katalogu głównego solucji, wskazując projekt startowy (`Web`) i projekt migracji (`Infrastructure`):

```bash
dotnet ef migrations add NazwaMigracji --project Infrastructure --startup-project Web
dotnet ef database update --project Infrastructure --startup-project Web
```
