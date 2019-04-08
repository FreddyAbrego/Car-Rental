using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental
{
    class Reservation
    {
        string reservationNo;
        int customerID, vehicleID, vehicleTypeID;
        string startDate, endDate, reservedDate;
        SQLToolBox sqlTB = new SQLToolBox("test");

        // contructor
        // the data insereted into this constructor has already gone through 
        // data validation
        public Reservation(int cID, int vID, int vTID, string sD, string eD, string rD)
        {
            customerID = cID;
            vehicleID = vID;
            vehicleTypeID = vTID;
            startDate = sD;
            endDate = eD;
            reservedDate = rD;
        }

        public void CreateReservation()
        {
            // string for reservation number, this is given to custome
            reservationNo = RandomString(10);
            // this verifies that the reservation number is not currently in used
            checkReservationNumber(ref reservationNo);


            // creates a contract object to send to database the information for contract
            Contract contract = new Contract(customerID, vehicleTypeID, startDate, endDate, reservationNo, reservedDate);
            // creates the contract that will be used and returns the contractID
            int contractID = contract.CreateContract();

            // The reservation database holds the contract and vehicle id
            string reservationQuery = String.Format("INSERT INTO Reservation(ContractID, VehicleID) " +
                "VALUES('{0}', '{1}');", contractID, vehicleID);
        }
        
        // used to generate the random string for the reservation number
        private static Random random = new Random();
        // function used to generate the random string LINQ
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Used to verify that reservation number is not already used
        // If the reservation number is alraedy in use then it creates a new reservation number
        // If it is replaced then the function is called recursively to verify the new reservation number
        // is not already in use
        private void checkReservationNumber(ref string rNo)
        {
            string checkQuery = String.Format("SELECT COUNT (*) FROM Contract WHERE ReservationN = '{0}';", reservationNo);
            int test;
            sqlTB.SingleAnswer(checkQuery, out test);
            if (test == 1)
            {
                reservationNo = RandomString(10);
                checkReservationNumber(ref reservationNo);
            }
        }
    }

    
}
