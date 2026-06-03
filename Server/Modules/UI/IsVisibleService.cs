namespace Fracture.Server.Modules.UI;

public class IsVisibleService
{
    public event Action? OnChange;

    public bool IsVisible { get; private set; }
    public Type? ComponentType { get; private set; }
    public IDictionary<string, object>? Parameters { get; private set; }

    public void Show<T>(IDictionary<string, object>? parameters = null)
    {
        ComponentType = typeof(T);
        Parameters = parameters;
        IsVisible = true;
        NotifyStateChanged();
    }

    public void Hide()
    {
        IsVisible = false;
        ComponentType = null;
        Parameters = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }
}
