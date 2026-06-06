# Application

## Rola projektu

Warstwa aplikacyjna – logika przypadków użycia systemu. Tutaj trafia cały kod biznesowy, który orkiestruje encje z `Domain` i korzysta z interfejsów repozytoriów lub zewnętrznych usług.

Zna tylko projekt `Domain`. Nie zna EF Core, ASP.NET ani żadnej konkretnej bazy danych.

## Co tutaj trafia

| Element | Przykłady |
|---|---|
| Interfejsy serwisów | `IPatientService`, `IVisitService`, `IMedicationService` |
| Implementacje serwisów | `PatientService`, `VisitService` – zawierają logikę (walidacja, reguły, przepływ danych) |
| DTO (obiekty transferu danych) | `PatientCreateDto`, `PatientDetailsDto`, `VisitSummaryDto` |
| Mappery Mapperly | `PatientMapper`, `VisitMapper` – mapowanie DTO ↔ encje |
| Interfejsy zewnętrznych usług | `IEmailService`, `IPdfService`, `IFileStorageService` – implementowane w Infrastructure |

## Struktura folderów (propozycja)

```
Application/
├── Patients/
│   ├── Dtos/
│   ├── Interfaces/
│   ├── Mappers/
│   └── Services/
├── Visits/
├── Medications/
└── ...
```

## Czego tutaj nie ma

- Żadnego kodu EF Core ani SQL.
- Żadnych atrybutów ASP.NET (`[HttpGet]`, `[Authorize]`, itd.).
- Żadnych szczegółów infrastrukturalnych (SMTP, pliki, PDF) – tylko interfejsy.

## Zasada

Serwis w tym projekcie powinien opisywać **co użytkownik może zrobić** w systemie (np. `CreatePatientAsync`, `SearchPatientsByPeselAsync`), a nie jak dane są zapisywane do bazy.
