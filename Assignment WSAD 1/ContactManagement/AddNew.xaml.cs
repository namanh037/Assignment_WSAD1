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
    public sealed partial class AddNew : Page
    {
        public List<string> _lstGroup { get; set; }
        public AddNew()
        {
            this.InitializeComponent();
            _lstGroup = new List<string>();
            _lstGroup.Add("");
            _lstGroup.AddRange(Contact.ListFixedGroup);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ltrError.Text = string.Empty;

            try
            {
                ValidateContact();
                Contact existedItem = App._currentContacts.Where(c => c.Name == txtName.Text.Trim()).FirstOrDefault();
                if (existedItem != null)
                    throw new Exception("Tên danh bạ không được trùng");

                App._currentContacts.Add(new Contact()
                {
                    Name = txtName.Text,
                    Mobile = txtMobile.Text,
                    GroupName = cbbGroup.SelectedValue.ToString()
                });

                Frame.Navigate(typeof(Display), txtName.Text.Trim());
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

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ltrError.Text = string.Empty;

            try
            {
                txtName.Text = string.Empty;
                txtMobile.Text = string.Empty;
                cbbGroup.SelectedIndex = 0;
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
    }
}
