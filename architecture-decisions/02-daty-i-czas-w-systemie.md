# 02 - Daty i czas w systemie

**Zgłaszający:** Michał Wilczyński

**Zatwierdzający:** Dariusz Pawlukiewicz

## Kontekst

Większość systemów informatycznych operuje (w mniejszym lub większym stopniu) na czasie i datach.

Celem uniknięcia niespójności (lub niepotrzebnej konwersji) należy ustandaryzować sposób przechowywania i przesyłania dat w całym systemie.

## Rozważane opcje

- **Opcja #1** - używanie lokalnego czasu polskiego
- **Opcja #2** - używanie UTC

## Decyzja

Naprostszym rozwiązaniem (i jednocześnie najmniej błędo-gennym) jest użycie **Opcji #2** tj. czasu UTC.

Ew. kwestie lokalizacji należy zostawić aplikacjom klienckim, np. frontendowi w przeglądarce czy aplikacjom mobilnym.

## Oczekiwany wynik

Zalożenie, że każda data jest w formacie UTC w systemie pozwala na minimalizację problemów (i zadań) związanych z jej przechowywaniem w bazie czy transporcie.

Jednocześnie klienci aplikacji mają jasny kontrakt API, na którym mogą polegać prezentując lub przesyłając daty.

### Dodatkowa rekomendacje

Zaleca się używanie abstrakcji nad API systemowym `DateTime.UtcNow`, tak by umożliwić testowanie budowanych rozwiązań.

W ramach projektu `GroupFlights.Shared.Types` udostępniono w tym celu interfejs `IClock` wraz z (już zarejestrowaną w kontenerze IoC) implementacją.

## Linki

W .NET 8 pojawi się w bibliotece standardowej odpowiednik naszego `IClock`: https://github.com/dotnet/runtime/issues/36617