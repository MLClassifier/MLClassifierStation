using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MLClassifierStation.UserControls
{
    /// <summary>
    /// Interaction logic for FilePickerControl.xaml
    /// </summary>
    public partial class FilePickerControl : UserControl
    {
        public FilePickerControl()
        {
            InitializeComponent();

            Validation.SetValidationAdornerSite(this, this.tbxFilePath);
        }

        public string Caption
        {
            get => (string)GetValue(CaptionProperty);
            set => SetValue(CaptionProperty, value);
        }

        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string),
                                           typeof(FilePickerControl));

        public string FilePath
        {
            get => (string)GetValue(FilePathProperty);
            set => SetValue(FilePathProperty, value);
        }

        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(FilePickerControl));

        public string ButtonTitle
        {
            get => (string)GetValue(ButtonTitleProperty);
            set => SetValue(ButtonTitleProperty, value);
        }

        public static readonly DependencyProperty ButtonTitleProperty =
            DependencyProperty.Register("ButtonTitle", typeof(string), typeof(FilePickerControl));

        public ICommand OpenFileCommand
        {
            get => (ICommand)GetValue(OpenFileCommandProperty);
            set => SetValue(OpenFileCommandProperty, value);
        }

        public static readonly DependencyProperty OpenFileCommandProperty =
            DependencyProperty.Register("OpenFileCommand", typeof(ICommand),
                                           typeof(FilePickerControl));
    }
}