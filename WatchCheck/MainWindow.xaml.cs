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
using Microsoft.Win32;

namespace WatchCheck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private NAudio.Wave.WaveFileReader wave = null;

        private NAudio.Wave.DirectSoundOut output = null;

        public MainWindow()
        {
            InitializeComponent();
            ApplicationCommands.Close.InputGestures.Add(new KeyGesture(Key.X, ModifierKeys.Control));
            if (wave == null && output == null) playButton.IsEnabled = false;
            mainWindowText.Text = "No file loaded";
        }

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Wave File (*.wav)|*.wav";
            open.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            if (open.ShowDialog() == true)
            {
                DisposeWave();
                wave = new NAudio.Wave.WaveFileReader(open.FileName);
                mainWindowText.Text = $"File loaded: {open.FileName}\nTotal time: {wave.TotalTime},\nWave format: {wave.WaveFormat}";
                output = new NAudio.Wave.DirectSoundOut();
                output.Init(new NAudio.Wave.WaveChannel32(wave));
                playButton.IsEnabled = true;
            }
        }

        private void New_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("New file to record.....");
        }

        private void Exit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            DisposeWave();
            if (wave == null && output == null)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DisposeWave();
            Application.Current.Shutdown();
        }

        private void PlayOrPause(object sender, RoutedEventArgs e)
        {
            if (output != null)
            {
                if (output.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                    output.Pause();
                else if (output.PlaybackState == NAudio.Wave.PlaybackState.Paused || output.PlaybackState == NAudio.Wave.PlaybackState.Stopped)
                    output.Play();
            }
        }

        private void DisposeWave()
        {
            if (output != null)
            {
                if (output.PlaybackState == NAudio.Wave.PlaybackState.Playing) output.Stop();
                output.Dispose();
                output = null;
            }
            if (wave != null)
            {
                wave.Dispose();
                wave = null;
            }
            playButton.IsEnabled = false;
        }

        private void CloseFile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            DisposeWave();
            if (wave == null && output == null)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        private void CloseFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mainWindowText.Text = "No file loaded";
        }
    }
}
