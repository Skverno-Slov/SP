using ConsoleFileManager;
using Terminal.Gui;

Application.Init();
Application.Run<MainWindow>();

public class MainWindow : Window
{
    FilesPanel leftPanel;
    FilesPanel rightPanel;

    private void InitializeComponent()
    {
        leftPanel = new FilesPanel()
        {
            X = 0,
            Y = 0,
            Height = Dim.Fill(),
            Width = Dim.Percent(50),
        };

        rightPanel = new FilesPanel()
        {
            X = Pos.Percent(50),
            Y = 0,
            Height = Dim.Fill(),
            Width = Dim.Percent(50),
        };

        Add(rightPanel);
        Add(leftPanel);

        this.KeyDown += MainWindow_KeyDown;
    }

    private void MainWindow_KeyDown(KeyEventEventArgs obj)
    {
        switch (obj)
        {
            case Key.F5:
                var panels = GetPanel();
                var file = panels.source.SelectedFullName;
                File.Copy(file, Path.Combine(panels.target.CurrentPath, Path.GetFileName(file)));
                panels.target.Refrash();
                break;
        }
    }

    private (FilesPanel source, FilesPanel target) GetPanel()
    {
        return leftPanel.IsSelected ? (leftPanel, rightPanel) : (rightPanel, leftPanel);
    }

    public MainWindow()
    {
        InitializeComponent();
    }
}
