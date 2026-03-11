# CLAUDE.md — Tatweer ERP System (Monorepo Root)

## Project Overview
Multi-app ERP system. Each project lives in its own directory with its own `CLAUDE.md` for project-specific rules. This file covers cross-cutting rules.

## Monorepo Structure
| Project | Stack | Package Manager |
|---|---|---|
| `erp-api/` | NestJS 10 + Sequelize + PostgreSQL 16 | npm |
| `erp-web/` | React 19 + Vite 7 + TypeScript | pnpm |
| `erp-pos-web/` | React 19 + Vite 7 + TypeScript | pnpm |
| `erp-backoffice/` | React 19 + Vite 7 + TypeScript | pnpm |
| `erp-pos-desktop/` | WPF + .NET 8 + C# 12 | NuGet |

## Critical Rules

### 1. Respect per-project package managers
- `erp-api/` uses **npm** — never use pnpm/yarn there
- All frontend projects use **pnpm** — never use npm/yarn there
- `erp-pos-desktop/` uses **NuGet** / dotnet CLI

### 2. Never commit secrets
- Never commit `.env` files, API keys, credentials, or tokens
- Use `.env.example` for documenting required env vars

### 3. Language & i18n
- All user-facing strings must support **English + Arabic**
- Arabic requires RTL layout support
- Never hardcode UI strings — use the project's i18n system

### 4. Code style
- TypeScript: strict mode, no `any` unless absolutely necessary
- Use conventional commits (e.g., `feat:`, `fix:`, `chore:`)
- Run formatters/linters before committing

### 5. Git branching strategy
All projects follow the same branching model:

| Branch | Purpose | Merges into |
|--------|---------|-------------|
| `prod` | Production-ready code (protected) | — |
| `stg` | Pre-release testing & QA | `prod` |
| `dev` | Integration branch for features | `stg` |
| `feat/<name>` | New feature development | `dev` |
| `fix/<name>` | Bug fixes | `dev` |
| `hotfix/<name>` | Urgent production fixes | `prod` + `dev` |
| `chore/<name>` | Maintenance, tooling, CI/CD | `dev` |
| `refactor/<name>` | Code refactoring | `dev` |

**Rules:**
- Never push directly to `prod` or `stg` — always use PRs
- Branch names must be lowercase, kebab-case (e.g., `feat/invoice-export`)
- Delete branches after merge
- Keep feature branches short-lived (< 1 week ideally)
- Rebase feature branches on `dev` before opening a PR
- Hotfixes branch from `prod`, merge back into both `prod` and `dev`
- Use conventional commits in all commit messages

### 6. API conventions
- Backend API prefix: `/api/v1`
- All API calls from frontends go through a service layer, never directly in components
- Use axios instances configured per project

### 7. Architecture
- Backend: schema-per-tenant multi-tenancy in PostgreSQL
- Frontend state: server state in React Query, client state in Zustand/Context
- All route components are lazy-loaded

### 8. Before modifying any sub-project
- Read that project's `CLAUDE.md` first for specific rules
- Understand the existing patterns before adding new code

## Common Commands

### erp-api
```bash
cd erp-api && npm install && npm run start:dev    # dev server on :3000
npm run format                                     # mandatory before push
```

### Any frontend (erp-web, erp-pos-web, erp-backoffice)
```bash
cd <project> && pnpm install && pnpm dev           # dev server
pnpm build                                         # production build
pnpm lint                                          # lint check
```

## Database
- PostgreSQL 16 on port 5432 (database: `erp_core`)
- Redis cache on port 6379, Redis queue on port 6380
- Docker: `erp-api/docker-compose.yml` manages infrastructure
