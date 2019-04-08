using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CarRental
{
    class Contract
    {
        private string reservationNo;
        private int customerID, vehicleTypeID;
        private string startDate, endDate, reservedDate;
        private SQLToolBox sqlTB = new SQLToolBox("test");

        // Used as default constructor 
        public Contract(int cID, int vTID, string sD, string eD, string rD, string rNo)
        {
            customerID = cID;
            vehicleTypeID = vTID;
            startDate = sD;
            endDate = eD;
            reservedDate = rD;
            reservationNo = rNo;
        }

        // Contructor when only reservation number is known 
        // ex) When customer gives employee the reservation number
        public Contract(string rNo)
        {
            reservationNo = rNo;
            getContract(rNo);
        }

        public int CreateContract()
        {
            int contractID;
            double total = getTotal(startDate, endDate, vehicleTypeID);
            // Query to insert into database 
            string contractQuery = String.Format("INSERT INTO Contract(customerID, reservationN, StartDate, EndDate, DateRes, Total) " +
                            "VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');", customerID, reservationNo, startDate, endDate, reservedDate, total);

            sqlTB.InsertInto(contractQuery);

            // Query to grab the contractiD
            string getCID = String.Format("SELECT ContractID FROM Contract WHERE customerID = '{0}' AND reservationN = '{1}' AND StartDate = '{2}' AND EndDate = '{3}';",
                                            customerID, reservationNo, startDate, endDate);
            sqlTB.SingleAnswer(getCID, out  contractID);
            return contractID;
        }

        private double getTotal(string sD, string eD, int vID)
        {
            double total, rate;
            DateTime start, end;
            start = Convert.ToDateTime(sD);
            end = Convert.ToDateTime(eD);

            // this grabs the amount of days in between dates, plus 1 to include the day of rental
            double days = (end - start).TotalDays + 1;

            string rateQuery = String.Format("SELECT Price " +
                                             "FROM VehicleTypes " +
                                             "WHERE VehicleTypeID = '{0}';", vehicleTypeID);
            sqlTB.SingleAnswer(rateQuery, out rate);
            total = rate * days;
            return total;
        }

        // This function queries the Contract table in the database to get the information
        private void getContract(string rNo)
        {
            string getQuery = String.Format("SELECT * FROM Contract WHERE ReservationN = '{0}';", rNo);
            DataTable dt = new DataTable();
            sqlTB.FillDataTable(getQuery, ref dt);

            reservationNo = rNo;
            foreach (DataRow dr in dt.Rows)
            {
                //reservationNo = (string)dr["ReservationNo"];
                customerID = (int)dr["CustomerID"];
                vehicleTypeID = (int)dr["VehicleTypeID"];
                startDate = (string)dr["StartDate"].ToString();
                endDate = (string)dr["EndDate"].ToString();
                reservedDate = (string)dr["DateRes"].ToString();
            }
            // customerID = (from DataRow dr in dt.Rows select (int) dr["CustomerID"]).FirstOrDefault();
        }

    }
}
