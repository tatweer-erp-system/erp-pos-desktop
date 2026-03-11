# Create a new View + ViewModel

Create a new View and ViewModel pair named `$ARGUMENTS`.

## Steps

1. Read existing Views and ViewModels to understand the project's patterns
2. Create the ViewModel in the ViewModels directory:
   - Inherit from `ObservableObject` (CommunityToolkit.Mvvm)
   - Use `[ObservableProperty]` for bindable properties
   - Use `[RelayCommand]` for commands
   - Add `IsLoading` property for async operations
   - Follow the error handling pattern from CLAUDE.md
3. Create the View (XAML) in the Views directory:
   - Use MaterialDesignInXAML + HandyControl styles — never default WPF
   - All colors via `{StaticResource}` keys — never hardcode hex
   - All strings via `{StaticResource str_*}` — never hardcode text
   - Add staggered entrance animations using `AnimationHelper`
   - Add loading skeleton, empty state, and error state
4. Add localization strings in `Resources/Strings.xaml` (English + Arabic `_AR` suffix)
5. Register the ViewModel in DI container (`App.xaml.cs`)
6. Add navigation entry in `NavigationService` if needed
7. Run `dotnet build` to verify compilation
