# Web

## Rola projektu

Punkt wejścia całej aplikacji – projekt ASP.NET Core (Razor Pages + .NET 10). Odpowiada za obsługę żądań HTTP, autoryzację, routing, konfigurację Dependency Injection i wystawienie API/widoków.

Jest kompozycją pozostałych warstw: rejestruje serwisy z `Application` i `Infrastructure`, ale sam nie zawiera logiki biznesowej.

## Co tutaj trafia

| Element | Przykłady |
|---|---|
| Punkt startu aplikacji | `Program.cs` – rejestracja DI, middleware, konfiguracja |
| Kontrolery API | `PatientsController`, `VisitsController`, `MedicationsController` |
| Razor Pages (widoki) | `Pages/Patients/`, `Pages/Visits/`, widoki listy, szczegółów, formularze |
| Konfiguracja Identity | rejestracja ASP.NET Identity, role (`Admin`, `Lekarz`, `Rejestratorka`) |
| Konfiguracja OpenAPI | Swagger/Scalar – dokumentacja endpointów API |
| Konfiguracja NLog | podpięcie NLog do `ILoggerFactory` w `Program.cs` |
| Pliki statyczne i upload | `wwwroot/uploads/` – skany dokumentów pacjentów |
| Ustawienia aplikacji | `appsettings.json`, `appsettings.Development.json` – connection string, SMTP, itd. |

## Struktura folderów (propozycja)

```
Web/
├── Controllers/
│   ├── PatientsController.cs
│   ├── VisitsController.cs
│   └── ...
├── Pages/
│   ├── Patients/
│   ├── Visits/
│   └── ...
├── wwwroot/
│   └── uploads/
├── Program.cs
└── appsettings.json
```

## Czego tutaj nie ma

- Żadnej logiki biznesowej – kontrolery wywołują serwisy z `Application` i zwracają wynik.
- Żadnego bezpośredniego dostępu do `DbContext` ani repozytoriów – zawsze przez interfejsy serwisów.
- Żadnych encji domenowych w odpowiedziach API – zawsze DTO z `Application`.

## Rejestracja zależności (Program.cs)

Wszystkie serwisy i repozytoria rejestruj w `Program.cs`. Dla czytelności możesz użyć metod rozszerzających:

```csharp
builder.Services.AddApplicationServices();   // metoda w Application
builder.Services.AddInfrastructureServices(builder.Configuration); // metoda w Infrastructure
```

## Autoryzacja ról

W projekcie obowiązują trzy role:
- `Admin` – pełny dostęp
- `Lekarz` – odczyt pacjentów, wizyt, notatek; tworzenie notatek i aktualizacja wizyt
- `Rejestratorka` – CRUD pacjentów, rejestracja wizyt, katalog leków

Używaj atrybutów `[Authorize(Roles = "Admin,Rejestratorka")]` lub polityk autoryzacji.
