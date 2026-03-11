# Add animations to a view

Add entrance and interaction animations to `$ARGUMENTS` (view name).

## Steps

1. Read the target View XAML to identify all major UI sections
2. Add staggered entrance animation on page load:
   - Use `AnimationHelper.StaggeredEntrance()` in code-behind or ViewModel
   - Elements slide up (30px) + fade in sequentially
   - 60-80ms delay between each element
3. Add interaction feedback:
   - Buttons: MaterialDesign ripple + hover glow
   - Cards: lift on hover (shadow + translateY)
   - Inputs: border color animation on focus
   - Destructive actions: shake animation on error
4. Follow the standard durations from CLAUDE.md:
   - Micro: 100ms, Fast: 150ms, Normal: 200ms, Page: 300ms
5. Use CubicEase Out for transitions, BackEase Out for entrances
6. Run `dotnet build` to verify
