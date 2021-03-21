using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        public SearchBar()
        {
            InitializeComponent();
        }
    }
}
