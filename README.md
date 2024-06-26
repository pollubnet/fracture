# Fracture

## Opis

Witaj w repozytorium dla projektu "Fracture". Jako "Grupa .NET Politechniki
Lubelskiej" (aka pollub.net) budujemy aplikację internetową będącą grą w stylu
RPG, której założeniem jest niepowtarzalność przygód w niej poprzez
wykorzystanie m.in. generowania proceduralnego oraz dużych modeli językowych.

## O projekcie

Nasza gra ma trzy cele – z jednej strony chodzi o zabawę i rozrywkę, z drugiej o
naukę programowania, a z trzeciej o naukę wykorzystania wzorców architektury
oprogramowania.

- Platforma: Gra jest tworzona na platformie .NET, co pozwala nam wykorzystać
  bogaty ekosystem narzędzi dostępnych w środowisku .NET. Wykorzystana jest
  platforma ASP.NET WebAPI oraz Blazor do tworzenia back-endu i front-endu
  aplikacji,
- Rodzaj gry: Nasza gra to przeglądarkowa produkcja z gatunku RPG (Role-Playing
  Game), która pozwala graczom wcielić się w postać i odgrywać niepowtarzalne
  przygody,
- Technologie: W trakcie projektu będziemy używać takich technologii jak baza
  danych PostgreSQL, system kolejkowy RabbitMQ, modele językowe LLM, aby
  dostarczyć nowatorskie rozwiązania,
- Architektura: Aplikacja została utworzona jako modularny monolit.

## W jaki sposób uruchomić projekt i pomóc w jego rozwoju

### AI/LLM

Projekt wykorzystuje system Sztucznej Inteligencji, tzw. Duży Model Językowy
(Large Language Model -- LLM), który jest uruchomiony na naszym serwerze
uczelnianym. Z uwagi na to, że chcemy chronić się przed niepowołanym użyciem,
adres i klucz dostępu do tego serwera są tajne, możesz dostać do nich dostęp na
naszym serwerze Discord. Nie dodawaj ich do repozytorium! Zamiast tego korzysta
się z pliku sekretów.

Można też uruchomić model i serwer LLM na swoim własnym komputerze, korzystając
z instrukcji opisanej w dokumencie [AI](docs/ai.md).

### Rozwój projektu, dodawanie własnych commitów

Aby móc uczestniczyć w rozwoju tego projektu możesz wykonać jego *fork* i
utworzyć jego kopię na własnym koncie GitHub, a następnie poprosić o połączenie
twoich zmian za pomocą funkcji Pull Request. Każdy Pull Request (PR) jest
testowany na to, czy m.in. się poprawnie kompiluje, czy formatowanie kodu jest
poprawne i tak dalej. Dodatkowo, każdy z PR zostanie przejrzany przez kogoś z
głównej ekipy rozwoju projektu przed dołączeniem go głównej gałęzi.

Jeżeli chcesz "wypychać" zmiany bezpośrednio do głównego repozytorium, ale do
innych gałęzi niż główna, to też nie ma problemu - tak robimy! Ale musisz podać
swój nick na GitHubie, abyśmy mogli dodać Ciebie bezpośrednio do użytkowników z
prawami zapisu do tego projektu.

### Formatowanie kodu i tytułów commitów

Projekt wykorzystuje narzędzie
[git-conventional-commits](https://github.com/qoomon/git-conventional-commits)
do kontroli, czy tytuły commitów zgadzają się ze specyfikacją. A by móc z niego
skorzystać, należy je zainstalować z wykorzystaniem NPM:

```bash
npm install --global git-conventional-commits
```

Narzędzie zostanie aktywowane automatycznie poprzez wykorzystanie
[Husky.Net](https://alirezanet.github.io/Husky.Net/guide/) -- aby skorzystać z
Husky, należy wykonać pierwszy raz komendę `dotnet restore` na projekcie
`Server\Fracture.Server.csproj` i zostanie automatycznie zainstalowane wraz z
innymi zależnościami.

W podobny sposób, przed każdym commitem, pliki zostaną sprawdzone i
przeformatowane z wykorzystaniem narzędzia [csharpier](https://csharpier.com/) w
celu ujednolicenia stylu pisania kodu.

## Autorzy

Ten projekt jest rozwijany przez członków Koła Naukowego "Grupa .NET
Politechniki Lubelskiej".

## Podziękowania

Chcielibyśmy podziękować wszystkim, którzy przyczynili się do tego projektu.
Dziękujemy za zainteresowanie naszym projektem i zapraszamy do wspólnego rozwoju
gry!
