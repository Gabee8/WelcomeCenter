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
using Ini;
using System.Xml;
using System.Xml.Linq;
using System.Data;
using System.Threading;
using TsudaKageyu;
using System.IO;

namespace WelcomeCenter
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public static bool isW7 = false;
        public static int theme = 0;
        public delegate void SettingEventHandler();
        public event SettingEventHandler SettingEvent;
        string rootPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string macska = "\"";
        
        //Konfigurációfájl helye
        IniFile ini = new IniFile(Environment.CurrentDirectory + "\\Settings.ini");
        //Lista elemei
        public InstallApps ListviewObject;
        public class InstallApps
        {
            public string AppName { get; set; }
            public string Location { get; set; }
            public string Silent { get; set; }
            public string Icon { get; set; }
            public string DescTitle { get; set; }
            public string Description { get; set; }
        }

        public Settings()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (theme == int.Parse(rb0.Tag.ToString()))
                rb0.IsChecked = true;
            if (theme == int.Parse(rb1.Tag.ToString()))
                rb1.IsChecked = true;
            if (theme == int.Parse(rb2.Tag.ToString()))
                rb2.IsChecked = true;

            SettingsImport(rootPath + "\\WelcomeCenterApps.xml");
            if (ini.IniReadValue("settings", "sort") == "true")
            {
                sortName.IsChecked = true;
            }
            string SelectedLang = "";
            if (ini.IniReadValue("Settings", "Language") != "")
            {
                SelectedLang = ini.IniReadValue("Settings", "Language");
            }
            
            if (Directory.Exists(rootPath + "\\Languages"))
            {
                foreach (string f in Directory.GetFiles(rootPath + "\\Languages", "*.xaml"))
                {
                    string filename = Path.GetFileName(f);
                    string Onlyfilename = Path.GetFileNameWithoutExtension(f);
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = Onlyfilename;
                    item.Tag = "/Languages/" + filename;
                    lngCb.Items.Add(item);
                    if (SelectedLang == Onlyfilename)
                    {
                        lngCb.SelectedIndex = lngCb.Items.Count - 1;
                    }
                }
            }
            else if (Directory.Exists(rootPath + "\\Lang"))
            {
                foreach (string f in Directory.GetFiles(rootPath + "\\Lang", "*.xaml"))
                {
                    string filename = Path.GetFileName(f);
                    string Onlyfilename = Path.GetFileNameWithoutExtension(f);
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = Onlyfilename;
                    item.Tag = "/Lang/" + filename;
                    lngCb.Items.Add(item);
                    if (SelectedLang == Onlyfilename)
                    {
                        lngCb.SelectedIndex = lngCb.Items.Count - 1;
                    }
                }
            }
            else
            {
                foreach (string f in Directory.GetFiles(rootPath + "\\", "*.xaml"))
                {
                    string filename = Path.GetFileName(f);
                    string Onlyfilename = Path.GetFileNameWithoutExtension(f);
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = Onlyfilename;
                    item.Tag = "/" + filename;
                    lngCb.Items.Add(item);
                    if (SelectedLang == Onlyfilename)
                    {
                        lngCb.SelectedIndex = lngCb.Items.Count - 1;
                    }
                }
            }
            
        }

        public void SettingsImport(string pathxml)
        {
            try
            {
                if (pathxml != null)
                {
                    DataSet channelsdata = new DataSet("AppsDataSet");
                    channelsdata.ReadXml(pathxml);

                    for (int i = 0; i < channelsdata.Tables[0].Rows.Count; i++)
                    {
                        DataRow rw = channelsdata.Tables[0].Rows[i];

                        ListLv.Items.Add(new InstallApps()
                        {
                            AppName = rw[0].ToString(),
                            Location = rw[1].ToString(),
                            Silent = rw[2].ToString(),
                            Icon = rw[3].ToString(),
                            DescTitle = rw[4].ToString(),
                            Description = rw[5].ToString()
                        });

                    }
                }
            }
            catch (Exception ex)
            { }
        }
        
        private void Window_Closed(object sender, EventArgs e)
        {
            SettingEvent();
        }

       
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            theme = int.Parse(rb.Tag.ToString());
        }

        private void browseAppBt_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (Alocation.Text != "")
            {
                ofd.InitialDirectory = Path.GetDirectoryName(Alocation.Text);
            }
            ofd.Filter = (string)FindResource("openFilter1") + " (*.exe;*.cmd)|*.exe;*.cmd;*.bat|" + (string)FindResource("openFilter2") + " (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == true)
            {
                Alocation.Text = ofd.FileName;
                var extractor = new IconExtractor(ofd.FileName);
                System.Drawing.Icon appico = extractor.GetIcon(0);
                iconImg.Source = IconToBitmapImage(appico, 48);
                iconImg.Tag = ofd.FileName;
            }
        }

        private void ListLv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListLv.SelectedItem != null)
            {
                ListviewObject = (InstallApps)ListLv.SelectedItem;
                Aname.Text = ListviewObject.AppName;
                Alocation.Text = ListviewObject.Location;
                Asw.Text = ListviewObject.Silent;
                if (ListviewObject.Icon != null)
                {
                    var extractor = new IconExtractor(ListviewObject.Icon);
                    System.Drawing.Icon appico = extractor.GetIcon(0);
                    iconImg.Source = IconToBitmapImage(appico, 48);
                    iconImg.Tag = ListviewObject.Icon;
                }
                
                DescriptName.Text = ListviewObject.DescTitle;
                Description.Text = ListviewObject.Description;
            }
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

        private void saveApps(string XMLPath)
        {
            if (ListLv.SelectedItem != null)
            {
                ListviewObject = (InstallApps)ListLv.SelectedItem;
                string title = ListviewObject.AppName;
                bool isNew = true;
                XmlDocument doc2 = new XmlDocument();
                doc2.Load(XMLPath);
                foreach (XmlNode xNode in doc2.SelectNodes("Apps/App"))
                {
                    if (xNode.SelectSingleNode("Name").InnerText == title)
                    {
                        xNode.ChildNodes[0].InnerText = Aname.Text;
                        xNode.ChildNodes[1].InnerText = Alocation.Text;
                        xNode.ChildNodes[2].InnerText = Asw.Text;
                        xNode.ChildNodes[3].InnerText = iconImg.Tag.ToString();
                        xNode.ChildNodes[4].InnerText = DescriptName.Text;
                        xNode.ChildNodes[5].InnerText = Description.Text;
                        xNode.ParentNode.AppendChild(xNode);
                        ListviewObject.AppName = Aname.Text;
                        ListviewObject.Location = Alocation.Text;
                        ListviewObject.Silent =  Asw.Text;
                        ListviewObject.Icon = iconImg.Tag.ToString();
                        ListviewObject.DescTitle = DescriptName.Text;
                        ListviewObject.Description = Description.Text;
                        isNew = false;
                    }
                }
                if (isNew)
                {
                    XDocument doc = XDocument.Load(XMLPath);  
                    XElement root = new XElement("App");
                    root.Add(new XElement("Name", Aname.Text));
                    root.Add(new XElement("Path", Alocation.Text));
                    root.Add(new XElement("Switch", Asw.Text));
                    root.Add(new XElement("Icon", iconImg.Tag.ToString()));
                    root.Add(new XElement("Title", DescriptName.Text));
                    root.Add(new XElement("Description", Description.Text));
                    doc.Element("Apps").Add(root);
                    doc.Save(XMLPath);
                    ListLv.Items.Clear();
                    SettingsImport(XMLPath);
                }
                else
                {
                    doc2.Save(XMLPath);
                }
                
                Dispatcher.BeginInvoke(new ThreadStart(() =>
                {
                    ListLv.SelectedItem = ListviewObject;
                    ListLv.Items.Refresh();
                }));
            }
        }

        private void delApps(string XMLPath)
        {
            ListviewObject = (InstallApps)ListLv.SelectedItem;
            string title = ListviewObject.AppName;
            XmlDocument doc2 = new XmlDocument();
            doc2.Load(XMLPath);
            XmlNodeList nodes = doc2.SelectNodes("Apps/App");
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                if (nodes[i].SelectSingleNode("Name").InnerText == title)
                {
                    nodes[i].ParentNode.RemoveChild(nodes[i]);
                }
            }
            doc2.Save(XMLPath);
           
            ListLv.Items.Remove(ListLv.SelectedItem);
            Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                ListLv.Items.Refresh();
            }));
        }

        private void okBt_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void saveBt_Click(object sender, RoutedEventArgs e)
        {
            saveApps(rootPath + "\\WelcomeCenterApps.xml");
        }

        private void addBt_Click(object sender, RoutedEventArgs e)
        {
            ListviewObject = new InstallApps();
            ListviewObject.AppName = (string)FindResource("appNew");
            ListLv.Items.Add(ListviewObject);
            ListLv.SelectedIndex = ListLv.Items.Count - 1;
        }

        private void MFileDel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show((string)FindResource("appDelQ"),
                                          (string)FindResource("appDel"), MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                delApps(rootPath + "\\WelcomeCenterApps.xml");
            }
        }

        private void sortName_Checked(object sender, RoutedEventArgs e)
        {
            ini.IniWriteValue("settings", "sort", "true");
        }

        private void sortName_Unchecked(object sender, RoutedEventArgs e)
        {
            ini.IniWriteValue("settings", "sort", "false");
        }

        private void lngCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ini.IniWriteValue("settings", "Language", lngCb.SelectedValue.ToString());
        }
    }
}
