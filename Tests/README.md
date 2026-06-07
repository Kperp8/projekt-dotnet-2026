# Tests

## Rola projektu

Projekt testowy – NUnit (.NET 10). Zawiera testy jednostkowe i integracyjne dla całej aplikacji.

## Co tutaj trafia

| Rodzaj testu | Co testuje | Przykłady |
|---|---|---|
| **Jednostkowe** | Serwisy z `Application` w izolacji (mock repozytoriów) | `PatientServiceTests`, `VisitServiceTests` |
| **Jednostkowe** | Reguły domenowe z `Domain` | walidacja PESEL, reguły statusów wizyty |
| **Integracyjne** | Kontrolery / endpointy API (z bazą in-memory lub testową) | `PatientsControllerTests` |

## Jak pisać testy jednostkowe serwisów

Serwisy aplikacyjne zależą od interfejsów repozytoriów. W testach zastępuj je mockami (np. biblioteka **Moq** lub **NSubstitute**):

```csharp
[Test]
public async Task CreatePatient_DuplicatePesel_ThrowsException()
{
    var repo = Substitute.For<IPatientRepository>();
    repo.ExistsByPeselAsync("12345678901").Returns(true);

    var service = new PatientService(repo);

    Assert.ThrowsAsync<DomainException>(() =>
        service.CreateAsync(new PatientCreateDto { Pesel = "12345678901" }));
}
```

## Struktura folderów (propozycja)

```
Tests/
├── Unit/
│   ├── Patients/
│   │   └── PatientServiceTests.cs
│   ├── Visits/
│   │   └── VisitServiceTests.cs
│   └── ...
├── Integration/
│   ├── PatientsControllerTests.cs
│   └── ...
└── Helpers/
    └── TestDataBuilder.cs  // pomocnicze dane testowe
```

## Przydatne biblioteki do zainstalowania

```bash
dotnet add Tests package NSubstitute         # mockowanie
dotnet add Tests package FluentAssertions    # czytelniejsze asercje
dotnet add Tests package Microsoft.AspNetCore.Mvc.Testing  # testy integracyjne
```

## Wymagania projektu

Specyfikacja wymaga testów dla co najmniej:
- Tworzenia pacjenta z poprawnymi danymi.
- Blokady duplikatu PESEL.
- Wyszukiwania po nazwisku / PESEL.
- Odpowiedzi 404 dla nieistniejącego zasobu.
- Odpowiedzi 409 przy konflikcie PESEL.

Testy uruchamiasz komendą:

```bash
dotnet test
```
