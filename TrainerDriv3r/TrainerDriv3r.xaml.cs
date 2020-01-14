using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TrainerDriv3r.Weaponry;

namespace TrainerDriv3r
{
    public partial class Trainer_Driv3r : Window
    {
        private const string _processName = "Driv3r";
        private ProcessMemory _processMemory { get; set; }

        public Trainer_Driv3r()
        {
            Process[] driv3rProcesses = Process.GetProcessesByName(_processName);

            if(driv3rProcesses.Length < 1)
            {
                MessageBox.Show($"{_processName}.exe is not open!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }

            _processMemory = new ProcessMemory(driv3rProcesses[0]);

            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            foreach(ListBoxItem item in ListBoxWeapons.SelectedItems)
            {
                // + 1 for Tanner's handgun
                Weapon weapon = (Weapon)ListBoxWeapons.Items.IndexOf(item) + 1;

                Ammo.SetGivenAmmunition(_processMemory, weapon, Convert.ToInt32(TextBoxAmmunition.Text));
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Ammo.SetInfiniteAmmunition(_processMemory, true);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Ammo.SetInfiniteAmmunition(_processMemory, false);
        }
    }
}
