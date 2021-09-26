using System;
using System.Diagnostics;
using System.Windows;

namespace Hanoi_Towers
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

        }


        private void RingsCountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                if (ringsCountLabel != null)
                {
                    ringsCountLabel.Content = ringsCountSlider.Value.ToString();
                }
                
            }catch(NullReferenceException err)
            {
                Debug.WriteLine(err.Message);
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GameSettings gameSettings = new GameSettings();
            gameSettings.gameType = gameMode.SelectedItem.ToString();
            Debug.WriteLine(gameSettings.gameType);
            gameSettings.ringsCount = Int32.Parse((string)ringsCountLabel.Content);


            if (gameMode.SelectedIndex == 0)
            {
                AutomaticGame auto = new AutomaticGame(gameSettings);
                auto.Owner = this;
                auto.ShowDialog();
            }
            if (gameMode.SelectedIndex == 1)
            {
                ManualGame manual = new ManualGame(gameSettings);
                manual.Owner = this;
                manual.ShowDialog();
            }
        }
    }
}
