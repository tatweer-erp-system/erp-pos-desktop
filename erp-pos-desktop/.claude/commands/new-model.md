# Create a new data model

Create a new model named `$ARGUMENTS`.

## Steps

1. Read existing models to follow the naming and structure conventions
2. Create `Models/<Name>.cs` with:
   - Proper C# 12 features (primary constructors, records where appropriate)
   - JSON serialization attributes (Newtonsoft.Json)
   - Validation attributes if needed
3. If this is a display model, add it to the relevant ViewModel
4. Add mock data entries in the MockData class following realistic data guidelines
5. Run `dotnet build` to verify compilation
