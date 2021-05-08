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
using System.Diagnostics;


namespace Timer_For_Down_off
{
    public partial class MainWindow : Window
    {

        private System.Windows.Threading.DispatcherTimer dispatcherTimer;
        private int hour;
        private int minutes;
        private bool nextDay = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            if (this.dispatcherTimer == null)
            {
                this.hour = (int)Hours.Value;
                this.minutes = (int)Minutes.Value;

                DateTime currentTime = DateTime.Now;
                if (
                    (currentTime.Hour >= this.hour && currentTime.Minute > this.minutes)
                    || currentTime.Hour > this.hour
                    )
                {
                    this.nextDay = true;
                }

                this.dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                this.dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                this.dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
                this.dispatcherTimer.Start();
            } else
            {
                //everyday we stray further from god
                Button_Click_Stop(sender, e);
                this.dispatcherTimer = null;
                Button_Click_Start(sender, e);
            }

        }
        
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            DateTime remainingTime;
            if (this.nextDay == false)
            {
                remainingTime = new DateTime(
                        currentTime.Year,
                        currentTime.Month,
                        currentTime.Day,
                        this.hour,
                        this.minutes,
                        0
                    );
            } else
            {
                remainingTime = new DateTime(
                        currentTime.Year,
                        currentTime.Month,
                        currentTime.Day+1,
                        this.hour,
                        this.minutes,
                        0
                    );
            }
            TimeSpan diff = remainingTime - currentTime;
            TimerRemaining.Text = diff.ToString(@"hh\:mm\:ss");
            if (diff.Hours == 0 && diff.Minutes == 0 & diff.Seconds < 1)
            {
                TimerRemaining.Text = "The end.";
                //Console.WriteLine("shutdown");
                shutdown();
            }
            //Console.WriteLine(this.nextDay);

            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
        }

        private void Button_Click_Stop(object sender, RoutedEventArgs e)
        {
            if (this.dispatcherTimer != null)
            {
                this.dispatcherTimer.Stop();
            }
            this.dispatcherTimer = null;
        }

        private void shutdown()
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.WriteLine("shutdown -s -t 1");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
        }

        private void Hours_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
