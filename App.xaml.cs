using System.IO;
using System.Windows;

namespace SecurityApp;

public partial class App : Application
{
    private readonly string folder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WpfSecurity10");
    private string ThemeFile => Path.Combine(folder, "theme.txt");
    public bool Dark { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        try
        {
            if (File.Exists(ThemeFile) && File.ReadAllText(ThemeFile) == "dark")
                ApplyTheme(true);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException) { }
        base.OnStartup(e);
    }

    private void ApplyTheme(bool dark)
    {
        string file = dark ? "Dark.xaml" : "Light.xaml";
        Resources.MergedDictionaries[0] = new ResourceDictionary
        {
            Source = new Uri($"pack://application:,,,/SecurityApp;component/Themes/{file}", UriKind.Absolute)
        };
        Dark = dark;
    }

    public void ToggleTheme()
    {
        ApplyTheme(!Dark);
        Directory.CreateDirectory(folder);
        File.WriteAllText(ThemeFile, Dark ? "dark" : "light");
    }
}
