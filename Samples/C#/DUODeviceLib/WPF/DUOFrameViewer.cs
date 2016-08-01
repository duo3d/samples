using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Controls;
using DUODeviceLib;

namespace DUODeviceLib.WPF
{
    public class DUOFrameViewer : Grid, IDisposable
    {
        #region Depedency Properties
        public static readonly DependencyProperty FramerateProperty = DependencyProperty.Register("Framerate", typeof(double), typeof(DUOFrameViewer), new UIPropertyMetadata(60.0, new PropertyChangedCallback(FrameratePropertyChanged)));
        public double Framerate
        {
            get { return (double)GetValue(FramerateProperty); }
            set { SetValue(FramerateProperty, value); }
        }
        private static void FrameratePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            DUOFrameViewer instance = obj as DUOFrameViewer;
            if (instance == null)
                return;
            double newFramerate = (double)e.NewValue;
            lock (instance.updateLock)
            {
                if (instance.timer != null)
                {
                    bool needRestart = instance.timer.IsEnabled;
                    instance.timer.Stop();
                    if (newFramerate < 1.0)
                        newFramerate = 1.0;
                    if (newFramerate > 120.0)
                        newFramerate = 120.0;
                    instance.timer.Interval = TimeSpan.FromMilliseconds(1000.0 / newFramerate);
                    if (needRestart)
                        instance.timer.Start();
                }
            }
        }
        #endregion
        #region Properties
        private bool disposed = false;
        DispatcherTimer timer = null;
        private object updateLock = new object();
        private bool updated = false;
        private bool needToStart = false;
        private DUOImage[] images = null;
        private DUODevice device;
        public DUODevice Device
        {
            set
            {
                if (device == value)
                    return;
                SetDevice(value);
            }
            get
            {
                return device;
            }
        }
        #endregion
        #region .ctor
        public DUOFrameViewer()
        {
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            images = new DUOImage[2];
            Loaded += DUOFrameViewer_Loaded;
        }
        void DUOFrameViewer_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                images[i] = new DUOImage();
                Children.Add(images[i]);
                SetColumn(images[i], i);
                if (device != null)
                    images[i].SetBitmapResolution((uint)device.FrameWidth, (uint)device.FrameHeight);
            }
            timer = new DispatcherTimer();
            double newFramerate = Framerate;
            if (newFramerate < 1.0)
                newFramerate = 1.0;
            if (newFramerate > 120.0)
                newFramerate = 120.0;
            timer.Interval = TimeSpan.FromMilliseconds(1000.0 / newFramerate);
            timer.Tick += DUOTimerUpdate;
            lock (updateLock)
            {
                if (needToStart)
                    timer.Start();
                needToStart = false;
            }
        }
        ~DUOFrameViewer()
        {
            Dispose(false);
        }
        #endregion
        #region Methods
        private void Start(DUODevice device)
        {
            if (timer != null)
                timer.Start();
            else
            {
                lock (updateLock)
                {
                    needToStart = true;
                }
            }
        }
        private void Stop(DUODevice device)
        {
            if (timer != null)
                timer.Stop();

        }
        private void SetDevice(DUODevice newDevice)
        {
            if (device != null)
            {
                device.DUOFrameReceived -= DUOFrameReceivedHandler;
                device.DUODeviceStatusChanged -= DUODeviceStatusChangedHandler;
            }
            if (newDevice != null)
            {
                newDevice.DUOFrameReceived += DUOFrameReceivedHandler;
                newDevice.DUODeviceStatusChanged += DUODeviceStatusChangedHandler;
                for (int i = 0; i < 2; i++)
                {
                    if (images[i] == null)
                        continue;
                    images[i].SetBitmapResolution((uint)newDevice.FrameWidth, (uint)newDevice.FrameHeight);
                }
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    if (images[i] == null)
                        continue;
                    images[i].SetBitmapResolution(0, 0);
                }
            }
            device = newDevice;
        }
        #endregion
        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (images[i] != null)
                            images[i].Dispose();
                    }
                }
                disposed = true;
            }
        }
        #endregion
        #region Event Handlers
        private void DUOFrameReceivedHandler(DUODevice sender, ref DUOFrame pFrameData)
        {
            lock (updateLock)
            {
                updated = true;
                if (images[0] != null)
                    images[0].SetDUOImageData(pFrameData.leftData);
                if (images[1] != null)
                    images[1].SetDUOImageData(pFrameData.rightData);
            }
        }
        private void DUODeviceStatusChangedHandler(DUODevice sender, bool isRunning)
        {
            if (isRunning)
                Start(sender);
            else
                Stop(sender);
        }
        private void DUOTimerUpdate(object sender, EventArgs e)
        {
            lock (updateLock)
            {
                if (!updated)
                    return;
                updated = false;
                for (int i = 0; i < 2; i++)
                {
                    if ((images[i] == null) || (images[i].Bitmap == null))
                        continue;
                    images[i].Bitmap.Invalidate();
                }
            }
        }
        #endregion
    }
}
