using System.Windows;
using System.Windows.Controls;

namespace MLClassifierStation.UserControls
{
    /// <summary>
    /// Interaction logic for InfoControl.xaml
    /// </summary>
    public partial class InfoControl : UserControl
    {
        public InfoControl()
        {
            InitializeComponent();
        }

        public string InfoTitle
        {
            get => (string)GetValue(InfoTitleProperty); 
            set => SetValue(InfoTitleProperty, value);
        }

        public static readonly DependencyProperty InfoTitleProperty =
            DependencyProperty.Register("InfoTitle", typeof(string),
                                           typeof(InfoControl));

        public string InfoContent
        {
            get => (string)GetValue(InfoContentProperty); 
            set => SetValue(InfoContentProperty, value);
        }

        public static readonly DependencyProperty InfoContentProperty =
            DependencyProperty.Register("InfoContent", typeof(string),
                                           typeof(InfoControl));
    }
}