# Backend AI

Projekt komunikuje się z modelem językowym na backendzie, w tym momencie
dostosowany jest do serwera REST uruchamianego przez
[llama.cpp](https://github.com/ggerganov/llama.cpp) (aplikacja `server`), oraz
testowana była na modelu Mistral-7B i jego pochodnych, w szczególności
[Mistral-RP-0.1-7B-GGUF](https://huggingface.co/Undi95/Mistral-RP-0.1-7B-GGUF?not-for-all-audiences=true).

Aby uruchomić serwer modelu językowego, należy pobrać (lub skompilować)
llama.cpp, pobrać plik modelu z serwera huggingface.co i uruchomić serwer
wydając polecenie, na przykład:

```bash
./server -m <plik modelu> -ngl 35 --host 127.0.0.1
```

(tutaj następuje przeniesienie 35 warstw modelu na urządzenie CUDA, wymagana
jest odmiana llama.cpp z obsługą CUDA, w przeciwnym wypadku parametr `-ngl` nie
jest dostępny)

Pełna dokumentacja serwera:
<https://github.com/ggerganov/llama.cpp/blob/master/examples/server/README.md>

## docker

Można też uruchomić Backend AI llama.cpp w oparciu o plik
`docker-compose-;lamacpp.yml`. W tym celu należy w pliku `.env`, w którym
znajduje się konfiguracja ustawień lokalnej bazy danych dodać kolejne dwie
zmienne definiujące wariant serwera oraz ścieżkę lokalną do pliku modelu, np.:

```env
LLAMA_VARIANT=full
MODEL_PATH=U:\ml\krakowiak-7b.gguf.q4_k_m.bin
```

Dostępne warianty serwera to: `full` (CPU), `full-cuda` (NVIDIA GPU) i
`full-rocm` (AMD ROCm GPU).

Teraz, zamiast wydawać polecenie `docker compose up` tak jak zwykle, możesz
wydać komendę:

```sh
docker compose -f docker-compose-llamacpp.yml up
```

Co uruchomi zarówno serwer modelu językowego, jak i bazy danych niezbędne
aplikacji głównej.

## Konfiguracja backendu AI w aplikacji

Konfiguracja backendu AI jest oparta o plik sekretów (`secrets.json`).
[Przeczytaj
dokumentację.](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=linux).

Sekrety aplikacji definiują URL serwera llama.cpp, na przykład:

```json
"AiEndpoint": {
    "EndpointUrl": "http://127.0.0.1:8080/completion"
}
```

Opcjonalnie można również dostarczyć klucz API wykorzystywany do komunikacji:

```json
{
  "AiEndpoint": {
    "EndpointUrl": "http://127.0.0.1:8080/completion",
    "ApiKey": "this-is-secret"
  }
}
```

URL i klucz API serwera uczelnianego są na Discordzie.

Niezbędne jest również wybranie modułów odpowiedzialnych za komunikację z
backendem AI i przygotowaniem promptów, które to należy wybrać jako pełne nazwy
typów, włącznie z ich _assembly_, dla Mistral-7B należy wybrać
`AlpacaPromptProvider`:

```json
"AiBackendProvider": "Fracture.Shared.External.Providers.Ai.LlamaCpp.LlamaCppBackendProvider, Fracture.Shared.External, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
"AiPromptTemplateProvider": "Fracture.Shared.External.Providers.Ai.AlpacaPromptProvider, Fracture.Shared.External, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
```
