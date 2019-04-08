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
using System.Windows.Shapes;

namespace CarRental
{
    /// <summary>
    /// Interaction logic for New_Reservation.xaml
    /// </summary>
    public partial class New_Reservation : Window
    {
        SQLToolBox sqlTB = new SQLToolBox("test");
        private int vehicleID = 0, vehicleTypeID = 0, customerID = 0, contractID = 0;
        private string startDate, endDate, reservedDate;
        private string reservationQuery, contractQuery;

        // This is randomly generated, will be used for customer reference when picking up the vehicle
        private string reservationNumber = "";

        // used to grab strings, it is to be converted into integer values
        private string strVehicleID, strVehicleTypeID, strCustomerID;

        // This variable will be used to for the when the reservation was made
        // Also used to set Default values in the start and end date DatePicker objects on the form
        DateTime rD = DateTime.Today;

        public New_Reservation()
        {
            InitializeComponent();
            setDefaultDates();
            // TESTING
            lblReservationNumber.Text = "";
            lblVehicleID.Text = "2";
            lblCustomerID.Text = "3";
            strCustomerID = lblCustomerID.Text;
            strVehicleTypeID = "1";
            strVehicleID = "2";
            // TESTING
        }
        public New_Reservation(string vID, string vTID)
        {
            InitializeComponent();
            setDefaultDates();
            // Gets values from Vehicle Inquiry Form 
            // Sets label to show vehicle ID and sets the strVehicleID as vehicle ID
            lblVehicleID.Text = vID;
            strVehicleID = vID;
            // Stores the vehicle ID from Vehicle Inquiry Form
            strVehicleTypeID = vTID;
            
        }

        // Grabs user information from create, edit, or search customer
        // Used to Verify all information is correct to customer
        public void updateData(string fN, string lN, string bD, string pN, string lsN, string a1, string a2, string cy, string st, string zp, int cID)
        {
            lblFirstName.Text = fN;
            lblLastName.Text = lN;
            lblBirthDate.Text = bD;
            lblPhoneNumber.Text = pN;
            lblLicenseNumber.Text = lsN;
            lblAddress1.Text = a1;
            lblAddress2.Text = a2;
            lblCity.Text = cy;
            lblState.Text = st;
            lblZipcode.Text = zp;
            lblCustomerID.Text = cID.ToString();
        }

        private void btnNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            
            MainWindow w = new MainWindow(this);
            w.Owner = Application.Current.MainWindow;
            w.ShowInTaskbar = false;
            w.Show();
        }

        private void btnSearchCustomer_Click(object sender, RoutedEventArgs e)
        {
            Contract contract = new Contract("ASLKU450");


        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
           
            wndNewReservation.Close();
        }

        private void btnCreateReservation_Click(object sender, RoutedEventArgs e)
        {
           
            // used to grab today's date, as it will be the date the reservation was made
            // These two values will be used to check that the end date of reservation is after the start date of the reservation
            DateTime sD = dtpStartDate.SelectedDate.Value;
            DateTime eD = dtpEndDate.SelectedDate.Value;

            // startDate must be before endDate
            if (eD == sD.AddDays(1))
            {
                MessageBoxResult result = MessageBox.Show("These are the default dates, are you sure this is the correct dates?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    setDates();
                }
                else
                {
                    // Sets these variables to empty string to be catched by the if statement below
                    startDate = endDate = "";
                }
            }
            else if (sD < eD)
            {
                setDates();
            }
            else
            {
                MessageBox.Show("End Date Must Be After Start Date.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // Sets these variables to empty string to be catched by the if statement below
                startDate = endDate = "";
            }
            // used to convert values from strings into Integer values
            if (strVehicleTypeID != "" && strCustomerID != "")
            {
                Int32.TryParse(strVehicleID, out vehicleID);
                Int32.TryParse(strVehicleTypeID, out vehicleTypeID);
                Int32.TryParse(strCustomerID, out customerID);
            }
            
            // Grabs customerID

            // only executes if all data is filled
            if (startDate != "" && endDate != "" && vehicleTypeID != 0 && customerID != 0 && reservedDate != "")
            {
                Reservation reservation = new Reservation(customerID, vehicleID, vehicleTypeID, startDate, endDate, reservedDate);
                reservation.CreateReservation();

            }
            else if (customerID == 0)
            {
                MessageBox.Show("Please search for a customer or create a new customer");
            }
        }

        private void setDates()
        {
            // grabs the strings from the labels
            //strVehicleTypeID = lblVehicleType.Text;
            startDate = dtpStartDate.Text;
            endDate = dtpEndDate.Text;

            // converts data into date for database
            reservedDate = rD.ToString("yyyy-MM-dd");
            startDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd");
            endDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd");
        }

        // This function is used to set default values for the DatePicker Objects
        // It is set for the startDate to be the current date and the endDate to the next day
        private void setDefaultDates()
        {
            dtpStartDate.Text = rD.Date.ToString();
            dtpEndDate.Text = rD.AddDays(1).Date.ToString();
        }        
    }
}
