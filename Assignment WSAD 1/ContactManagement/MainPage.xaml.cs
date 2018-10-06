using ContactManagement.Model;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ContactManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public StorageFile _storageFile;
        public MainPage()
        {
            InitializeComponent();
            GenFileOrRead();
        }

        private async void GenFileOrRead()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            _storageFile = await storageFolder.CreateFileAsync("Contact.txt", CreationCollisionOption.OpenIfExists);
        }

        public async void WriteData()
        {
            string content = "";
            foreach (var item in App._currentContacts)
            {
                string line = string.Join("|", item.Name, item.Mobile, item.GroupName);
                content += (line + Environment.NewLine);
            }
            await FileIO.WriteTextAsync(_storageFile, content);
            App._currentContacts = new System.Collections.ObjectModel.ObservableCollection<Model.Contact>();
        }

        public async Task ReadData()
        {
            App._currentContacts = new ObservableCollection<Contact>();
            var buffer = await Windows.Storage.FileIO.ReadBufferAsync(_storageFile);

            using (var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer))
            {
                string text = dataReader.ReadString(buffer.Length);

                string[] lstContactLine = text.Split(new string[] {"\r\n" }, StringSplitOptions.None);
                if (lstContactLine.Length == 1 && string.IsNullOrEmpty(lstContactLine[0]))
                    return;

                for (int i = 0; i < lstContactLine.Length; i++)
                {
                    string[] properties = lstContactLine[i].Split('|');
                    if (properties.Length != 3)
                        return;

                    App._currentContacts.Add(new Contact()
                    {
                        Name = properties[0],
                        Mobile = properties[1],
                        GroupName = properties[2]
                    });
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void btnList_Click(object sender, RoutedEventArgs e)
        {
            ltrError.Text = string.Empty;

            try
            {
                await ReadData();
                Frame.Navigate(typeof(Default));
            }
            catch (Exception ex)
            {
                ltrError.Text = ex.Message;
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WriteData();
            }
            catch (Exception ex)
            {
                ltrError.Text = ex.Message;
            }
        }
    }
}

