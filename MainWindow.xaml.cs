using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace CarRental
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLToolBox sqlTB = new SQLToolBox("test");
        // parent of the New Customer Form
        New_Reservation parent;

        public MainWindow(New_Reservation nR)
        {
            InitializeComponent();
            // 
            parent = nR;
            // puts first name textbox to focus
            txtFirstName.Focus();
        }

        // closes the create customer window
        // TO BE IMPLEMENTED: go back to create reservation
        private void btnCreateCancel_Click(object sender, RoutedEventArgs e)
        {
            wndNewCustomer.Close();
            parent.Show();
        }

        // Creates customer
        private void btnCustomerCreate_Click(object sender, RoutedEventArgs e)
        {
            // data from the previous 
            // variables for the textboxes
            string firstName, lastName, phoneNumber, licenseNumber = "1",
                address1, address2 = "", city, state, zipcode, birthDate;
            int customerID = 0;
            string bDay = "", sqlQuery, returnQuery = "";

            // getting values from textboxes
            firstName = txtFirstName.Text;
            lastName = txtLastName.Text;
            phoneNumber = txtPhoneNumber.Text;
            //licenseNumber = txtLicenseNumber.Text;
            address1 = txtAddress1.Text;
            address2 = txtAddress2.Text;
            city = txtCity.Text;
            state = ((ComboBoxItem)cmbState.SelectedItem).Tag.ToString();
            zipcode = txtZipcode.Text;
            
            // This is used if the date field is empty
            birthDate = dtpBirthdate.Text;
            if (birthDate != "")
            {
                // used to convert to SQL Date format 
                bDay = Convert.ToDateTime(birthDate).ToString("yyyy-MM-dd");
            }

            // Converts name, city, to have the first letter upper case and rest lower case
            firstName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(firstName.ToLower());
            lastName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(lastName.ToLower());
            address1 = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(address1.ToLower());
            address2 = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(address2.ToLower());
            city = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(city.ToLower());

            // Puts phone number in XXX-XXX-XXXX format
            if (phoneNumber != "")
            {
                phoneNumber = String.Format("{0:(###) ###-####}", double.Parse(phoneNumber));
            }

            // To make sure that the fields have a value within it
            // Address2 textbox is not needed to be filled out
            if (firstName == "" || lastName == "" || phoneNumber == "" || licenseNumber == "" || address1 == "" || city == "" || state == "NA" || zipcode == "" || bDay == "")
            {
                MessageBox.Show("One or more fields are empty.", "Empty fields!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                
                // Query to be submitted
                sqlQuery = String.Format("INSERT INTO Customers(FirstName, LastName, BirthDate, Address1, Address2, City, State, ZipCode, PhoneNumber, LicenseNumber) " +
                         "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');", firstName, lastName, bDay, address1, address2, city, state, zipcode, phoneNumber, licenseNumber);
                //MessageBox.Show(sqlQuery);
                sqlTB.InsertInto(sqlQuery);



                // TO IMPLEMENT: In statement that verfies sqlQuery went to DB correctly and return correct value
                // Customer ID will come from the database after the query has correctly gone through.

                returnQuery = String.Format("SELECT SCOPE_IDENTITY();");
                sqlTB.SingleAnswer(returnQuery, out customerID);

                parent.updateData(firstName, lastName, birthDate, phoneNumber, licenseNumber, address1, address2, city, state, zipcode, customerID);
                parent.Show();
                wndNewCustomer.Close();
            }
        }


        // These functions are used for data verification, however it only does it for the first character.
        private void txtFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtFirstName.Text, "^[a-zA-Z]"))
            {
                if (txtFirstName.Text.Length != 0)
                {
                    txtFirstName.Text = txtFirstName.Text.Substring(0, txtFirstName.Text.Length - 1);
                }
            }
        }

        private void txtLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtLastName.Text, "^[a-zA-Z]"))
            {
                if (txtLastName.Text.Length != 0)
                {
                    txtLastName.Text = txtLastName.Text.Substring(0, txtLastName.Text.Length - 1);
                }
            }
        }
        private void txtZipcode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtZipcode.Text, "^[a-zA-Z]"))
            {
                if (txtZipcode.Text.Length != 0)
                {
                    txtZipcode.Text = txtZipcode.Text.Substring(0, txtZipcode.Text.Length - 1);
                }
            }
        }
        private void txtPhoneNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            isNumeric(e);
            
        }

        private void txtZipcode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            isNumeric(e);
        }

        private void txtFirstName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            isString(e);
        }

        private void txtLastName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            isString(e);
        }

        // used to check input for phone number and zip code
        public void isNumeric(TextCompositionEventArgs e)
        {
            int result;
            if (!(int.TryParse(e.Text, out result) || e.Text == "."))
            {
                e.Handled = true;
            }
        }
        // used to check input for first and last name
        public void isString(TextCompositionEventArgs e)
        {
            int result;
            if ((int.TryParse(e.Text, out result) || e.Text == "."))
            {
                e.Handled = true;
            }
        }

        // Change txtPhoneNumber to the textbox name for phonenumber for all
        // TextChanged Event Handler is used
        // Textbox should have the MaxLength Property at 14 (###) ###-####
        // MaxLength 
        private void txtPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (txtPhoneNumber.Text.Length)
            {
                case 3:                                                                  // grabs the first 3 numbers ### and puts () around them => (###)
                    txtPhoneNumber.Text = System.Text.RegularExpressions.Regex.Replace(txtPhoneNumber.Text, @"(\d{3})", "($1)");
                    txtPhoneNumber.SelectionStart = txtPhoneNumber.Text.Length;
                    break;
                case 6:                                                                 // grabs (###)# and sepearates them with a space => (###) #
                    txtPhoneNumber.Text = System.Text.RegularExpressions.Regex.Replace(txtPhoneNumber.Text, @"\((\d{3})\)(\d{1})", "($1) $2");
                    txtPhoneNumber.SelectionStart = txtPhoneNumber.Text.Length;
                    break;
                case 10:                                                                // grabs (###) #### and seperates them with a - => (###) ###-#
                    txtPhoneNumber.Text = System.Text.RegularExpressions.Regex.Replace(txtPhoneNumber.Text, @"\((\d{3})\) (\d{3})(\d{1})", "($1) $2-$3");
                    txtPhoneNumber.SelectionStart = txtPhoneNumber.Text.Length;
                    break;
            }
        }

        // Change txtLicenseNumber to the textbox name for License Number for all
        // TextChanged Event Handler is used
        // Textbox should have the MaxLength Property at 17 L###-###-##-###-#
        private void txtBirthDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (txtBirthDate.Text.Length)
            {
                case 5:
                    txtBirthDate.Text = System.Text.RegularExpressions.Regex.Replace(txtBirthDate.Text, @"(\d{3})(\d{1})", "$1-$2");
                    txtBirthDate.SelectionStart = txtBirthDate.Text.Length;
                    break;
                case 9:
                    txtBirthDate.Text = System.Text.RegularExpressions.Regex.Replace(txtBirthDate.Text, @"(\d{3})-(\d{3})(\d{1})", "$1-$2-$3");
                    txtBirthDate.SelectionStart = txtBirthDate.Text.Length;
                    break;
                case 12:
                    txtBirthDate.Text = System.Text.RegularExpressions.Regex.Replace(txtBirthDate.Text, @"(\d{3})-(\d{3})-(\d{2})(\d{1})", "$1-$2-$3-$4");
                    txtBirthDate.SelectionStart = txtBirthDate.Text.Length;
                    break;
                case 16:
                    txtBirthDate.Text = System.Text.RegularExpressions.Regex.Replace(txtBirthDate.Text, @"(\d{3})-(\d{3})-(\d{2})-(\d{3})(\d{1})", "$1-$2-$3-$4-$5");
                    txtBirthDate.SelectionStart = txtBirthDate.Text.Length;
                    break;
            }
        }

        /*
        // Change txtBirthDate to the textbox name for birthdate for all
        // TextChanged Event Handler is used
        // Textbox should have the MaxLength Property at 10 mm/dd/yyyy
        private void txtBirthDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (txtBirthDate.Text.Length)
            {
                case 3:                                                                 // grabs the first 3 numbers mmd and places a / => mm/d
                    txtBirthDate.Text = System.Text.RegularExpressions.Regex.Replace(txtBirthDate.Text, @"(\d{2})(\d{1})", "$1/$2");
                    txtBirthDate.SelectionStart = txtBirthDate.Text.Length;
                    break;
                case 6:                                                                 // grabs mm/ddy and seperates with / => mm/dd/y
                    txtBirthDate.Text = System.Text.RegularExpressions.Regex.Replace(txtBirthDate.Text, @"(\d{2})\/(\d{2})(\d{1})", "$1/$2/$3");
                    txtBirthDate.SelectionStart = txtBirthDate.Text.Length;
                    break;
            }
        }

        */
       
    }
}
