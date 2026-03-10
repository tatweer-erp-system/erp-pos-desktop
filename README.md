# Tatweer ERP System

Multi-app ERP workspace. All projects live in this root directory and run independently.

## Projects

| Project | Path | Stack | Dev Port | API Target |
|---|---|---|---|---|
| **erp-api** | `erp-api/` | NestJS 10 + Sequelize + PostgreSQL 16 | 3000 | — |
| **erp-web** | `erp-web/` | React 19 + Vite 7 + TypeScript | 4200 | localhost:3000 |
| **erp-pos-web** | `erp-pos-web/` | React 19 + Vite 7 + TypeScript | 4201 | localhost:4000/api |
| **erp-backoffice** | `erp-backoffice/` | React 19 + Vite 7 + TypeScript | 4202 | localhost:4300 |
| **erp-mobile** | `erp-mobile/` | TBD | — | — |
| **erp-pos-desktop** | `erp-pos-desktop/` | TBD | — | — |

> **Note:** `erp-mobile` and `erp-pos-desktop` are empty — development will start after the other projects are ready.

## erp-api (Backend)

- **Framework:** NestJS 10.3.3 with TypeScript 5.4.2
- **ORM:** Sequelize (sequelize-typescript) — schema-per-tenant multi-tenancy
- **Database:** PostgreSQL 16 on port 5432 (database: `erp_core`)
- **Cache:** Redis on port 6379
- **Queue:** BullMQ with Redis on port 6380
- **Auth:** JWT (15m access) + refresh tokens (7d)
- **File Storage:** AWS S3
- **Chat:** Firebase Firestore
- **Notifications:** Firebase FCM + Twilio SMS + Nodemailer (SendGrid)
- **PDF:** Puppeteer
- **i18n:** Arabic + English
- **Error Tracking:** Sentry
- **Logging:** Winston
- **API Prefix:** `/api/v1`
- **Swagger Docs:** `/api/v1/docs`
- **Package Manager:** npm
- **Entry:** `src/main.ts`
- **Docker:** `docker-compose.yml` (PostgreSQL + 2x Redis + App)

### Key Directories
```
erp-api/src/
├── config/          # app, database, redis, jwt, firebase, storage, payment configs
├── database/        # migrations (Umzug), entities
├── common/          # decorators, guards, interceptors, filters, pipes, middleware
├── infrastructure/  # audit, cache, firebase, mail, pdf, queues, storage, websockets
├── modules/         # auth, chat, crm, hr, inventory, notifications, projects,
│                    # purchasing, reporting, roles, subscriptions, tenants, users
├── health/          # health check endpoints
└── i18n/            # en/, ar/ translation files
```

### Run
```bash
cd erp-api
npm install
# configure .env from .env.example
docker-compose up -d   # starts postgres + redis
npm run start:dev
```

## Frontend Projects (erp-web, erp-pos-web, erp-backoffice)

All share the same base stack:
- **React 19** + **Vite 7** + **TypeScript 5.6**
- **UI:** Ant Design 6 (primary) + shadcn/ui (Radix primitives)
- **Styling:** Tailwind CSS 4
- **Routing:** wouter 3 (lightweight router)
- **State:** Zustand 5 (client) + React Query 5 (server)
- **Forms:** react-hook-form 7 + zod validation
- **HTTP:** axios
- **Charts:** recharts
- **Animations:** framer-motion
- **Package Manager:** pnpm (corepack enabled)

### Folder Convention (all frontends)
```
<project>/client/src/
├── main.tsx         # entry point
├── App.tsx          # root component, providers, routing
├── index.css        # tailwind + global styles
├── components/      # UI components (layout, common, shadcn primitives)
├── contexts/        # React contexts (auth, settings)
├── hooks/           # custom hooks
├── lib/             # api client, routes, utils, constants
├── modules/         # feature modules
├── pages/           # page components
├── services/        # API service functions
├── store/           # Zustand stores
└── types/           # TypeScript type definitions
```

### Path Aliases
- `@` → `client/src/`
- `@shared` → `shared/`
- `@assets` → `attached_assets/` (erp-web only)

### Run any frontend
```bash
cd <project>
pnpm install
# configure .env from .env.example
pnpm dev
```

## erp-web Specifics
- Main tenant-facing web app
- Google Maps integration
- OAuth support (VITE_OAUTH_PORTAL_URL)
- Analytics integration
- Streaming support (streamdown)
- Full i18n (client/src/i18n/)

## erp-pos-web Specifics
- Point of Sale web interface
- IndexedDB via `idb` for offline data
- Connects to a separate API at port 4000

## erp-backoffice Specifics
- Platform admin dashboard (manage tenants, plans, subscriptions)
- Has its own `CLAUDE.md` with detailed architecture docs
- Production served via Express static server on port 4300

## Infrastructure

- **PostgreSQL 16:** Schema-per-tenant architecture. Each tenant gets its own schema in the `erp_core` database.
- **Redis (cache):** Port 6379 — general caching
- **Redis (queue):** Port 6380 — BullMQ job processing
- **Docker:** `erp-api/docker-compose.yml` manages all infrastructure services
# erp-pos-desktop
