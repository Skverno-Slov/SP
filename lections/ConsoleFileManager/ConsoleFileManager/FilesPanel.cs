using Terminal.Gui;

namespace ConsoleFileManager
{
    public class FilesPanel : FrameView
    {
        public string SelectedFullName { get; set; }

        public string CurrentPath { get; private set; } = @"\";

        public bool IsSelected => listView.HasFocus;

        private ListView listView;

        public FilesPanel()
        {
            listView = new ListView()
            {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            listView.SelectedItemChanged += ListView_SelectedItemChanged;

            listView.MouseClick += ListView_MouseClick; ;

            listView.Leave += ListView_Leave;

            Add(listView);
            Refrash();
        }

        private void ListView_SelectedItemChanged(ListViewItemEventArgs obj)
        {
            SelectedFullName = Path.Combine(CurrentPath, (listView.Source.ToList()[listView.SelectedItem] as string).TrimStart('\\'));
        }

        private void ListView_MouseClick(MouseEventArgs obj)
        {
            var selected = listView.Source.ToList()[listView.SelectedItem] as string;
            Navigate(selected);
        }

        private void Navigate(string target)
        {
            if(target == "..")
            {
                CurrentPath = Directory.GetParent(CurrentPath)?.FullName ?? CurrentPath;
                Refrash();
                return;
            }

            string path = Path.Combine(CurrentPath, target.TrimStart('\\'));
            if (File.Exists(path))
            {
                CurrentPath = path;
                Refrash();
            }
        }

        private void ListView_Leave(FocusEventArgs obj)
        {
            if (IsSelected)
            {
                Border.BorderBrush = Color.Red;
                return;
            }
            Border.BorderBrush = Color.Black;
        }

        public void Refrash()
        {
            try
            {
                Title = CurrentPath;
                var entries = Directory.GetFileSystemEntries(CurrentPath)
                    .OrderBy(e => e)
                    .Select(e => Path.GetFileName(e) + (Directory.Exists(e) ? @"\" : ""))
                    .Prepend("..")
                    .ToList();
                listView.SetSource(entries);
            }
            catch
            {
                listView.SetSource(new List<string>() { "..", "<Error>" });
            }
        }
    }
}
