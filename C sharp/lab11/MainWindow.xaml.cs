using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using MessageBox = System.Windows.MessageBox;

namespace Lab11
{
    public partial class MainWindow : Window
    {
        private int? K => !int.TryParse(KTextBox.Text, out var result) ? null : result;
        private int? N => !int.TryParse(NTextBox.Text, out var result) ? null : result;
        private BackgroundWorker? backgroundWorker = null;
        private string selectedMethod;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void Tasks_Click(object sender, EventArgs e)
        {
            int k = K.Value;
            int n = N.Value;

            Task.Run(() => CalculateNewton(k, n))
                .ContinueWith(t => Dispatcher.Invoke(() => MethodResultTextBox.Text = t.Result.ToString()));
        }

        private void Delegates_Click(object sender, EventArgs e)
        {
            int k = K.Value;
            int n = N.Value;

            Func<int, int, long> calculateNewtonDelegate = new Func<int, int, long>(CalculateNewton);
            AsyncCallback callback = ar =>
            {
                long result = calculateNewtonDelegate.EndInvoke(ar);
                Dispatcher.Invoke(() => MethodResultTextBox.Text = result.ToString());
            };
            calculateNewtonDelegate.BeginInvoke(k, n, callback, null);
        }

        private async void Async_Click(object sender, EventArgs e)
        {
            int k = K.Value;
            int n = N.Value;

            long result = await Task.Run(() => CalculateNewton(k, n));
            MethodResultTextBox.Text = result.ToString();
        }
      
        private long CalculateNewton(int k, int n)
        {
            if (k > n) return 0;
            if (k == 0 || k == n) return 1;

            long result = 1;
            for (int i = 1; i <= k; i++)
            {
                result *= n - (k - i);
                result /= i;
            }
            return result;
        }

        private void CalculateFibonacciButton_OnClick(object sender, RoutedEventArgs e)
        {
            int n;
            if (!int.TryParse(FibonacciITextBox.Text, out n) || n <= 0)
            {
                MessageBox.Show("Please provide a correct value of i.");
                return;
            }

            backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.RunWorkerAsync(n);
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var backWorker = sender as BackgroundWorker;
            var n = (int)e.Argument;

            var results = new BigInteger[n];
            results[0] = 1;
            results[1] = 1;

            for (var i = 2; i < n; i++)
            {
                results[i] = results[i - 2] + results[i - 1];
                backWorker?.ReportProgress((int)((double)(i + 1) / n * 100));
                Thread.Sleep(5);
            }

            e.Result = results[n - 1];
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FibonacciProgressBar.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FibonacciResultTextBox.Text = e.Result?.ToString() ?? "Error";
        }

        private void CompressFiles_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            var dirInfo = new DirectoryInfo(dialog.SelectedPath);
            Parallel.ForEach(dirInfo.EnumerateFiles(), fileInfo =>
            {
                using var fs = fileInfo.OpenRead();
                using var os = File.Open(fileInfo.FullName + ".gz", FileMode.Create);
                using var gs = new GZipStream(os, CompressionMode.Compress);
                fs.CopyTo(gs);
            });
            MessageBox.Show("The files are compressed.");
        }

        private void DecompressFiles_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            var dirInfo = new DirectoryInfo(dialog.SelectedPath);
            Parallel.ForEach(dirInfo.EnumerateFiles("*.gz"), fileInfo =>
            {
                using var fs = fileInfo.OpenRead();
                using var os = File.Open(fileInfo.FullName.Replace(".gz", ""), FileMode.Create);
                using var gs = new GZipStream(fs, CompressionMode.Decompress);
                gs.CopyTo(os);
            });
            MessageBox.Show("The files are decompressed.");
        }
    }
}
