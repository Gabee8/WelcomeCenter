using System;
using System.Collections.Generic;
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
//using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Management;
using System.ComponentModel;
using Microsoft.Win32;
using System.Security.Principal;
using System.Windows.Threading;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using System.Data;
using TsudaKageyu;
using Ini;

namespace WelcomeCenter
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        BackgroundWorker worker = new BackgroundWorker();
        public class MyApps
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public ImageSource Image { get; set; }
            public System.Drawing.Icon Icon { get; set; }
            public string Path { get; set; }
            public string PathSw { get; set; }
            public string DescTitle { get; set; }
            public string Description { get; set; }
        }
        public MyApps ListBoxObject;
        string rootPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string macska = "\"";
        string temp = System.IO.Path.GetTempPath();
        string programdata = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        //Konfigurációfájl helye
        IniFile ini = new IniFile(Environment.CurrentDirectory + "\\Settings.ini");
        int theme = 0;
        //Language Res
        public static ResourceDictionary mylangs;

        public MainWindow()
        {
            InitializeComponent();
            getSysInfo();
            welcomeText.Opacity = 0;
            
            timer.Interval = TimeSpan.FromMilliseconds(80);
            timer.Tick += starting_timer_Tick;

            //Language
            mylangs = Application.Current.Resources.MergedDictionaries[1];
        }

        
        public static ImageSource GetIcon(string strPath, bool bSmall)
        {
            Interop.SHFILEINFO info = new Interop.SHFILEINFO(true);
            int cbFileInfo = Marshal.SizeOf(info);
            Interop.SHGFI flags;
            if (bSmall)
                flags = Interop.SHGFI.Icon | Interop.SHGFI.SmallIcon | Interop.SHGFI.UseFileAttributes;
            else
                flags = Interop.SHGFI.Icon | Interop.SHGFI.LargeIcon | Interop.SHGFI.UseFileAttributes;

            Interop.SHGetFileInfo(strPath, 256, out info, (uint)cbFileInfo, flags);

            IntPtr iconHandle = info.hIcon;
           
            ImageSource img = Imaging.CreateBitmapSourceFromHIcon(
                        iconHandle,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
            Interop.DestroyIcon(iconHandle);
            return img;
        }

        public static ImageSource ToImageSource(System.Drawing.Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }

        private void Langs()
        {
            string SelectedLang = "";
            try
            {
                if (ini.IniReadValue("Settings", "Language") != "")
                {
                    SelectedLang = ini.IniReadValue("Settings", "Language");
                    if (Directory.Exists(rootPath + "\\Languages\\"))
                    {
                        mylangs = new ResourceDictionary { Source = new Uri(String.Concat(rootPath + "\\Languages\\" + SelectedLang + ".xaml")) };
                        this.Resources.MergedDictionaries.Add
                                (
                                    new ResourceDictionary { Source = new Uri(String.Concat(rootPath + "\\Languages\\" + SelectedLang + ".xaml")) }
                                );
                        Application.Current.Resources.MergedDictionaries[2] = mylangs;
                    }
                    else if (Directory.Exists(rootPath + "\\Lang\\"))
                    {
                        mylangs = new ResourceDictionary { Source = new Uri(String.Concat(rootPath + "\\Lang\\" + SelectedLang + ".xaml")) };
                        this.Resources.MergedDictionaries.Add
                                (
                                    new ResourceDictionary { Source = new Uri(String.Concat(rootPath + "\\Lang\\" + SelectedLang + ".xaml")) }
                                );
                        Application.Current.Resources.MergedDictionaries[2] = mylangs;
                    }
                    else
                    {
                        mylangs = new ResourceDictionary { Source = new Uri(String.Concat(rootPath + "\\" + SelectedLang + ".xaml")) };
                        this.Resources.MergedDictionaries.Add
                                (
                                    new ResourceDictionary { Source = new Uri(String.Concat(rootPath + "\\" + SelectedLang + ".xaml")) }
                                );
                        Application.Current.Resources.MergedDictionaries[2] = mylangs;
                    }
                    appList.Items.Refresh();
                }
            }
            catch (Exception)
            {

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Langs();
            logoIMG.Source = ExtractVistaIcon(Properties.Resources.mycomp);
            timer.Start();
            AddProgramsList(IconToBitmapImage(Properties.Resources.mycomp, 48), Properties.Resources.mycomp,(string)FindResource("compDetailsBt"), @"C:\Windows\explorer.exe", "shell:::{bb06c0e4-d293-4f75-8a90-cb05b6477eee}");
            AddProgramsList(IconToBitmapImage(Properties.Resources.personalize, 48), Properties.Resources.personalize, (string)FindResource("personalizeBt"), @"C:\Windows\explorer.exe", "shell:::{ED834ED6-4B5A-4bfe-8F11-A626DCB6A921}", (string)FindResource("personalizeTitle"), (string)FindResource("personalizeDetail"));
            AddProgramsForFile(rootPath + "\\WelcomeCenterApps.xml");
            descriptionPanel.Visibility = System.Windows.Visibility.Collapsed;
            appList.SelectedIndex = 0;
            try
            {
                theme = int.Parse(ini.IniReadValue("settings", "theme"));
                Themes();
            }
            catch (Exception)
            {
                
            }
            if (ini.IniReadValue("settings", "sort") == "true")
            {
                Sort(true);
            }
            
            
            
        }

        private void AddProgramsList(ImageSource image, System.Drawing.Icon icon, string name, string path, string pathSw)
        {
            appList.Items.Add(new MyApps()
            {
                Image = image,
                Icon = icon,
                Name = name,
                Path = path,
                PathSw = pathSw
            });
        }

        private void AddProgramsList(ImageSource image, System.Drawing.Icon icon, string name, string path, string pathSw, string descTitle, string description)
        {
            appList.Items.Add(new MyApps()
            {
                Id = 1,
                Image = image,
                Icon = icon,
                Name = name,
                Path = path,
                PathSw = pathSw,
                DescTitle = descTitle,
                Description = description
            });
        }
        private void AddProgramsForFile(string pathxml)
        {
             if (pathxml != null)
                {
                    DataSet channelsdata = new DataSet("AppsDataSet");
                    channelsdata.ReadXml(pathxml);
                    for (int i = 0; i < channelsdata.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            DataRow rw = channelsdata.Tables[0].Rows[i];
                            var extractor = new IconExtractor(rw[3].ToString());
                            System.Drawing.Icon appico = extractor.GetIcon(0);

                            AddProgramsList(IconToBitmapImage(appico, 48), appico, rw[0].ToString(), rw[1].ToString(), rw[2].ToString(), rw[4].ToString(), rw[5].ToString());
                        
                        }
                        catch (Exception ex)
                        {
                        }
                        

                    }
                }
        }
        void starting_timer_Tick(object sender, EventArgs e)
        { 
            Dispatcher.Invoke((Action)delegate
                    {
            if (welcomeText.Opacity < 0.70)
            {
                welcomeText.Opacity = welcomeText.Opacity +0.02;
            }
            else
            {
                timer.Stop();
            }
                    });
        }

        public BitmapImage IconToBitmapImage(System.Drawing.Icon ico, int Size)
        {
            using (var memory = new MemoryStream())
            {
                System.Drawing.Image img = new System.Drawing.Icon(ico, new System.Drawing.Size(Size, Size)).ToBitmap();
                img.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        BitmapImage ExtractVistaIcon(System.Drawing.Icon icoIcon)
        {
            BitmapImage bmpPngExtracted = null;
            try
            {
                byte[] srcBuf = null;
                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                { icoIcon.Save(stream); srcBuf = stream.ToArray(); }
                const int SizeICONDIR = 6;
                const int SizeICONDIRENTRY = 16;
                int iCount = BitConverter.ToInt16(srcBuf, 4);
                for (int iIndex = 0; iIndex < iCount; iIndex++)
                {
                    int iWidth = srcBuf[SizeICONDIR + SizeICONDIRENTRY * iIndex];
                    int iHeight = srcBuf[SizeICONDIR + SizeICONDIRENTRY * iIndex + 1];
                    int iBitCount = BitConverter.ToInt16(srcBuf, SizeICONDIR + SizeICONDIRENTRY * iIndex + 6);
                    if (iWidth == 0 && iHeight == 0 && iBitCount == 32)
                    {
                        int iImageSize = BitConverter.ToInt32(srcBuf, SizeICONDIR + SizeICONDIRENTRY * iIndex + 8);
                        int iImageOffset = BitConverter.ToInt32(srcBuf, SizeICONDIR + SizeICONDIRENTRY * iIndex + 12);
                        System.IO.MemoryStream destStream = new System.IO.MemoryStream();
                        System.IO.BinaryWriter writer = new System.IO.BinaryWriter(destStream);
                        writer.Write(srcBuf, iImageOffset, iImageSize);
                        destStream.Seek(0, System.IO.SeekOrigin.Begin);
                        bmpPngExtracted = new BitmapImage();
                        bmpPngExtracted.BeginInit();
                        bmpPngExtracted.StreamSource = destStream;
                        bmpPngExtracted.EndInit();
                        //bmpPngExtracted = new BitmapImage(destStream); // This is PNG! :)
                        break;
                    }
                }
            }
            catch { return null; }
            return bmpPngExtracted;
        }
        public static System.Drawing.Icon GetIconFromEmbeddedResource(string name, System.Drawing.Size size)
        {
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var rnames = asm.GetManifestResourceNames();
            var tofind = "." + name + ".ICO";
            foreach (string rname in rnames)
            {
                if (rname.EndsWith(tofind, StringComparison.CurrentCultureIgnoreCase))
                {
                    using (var stream = asm.GetManifestResourceStream(rname))
                    {
                        return new System.Drawing.Icon(stream, size);
                    }
                }
            }
            throw new ArgumentException("Icon not found");
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);


        public void getSysInfo()
        {
            uName.Content = Environment.UserName;
            string userSid = WindowsIdentity.GetCurrent().User.ToString();
            RegistryKey pic_key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
            pic_key = pic_key.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\AccountPicture\Users\" + userSid);

                if (pic_key != null)
                {
                    if (pic_key.GetValue("Image32") != null)
                    {
                        string path = pic_key.GetValue("Image32").ToString();
                        BitmapImage picimg = new BitmapImage();
                        picimg.BeginInit();
                        picimg.UriSource = new Uri(path);
                        picimg.EndInit();
                        sel_IMG.Source = picimg;
                    }
                   
                }
                else
                {
                    try
                    {
                        string path = temp + Environment.UserName + ".bmp";
                        BitmapImage picimg = new BitmapImage();
                        picimg.BeginInit();
                        picimg.UriSource = new Uri(path);
                        picimg.EndInit();
                        sel_IMG.Source = picimg;
                    }
                    catch (Exception)
                    {
                        try
                        {
                            string path = programdata + @"\Microsoft\User Account Pictures\user.png";
                            BitmapImage picimg = new BitmapImage();
                            picimg.BeginInit();
                            picimg.UriSource = new Uri(path);
                            picimg.EndInit();
                            sel_IMG.Source = picimg;
                        }
                        catch (Exception)
                        {
                            
                        }
                    }
                }
            var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();
            systemEdition.Content = name;
            ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            foreach (ManagementObject mo in mos.Get())
            {
                cpu.Content = (mo["Name"]);
            }
            ManagementObjectSearcher mos2 = new ManagementObjectSearcher("select * from Win32_VideoController");
            foreach (ManagementObject mo in mos2.Get())
            {
                gpu.Content = (mo["Name"]);
            }
            ulong ram2 = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
            const ulong GB_BYTES = 1024 * 1024 * 1024;
            ulong ramGB = (ulong)Math.Round((double)ram2 / GB_BYTES);

            long memKb;
            GetPhysicallyInstalledSystemMemory(out memKb);
            ram.Content = ((memKb / 1024 / 1024) + " GB RAM");
            if (memKb == 0)
            {
                ram.Content = ramGB.ToString() + " GB RAM";
            }
           
            pcname.Content = System.Environment.MachineName;
            
          
        }

        private void runAppBt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (appList.SelectedItem != null)
                {
                    ListBoxObject = (MyApps)appList.SelectedItem;
                    Process process = new Process();
                    process.StartInfo.FileName = macska + ListBoxObject.Path + macska;
                    process.StartInfo.Arguments = ListBoxObject.PathSw;
                    process.StartInfo.WorkingDirectory = Path.GetDirectoryName(ListBoxObject.Path);
                    process.Start();
                }
            }
            catch (Exception)
            {
               
            }
           
           

        }

        private void appList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (appList.SelectedItem != null)
            {
                ListBoxObject = (MyApps)appList.SelectedItem;
                BitmapImage im = ExtractVistaIcon(ListBoxObject.Icon);
                if (im != null)
                {
                    imageAnim(ExtractVistaIcon(ListBoxObject.Icon));
                }
                else
                {
                    imageAnim(IconToBitmapImage(ListBoxObject.Icon, 48));
                }
                if (ListBoxObject.DescTitle != null)
                {
                    descriptionPanel.Visibility = System.Windows.Visibility.Visible;
                    systemPropPanel.Visibility = System.Windows.Visibility.Collapsed;
                    titleTxt.Content = ListBoxObject.DescTitle;
                    descriptionTxt.Text = ListBoxObject.Description;
                }
                else
                {
                    descriptionPanel.Visibility = System.Windows.Visibility.Collapsed;
                    systemPropPanel.Visibility = System.Windows.Visibility.Visible;
                    titleTxt.Content = "";
                    descriptionTxt.Text = "";
                }
                if (ListBoxObject.Id == 1)
                {
                    welcomeText.Visibility = System.Windows.Visibility.Hidden;
                    welcomeText.Opacity = 0;
                    goText.Content = (string)FindResource("startBt");
                }
                else
                {
                    welcomeText.Visibility = System.Windows.Visibility.Visible;
                    timer.Start();
                    goText.Content = (string)FindResource("moreBt");
                }

            }
        }

        private void imageAnim(ImageSource imgNew)
        {
            int x = 0;
            int y = 0;
            double w = 128;
            double h = 128;
            bool complete = false;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Start();
            timer.Tick += (s, e) =>
            {
                if (w > 24 && complete == false)
                {
                    x = x - 8;
                    y = y + 8;
                    h = logoIMG.Height - 24;
                    w = logoIMG.Width - 24;
                    Canvas.SetLeft(logoIMG, x);
                    Canvas.SetTop(logoIMG, y);
                    logoIMG.Width = w;
                    logoIMG.Height = h;
                    logoIMG.Opacity = logoIMG.Opacity - 0.10;
                }
                else
                {
                    complete = true;
                    logoIMG.Source = imgNew;
                    x = 0;
                    y = 0;
                    logoIMG.Opacity = 0;
                }
                if (x == 0)
                {
                    h = logoIMG.Height + 24;
                    w = logoIMG.Width + 24;
                    Canvas.SetLeft(logoIMG, x);
                    Canvas.SetTop(logoIMG, y);
                    if (w < 128)
                    {
                        logoIMG.Width = w;
                        logoIMG.Height = h;
                        logoIMG.Opacity = logoIMG.Opacity + 0.10;
                    }
                    else
                    {
                        timer.Stop();
                        complete = false;
                        logoIMG.Opacity = 1;
                    }
                }
            };



        }

        private void settingsBt_Click(object sender, RoutedEventArgs e)
        {
            Settings sett = new Settings();
            sett.Owner = this;
            Settings.theme = theme;
            sett.SettingEvent += new Settings.SettingEventHandler(settingFrm_Events);
            sett.Show();
        }

        private void Themes()
        {
            Label element;
            if (theme == 1)
            {
                header.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/WelcomeCenter;component/Images/w7Head.png", UriKind.RelativeOrAbsolute)));
                for (int i = 0; i < descriptionPanel.Children.Count; i++)
                {
                    if (descriptionPanel.Children[i] is Label)
                    {
                        element = (Label)descriptionPanel.Children[i];
                        element.Foreground = Brushes.Black;
                    }
                }
                for (int i = 0; i < systemPropPanel.Children.Count; i++)
                {
                    if (systemPropPanel.Children[i] is Label)
                    {
                        element = (Label)systemPropPanel.Children[i];
                        element.Foreground = Brushes.Black;
                    }
                }
                descriptionTxt.Foreground = Brushes.Black;
                welcomeText.Foreground = Brushes.CadetBlue;
                return;
            }
            else if (theme == 0)
            {
                header.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/WelcomeCenter;component/Images/VistaHead.png", UriKind.RelativeOrAbsolute)));
            }
            else if (theme == 2)
            {
                LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();
                myLinearGradientBrush.StartPoint = new Point(0, 0.5);
                myLinearGradientBrush.EndPoint = new Point(1, 0.5);
                myLinearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 10, 41, 130), 0.0));
                myLinearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 83, 149, 230), 1.0));
                header.Background = myLinearGradientBrush;
            }
            
            for (int i = 0; i < descriptionPanel.Children.Count; i++)
            {
                if (descriptionPanel.Children[i] is Label)
                {
                    element = (Label)descriptionPanel.Children[i];
                    element.Foreground = Brushes.White;
                }
            }
            for (int i = 0; i < systemPropPanel.Children.Count; i++)
            {
                if (systemPropPanel.Children[i] is Label)
                {
                    element = (Label)systemPropPanel.Children[i];
                    element.Foreground = Brushes.White;
                }
            }
            descriptionTxt.Foreground = Brushes.White;
            welcomeText.Foreground = Brushes.White;
        }

        private void settingFrm_Events()
        {
            theme = Settings.theme;
            Themes();
            ini.IniWriteValue("settings", "theme", theme.ToString());
            appList.Items.Clear();
            AddProgramsList(IconToBitmapImage(Properties.Resources.mycomp, 48), Properties.Resources.mycomp, (string)FindResource("compDetailsBt"), @"C:\Windows\explorer.exe", "shell:::{bb06c0e4-d293-4f75-8a90-cb05b6477eee}");
            AddProgramsList(IconToBitmapImage(Properties.Resources.personalize, 48), Properties.Resources.personalize, (string)FindResource("personalizeBt"), @"C:\Windows\explorer.exe", "shell:::{ED834ED6-4B5A-4bfe-8F11-A626DCB6A921}", (string)FindResource("personalizeTitle"), (string)FindResource("personalizeDetail"));
            AddProgramsForFile(rootPath + "\\WelcomeCenterApps.xml");
            if (ini.IniReadValue("settings", "sort") == "true")
            {
                Sort(true);
            }
            Langs();
            appList.Items.Refresh();
        }

        private void Sort(bool name)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(appList.Items);
            view.SortDescriptions.Clear();
            if (name)
            {
                view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));  
            }
            else
            {
                view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Descending)); 
            }
            
        }

    }



}
