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
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Controls;
using DUODeviceLib;

namespace DUODeviceLib.WPF
{
    public class DUOImage : Image, IDisposable
    {
        #region Depedency Properties
        public static readonly DependencyProperty BitmapWidthProperty = DependencyProperty.Register("BitmapWidth", typeof(uint), typeof(DUOImage), new UIPropertyMetadata((uint)DUOLib.DUO_DEFAULT_RESOLUTION_WIDTH, new PropertyChangedCallback(BitmapWidthPropertyChanged)));
        public static readonly DependencyProperty BitmapHeightProperty = DependencyProperty.Register("BitmapHeight", typeof(uint), typeof(DUOImage), new UIPropertyMetadata((uint)DUOLib.DUO_DEFAULT_RESOLUTION_HEIGHT, new PropertyChangedCallback(BitmapHeightPropertyChanged)));
        public static readonly DependencyPropertyKey BitmapPropertyKey = DependencyProperty.RegisterReadOnly("Bitmap", typeof(InteropBitmap), typeof(DUOImage), new UIPropertyMetadata(default(InteropBitmap)));
        public static readonly DependencyProperty BitmapProperty = BitmapPropertyKey.DependencyProperty;
        public InteropBitmap Bitmap
        {
            get { return (InteropBitmap)GetValue(BitmapProperty); }
            private set { SetValue(BitmapPropertyKey, value); }
        }
        private uint bitmapWidth = (uint)DUOLib.DUO_DEFAULT_RESOLUTION_WIDTH;
        public uint BitmapWidth
        {
            get { return (uint)GetValue(BitmapWidthProperty); }
            set { SetValue(BitmapWidthProperty, value); }
        }
        private uint bitmapHeight = (uint)DUOLib.DUO_DEFAULT_RESOLUTION_HEIGHT;
        public uint BitmapHeight
        {
            get { return (uint)GetValue(BitmapHeightProperty); }
            set { SetValue(BitmapHeightProperty, value); }
        }
        private static void BitmapWidthPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            DUOImage instance = obj as DUOImage;
            if (instance == null)
                return;
            uint newWidth = (uint)e.NewValue;
            lock (instance.updateLock)
            {
                instance.bitmapWidth = newWidth;
            }
            instance.SetResolution(newWidth, instance.BitmapHeight);
        }
        private static void BitmapHeightPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            DUOImage instance = obj as DUOImage;
            if (instance == null)
                return;
            uint newHeight = (uint)e.NewValue;
            lock (instance.updateLock)
            {
                instance.bitmapHeight = newHeight;
            }
            instance.SetResolution(instance.BitmapWidth, newHeight);
       }
        #endregion
        #region Private Properties
        private IntPtr map = IntPtr.Zero;
        private IntPtr section = IntPtr.Zero;
        private object updateLock = new object();
        private bool disposed = false;
        #endregion
        #region .ctor
        public DUOImage()
        {
            Loaded += DUOImage_Loaded;
        }
        ~DUOImage()
        {
            Dispose(false);
        }
        #endregion
        #region Methods
        public void SetBitmapResolution(uint bitmapWidth, uint bitmapHeight)
        {
            BitmapWidth = bitmapWidth;
            BitmapHeight = bitmapHeight;
            SetResolution(bitmapWidth, bitmapHeight);
        }
        private void SetResolution(uint bitmapWidth, uint bitmapHeight)
        {
            lock (updateLock)
            {
                if (map != IntPtr.Zero)
                {
                    MemoryInternal.UnmapViewOfFile(map);
                    map = IntPtr.Zero;
                }
                if (section != IntPtr.Zero)
                {
                    MemoryInternal.CloseHandle(section);
                    section = IntPtr.Zero;
                }
                uint imageSize = (uint)bitmapWidth * (uint)bitmapHeight;
                if (imageSize == 0x00)
                    return;
                // create memory section and map
                section = MemoryInternal.CreateFileMapping(new IntPtr(-1), IntPtr.Zero, 0x04, 0, imageSize, null);
                map = MemoryInternal.MapViewOfFile(section, 0xF001F, 0, 0, imageSize);
            }
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, (SendOrPostCallback)delegate
            {
                Bitmap = Imaging.CreateBitmapSourceFromMemorySection(section, (int)bitmapWidth, (int)bitmapHeight, PixelFormats.Gray8, (int)bitmapWidth, 0) as InteropBitmap;
                Bitmap.Invalidate();
                Source = Bitmap;
                Stretch = System.Windows.Media.Stretch.Fill;
                StretchDirection = StretchDirection.Both;
            }, null);
        }
        public void SetDUOImageData(IntPtr imageData)
        {
            lock (updateLock)
            {
                if ((map == IntPtr.Zero) || (imageData == IntPtr.Zero))
                    return;
                uint len = bitmapWidth * bitmapHeight;
                if (len > 0x00)
                    MemoryInternal.CopyMemory(map, imageData, (int)len);
            }
        }
        void DUOImage_Loaded(object sender, RoutedEventArgs e)
        {
            SetBitmapResolution(BitmapWidth, BitmapHeight);
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
                    //dispose children objects
                }
                // free native resources if there are any.
                Destroy();
                disposed = true;
            }
        }
        private void Destroy()
        {
            lock (updateLock)
            {
                if (map != IntPtr.Zero)
                {
                    MemoryInternal.UnmapViewOfFile(map);
                    map = IntPtr.Zero;
                }
                if (section != IntPtr.Zero)
                {
                    MemoryInternal.CloseHandle(section);
                    section = IntPtr.Zero;
                }
            }
        }
        #endregion
    }
}
