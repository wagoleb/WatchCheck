using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
using NAudio.Wave;

namespace WatchCheck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private WaveFileReader wave = null;
        private DirectSoundOut output = null;
        private WaveFileWriter recorder = null;
        private WaveIn sourceStream = null;
        string tempFile = @"C:\temp\temp.wav";

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
                wave = new WaveFileReader(open.FileName);
                mainWindowText.Text = $"File loaded: {open.FileName}\nTotal time: {wave.TotalTime},\nWave format: {wave.WaveFormat}";
                output = new DirectSoundOut();
                output.Init(new WaveChannel32(wave));
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
                if (output.PlaybackState == PlaybackState.Playing)
                    output.Pause();
                else if (output.PlaybackState == PlaybackState.Paused || output.PlaybackState == PlaybackState.Stopped)
                    output.Play();
            }
        }

        private void DisposeWave()
        {
            if (output != null)
            {
                if (output.PlaybackState == PlaybackState.Playing) output.Stop();
                output.Dispose();
                output = null;
            }
            if (wave != null)
            {
                wave.Dispose();
                wave = null;
            }
            playButton.IsEnabled = false;
            if (recorder != null)
            {
                recorder.Dispose();
                recorder = null;
                File.Delete(tempFile);
            }
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

        private void SaveFile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Wave File (*.wav)|*.wav";
            saveFile.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            if (saveFile.ShowDialog() == true)
            {
                Console.WriteLine(saveFile.FileName);
            }
        }

        private void RefreshSource(object sender, RoutedEventArgs e)
        {
            List<WaveInCapabilities> sources = new List<WaveInCapabilities>();
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                sources.Add(WaveIn.GetCapabilities(i));
            }

            SourcesList.Items.Clear();

            foreach (var source in sources)
            {
                ListViewItem item = new ListViewItem();
                SourcesList.Items.Add(source.ProductName);
            }
        }

        private void StartRec(object sender, RoutedEventArgs e)
        {
            if (SourcesList.SelectedItems.Count == 1)
            {
                int deviceNumber = SourcesList.SelectedIndex;

                sourceStream = new WaveIn();
                sourceStream.DeviceNumber = deviceNumber;
                sourceStream.WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(deviceNumber).Channels);
                sourceStream.DataAvailable += new EventHandler<WaveInEventArgs>(SourceStream_DataAvailable);
                recorder = new WaveFileWriter(tempFile, sourceStream.WaveFormat);
                sourceStream.StartRecording();
            }
            else
                return;
        }

        private void SourceStream_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (recorder != null)
            {
                recorder.Write(e.Buffer, 0, e.BytesRecorded);
                recorder.Flush();
            }
        }

        private void StopRec(object sender, RoutedEventArgs e)
        {
            if (sourceStream != null)
            {
                sourceStream.StopRecording();
            }
        }
    }
}
