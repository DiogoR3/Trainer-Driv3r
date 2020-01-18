using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TrainerDriv3r.Miscellaneous;
using TrainerDriv3r.Weaponry;

namespace TrainerDriv3r
{
    public partial class Trainer_Driv3r : Window
    {
        private const string _processName = "Driv3r";
        private ProcessMemory _processMemory { get; set; }

        Timer healthTimer;

        public Trainer_Driv3r()
        {
            Process[] driv3rProcesses = Process.GetProcessesByName(_processName);

            if (driv3rProcesses.Length < 1)
            {
                MessageBox.Show($"{_processName}.exe is not open!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }

            _processMemory = new ProcessMemory(driv3rProcesses[0]);

            InitializeComponent();

            healthTimer = new Timer(500);
            healthTimer.Elapsed += HealthTimer_Elapsed;
            healthTimer.Start();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void AmmoButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListBoxItem item in ListBoxWeapons.SelectedItems)
            {
                // + 1 for Tanner's handgun
                Weapon weapon = (Weapon)ListBoxWeapons.Items.IndexOf(item) + 1;

                Ammo.SetGivenAmmunition(_processMemory, weapon, Convert.ToInt32(TextBoxAmmunition.Text));
            }
        }

        private void InfiniteAmmo_Checked(object sender, RoutedEventArgs e)
        {
            Ammo.SetInfiniteAmmunition(_processMemory, isInfinite: true);
        }

        private void InfiniteAmmo_Unchecked(object sender, RoutedEventArgs e)
        {
            Ammo.SetInfiniteAmmunition(_processMemory, false);
        }


        private void HealthTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var health = Health.GetHealth(_processMemory);
            healthBar.Dispatcher.BeginInvoke(new Action(() =>
            {
                healthBar.Value = health;
                healthBarSlider.Value = health / 10d ;
            }));
        }

        private void CrashVehicle_Checked(object sender, RoutedEventArgs e)
        {
            Health.PlayerCrashVehicleDamage(_processMemory, enabled: true);
        }

        private void CrashVehicle_Unchecked(object sender, RoutedEventArgs e)
        {
            Health.PlayerCrashVehicleDamage(_processMemory, enabled: false);
        }

        private void Explosion_Checked(object sender, RoutedEventArgs e)
        {
            Health.ExplosionDamage(_processMemory, enabled: true);
        }

        private void Explosion_Unchecked(object sender, RoutedEventArgs e)
        {
            Health.ExplosionDamage(_processMemory, enabled: false);
        }

        private void Shot_Checked(object sender, RoutedEventArgs e)
        {
            Health.ShotDamage(_processMemory, enabled: true);
        }

        private void Shot_Unchecked(object sender, RoutedEventArgs e)
        {
            Health.ShotDamage(_processMemory, enabled: false);
        }

        private void DecreaseHealth_Click(object sender, RoutedEventArgs e)
        {
            Health.AddHealth(_processMemory, -5);
        }

        private void IncreaseHealth_Click(object sender, RoutedEventArgs e)
        {
            Health.AddHealth(_processMemory, 5);
        }

        private void healthBarSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Health.SetGivenHealth(_processMemory, (int)e.NewValue * 10);
        }
    }
}
