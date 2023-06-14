using System;
using System.Windows;
using System.Windows.Input;
namespace DLC_3
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public string[] configs;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window mw = Application.Current.MainWindow;
            
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (mw.WindowState == WindowState.Maximized)
                {
                    mw.WindowState = WindowState.Normal;
                    Left = Mouse.GetPosition(mw).X - (mw.ActualWidth / 2) - 5;
                    Top = Mouse.GetPosition(mw).Y - 20;
                }
                DragMove();
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void WindowStateButton_Click(object sender, RoutedEventArgs e)
        {
            Window mw = Application.Current.MainWindow;            
            if (mw.WindowState != WindowState.Maximized)
            {
               mw.WindowState = WindowState.Maximized;
            }
            else
            {
                mw.WindowState = WindowState.Normal;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
