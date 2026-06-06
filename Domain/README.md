# Domain

## Rola projektu

Warstwa domenowa – rdzeń całej aplikacji. Zawiera definicje encji i reguł biznesowych systemu przychodni medycznej.

Ten projekt **nie ma żadnych zależności** do innych projektów w solucji ani do bibliotek infrastrukturalnych (EF Core, ASP.NET, itd.). Dzięki temu logika biznesowa jest w pełni izolowana i łatwa do testowania.

## Co tutaj trafia

| Element | Przykłady |
|---|---|
| Klasy encji (modele domenowe) | `Patient`, `Visit`, `Doctor`, `Medication`, `ClinicalNote`, `MedicalRecord` |
| Enumy domenowe | `VisitStatus` (Planned, InProgress, Completed, Cancelled) |
| Interfejsy repozytoriów | `IPatientRepository`, `IVisitRepository` – **tylko interfejsy**, implementacja jest w Infrastructure |
| Reguły / wyjątki domenowe | `DomainException`, walidacje niezmienników encji |

## Czego tutaj nie ma

- Żadnego kodu EF Core (`DbContext`, atrybuty `[Column]`, migracje).
- Żadnych DTO ani logiki mapowania.
- Żadnych zależności do ASP.NET Core.
- Żadnej logiki aplikacyjnej (serwisy biznesowe, orkiestracja przypadków użycia).

## Zasada

Każda klasa w tym projekcie powinna być zrozumiała bez znajomości technologii – powinna opisywać **co system robi**, a nie **jak to jest zaimplementowane**.
