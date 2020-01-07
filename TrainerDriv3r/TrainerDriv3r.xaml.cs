using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace TrainerDriv3r
{
    public partial class Trainer_Driv3r : Window
    {
        public Trainer_Driv3r()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
