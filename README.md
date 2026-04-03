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

Profesyonel sözlüklerin kullandığı **Word → Sense → Translation** mimarisi:

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

### Entity İlişki Diyagramı

```
┌──────────────┐     ┌──────────────┐     ┌──────────────────┐
│   Language    │     │ PartOfSpeech │     │    Register      │
│ (tr, en, de) │     │(NOUN,VERB..) │     │(formal, slang..) │
└──────┬───────┘     └──────┬───────┘     └────────┬─────────┘
       │                    │                       │
       │  ┌─────────────────┼───────────────────────┘
       │  │                 │
┌──────▼──▼─────────────────▼──┐
│           WORD               │
│  lemma, status, frequency,   │
│  difficulty, isCompound...   │
│  (AggregateRoot, SoftDelete) │
└──────┬───────────┬───────────┘
       │           │
┌──────▼───┐ ┌─────▼──────────┐  ┌──────────────┐
│ Pronun.  │ │   SENSE        │  │  Etymology   │
│ (IPA)    │ │  definition,   │  │  (origin)    │
└──────────┘ │  senseNumber   │  └──────────────┘
             └──┬─────────┬───┘
                │         │
      ┌─────────▼──┐ ┌───▼──────────┐
      │ TRANSLATION│ │   EXAMPLE    │
      │ text, eq., │ │ sourceText,  │
      │ confidence │ │ targetText   │
      └────────────┘ └──────────────┘
```

---

## Veritabanı Tabloları

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
| `sense_synonyms` | Anlam düzeyinde eş anlamlılar | İlişki |
| `sense_antonyms` | Anlam düzeyinde zıt anlamlılar | İlişki |
| `word_relationships` | Kelime ilişkileri (türetme, üst kavram) | İlişki |
| `Users` | Kullanıcı hesapları | AggregateRoot |

---

## Teknoloji Stack

### Backend

| Teknoloji | Versiyon | Kullanım |
|-----------|----------|----------|
| .NET | 10.0 | Runtime |
| ASP.NET Core | 10.0 | Web API Framework |
| Entity Framework Core | 10.0.5 | ORM |
| PostgreSQL | 17 | Veritabanı |
| MediatR | 12.4.1 | CQRS / Mediator |
| FluentValidation | 11.11.0 | Doğrulama |
| Anthropic SDK | 3.3.0 | Claude AI Entegrasyonu |
| Hangfire | 1.8+ | Arka Plan Görevleri |
| Serilog | 9.x | Yapılandırılmış Loglama |
| Scalar | 2.x | API Dokümantasyonu |
| JWT Bearer | - | Kimlik Doğrulama |

### Frontend

| Teknoloji | Versiyon | Kullanım |
|-----------|----------|----------|
| Angular | 21.2 | UI Framework |
| TypeScript | 5.9 | Programlama Dili |
| RxJS | 7.8 | Reaktif Programlama |
| ng-packagr | 21.2 | Library Build |
| SCSS | - | Stil |

### Altyapı

| Servis | Adres | Açıklama |
|--------|-------|----------|
| API | `http://localhost:5050` | REST API |
| Frontend | `http://localhost:4200` | Angular SPA |
| PostgreSQL | `localhost:5432` | Veritabanı |
| Redis | `localhost:6379` | Cache (opsiyonel) |
| Scalar UI | `/scalar/v1` | API Dokümantasyonu |
| Hangfire | `/hangfire` | İş Dashboard'u |

---

## Proje Yapısı

```
gvn.gvnai.dictinary/
│
├── src/                                       ← .NET Backend (Clean Architecture)
│   ├── Gvn.GvnAI.Dictionary.API/             ← Presentation Layer
│   │   ├── Controllers/                       ← 6 controller (Words, Senses, Translations...)
│   │   ├── Program.cs                         ← Startup + middleware
│   │   └── appsettings.json
│   ├── Gvn.GvnAI.Dictionary.Application/     ← Application Layer (CQRS)
│   │   ├── Features/                          ← Commands & Queries
│   │   ├── DTOs/                              ← Data Transfer Objects
│   │   └── Mappings/                          ← Entity → DTO
│   ├── Gvn.GvnAI.Dictionary.Domain/          ← Domain Layer (DDD)
│   │   ├── Entities/                          ← 14 entity (Word, Sense, Translation...)
│   │   ├── Events/                            ← Domain events
│   │   └── Repositories/                      ← Interfaces
│   ├── Gvn.GvnAI.Dictionary.Domain.Shared/   ← Shared Kernel
│   │   ├── Enums/                             ← 7 enum
│   │   └── Errors/                            ← Domain errors
│   └── Gvn.GvnAI.Dictionary.Infrastructure/  ← Infrastructure Layer
│       ├── Persistence/                       ← EF Core, 13 configuration
│       ├── Services/ClaudeAiDictionaryService ← AI entegrasyonu
│       └── Jobs/EnrichPendingWordsJob         ← Toplu AI zenginleştirme
│
├── frontend/                                  ← Angular Frontend
│   ├── src/app/                               ← Root app (routing, navbar)
│   └── projects/gvn-dictionary/               ← Angular Library
│       └── src/lib/
│           ├── models/                        ← TypeScript DTO'lar
│           ├── services/                      ← API servisleri (7 service)
│           ├── guards/                        ← Auth guard
│           ├── interceptors/                  ← JWT interceptor
│           └── modules/
│               ├── auth/                      ← Login, Register
│               └── dictionary/                ← 7 component (list, detail, search, forms)
│
└── README.md
```

---

## API Endpoint'leri

### Kimlik Doğrulama

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|:----:|
| `POST` | `/api/auth/register` | Kullanıcı kaydı | - |
| `POST` | `/api/auth/login` | Giriş (JWT token al) | - |

### Kelimeler

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|:----:|
| `GET` | `/api/words` | Kelime listesi (sayfalı) | - |
| `GET` | `/api/words/{id}` | Kelime detayı (tam hiyerarşi) | - |
| `GET` | `/api/words/search?q=&lang=&pos=` | Gelişmiş arama | - |
| `POST` | `/api/words` | Yeni kelime | JWT |
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
# Proje dizinine git
cd src/Gvn.GvnAI.Dictionary.API

# EF Core tools yükle (ilk seferde)
dotnet tool install --global dotnet-ef

# Migration uygula
dotnet ef database update -p ../Gvn.GvnAI.Dictionary.Infrastructure

# Seed data ekle
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

### 3. API'yi Çalıştır

```bash
dotnet run --project src/Gvn.GvnAI.Dictionary.API
```

> **API:** http://localhost:5050
> **Scalar UI:** http://localhost:5050/scalar/v1
> **Hangfire:** http://localhost:5050/hangfire

### 4. Frontend'i Çalıştır

```bash
cd frontend
npm install
ng serve
```

> **Frontend:** http://localhost:4200

---

## Kullanım Örnekleri

### Kayıt & Giriş

```bash
# Kayıt ol
curl -X POST http://localhost:5050/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"Pass123!","fullName":"Test User"}'

# Giriş yap → JWT token al
curl -X POST http://localhost:5050/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"Pass123!"}'
```

### Kelime Ekleme (EN + TR tek adımda)

```bash
curl -X POST http://localhost:5050/api/words/with-translation \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <TOKEN>" \
  -d '{
    "lemma": "love",
    "partOfSpeechId": "<NOUN_ID>",
    "definition": "A deep feeling of affection",
    "translationText": "aşk"
  }'
```

### AI ile Zenginleştirme

```bash
curl -X POST "http://localhost:5050/api/words/<ID>/enrich?targetLanguageCode=tr" \
  -H "Authorization: Bearer <TOKEN>"
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

`appsettings.json`'da Claude API key yapılandırması:

```json
{
  "Claude": {
    "ApiKey": "sk-ant-...",
    "Model": "claude-sonnet-4-20250514"
  }
}
```

---

## Clean Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                      API Layer                               │
│  Controllers, Middleware, Program.cs                          │
│  (ASP.NET Core, Scalar, Hangfire Dashboard, CORS)            │
├─────────────────────────────────────────────────────────────┤
│                   Application Layer                          │
│  CQRS Commands/Queries, Handlers, Validators, DTOs           │
│  (MediatR, FluentValidation)                                 │
├─────────────────────────────────────────────────────────────┤
│                    Domain Layer                              │
│  Entities, AggregateRoots, Events, Repository Interfaces     │
│  (DDD: Word → Sense → Translation hierarchy)                 │
├─────────────────────────────────────────────────────────────┤
│                 Domain.Shared Layer                           │
│  Enums, Constants, Error Definitions                         │
├─────────────────────────────────────────────────────────────┤
│                 Infrastructure Layer                          │
│  EF Core, PostgreSQL, Repositories, Claude AI, Hangfire      │
│  (Npgsql, Anthropic SDK)                                     │
└─────────────────────────────────────────────────────────────┘
```

---

## Framework

Bu proje **[Gvn.GvnFramework](https://github.com/gvnuysal/Gvn.GvnFramework)** altyapısı üzerine inşa edilmiştir:

| Modül | Katkı |
|-------|-------|
| `Gvn.GvnFramework.Core` | Result pattern, Guard, Exceptions |
| `Gvn.GvnFramework.Domain` | Entity, AggregateRoot, Repository interfaces |
| `Gvn.GvnFramework.Application` | CQRS abstractions, MediatR pipeline |
| `Gvn.GvnFramework.EntityFramewokCore` | EF Core repository, UnitOfWork |
| `Gvn.GvnFramework.AspNetCore` | ApiControllerBase, Exception middleware |
| `Gvn.GvnFramework.Security` | JWT, BCrypt, CurrentUser |
| `Gvn.GvnFramework.Caching` | Redis / Memory cache |
| `Gvn.GvnFramework.BackgroundJobs` | Hangfire integration |
| `Gvn.GvnFramework.Swagger` | Scalar / OpenAPI |

---

## Lisans

MIT License

---

<p align="center">
  <sub>Built with ❤️ using Gvn.GvnFramework + Claude AI</sub>
</p>
