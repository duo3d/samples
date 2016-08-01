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
using System.ComponentModel;
using DUODeviceLib;

namespace DUODeviceWPFDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DUODevice duoDevice = null;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            this.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
        }
        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            duoDevice.Stop();
        }
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            duoDevice = new DUODevice();
            frameViewer.Device = duoDevice;
            duoDevice.Resolution = new DUOResolutionInfo()
            {
                width = 320,
                height = 240,
                binning = DUOBinning.DUO_BIN_HORIZONTAL2 | DUOBinning.DUO_BIN_VERTICAL2,
                fps = 30
            };
            duoDevice.Start();
            duoDevice.Exposure = 100;
            duoDevice.Gain = 10;
        }
    }
}
