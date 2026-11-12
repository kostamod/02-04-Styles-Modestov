using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SecurityApp;

public partial class MainWindow : Window
{
    private readonly DispatcherTimer timer = new() { Interval = TimeSpan.FromMilliseconds(220) };
    private int step;

    public MainWindow()
    {
        InitializeComponent();
        timer.Tick += Tick;
        Protection.Unchecked += (_, _) =>
        {
            if (timer.IsEnabled) Finish("Проверка остановлена: защита отключена.");
        };
        Closed += (_, _) => timer.Stop();
    }

    private void Theme_Click(object sender, RoutedEventArgs e)
    {
        try { ((App)Application.Current).ToggleTheme(); }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MessageBox.Show("Не удалось записать настройку темы.");
        }
    }

    private void Start_Click(object sender, RoutedEventArgs e)
    {
        Progress.Value = 0;
        step = Quick.IsChecked == true ? 10 : 5;
        var target = (ComboBoxItem)Target.SelectedItem;
        Status.Text = $"Учебная проверка: {target.Content}";
        StopButton.IsEnabled = true;
        timer.Start();
    }

    private void Tick(object? sender, EventArgs e)
    {
        Progress.Value += step;
        if (Progress.Value >= Progress.Maximum) Finish("Учебная проверка завершена.");
    }

    private void Stop_Click(object sender, RoutedEventArgs e) => Finish("Проверка остановлена.");

    private void Finish(string message)
    {
        timer.Stop();
        StopButton.IsEnabled = false;
        Status.Text = message;
    }

    private void Disable_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("Отключить защиту на учебной панели?", "Панель защиты",
            MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            Protection.IsChecked = false;
            Status.Text = "Учебная защита отключена.";
        }
    }
}
