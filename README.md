<p align="center">
  <img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet" />
  <img src="https://img.shields.io/badge/Angular-21-DD0031?style=for-the-badge&logo=angular" />
  <img src="https://img.shields.io/badge/PostgreSQL-17-4169E1?style=for-the-badge&logo=postgresql&logoColor=white" />
  <img src="https://img.shields.io/badge/Claude_AI-Anthropic-191919?style=for-the-badge&logo=anthropic" />
  <img src="https://img.shields.io/badge/Google_Translate-API-4285F4?style=for-the-badge&logo=googletranslate" />
  <img src="https://img.shields.io/badge/License-MIT-green?style=for-the-badge" />
</p>

# GvnAI Dictionary

**AI destekli, profesyonel bilingual (EN ↔ TR) sozluk ve kelime ogrenme platformu.**

> **Temel Prensip:** *"Kelimeyi degil, anlami cevir."*

---

## Ozellikler

### Sozluk
- **Word → Sense → Translation** mimarisi (Oxford/Cambridge standardı)
- Claude AI ile otomatik kelime zenginlestirme (anlam, ceviri, ornek, telaffuz, koken)
- Kelime ekleme: Ingilizce kelime yazinca AI otomatik doldurur (tanim + ceviri + tur)
- Mikrofon ile sesli kelime girisi (Web Speech API)
- Hoporlor butonu ile kelimenin okunusunu dinleme (Text-to-Speech)
- Kullanici bazli kelime yonetimi — herkes kendi kelimelerini gorur
- Favori sistemi

### Kelime Oyunu
- Ingilizce kelimeden Turkce karsiligini bul
- +5 dogru, -3 yanlis/sure dolma, 10 saniye geri sayim
- Sinirsiz soru — istediginiz zaman bitirin
- Dogru cevapta kelime karti modali (tum anlamlar, ceviriler, ornekler)
- Sonuclar ve puan DB'de saklanir
- Sadece kendi kelimelerinizden sorular

### Kullanici Profili
- Profil bilgileri duzenleme
- Oyun istatistikleri (toplam puan, basari orani, en iyi skor, vb.)
- Favori kelimeler
- API Key yonetimi (Claude AI / Google Translate)

### AI & Ceviri Entegrasyonu
- **Claude AI:** Tanim, ceviri, soz turu algilama, kelime zenginlestirme
- **Google Translate:** Cloud Translation API v2
- Kullanici bazli API key yonetimi — key'ler DB'de saklanir, appsettings'de degil
- Profil sayfasindan provider secimi ve key girisi

### Tema
- Dark / Light / Auto mod
- Otomatik: 18:00-06:00 karanlik, 06:00-18:00 aydinlik
- Glassmorphism UI tasarimi

---

## Mimari

```
┌─────────────────────────────────────────┐
│         Angular Frontend                 │
│    (gvn-dictionary library)              │
│      http://localhost:4200               │
└──────────────┬──────────────────────────┘
               │ HTTP/REST + JWT
┌──────────────▼──────────────────────────┐
│       ASP.NET Core API                   │
│     http://localhost:5050                │
└──────────────┬──────────────────────────┘
               │
  ┌────────────┼─────────────┐
  │            │             │
┌─▼──────┐ ┌──▼────────┐ ┌──▼─────────┐
│PostgreSQL│ │ Claude AI │ │Google Trans│
│ :5432   │ │(Anthropic)│ │  (opt.)    │
└─────────┘ └───────────┘ └────────────┘
```

---

## Veri Modeli

```
Word: "run" (VERB, English)
├── Sense 1: "To move swiftly on foot"
│   ├── Translation → "kosmak" (TR, Exact, %98)
│   └── Example: "She runs every morning." → "Her sabah kosar."
├── Sense 2: "To operate or manage something"
│   ├── Translation → "yonetmek" (TR, Near, %85)
│   └── Example: "He runs a company." → "Bir sirket yonetiyor."
├── Pronunciation: /rʌn/ (IPA)
└── Etymology: Old English "rinnan"
```

---

## Veritabani (19 tablo)

| Tablo | Aciklama |
|-------|----------|
| `words` | Kelimeler (kullanici bazli) |
| `senses` | Kelime anlamlari |
| `translations` | Anlam cevirileri |
| `examples` | Ornek cumleler |
| `pronunciations` | IPA telaffuz |
| `etymologies` | Kelime kokeni |
| `languages` | Dil tanimlari |
| `parts_of_speech` | Sozcuk turleri |
| `registers` | Kullanim kaydi |
| `domains` | Konu alani |
| `sense_synonyms` | Es anlamlilar |
| `sense_antonyms` | Zit anlamlilar |
| `word_relationships` | Kelime iliskileri |
| `Users` | Kullanici hesaplari + API key'ler |
| `quiz_sessions` | Oyun oturumlari |
| `quiz_answers` | Oyun cevaplari |
| `favorites` | Favori kelimeler |

---

## API Endpointleri (30+)

### Auth & Profil
| Method | Endpoint | Auth |
|--------|----------|:----:|
| `POST` | `/api/auth/register` | - |
| `POST` | `/api/auth/login` | - |
| `GET` | `/api/profile` | JWT |
| `PUT` | `/api/profile` | JWT |
| `PUT` | `/api/profile/api-settings` | JWT |

### Kelimeler
| Method | Endpoint | Auth |
|--------|----------|:----:|
| `GET` | `/api/words` | JWT |
| `GET` | `/api/words/{id}` | JWT |
| `GET` | `/api/words/search` | JWT |
| `POST` | `/api/words/with-translation` | JWT |
| `PUT` | `/api/words/{id}` | JWT |
| `DELETE` | `/api/words/{id}` | JWT |
| `POST` | `/api/words/{id}/enrich` | JWT |

### Anlamlar / Ceviriler / Ornekler
| Method | Endpoint | Auth |
|--------|----------|:----:|
| `POST/PUT/DELETE` | `/api/words/{wId}/senses/...` | JWT |
| `POST/DELETE` | `.../translations/...` | JWT |
| `POST/DELETE` | `.../examples/...` | JWT |

### Oyun
| Method | Endpoint | Auth |
|--------|----------|:----:|
| `POST` | `/api/quiz/start` | JWT |
| `GET` | `/api/quiz/{id}/next` | JWT |
| `POST` | `/api/quiz/{id}/answer` | JWT |
| `POST` | `/api/quiz/{id}/complete` | JWT |
| `GET` | `/api/quiz/leaderboard` | - |

### Favoriler
| Method | Endpoint | Auth |
|--------|----------|:----:|
| `GET` | `/api/favorites` | JWT |
| `POST` | `/api/favorites/{wordId}` | JWT |
| `DELETE` | `/api/favorites/{wordId}` | JWT |

### AI / Ceviri
| Method | Endpoint | Auth |
|--------|----------|:----:|
| `GET` | `/api/translate/define?word=` | JWT |
| `GET` | `/api/translate/ai-translate?word=` | JWT |
| `GET` | `/api/translate/detect-pos?word=` | JWT |
| `GET` | `/api/translate/auto?word=` | JWT |
| `GET` | `/api/translate/google?text=` | JWT |
| `GET` | `/api/lookups` | - |

---

## Proje Yapisi

```
gvn.gvnai.dictinary/
├── src/                              ← .NET Backend
│   ├── API/Controllers/              ← 10 controller
│   ├── Application/Features/         ← CQRS: Words, Senses, Quiz, Favorites, Profile, Auth, Lookups
│   ├── Domain/Entities/              ← 18 entity
│   └── Infrastructure/               ← EF Core, Claude AI, Google Translate, Quiz, Hangfire
│
├── frontend/                         ← Angular Frontend
│   ├── src/app/                      ← Root (routing, navbar, theme toggle)
│   └── projects/gvn-dictionary/      ← Angular Library
│       └── src/lib/
│           ├── models/               ← 9 model dosyasi
│           ├── services/             ← 12 service
│           ├── guards/               ← Auth guard
│           ├── interceptors/         ← JWT interceptor
│           └── modules/
│               ├── auth/             ← Login, Register (split-screen)
│               └── dictionary/       ← 10 component
│                   ├── home/         ← Landing page
│                   ├── word-list/    ← Kart grid
│                   ├── word-detail/  ← Hero + senses + TTS
│                   ├── word-form/    ← AI auto-fill + mic + TTS
│                   ├── quiz/         ← Oyun + modal + TTS
│                   ├── profile/      ← Stats + API settings + favoriler
│                   └── ...forms/     ← sense, translation, example
│
└── README.md
```

---

## Baslangic

### Gereksinimler
- .NET 10 SDK
- Node.js 20+
- Docker (PostgreSQL icin)
- Angular CLI (`npm install -g @angular/cli`)

### 1. PostgreSQL
```bash
docker run -d --name gvn-postgres \
  -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=GvnAIDictionary -p 5432:5432 postgres:17-alpine
```

### 2. Veritabani
```bash
dotnet tool install --global dotnet-ef
cd src/Gvn.GvnAI.Dictionary.API
dotnet ef database update -p ../Gvn.GvnAI.Dictionary.Infrastructure
```

### 3. Seed Data
```bash
docker exec gvn-postgres psql -U postgres -d GvnAIDictionary -c "
INSERT INTO languages (\"Id\",\"Code\",\"Name\",\"NativeName\",\"Direction\") VALUES
  (gen_random_uuid(),'tr','Turkish','Turkce','LTR'),
  (gen_random_uuid(),'en','English','English','LTR');
INSERT INTO parts_of_speech (\"Id\",\"Code\",\"Name\",\"Abbreviation\") VALUES
  (gen_random_uuid(),'NOUN','Noun','n.'),(gen_random_uuid(),'VERB','Verb','v.'),
  (gen_random_uuid(),'ADJ','Adjective','adj.'),(gen_random_uuid(),'ADV','Adverb','adv.');
INSERT INTO registers (\"Id\",\"Code\",\"Name\") VALUES
  (gen_random_uuid(),'formal','Formal'),(gen_random_uuid(),'informal','Informal');
INSERT INTO domains (\"Id\",\"Code\",\"Name\") VALUES
  (gen_random_uuid(),'everyday','Everyday'),(gen_random_uuid(),'computing','Computing');
"
```

### 4. API
```bash
dotnet run --project src/Gvn.GvnAI.Dictionary.API
```
> API: http://localhost:5050 | Scalar: /scalar/v1 | Hangfire: /hangfire

### 5. Frontend
```bash
cd frontend && npm install && ng serve
```
> Frontend: http://localhost:4200

### 6. API Key Ayarlama
1. Kayit olun ve giris yapin
2. Profil sayfasina gidin (`/profile`)
3. **API Ayarlari** bolumunden Claude API key'inizi girin
4. Kelime eklerken AI otomatik dolduracak

---

## Teknoloji

| Backend | Frontend |
|---------|----------|
| .NET 10, ASP.NET Core | Angular 21, TypeScript 5.9 |
| EF Core 10, PostgreSQL 17 | RxJS 7.8, SCSS |
| MediatR, FluentValidation | Web Speech API (STT/TTS) |
| Anthropic SDK (Claude AI) | Glassmorphism Dark Theme |
| Hangfire, Serilog, JWT | Standalone Components |

---

## Framework

[Gvn.GvnFramework](https://github.com/gvnuysal/Gvn.GvnFramework) uzerine insa edilmistir.

---

## Lisans

MIT License

<p align="center"><sub>Built with Gvn.GvnFramework + Claude AI</sub></p>
