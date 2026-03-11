# Monorepo status check

Check the health and status of all projects in the monorepo.

## Steps

1. Show current git branch and recent commits: `git log --oneline -5`
2. Show git status for uncommitted changes: `git status`
3. For each project, check if dependencies are installed:
   - `erp-api/`: check if `node_modules` exists
   - `erp-web/`: check if `node_modules` exists
   - `erp-backoffice/`: check if `node_modules` exists
   - `erp-pos-web/`: check if `node_modules` exists
4. Report a summary table of each project's status
