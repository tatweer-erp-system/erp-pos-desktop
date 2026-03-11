# Create a new service

Create a new service named `$ARGUMENTS`.

## Steps

1. Read existing services to understand the project's DI and patterns
2. Create `Services/<Name>Service.cs` with:
   - Interface `I<Name>Service` for DI abstraction
   - Implementation class with constructor injection
   - Proper async/await patterns (never `Thread.Sleep`)
   - Serilog logging for errors and key operations
   - Audit logging for significant actions via `AuditService`
3. Register in DI container in `App.xaml.cs`:
   - `AddSingleton` for stateful services
   - `AddTransient` for stateless services
4. If the service mutates data, integrate with `SyncQueue`
5. Add cache support via `CacheService` if applicable
6. Run `dotnet build` to verify compilation
