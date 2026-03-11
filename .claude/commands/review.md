# Code review

Review the current changes for quality, security, and adherence to project conventions.

## Steps

1. Run `git diff` to see all unstaged changes, and `git diff --cached` for staged changes
2. For each changed file, check:
   - **Security:** No hardcoded secrets, API keys, credentials, or .env values
   - **i18n:** No hardcoded user-facing strings — must use the project's i18n system
   - **Types:** No `any` types in TypeScript unless absolutely necessary
   - **Package managers:** Correct package manager used per project (npm for API, pnpm for frontends)
   - **API conventions:** All API calls through service layer, endpoints under `/api/v1/`
   - **Code style:** Follows existing patterns in the codebase
3. Check for common issues:
   - Console.log / debugger statements left in
   - TODO/FIXME comments that should be addressed
   - Unused imports or variables
4. Report findings organized by severity (critical, warning, suggestion)
