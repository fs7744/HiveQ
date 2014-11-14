using HiveQuery.Common;
using HiveQuery.ViewMode;
using MahApps.Metro.Controls;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace HiveQuery.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var model = new MainViewModel() { Window = this };
            DataContext = model;
            model.PropertyChanged += model_PropertyChanged;
        }

        private void model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Result")
            {
                Dispatcher.Invoke(() => UpdateResultGrid(sender as MainViewModel));
            }
        }

        private void UpdateResultGrid(MainViewModel model)
        {
            ResultGrid.Children.Clear();
            if (model == null || model.Result.IsEmpty()) return;
            ResultGrid.RowDefinitions.Clear();
            for (int i = 0; i < model.Result.Count; i++)
            {
                ResultGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto, MinHeight = 100 });
            }
            ResultGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            int count = 0;
            var cellStyle = new Style { TargetType = typeof(DataGridCell) };
            var cellTrigger = new Trigger { Property = DataGridCell.IsSelectedProperty, Value = true };
            cellTrigger.Setters.Add(new Setter(ForegroundProperty, Brushes.DarkBlue));
            cellTrigger.Setters.Add(new Setter(BackgroundProperty, Brushes.LightGreen));
            cellStyle.Triggers.Add(cellTrigger);
            model.Result.ForEach(i =>
            {
                DataGrid grid = new DataGrid();

                grid.ItemsSource = i.DefaultView;
                grid.CanUserAddRows = false;
                grid.CanUserDeleteRows = false;
                grid.IsReadOnly = true;
                grid.SetValue(Grid.RowProperty, count);
                grid.Margin = new Thickness(0, 5, 0, 0);
                ResultGrid.Children.Add(grid);
                grid.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                grid.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                grid.EnableRowVirtualization = true;
                grid.EnableColumnVirtualization = true;
                grid.SelectionUnit = DataGridSelectionUnit.Cell;
                grid.SelectionMode = DataGridSelectionMode.Extended;
                grid.CellStyle = cellStyle;
                grid.ContextMenu = new ContextMenu();
                var item = new MenuItem() { Header = "Copy all selected rows" };
                item.Click += item_Click;
                item.CommandParameter = grid;
                grid.ContextMenu.Items.Add(item);
                grid.SetBinding(DataGrid.HeightProperty,
                    new Binding(string.Format("RowDefinitions[{0}].Height.Value", count))
                    {
                        Source = ResultGrid,
                        Mode = BindingMode.OneWay
                    });
                grid.MinHeight = 100;
                var splitter = new GridSplitter()
                {
                    ShowsPreview = true,
                    BorderBrush = Brushes.Black,
                    Height = 2,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    BorderThickness = new Thickness(1),
                    IsEnabled = true,
                    VerticalAlignment = VerticalAlignment.Top
                };
                splitter.SetValue(Grid.RowProperty, ++count);
                ResultGrid.Children.Add(splitter);
            });
        }

        private void item_Click(object sender, RoutedEventArgs e)
        {
            var menu = sender as MenuItem;
            if (menu == null) return;
            var grid = menu.CommandParameter as DataGrid;
            if (grid == null || grid.SelectedCells.IsEmpty()) return;
            var list = grid.SelectedCells.Select(i => i.Item as DataRowView).Distinct().Where(i => i != null).Select(i => i.Row.ItemArray).ToList();
            if (list.IsEmpty()) return;
            StringBuilder sb = new StringBuilder();
            list.ForEach(i =>
            {
                sb.Append(string.Join("\t", i));
                sb.Append("\n");
            });
            Clipboard.SetText(sb.ToString());
        }

        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Flyout_IsOpenChanged(object sender, EventArgs e)
        {
            var flyout = sender as Flyout;
            if (flyout != null)
                flyout.Visibility = flyout.IsOpen ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}