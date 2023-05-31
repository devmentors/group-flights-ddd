# 01 - Zależności pomiędzy modułami

**Zgłaszający:** Michał Wilczyński

**Zatwierdzający:** Dariusz Pawlukiewicz

## Kontekst

Ze względu na wybór architektury wdrożeniowej jaką jest modularny monolit, należy wybrać taki sposób integracji, który:
- pozwoli na komunikację pomiędzy modułami
- zapewni enkapsulację szczegółów implementacyjnych w modułach

Bez narzucenia jakichkolwiek zasad istnieje techniczna możliwość tworzenia referencji pomiędzy modułami z pominięciem ich publicznego API.

Może to prowadzić do oparcia logiki w module A na bazie szczegółów implementacji w module B, które (jeśli nie zostały upublicznione w API), powinny móc się zmieniać niezależnie od ich konsumentów.

## Rozważane opcje

### Komunikacja
- **Opcja #1** - współdzielone kontrakty - projekty `<Module>.Shared` z interfejsem reprezentującym publiczne API
- **Opcja #2** - lokalne kontrakty - moduly komunikuja sie ze sobą po adresach udając protokół podobny do HTTP (wraz z serializacją na potrzeby transportu etc.)

### Referencje projektowe pomijające publiczne API
- **Opcja #1** - ustne zakazanie tworzenia takich referencji
- **Opcja #2** - utworzenie "testu architektury", ktory refleksja analizuje, czy takie zjawiska nie występują
- **Opcja #3** - dekompozycja solucji na mniejsze solucje (per moduł) i łączenie je w jeden artefakt wdrożeniowy przy budowaniu

## Decyzja

### Komunikacja

**Opcja #1** - ze względu na małą złożoność (porównując z systemami klasy enterprise), współdzielone kontrakty powinny wystarczyć.

### Referencje projektowe pomijające publiczne API
**Opcja #1** - ze względu na małą liczbę developerów tworzących system, zasady współpracy powinno udać się utrzymać bez żadnych dodatkowych rozwiązań.

Wraz z rozwojem systemu i dochodzeniem do niego nowych developerów (czy zespołów programistycznych) zalecane jest przejście na **Opcję #2**.

## Oczekiwany wynik

Ustalony explicite sposób tworzenia zależności pomiędzy modułami pozwoli im na swobodną komunikację jednocześnie utrzymując ich enkapsulację.

W przypadku niedostosowania się developerów do ustalonych zasad należy wprowadzić dodatkowe, zautomatyzowane rozwiązania, które wymuszą stosowanie się do tych zasad.

### Dodatkowa rekomendacja

Zalecane jest:
- używanie (w miarę możliwości) modyfikatora dostępu `internal` na każdym typie, 
- wpisu `[assembly: InternalsVisibleTo(...)]` w danym projekcie o widoczności "w górę" danego modułu jeśli zaistnieje taka potrzeba

W przypadku złożonego modelu (w ramach danego projektu `<Module>.Domain`) zalecane jest:
- używanie modyfikatora dostępu `internal` dla ukrycia szczegółów implementacji
- używanie modyfikatora dostępu `public` tylko na potrzeby udostępniania publicznego API modelu do warstwy "wyżej"
- **nie używanie** `[assembly: InternalsVisibleTo(...)]`, który mógłby ujawnić potencjalne szczegóły implementacyjne do warstwy aplikacyjnej czy infrastrukturalnej w tym samym module (tym samym łamiąc enkapsulację)

## Linki

Przykładowe, wdrożone podejścia:
- NPay: https://github.com/devmentors/NPay
- InFlow: https://github.com/devmentors/Inflow