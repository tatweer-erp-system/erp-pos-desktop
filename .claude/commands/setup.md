# Setup all projects

Install dependencies for all projects in the monorepo.

## Steps

1. Install erp-api dependencies: `cd erp-api && npm install`
2. Install erp-web dependencies: `cd erp-web && pnpm install`
3. Install erp-backoffice dependencies: `cd erp-backoffice && pnpm install`
4. Install erp-pos-web dependencies: `cd erp-pos-web && pnpm install`
5. For erp-pos-desktop, run `dotnet restore` if a .csproj/.sln is present
6. Report any install errors or warnings
7. Verify Docker infrastructure: check if `erp-api/docker-compose.yml` services are available
