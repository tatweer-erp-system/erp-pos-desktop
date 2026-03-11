namespace TatweerPOS.Services;

/// <summary>
/// Singleton navigation service that manages page switching and navigation history.
/// Swaps ContentControl content in MainWindow by resolving ViewModel types from a registry.
/// </summary>
public class NavigationService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Stack<string> _history = new();
    private readonly Dictionary<string, Type> _pageViewModelMap = new();

    private object? _currentPage;
    private string _currentPageTitle = "";

    public object? CurrentPage
    {
        get => _currentPage;
        private set
        {
            _currentPage = value;
            PageChanged?.Invoke(value!);
        }
    }

    public string CurrentPageTitle
    {
        get => _currentPageTitle;
        private set => _currentPageTitle = value;
    }

    public bool CanGoBack => _history.Count > 0;

    public event Action<object>? PageChanged;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Registers a page name to its ViewModel type for later resolution.
    /// </summary>
    public void RegisterPage(string pageName, Type viewModelType)
    {
        _pageViewModelMap[pageName] = viewModelType;
    }

    /// <summary>
    /// Registers multiple page-to-ViewModel mappings at once.
    /// </summary>
    public void RegisterPages(Dictionary<string, Type> mappings)
    {
        foreach (var kvp in mappings)
        {
            _pageViewModelMap[kvp.Key] = kvp.Value;
        }
    }

    /// <summary>
    /// Navigates to a named page by resolving its ViewModel from the DI container.
    /// Pushes current page onto the history stack before navigating.
    /// </summary>
    public void NavigateTo(string pageName)
    {
        if (!_pageViewModelMap.TryGetValue(pageName, out var viewModelType))
        {
            throw new InvalidOperationException(
                $"No ViewModel registered for page '{pageName}'. " +
                $"Call RegisterPage(\"{pageName}\", typeof(YourViewModel)) first.");
        }

        // Push current page to history if we have one
        if (_currentPageTitle is { Length: > 0 })
        {
            _history.Push(_currentPageTitle);
        }

        CurrentPageTitle = pageName;
        CurrentPage = _serviceProvider.GetService(viewModelType)
            ?? throw new InvalidOperationException(
                $"Could not resolve ViewModel type '{viewModelType.Name}' from DI container.");
    }

    /// <summary>
    /// Navigates back to the previous page in the history stack.
    /// </summary>
    public void GoBack()
    {
        if (!CanGoBack) return;

        var previousPage = _history.Pop();
        CurrentPageTitle = previousPage;

        if (_pageViewModelMap.TryGetValue(previousPage, out var viewModelType))
        {
            CurrentPage = _serviceProvider.GetService(viewModelType);
        }
    }

    /// <summary>
    /// Clears the navigation history stack.
    /// </summary>
    public void ClearHistory()
    {
        _history.Clear();
    }
}
