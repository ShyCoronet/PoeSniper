using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PoeSniperUI.CustomControls
{
    /// <summary>
    /// Interaction logic for SearchBar.xaml
    /// </summary>
    public partial class SearchBar : UserControl
    {
        private static readonly DependencyProperty TextNameProperty =
            DependencyProperty.Register("TextName", typeof(string), typeof(SearchBar));

        public string TextName
        {
            get { return (string)GetValue(TextNameProperty); }
            set { SetValue(TextNameProperty, value); }
        }

        private static readonly DependencyProperty TextUrlProperty =
            DependencyProperty.Register("TextUrl", typeof(string), typeof(SearchBar));

        public string TextUrl
        {
            get { return (string)GetValue(TextUrlProperty); }
            set { SetValue(TextUrlProperty, value); }
        }

        private static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(SearchBar));

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        private static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(SearchBar));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        private static readonly DependencyProperty IsConnectedProperty =
           DependencyProperty.Register("IsConnected", typeof(bool), typeof(SearchBar));

        public bool IsConnected
        {
            get { return (bool)GetValue(IsConnectedProperty); }
            set { SetValue(IsConnectedProperty, value); }
        }

        private static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(string), typeof(SearchBar));

        public string Id
        {
            get { return (string)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        private static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(SearchBar));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        private static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(SearchBar));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        private static readonly DependencyProperty UrlBoxCommandProperty =
            DependencyProperty.Register("UrlBoxCommand", typeof(ICommand), typeof(SearchBar));

        public ICommand UrlBoxCommand
        {
            get { return (ICommand)GetValue(UrlBoxCommandProperty); }
            set { SetValue(UrlBoxCommandProperty, value); }
        }

        private static readonly DependencyProperty ToggleStateCommandProperty =
            DependencyProperty.Register("ToggleStateCommand", typeof(ICommand), typeof(SearchBar));

        public ICommand ToggleStateCommand
        {
            get { return (ICommand)GetValue(ToggleStateCommandProperty); }
            set { SetValue(ToggleStateCommandProperty, value); }
        }

        private static readonly DependencyProperty RemoveSearchCommandProperty =
            DependencyProperty.Register("RemoveSearchCommand", typeof(ICommand), typeof(SearchBar));

        public ICommand RemoveSearchCommand
        {
            get { return (ICommand)GetValue(RemoveSearchCommandProperty); }
            set { SetValue(RemoveSearchCommandProperty, value); }
        }

        public SearchBar()
        {
            InitializeComponent();
        }
    }
}
