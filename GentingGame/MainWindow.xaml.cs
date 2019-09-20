using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GentingGame
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        int RowNum = 9;
        int ColumnNum = 8;
        int DataOnShow = 0;
        public MainWindow()
        {
            InitializeComponent();
            AddFirstRow();

        }

        private void AddFirstRow()
        {
            for (int i = 1; i <= ColumnNum; i++)
            {
                BitmapImage addr = new BitmapImage();
                addr.BeginInit();
                addr.UriSource = new Uri("image/1 (" + i + ").png", UriKind.Relative);
                addr.EndInit();
                addr.Freeze();

                Image image = new Image();
                image.Source = addr;
                image.MouseLeftButtonDown += FirstRow_MouseLeftButtonDown;

                ShowGrid.Children.Add(image);
                Grid.SetColumn(image, i - 1);
                Grid.SetRow(image, 0);
            }
        }

        private void AddData(int Columns)
        {
            for (int i = 1; i < RowNum; i++)
            {
                {
                    BitmapImage addr = new BitmapImage();
                    addr.BeginInit();
                    addr.UriSource = new Uri("image/10 (" + i + ").png", UriKind.Relative);
                    addr.EndInit();
                    addr.Freeze();

                    Image image = new Image();
                    image.Source = addr;
                    image.HorizontalAlignment = HorizontalAlignment.Right;

                    ShowGrid.Children.Add(image);
                    Grid.SetColumn(image, Columns);
                    Grid.SetRow(image, i);
                }

                {
                    BitmapImage addr = new BitmapImage();
                    addr.BeginInit();
                    addr.UriSource = new Uri("image/Plus.png", UriKind.Relative);
                    addr.EndInit();
                    addr.Freeze();

                    Image image = new Image();
                    image.Source = addr;
                    image.HorizontalAlignment = HorizontalAlignment.Left;

                    ShowGrid.Children.Add(image);
                    Grid.SetColumn(image, Columns);
                    Grid.SetRow(image, i);
                }

                {
                    BitmapImage addr = new BitmapImage();
                    addr.BeginInit();
                    addr.UriSource = new Uri("image/" + (i + 1) + " (" + (Columns + 1) + ").png", UriKind.Relative);
                    addr.EndInit();
                    addr.Freeze();

                    Image image = new Image();
                    image.Source = addr;
                    image.HorizontalAlignment = HorizontalAlignment.Right;

                    ShowGrid.Children.Add(image);
                    Grid.SetColumn(image, Columns + 1);
                    Grid.SetRow(image, i);
                }

            }
        }
        private void CleanData()
        {
            ShowGrid.Children.Clear();
            AddFirstRow();
        }
        private void FirstRow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image imagenow = sender as Image;
            string[] addr = imagenow.Source.ToString().Split(new string[] { "(", ").png" }, StringSplitOptions.RemoveEmptyEntries);
            int column_click = int.Parse(addr[1]);
            if (DataOnShow == 0)
            {
                AddData(column_click - 1);
                DataOnShow = column_click;
            }
            else
            {
                if (DataOnShow == column_click)
                {
                    CleanData();
                    DataOnShow = 0;
                }
                else
                {
                    CleanData();
                    AddData(column_click - 1);
                    DataOnShow = column_click;
                }
            }

        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
            {
                hwndSource.AddHook(new HwndSourceHook(this.WndProc));
            }
        }

        private const int WM_NCHITTEST = 0x0084;
        private readonly int agWidth = 20; //拐角宽度
        private readonly int bThickness = 10; // 边框宽度
        private readonly int TitleBarHeight = 30; // 边框宽度
        private Point mousePoint = new Point(); //鼠标坐标
        public enum HitTest : int
        {
            HTERROR = -2, //错误
            HTTRANSPARENT = -1, //透明的
            HTNOWHERE = 0,
            HTCLIENT = 1,
            HTCAPTION = 2,
            HTSYSMENU = 3,
            HTGROWBOX = 4,
            HTSIZE = HTGROWBOX,
            HTMENU = 5,
            HTHSCROLL = 6,
            HTVSCROLL = 7,
            HTMINBUTTON = 8,
            HTMAXBUTTON = 9,
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17,
            HTBORDER = 18,
            HTREDUCE = HTMINBUTTON,
            HTZOOM = HTMAXBUTTON,
            HTSIZEFIRST = HTLEFT,
            HTSIZELAST = HTBOTTOMRIGHT,
            HTOBJECT = 19,
            HTCLOSE = 20,
            HTHELP = 21,
        }
        protected virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_NCHITTEST:
                    {
                        this.mousePoint.X = (lParam.ToInt32() & 0xFFFF);
                        this.mousePoint.Y = (lParam.ToInt32() >> 16);

                        //告诉系统你已经处理过该消息，不然设置为false
                        handled = true;
                        #region 测试鼠标位置
                        // 窗口左上角
                        if (this.mousePoint.Y - this.Top <= this.agWidth
                                         && this.mousePoint.X - this.Left <= this.agWidth)
                        {
                            return new IntPtr((int)HitTest.HTTOPLEFT);
                        }
                        // 窗口左下角　　
                        else if (this.ActualHeight + this.Top - this.mousePoint.Y <= this.agWidth
                                         && this.mousePoint.X - this.Left <= this.agWidth)
                        {
                            return new IntPtr((int)HitTest.HTBOTTOMLEFT);
                        }
                        // 窗口右上角
                        else if (this.mousePoint.Y - this.Top <= this.agWidth
                         && this.ActualWidth + this.Left - this.mousePoint.X <= this.agWidth)
                        {
                            return new IntPtr((int)HitTest.HTTOPRIGHT);
                        }
                        // 窗口右下角
                        else if (this.ActualWidth + this.Left - this.mousePoint.X <= this.agWidth
                         && this.ActualHeight + this.Top - this.mousePoint.Y <= this.agWidth)
                        {
                            return new IntPtr((int)HitTest.HTBOTTOMRIGHT);
                        }
                        // 窗口左侧
                        else if (this.mousePoint.X - this.Left <= this.bThickness)
                        {
                            return new IntPtr((int)HitTest.HTLEFT);
                        }
                        // 窗口右侧
                        else if (this.ActualWidth + this.Left - this.mousePoint.X <= this.bThickness)
                        {
                            return new IntPtr((int)HitTest.HTRIGHT);
                        }
                        // 窗口上方
                        else if (this.mousePoint.Y - this.Top <= 0)
                        {
                            return new IntPtr((int)HitTest.HTTOP);
                        }
                        // 窗口下方
                        else if (this.ActualHeight + this.Top - this.mousePoint.Y <= this.bThickness)
                        {
                            return new IntPtr((int)HitTest.HTBOTTOM);
                        }
                        else
                        {
                            handled = false;
                            return IntPtr.Zero;
                        }
                        #endregion
                    }
            }
            return IntPtr.Zero;
        }

        private void MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
