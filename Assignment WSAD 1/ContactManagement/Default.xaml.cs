using ContactManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ContactManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
     sealed partial class Default : Page
    {
        public ObservableCollection<Contact> ListContact = new ObservableCollection<Contact>();
        public static ObservableCollection<Contact> ListAll;
        public Default()
        {
            this.InitializeComponent();
            ListAll = App._currentContacts;

            for (int i = 0; i < App._currentContacts.Count; i++)
            {
                ListContact.Add(App._currentContacts[i]);
            }
        }

        private void txtNewString_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            ltrError.Text = string.Empty;
            try
            {
                string keywords = sender.Text.Trim().ToLower();
                if (string.IsNullOrEmpty(keywords))
                {
                    if (ListContact.Count == ListAll.Count)
                        return;
                }
                else
                {
                    int allCount = ListAll.Count;
                    for (int i = 0; i < allCount; i++)
                    {
                        ListContact.Add(ListAll[i]);
                    }
                }


                List<Contact> lstItem = ListAll.Where(c => c.Name.ToLower().Contains(keywords)).ToList();

                int size = ListContact.Count;
                for (int i = 0; i < size; i++)
                {
                    ListContact.RemoveAt(size - 1 - i);
                }

                foreach (var item in lstItem)
                {
                    ListContact.Add(item);
                }
            }
            catch (Exception ex)
            {
                ltrError.Text = ex.Message;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void btnList_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Default));
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Frame.Navigate(typeof(AddNew));
            }
            catch (Exception ex)
            {
                ltrError.Text = ex.Message;
            }
        }

        private void viewContact_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ltrError.Text = string.Empty;

            try
            {
                Contact selectedItem = (sender as ListView).SelectedItem as Contact;
                string name = selectedItem.Name;

                Frame.Navigate(typeof(Display), name);
            }
            catch (Exception ex)
            {
                ltrError.Text = ex.Message;
            }
        }
    }
}
