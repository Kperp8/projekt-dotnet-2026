# Checklista implementacji pełnego CRUD w projekcie warstwowym

Dokument opisuje, jak wdrożyć pełny CRUD dla dowolnego modułu danych, np. Leki, Wizyty, Kartoteka.

## 1. Wybierz moduł i nazewnictwo

Przykład poniżej używa nazwy EntityName. Podstaw swoją nazwę, np. Medication.

- EntityName
- EntityNameCreateDto
- EntityNameUpdateDto
- EntityNameDetailsDto
- IEntityNameService
- IEntityNameRepository

## 2. Domain: model domenowy i kontrakt repozytorium

Dodaj pliki:

- Domain/EntityName.cs
- Domain/EntityNames/IEntityNameRepository.cs

Co powinno się tam znaleźć:

- Encja z polami biznesowymi i ewentualnym IsDeleted
- Interfejs repozytorium z metodami:
  - GetByIdAsync
  - GetPagedAsync
  - SearchAsync
  - AddAsync
  - UpdateAsync
  - DeleteAsync lub soft delete
  - SaveChangesAsync

## 3. Application: DTO, mapper, serwis

Dodaj pliki:

- Application/EntityNames/Dtos/EntityNameCreateDto.cs
- Application/EntityNames/Dtos/EntityNameUpdateDto.cs
- Application/EntityNames/Dtos/EntityNameListItemDto.cs
- Application/EntityNames/Dtos/EntityNameDetailsDto.cs
- Application/EntityNames/Mappers/EntityNameMapper.cs
- Application/EntityNames/Interfaces/IEntityNameService.cs
- Application/EntityNames/Services/EntityNameService.cs

Co powinno się tam znaleźć:

- DTO wejścia z DataAnnotations
- DTO wyjścia do listy i szczegółów
- Mapperly mapper encja <-> DTO
- Interfejs serwisu z pełnym CRUD
- Implementacja serwisu z walidacją biznesową i ILogger

## 4. Infrastructure: DbContext, konfiguracja EF, repozytorium

Dodaj pliki:

- Infrastructure/Data/Configurations/EntityNameConfiguration.cs
- Infrastructure/Repositories/EntityNameRepository.cs

Zaktualizuj plik:

- Infrastructure/Data/AppDbContext.cs

Co powinno się tam znaleźć:

- DbSet<EntityName>
- ApplyConfiguration(new EntityNameConfiguration())
- Konfiguracja długości pól, relacji i indeksów
- Implementacja repozytorium EF Core

## 5. Web: rejestracja DI i warstwa UI/API

Zaktualizuj plik:

- Web/Program.cs

Dodaj jeden z wariantów:

Wariant Razor Pages:

- Web/Pages/EntityNames/Index.cshtml
- Web/Pages/EntityNames/Index.cshtml.cs
- Web/Pages/EntityNames/Create.cshtml
- Web/Pages/EntityNames/Create.cshtml.cs
- Web/Pages/EntityNames/Edit.cshtml
- Web/Pages/EntityNames/Edit.cshtml.cs
- Web/Pages/EntityNames/Details.cshtml
- Web/Pages/EntityNames/Details.cshtml.cs
- Web/Pages/EntityNames/Delete.cshtml
- Web/Pages/EntityNames/Delete.cshtml.cs

Wariant API:

- Web/Controllers/EntityNamesController.cs

Co powinno się tam znaleźć:

- Rejestracja DI: repozytorium, serwis, mapper
- Atrybuty Authorize dla ról
- Obsługa błędów 404, 409, 400

## 6. Konfiguracja bazy

Zaktualizuj plik:

- Web/appsettings.json

Co powinno się tam znaleźć:

- ConnectionStrings: DefaultConnection

## 7. Migracje EF Core

Uruchom komendy:

- dotnet ef migrations add AddEntityName --project Infrastructure --startup-project Web
- dotnet ef database update --project Infrastructure --startup-project Web

## 8. Testy

Dodaj pliki:

- Tests/Unit/EntityNames/EntityNameServiceTests.cs
- Tests/Integration/EntityNames/EntityNamesPagesTests.cs lub EntityNamesControllerTests.cs

Co sprawdzić:

- Create zapisuje poprawne dane
- GetById zwraca 404/null dla braku rekordu
- Update aktualizuje dane
- Delete usuwa logicznie lub fizycznie zgodnie z projektem
- Search i paginacja działają poprawnie

## 9. Minimalna definicja ukończenia modułu

Moduł CRUD uznaj za ukończony, gdy:

- Ma działające Create Read Update Delete
- Ma walidację wejścia
- Ma autoryzację zgodną z rolami
- Ma migrację bazy
- Ma testy jednostkowe i przynajmniej podstawowy test integracyjny
- Jest podpięty do nawigacji UI lub endpointów API

## 10. Szybka lista kontrolna przed commitem

- Czy wszystkie nowe pliki są w odpowiednich projektach
- Czy Program.cs ma komplet rejestracji DI
- Czy dotnet build przechodzi
- Czy dotnet test przechodzi
- Czy migracja została wygenerowana i zastosowana
- Czy CRUD działa ręcznie z UI lub przez API
