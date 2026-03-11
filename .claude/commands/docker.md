# Docker infrastructure management

Manage Docker infrastructure services: `$ARGUMENTS` (up, down, restart, logs, status).

## Steps

1. Navigate to `erp-api/` where docker-compose.yml lives
2. Based on the argument:
   - `up` or empty: `docker compose up -d` — start all services (PostgreSQL, Redis)
   - `down`: `docker compose down` — stop all services
   - `restart`: `docker compose restart` — restart all services
   - `logs`: `docker compose logs --tail=50` — show recent logs
   - `status`: `docker compose ps` — show running services
3. Report the status of each service (PostgreSQL on 5432, Redis on 6379/6380)
