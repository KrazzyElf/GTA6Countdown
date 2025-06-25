using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace GTA_6_Countdown
{
    public partial class MainWindow : Window
    {
        private readonly DateTime gta6ReleaseDate = new DateTime(2026, 5, 26);
        private bool isSimpleView = false;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                // Start in simple view
                SetToSimpleView();

                StartCountdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during initialization: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StartCountdown()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += UpdateCountdown;
            timer.Start();
        }

        private void UpdateCountdown(object? sender, EventArgs e)
        {
            try
            {
                TimeSpan timeLeft = gta6ReleaseDate - DateTime.Now;

                if (timeLeft.TotalSeconds <= 0)
                {
                    DaysText.Text = "0";
                    HoursText.Text = "00";
                    MinutesText.Text = "00";
                    SecondsText.Text = "00";

                    if (SimpleDaysText != null)
                    {
                        SimpleDaysText.Text = "0";
                    }
                }
                else
                {
                    DaysText.Text = timeLeft.Days.ToString();
                    HoursText.Text = timeLeft.Hours.ToString("D2");
                    MinutesText.Text = timeLeft.Minutes.ToString("D2");
                    SecondsText.Text = timeLeft.Seconds.ToString("D2");

                    if (SimpleDaysText != null)
                    {
                        SimpleDaysText.Text = timeLeft.Days.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating countdown: {ex.Message}");
            }
        }

        // Allows the window to be dragged by clicking anywhere
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    this.DragMove();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during drag: {ex.Message}");
            }
        }

        // Full View Menu Item Click Handler
        private void SetFullView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isSimpleView)
                {
                    FullViewGrid.Visibility = Visibility.Visible;
                    SimpleViewGrid.Visibility = Visibility.Collapsed;
                    FullViewMenuItem.IsChecked = true;
                    SimpleViewMenuItem.IsChecked = false;

                    this.Width = 400;
                    this.Height = 280;

                    isSimpleView = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error switching to full view: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Simple View Menu Item Click Handler
        private void SetSimpleView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isSimpleView)
                {
                    FullViewGrid.Visibility = Visibility.Collapsed;
                    SimpleViewGrid.Visibility = Visibility.Visible;
                    FullViewMenuItem.IsChecked = false;
                    SimpleViewMenuItem.IsChecked = true;

                    this.Width = 240;
                    this.Height = 260;

                    // Update the simple view day counter
                    if (SimpleDaysText != null && DaysText != null)
                    {
                        SimpleDaysText.Text = DaysText.Text;
                    }

                    isSimpleView = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error switching to simple view: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Exit Menu Item Click Handler
        private void ExitApplication_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error shutting down: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0); // Force exit if normal shutdown fails
            }
        }

        private void SetToSimpleView()
        {
            // Always start in simple view
            FullViewGrid.Visibility = Visibility.Collapsed;
            SimpleViewGrid.Visibility = Visibility.Visible;
            FullViewMenuItem.IsChecked = false;
            SimpleViewMenuItem.IsChecked = true;

            this.Width = 240;
            this.Height = 260;
            isSimpleView = true;
        }

        // Mode functionality - window positioning methods
        private void SetAlwaysOnTop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Topmost = true;
                Console.WriteLine("Window set to always on top");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting always on top: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetNormalWindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Topmost = false;
                Console.WriteLine("Window set to normal");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting normal window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetAlwaysOnBottom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Topmost = false;
                Console.WriteLine("Window set to always on bottom");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting always on bottom: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
