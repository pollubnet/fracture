# Backend AI

Projekt komunikuje się z modelem językowym na backendzie, w tym momencie
dostosowany jest do serwerów, które oferują API podobne do tego, które oferuje
OpenAI, więc można wykorzystać zarówno oficjalny system OpenAI, jak i produkty w
rodzaju vLLM, serwera llama.cpp lub [ollama](https://ollama.com/).

## ollama

[Ollama](https://ollama.com/) to najłatwiejszy sposób na uruchomienie modelu na
własnym komputerze - wystarczy zainstalować oprogramowanie, wydać komendę
`ollama pull mistral` i `ollama serve` aby uruchomić kompatybilny z OpenAI
serwer na własnym komputerze, który będzie mogł uruchamiać popularny i mało
wymagający model, np. Mistral-7B. Ollama dostępna jest zarówno dla Windows,
Linuksa, jak i macOS i dostosowana jest zarówno do wykorzystania CUDA na kartach
NVIDII, jak i ROCm na kartach graficznych AMD (lub pozwala również skorzystać
tylko z procesora).

## Konfiguracja backendu AI w aplikacji

Konfiguracja backendu AI jest oparta o plik sekretów (`secrets.json`), aby klucz
API (potencjalnie bardzo wrażliwa informacja) nie "wyciekł", a także, aby każdy
programista mógł korzystać z własnej konfiguracji AI.

[Przeczytaj
dokumentację.](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=linux).

Sekrety aplikacji definiują URL serwera i domyślny model, na przykład:

```json
"AiBackend": {
    "EndpointUrl": "http://127.0.0.1:11434",
    "Model": "mistral"
}
```

Opcjonalnie można również dostarczyć klucz API wykorzystywany do komunikacji:

```json
"AiBackend": {
    "EndpointUrl": "http://127.0.0.1:11434",
    "Model": "mistral",
    "ApiKey": "this is a secret"
}
```

URL i klucz API serwera uczelnianego są na Discordzie. Obecnie testowo używamy
modelu Llama 3 8B.

## Włączenie funkcji AI

Z uwagi na to, że nie każdy chce (i może) korzystać z własnego systemu typu LLM,
wszystkie funkcje oparte o AI można obecnie wyłączyć i są domyślnie wyłączone.
Aby je włączyć, musisz aktywować tzw. flagę funkcji. W pliku sekretów dodaj
sekcję, które nadpisze domyślne wartości:

```json
"FeatureFlags": {
  "UseAI": true
}
```
