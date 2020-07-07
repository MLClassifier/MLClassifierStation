using System.Windows;
using System.Windows.Controls;

namespace MLClassifierStation.Views
{
    /// <summary>
    /// Interaction logic for OpenExamplesView.xaml
    /// </summary>
    public partial class OpenExamplesView : UserControl
    {
        public OpenExamplesView()
        {
            InitializeComponent();
        }

        public string ExamplesCaption
        {
            get => (string)GetValue(ExamplesCaptionProperty);
            set => SetValue(ExamplesCaptionProperty, value);
        }

        public static readonly DependencyProperty ExamplesCaptionProperty =
            DependencyProperty.Register("ExamplesCaption", typeof(string),
                                           typeof(OpenExamplesView));

        public string ExamplesButtonTitle
        {
            get => (string)GetValue(ExamplesButtonTitleProperty);
            set => SetValue(ExamplesButtonTitleProperty, value);
        }

        public static readonly DependencyProperty ExamplesButtonTitleProperty =
            DependencyProperty.Register("ExamplesButtonTitle", typeof(string),
                                           typeof(OpenExamplesView));

        public string ExamplesInformationCaption
        {
            get => (string)GetValue(ExamplesInformationCaptionProperty);
            set => SetValue(ExamplesInformationCaptionProperty, value);
        }

        public static readonly DependencyProperty ExamplesInformationCaptionProperty =
            DependencyProperty.Register("ExamplesInformationCaption", typeof(string),
                                           typeof(OpenExamplesView));
    }
}