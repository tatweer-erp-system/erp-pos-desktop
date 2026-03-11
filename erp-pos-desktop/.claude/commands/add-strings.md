# Add localization strings

Add localization strings for `$ARGUMENTS` (feature or view name).

## Steps

1. Read `Resources/Strings.xaml` to understand the existing key structure
2. Search the target View XAML for any hardcoded text
3. For each hardcoded string:
   - Add `<sys:String x:Key="str_KeyName">English text</sys:String>`
   - Add `<sys:String x:Key="str_KeyName_AR">Arabic text</sys:String>`
   - Replace hardcoded text with `{StaticResource str_KeyName}`
4. Arabic translations must be proper Arabic, not transliterated English
5. Verify RTL FlowDirection is handled for Arabic
6. Run `dotnet build` to verify
