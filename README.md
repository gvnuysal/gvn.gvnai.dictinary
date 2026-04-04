<p align="center">
  <img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet" />
  <img src="https://img.shields.io/badge/Angular-21-DD0031?style=for-the-badge&logo=angular" />
  <img src="https://img.shields.io/badge/PostgreSQL-17-4169E1?style=for-the-badge&logo=postgresql&logoColor=white" />
  <img src="https://img.shields.io/badge/Claude_AI-Anthropic-191919?style=for-the-badge&logo=anthropic" />
  <img src="https://img.shields.io/badge/License-MIT-green?style=for-the-badge" />
</p>

# GvnAI Dictionary

**AI destekli, profesyonel bilingual (EN ↔ TR) sözlük uygulaması.**

> **Temel Prensip:** *"Kelimeyi değil, anlamı çevir."*
> — Oxford, Cambridge, TDK gibi profesyonel sözlüklerin kullandığı **Word → Sense → Translation** mimarisi.

---

## Öne Çıkan Özellikler

- **Word → Sense → Translation** mimarisi (profesyonel sözlük standardı)
- **Claude AI ile otomatik zenginleştirme** (anlam, çeviri, örnek, telaffuz, köken)
- **Kelime Oyunu** — İngilizce kelimeden Türkçe karşılığını bul (+5/-3 puan, 10 saniye süre)
- **Kullanıcı Profili** — Oyun istatistikleri, favori kelimeler, profil düzenleme
- **Favori Sistemi** — Kelimeleri favorilere ekle/çıkar
- **Glassmorphism UI** — Modern dark theme, animasyonlu kartlar
- **JWT Kimlik Doğrulama** — Güvenli API erişimi
- **Clean Architecture** — DDD, CQRS, MediatR, FluentValidation

---

## Mimariye Genel Bakış

```
                    ┌─────────────────────────────────────────┐
                    │           Angular Frontend               │
                    │     (gvn-dictionary library)             │
                    │        http://localhost:4200              │
                    └──────────────┬──────────────────────────┘
                                   │ HTTP/REST + JWT
                    ┌──────────────▼──────────────────────────┐
                    │         ASP.NET Core API                 │
                    │       http://localhost:5050               │
                    │                                          │
                    │  ┌─────────┐ ┌──────────┐ ┌──────────┐  │
                    │  │ Scalar  │ │ Hangfire │ │  CORS    │  │
                    │  │  /scalar│ │ /hangfire│ │          │  │
                    │  └─────────┘ └──────────┘ └──────────┘  │
                    └──────────────┬──────────────────────────┘
                                   │
              ┌────────────────────┼────────────────────┐
              │                    │                    │
     ┌────────▼───────┐  ┌────────▼───────┐  ┌────────▼───────┐
     │  PostgreSQL     │  │   Claude AI    │  │  Redis Cache   │
     │  :5432          │  │   (Anthropic)  │  │  :6379 (opt.)  │
     └─────────────────┘  └────────────────┘  └────────────────┘
```

---

## Sözlük Veri Modeli

```
Word: "run" (VERB, English)
│
├── Sense 1: "To move swiftly on foot"
│   ├── Translation → "koşmak" (TR, Exact, %98)
│   └── Example: "She runs every morning." → "Her sabah koşar."
│
├── Sense 2: "To operate or manage something"
│   ├── Translation → "yönetmek" (TR, Near, %85)
│   ├── Translation → "işletmek" (TR, Near, %85)
│   └── Example: "He runs a small company." → "Küçük bir şirket yönetiyor."
│
├── Sense 3: "To function or work properly"
│   ├── Translation → "çalışmak" (TR, Near, %80)
│   └── Example: "The engine runs smoothly." → "Motor düzgün çalışıyor."
│
├── Pronunciation: /rʌn/ (IPA, Standard)
└── Etymology: Old English "rinnan"
```

---

## Kelime Oyunu

İngilizce kelimenin Türkçe karşılığını bul! Sistemdeki tüm kelimelerden rastgele sorular.

```
┌─────────────────────────────────────────────────┐
│  Puan: 25    Soru: 6    Doğru: 5    Yanlış: 1  │
│                                          ┌────┐ │
│                                          │ 7  │ │  ← 10 sn geri sayım
│                                          └────┘ │
│         ┌──────────────────────────┐            │
│         │  EN   beautiful          │            │  ← İngilizce kelime
│         │  "Pleasing to senses"    │            │
│         └──────────────────────────┘            │
│                                                 │
│   ┌──────────┐  ┌──────────┐  ┌──────────┐    │
│   │  güzel   │  │  hızlı   │  │  büyük   │    │  ← 5 Türkçe seçenek
│   │  ✓ ████ │  │          │  │          │    │
│   └──────────┘  └──────────┘  └──────────┘    │
│   ┌──────────┐  ┌──────────┐                   │
│   │  küçük   │  │  kolay   │                   │
│   └──────────┘  └──────────┘                   │
│                                                 │
│   🎉 Doğru! +5 puan                            │
└─────────────────────────────────────────────────┘
```

**Kurallar:**
- Doğru cevap: **+5 puan**
- Yanlış cevap / süre dolması: **-3 puan**
- Süre: **10 saniye** (SVG dairesel geri sayım)
- Sınırsız soru — istediğiniz zaman bitirin
- Tüm sonuçlar veritabanında saklanır

---

## Kullanıcı Profili

```
┌─────────────────────────────────────────────────┐
│  ┌──┐                                           │
│  │GU│  Güven Uysal                  ✏ Düzenle  │
│  └──┘  admin@gvn.dev                             │
│        🏷 User   📅 Üye: 02.04.2026             │
├─────────────────────────────────────────────────┤
│                                                  │
│  🏆 25       🎮 3        ✅ 8       ❌ 2       │
│  Toplam     Oyun       Doğru     Yanlış        │
│  Puan       Sayısı     Cevap     Cevap         │
│                                                  │
│  🎯 %80     ⭐ 15      ❓ 10      ♥ 5          │
│  Başarı    En İyi     Toplam    Favori         │
│  Oranı     Skor       Soru      Kelime         │
├─────────────────────────────────────────────────┤
│  Favori Kelimeler                                │
│  ┌────────┐ ┌────────┐ ┌────────┐              │
│  │love ADJ│ │run VERB│ │time N. │              │
│  │→ aşk   │ │→ koşmak│ │→ zaman │              │
│  └────────┘ └────────┘ └────────┘              │
└─────────────────────────────────────────────────┘
```

---

## AI Zenginleştirme

Claude AI ile kelimeler otomatik zenginleştirilebilir:

```
Önce:                              Sonra:
┌──────────────────┐               ┌─────────────────────────────────┐
│ love (NOUN, EN)  │               │ love (NOUN, EN)                │
│ Status: Pending  │   Claude AI   │ Status: Enriched               │
│ Senses: []       │ ────────────► │                                │
│ Pronunciation: - │               │ Sense 1: "Deep affection"      │
│ Etymology: -     │               │   → aşk (TR, Exact, %95)      │
└──────────────────┘               │   → sevgi (TR, Near, %90)     │
                                   │   📝 "Love is powerful."      │
                                   │                                │
                                   │ Sense 2: "Strong liking"      │
                                   │   → tutku (TR, Near, %85)     │
                                   │   📝 "A love for music."      │
                                   │                                │
                                   │ 🔊 /lʌv/ (IPA)               │
                                   │ 📜 Old English "lufu"         │
                                   └─────────────────────────────────┘
```

---

## Veritabanı Tabloları (17 tablo)

| Tablo | Açıklama | Tip |
|-------|----------|-----|
| `words` | Ana kelime tablosu (lemma) | AggregateRoot |
| `senses` | Kelime anlamları | Entity |
| `translations` | Anlam çevirileri | Entity |
| `examples` | Örnek cümleler (paralel çeviri) | Entity |
| `pronunciations` | IPA telaffuz | Entity |
| `etymologies` | Kelime kökeni | Entity |
| `languages` | Dil tanımları (tr, en, de...) | Lookup |
| `parts_of_speech` | Sözcük türleri (NOUN, VERB...) | Lookup |
| `registers` | Kullanım kaydı (formal, slang...) | Lookup |
| `domains` | Konu alanı (medicine, law...) | Lookup |
| `sense_synonyms` | Eş anlamlılar | İlişki |
| `sense_antonyms` | Zıt anlamlılar | İlişki |
| `word_relationships` | Kelime ilişkileri | İlişki |
| `Users` | Kullanıcı hesapları | AggregateRoot |
| `quiz_sessions` | Oyun oturumları | AggregateRoot |
| `quiz_answers` | Oyun cevapları | Entity |
| `favorites` | Favori kelimeler | Entity |

---

## Teknoloji Stack

### Backend

| Teknoloji | Versiyon | Kullanım |
|-----------|----------|----------|
| .NET | 10.0 | Runtime |
| ASP.NET Core | 10.0 | Web API |
| Entity Framework Core | 10.0.5 | ORM |
| PostgreSQL | 17 | Veritabanı |
| MediatR | 12.4.1 | CQRS |
| FluentValidation | 11.11.0 | Doğrulama |
| Anthropic SDK | 3.3.0 | Claude AI |
| Hangfire | 1.8+ | Arka Plan Görevleri |
| Serilog | 9.x | Loglama |
| Scalar | 2.x | API Dokümantasyonu |

### Frontend

| Teknoloji | Versiyon | Kullanım |
|-----------|----------|----------|
| Angular | 21.2 | UI Framework |
| TypeScript | 5.9 | Dil |
| RxJS | 7.8 | Reaktif Programlama |
| SCSS | - | Glassmorphism Dark Theme |

---

## Proje Yapısı

```
gvn.gvnai.dictinary/
│
├── src/                                       ← .NET Backend (Clean Architecture)
│   ├── Gvn.GvnAI.Dictionary.API/             ← 8 Controller
│   │   └── Controllers/
│   │       ├── WordsController.cs             ← Kelime CRUD + AI Enrich
│   │       ├── SensesController.cs            ← Anlam yönetimi
│   │       ├── TranslationsController.cs      ← Çeviri yönetimi
│   │       ├── ExamplesController.cs          ← Örnek yönetimi
│   │       ├── QuizController.cs              ← Oyun (start, next, answer, complete)
│   │       ├── FavoritesController.cs         ← Favori kelimeler
│   │       ├── ProfileController.cs           ← Profil + istatistikler
│   │       ├── LookupsController.cs           ← Referans veriler
│   │       └── AuthController.cs              ← Kimlik doğrulama
│   ├── Gvn.GvnAI.Dictionary.Application/     ← CQRS Features
│   │   └── Features/
│   │       ├── Words/                         ← Create, Update, Delete, Enrich, Search
│   │       ├── Senses/                        ← Add, Update, Remove
│   │       ├── Translations/                  ← Add, Remove
│   │       ├── Examples/                      ← Add, Remove
│   │       ├── Quiz/                          ← Start, SubmitAnswer, Complete, Leaderboard
│   │       ├── Favorites/                     ← Add, Remove, List
│   │       ├── Profile/                       ← Get, Update
│   │       ├── Lookups/                       ← GetLookups
│   │       └── Auth/                          ← Register, Login
│   ├── Gvn.GvnAI.Dictionary.Domain/          ← 16 Entity
│   ├── Gvn.GvnAI.Dictionary.Domain.Shared/   ← 7 Enum, Constants, Errors
│   └── Gvn.GvnAI.Dictionary.Infrastructure/  ← EF Core, Claude AI, Hangfire
│
├── frontend/                                  ← Angular Frontend
│   ├── src/app/                               ← Root app (routing, navbar)
│   └── projects/gvn-dictionary/               ← Angular Library
│       └── src/lib/
│           ├── models/                        ← 8 model dosyası
│           ├── services/                      ← 9 service (auth, word, quiz, favorite, profile...)
│           ├── guards/                        ← Auth guard
│           ├── interceptors/                  ← JWT interceptor
│           └── modules/
│               ├── auth/                      ← Login, Register
│               └── dictionary/                ← 9 component
│                   ├── word-list/             ← Kart grid, filtre, sayfalama
│                   ├── word-detail/           ← Hero + Sense kartları + Favori
│                   ├── word-search/           ← Arama + filtreler
│                   ├── word-form/             ← EN kelime + TR çeviri formu
│                   ├── quiz/                  ← Kelime oyunu (timer, skor, animasyonlar)
│                   ├── profile/               ← Profil + istatistikler + favoriler
│                   ├── sense-form/            ← Anlam ekleme
│                   ├── translation-form/      ← Çeviri ekleme
│                   └── example-form/          ← Örnek ekleme
│
└── README.md
```

---

## API Endpoint'leri

### Kimlik Doğrulama & Profil

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|:----:|
| `POST` | `/api/auth/register` | Kullanıcı kaydı | - |
| `POST` | `/api/auth/login` | Giriş (JWT token) | - |
| `GET` | `/api/profile` | Profil + oyun istatistikleri | JWT |
| `PUT` | `/api/profile` | Profil güncelle | JWT |

### Kelimeler

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|:----:|
| `GET` | `/api/words` | Kelime listesi (sayfalı) | - |
| `GET` | `/api/words/{id}` | Kelime detayı (tam hiyerarşi) | - |
| `GET` | `/api/words/search?q=&lang=&pos=` | Gelişmiş arama | - |
| `POST` | `/api/words/with-translation` | Kelime + çeviri (tek adım) | JWT |
| `PUT` | `/api/words/{id}` | Kelime güncelle | JWT |
| `DELETE` | `/api/words/{id}` | Kelime sil (soft delete) | JWT |
| `POST` | `/api/words/{id}/enrich` | AI ile zenginleştir | JWT |

### Anlamlar, Çeviriler, Örnekler

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|:----:|
| `POST` | `/api/words/{wId}/senses` | Anlam ekle | JWT |
| `PUT` | `/api/words/{wId}/senses/{sId}` | Anlam güncelle | JWT |
| `DELETE` | `/api/words/{wId}/senses/{sId}` | Anlam sil | JWT |
| `POST` | `.../senses/{sId}/translations` | Çeviri ekle | JWT |
| `DELETE` | `.../translations/{tId}` | Çeviri sil | JWT |
| `POST` | `.../senses/{sId}/examples` | Örnek ekle | JWT |
| `DELETE` | `.../examples/{eId}` | Örnek sil | JWT |

### Kelime Oyunu

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|:----:|
| `POST` | `/api/quiz/start` | Yeni oyun başlat | JWT |
| `GET` | `/api/quiz/{sessionId}/next` | Sonraki soru | JWT |
| `POST` | `/api/quiz/{sessionId}/answer` | Cevap gönder | JWT |
| `POST` | `/api/quiz/{sessionId}/complete` | Oyunu bitir | JWT |
| `GET` | `/api/quiz/{sessionId}/result` | Oyun sonucu | JWT |
| `GET` | `/api/quiz/leaderboard` | Liderlik tablosu | - |

### Favoriler

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|:----:|
| `GET` | `/api/favorites` | Favori kelimeler | JWT |
| `POST` | `/api/favorites/{wordId}` | Favoriye ekle | JWT |
| `DELETE` | `/api/favorites/{wordId}` | Favoriden çıkar | JWT |
| `GET` | `/api/favorites/{wordId}/check` | Favori durumu | JWT |

### Lookup

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|:----:|
| `GET` | `/api/lookups` | Tüm referans verileri | - |

---

## Başlangıç

### Gereksinimler

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- [Docker](https://www.docker.com/) (PostgreSQL için)
- [Angular CLI](https://angular.dev/) (`npm install -g @angular/cli`)

### 1. PostgreSQL Başlat

```bash
docker run -d --name gvn-postgres \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=GvnAIDictionary \
  -p 5432:5432 \
  postgres:17-alpine
```

### 2. Veritabanını Oluştur

```bash
cd src/Gvn.GvnAI.Dictionary.API
dotnet tool install --global dotnet-ef
dotnet ef database update -p ../Gvn.GvnAI.Dictionary.Infrastructure
```

### 3. Seed Data

```bash
docker exec gvn-postgres psql -U postgres -d GvnAIDictionary -c "
INSERT INTO languages (\"Id\", \"Code\", \"Name\", \"NativeName\", \"Direction\") VALUES
  (gen_random_uuid(), 'tr', 'Turkish', 'Türkçe', 'LTR'),
  (gen_random_uuid(), 'en', 'English', 'English', 'LTR');
INSERT INTO parts_of_speech (\"Id\", \"Code\", \"Name\", \"Abbreviation\") VALUES
  (gen_random_uuid(), 'NOUN', 'Noun', 'n.'),
  (gen_random_uuid(), 'VERB', 'Verb', 'v.'),
  (gen_random_uuid(), 'ADJ', 'Adjective', 'adj.'),
  (gen_random_uuid(), 'ADV', 'Adverb', 'adv.');
INSERT INTO registers (\"Id\", \"Code\", \"Name\") VALUES
  (gen_random_uuid(), 'formal', 'Formal'),
  (gen_random_uuid(), 'informal', 'Informal'),
  (gen_random_uuid(), 'slang', 'Slang');
INSERT INTO domains (\"Id\", \"Code\", \"Name\") VALUES
  (gen_random_uuid(), 'everyday', 'Everyday'),
  (gen_random_uuid(), 'computing', 'Computing'),
  (gen_random_uuid(), 'medicine', 'Medicine');
"
```

### 4. API'yi Çalıştır

```bash
dotnet run --project src/Gvn.GvnAI.Dictionary.API
```

> **API:** http://localhost:5050 | **Scalar UI:** http://localhost:5050/scalar/v1 | **Hangfire:** http://localhost:5050/hangfire

### 5. Frontend'i Çalıştır

```bash
cd frontend && npm install && ng serve
```

> **Frontend:** http://localhost:4200

---

## Clean Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                      API Layer                               │
│  9 Controller, Middleware, CORS, JWT                          │
├─────────────────────────────────────────────────────────────┤
│                   Application Layer                          │
│  CQRS: Words, Senses, Translations, Examples,                │
│  Quiz, Favorites, Profile, Lookups, Auth                     │
├─────────────────────────────────────────────────────────────┤
│                    Domain Layer                              │
│  16 Entity, Events, Repository Interfaces                    │
│  Word → Sense → Translation hierarchy                        │
├─────────────────────────────────────────────────────────────┤
│                 Infrastructure Layer                          │
│  EF Core, PostgreSQL, Claude AI, Hangfire                    │
│  QuizService, ProfileQueryService, FavoriteQueryService       │
└─────────────────────────────────────────────────────────────┘
```

---

## Framework

Bu proje **[Gvn.GvnFramework](https://github.com/gvnuysal/Gvn.GvnFramework)** altyapısı üzerine inşa edilmiştir.

---

## Lisans

MIT License

---

<p align="center">
  <sub>Built with Gvn.GvnFramework + Claude AI</sub>
</p>
