using ContactManagement.Model;
using System;
using System.Collections.Generic;
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
    public sealed partial class Display : Page
    {
        private string _name = "";
        public List<string> _lstGroup = Contact.ListFixedGroup;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _name = e.Parameter as string;

            Contact item = App._currentContacts.Where(c => c.Name.ToLower() == _name.ToLower()).FirstOrDefault();
            if (item == null)
                return;

            txtName.Text = item.Name;
            txtMobile.Text = item.Mobile;
            txtGroup.Text = item.GroupName;

            txtName.IsEnabled = txtMobile.IsEnabled = txtGroup.IsEnabled = false;
            cbbGroup.Visibility = Visibility.Collapsed;
        }

        public Display()
        {
            this.InitializeComponent();
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

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnEdit.Content.ToString() == "Edit")
                {
                    txtName.IsEnabled = txtMobile.IsEnabled = true;
                    txtGroup.Visibility = Visibility.Collapsed;
                    cbbGroup.Visibility = Visibility.Visible;

                    btnEdit.Content = "Save";
                    btnRemove.Content = "Cancel";
                }
                else
                {
                    Contact item = App._currentContacts.Where(c => c.Name.ToLower() == _name.ToLower()).FirstOrDefault();
                    if (item == null)
                        return;

                    ValidateContact();

                    item.Name = txtName.Text.Trim();
                    item.Mobile = txtMobile.Text.Trim();
                    item.GroupName = cbbGroup.SelectedItem.ToString();

                    Frame.Navigate(typeof(Display), item.Name);
                }
            }
            catch (Exception ex)
            {
                ltrError.Text = ex.Message;
            }
        }

        private void ValidateContact()
        {
            List<string> lstError = new List<string>();

            if (string.IsNullOrEmpty(txtName.Text.Trim()))
                lstError.Add("Tên không được trống");

            if (string.IsNullOrEmpty(txtMobile.Text.Trim()))
                lstError.Add("SDT không được trống");

            if (string.IsNullOrEmpty((string)cbbGroup.SelectedValue))
                lstError.Add("Nhóm không được trống");

            if (lstError.Count > 0)
                throw new Exception(string.Join("\n", lstError));
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (btnRemove.Content.ToString() == "Cancel")
            {
                Contact item = App._currentContacts.Where(c => c.Name.ToLower() == _name.ToLower()).FirstOrDefault();
                if (item == null)
                    return;

                Frame.Navigate(typeof(Display), item.Name);
            }
            else if (btnRemove.Content.ToString() == "Remove")
            {
                Contact item = App._currentContacts.Where(c => c.Name.ToLower() == _name.ToLower()).FirstOrDefault();
                if (item == null)
                    return;

                App._currentContacts.Remove(item);
                Frame.Navigate(typeof(Default));
            }
        }
    }
}
