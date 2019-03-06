using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ActiveDirectorySearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AddUsersADToAgilepoint : Window
    {
        ActiveDirectoryHelper AD_Operations = null;

        public AddUsersADToAgilepoint()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateAgilepointServiceAccountControls())
                {
                    AD_Operations = new ActiveDirectoryHelper(txtLDAPPath.Text.Trim());
                    UpdateAgilepointConfiguration();
                    List<NXUserProfile> UserProfiles = AD_Operations.GetADUserProfiles(txtEmailAddress.Text.Trim());
                    if (UserProfiles?.Count >0)
                    {
                        dgvUsersDetails.CanUserAddRows = false;
                        dgvUsersDetails.ItemsSource = UserProfiles;
                       
                    }
                    else
                    {
                        Logger.Info($"No user found in Active Directory with specified details");
                        System.Windows.Forms.MessageBox.Show($"No user found in Active Directory with specified details");
                        
                    }
                  
                    
                }
            }
            catch (Exception ex)
            {

                Logger.Error(ex.Message);
            }

        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            if (dgvUsersDetails.SelectedItems.Count>0)
            {
                NXOperation NXOperation = new NXOperation();
                foreach (NXUserProfile UserProfile in dgvUsersDetails.SelectedItems)
                {
                    UserProfile.RegisteredDate = DateTime.UtcNow;
                    NXOperation.RegisterUser(UserProfile);
                }
                System.Windows.Forms.MessageBox.Show("Registation is successful. Please check log for more details");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please Select User from table");
            }
            
        }

        private void UpdateAgilepointConfiguration()
        {
            ADConfig.AgilePointServiceBaseUrl = txtAPRestUrl.Text.Trim();
            ADConfig.AgilePointUsername = txtAPUserName.Text.Trim();
            ADConfig.AgilePointPassword = txtAPPassword.Password.ToString().Trim();

        }

        private void OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private bool ValidateAgilepointServiceAccountControls()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(txtAPRestUrl.Text) || string.IsNullOrEmpty(txtAPUserName.Text) || string.IsNullOrEmpty(txtAPPassword.Password.ToString()) || string.IsNullOrEmpty(txtLDAPPath.Text.ToString()) || string.IsNullOrEmpty(txtEmailAddress.Text.ToString()))
            {
                isValid = false;
                System.Windows.Forms.MessageBox.Show("Please provide the AgilePoint and Active directory  Account Details ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return isValid;

        }
    }
}

