# Uruchomienie aplikacji

Ten projekt wykorzystuje technologię Docker oraz Docker Compose do zarządzania
kontenerami i uruchamiania infrastruktury aplikacji. Aby uruchomić tę aplikację
na swoim komputerze, postępuj zgodnie z poniższymi krokami:

## 1. Instalacja Docker Desktop

Jeśli nie masz jeszcze zainstalowanego Docker Desktop, musisz to zrobić przed
uruchomieniem aplikacji. Docker Desktop jest dostępny dla systemów Windows,
Linux oraz macOS. Pobierz go stąd:
<https://www.docker.com/products/docker-desktop/>

(Możesz również skorzystać z Rancher Desktop, który jest produktem open source,
<https://github.com/rancher-sandbox/rancher-desktop/releases>, bądź innego,
kompatybilnego z Docker Compose dowolnego rozwiazania)

Po pobraniu i zainstalowaniu Docker Desktop, upewnij się, że jest uruchomiony.

## 2. Pobranie repozytorium

W kolejnym kroku pobierz poniższe repozytorium na swój komputer, wykonując
polecenia w terminalu lub wierszu poleceń w twoim lokalnym katalogu z
repozytoriami kodu.

```bash
git clone https://github.com/pollubnet/fracture.git
cd fracture
```

## 3. Dostosowanie pliku `.env`

W pliku `docker-compose.yml` zostały wykorzystane zmienne środowiskowe. W celu
utworzenia zmiennych środowiskowych skopiuj zawartość pliku `.sample.env` do
pliku o nazwie `.env`. Plik `.env` utwórz w głównym katalogu, w tym samym
miejscu gdzie znajduje się plik `docker-compose.yml`.

Następnie dostosuj zmienne środowiskowe w `.env` do swoich potrzeb oraz dokonaj
analogicznie zmian w projekcie `Fracture.Server` w pliku konfiguracyjnym
`appsettings.json`. Jeżeli używasz domyślnych opcji z pliku przykładowego
`.sample.env`, to nie trzeba dostosowywać pliku `appsettings.json`.

## 4. Uruchomienie infrastruktury aplikacji za pomocą Docker Compose

Po pobraniu repozytorium oraz zainstalowaniu Docker Desktop, możesz uruchomić
infrastrukturę aplikacji za pomocą Docker Compose.

W katalogu projektu znajdziesz plik `docker-compose.yml`, który zawiera
konfigurację kontenerów.

W terminalu przejdź do katalogu głównego projektu i wydaj komendę:

```bash
docker-compose up
```

Docker automatycznie pobierze i uruchomi odpowiednie wersje baz danych i innych
niezbędnych narzędzi.

## 5. Uruchomienie aplikacji w Visual Studio

Aby uruchomić tę aplikację w Visual Studio, rozpocznij od projektu
`Fracture.Server`, który trzeba ustawić jako domyślny. Wykonaj następujące
kroki:

1. Otwórz solucję projektu w Visual Studio (plik `Fracture.sln`),
2. Przejdź do Eksploratora Rozwiązań (Solution Explorer) po prawej stronie,
3. Kliknij prawym przyciskiem myszy na projekcie `Fracture.Server`, a następnie
   wybierz opcję "Ustaw jako projekt startowy" (Set as StartUp Project) z menu
   kontekstowego.

Teraz `Fracture.Server` jest ustawiony jako domyślny projekt. Możesz zacząć
pracować nad nim i uruchamiać aplikację w trybie debugowania, naciskając klawisz
F5 lub wybierając opcję "Uruchom" (Run) z menu Visual Studio.

Aplikacja uruchomi się, a Visual Studio uruchomi także przeglądarkę internetową
z dostępem do pierwszej strony aplikacji.

## Dodatkowe narzędzia

Dodatkowo w celu zarządzania infrastrukturą aplikacji został uruchomiony
pgAdmin4 oraz Redis Commander pod domyślnymi adresami:

### pgAdmin4

Adres: <http://localhost:8081>

Dostęp:

```
EMAIL=root@pollub.net
PASSWORD=root
```

### Redis Commander

Adres: <http://localhost:8082>

Dostęp:

```
USER=redis
PASSWORD=password
```

Numery portów, nazwy użytkowników i hasła można edytować w pliku `.env` w
katalogu głównym projektu.

Warto pamiętać, że na tym etapie wszystko powinno poprawnie się uruchomić, jeśli
zostało wcześniej poprawnie skonfigurowane. Jeśli masz jakiekolwiek problemy z
konfiguracją, upewnij się, że wszystkie zależności i środowisko są właściwie
skonfigurowane.
