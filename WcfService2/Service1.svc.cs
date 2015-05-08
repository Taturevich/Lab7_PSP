using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.Globalization;
using System.Data.Common;
using System.Data.SqlClient;

namespace WcfService2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        static int count = 2; //счетчик ходов
        static int sender = 0; //счетчик для отправки
        static int number = 0;
        static string connectionString = "Data Source=FINDER-ПК;Initial Catalog=Lab6;Integrated Security=True";
        static SqlConnection conn = new SqlConnection(connectionString);
        static Object thisLock = new Object();
        static List<int> locker = new List<int>();
        static bool accept = false;

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        /// <summary>
        /// Метод для получения номера игрока
        /// </summary>
        /// <returns></returns>
        public int GetNumber()
        {
            lock (thisLock)
            {
                number++;
                return number;
            }
        }

        /// <summary>
        /// Запись данных в БД
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string WriteData(string value)
        {
            lock (thisLock)
            {
                //Добавление данных в БД
                conn.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Coordinates (Date) VALUES ('" + value + "');", conn);
                command.ExecuteNonQuery();
                conn.Close();
            }
            locker.Clear();
            accept = true;
            sender = 0;
            //string msg = "STOP";
            return string.Format("STOP");
        }

        /// <summary>
        /// Метод для отправки данных остальным игрокам
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string SendData(int value)
        {
            string Date;
            if (accept == true && !locker.Contains(value))
            {
                lock (thisLock)
                {
                    //Чтение данных из БД
                    conn.Open();
                    SqlDataReader dataread;
                    SqlCommand command = new SqlCommand("SELECT * FROM Coordinates WHERE Id=(SELECT MAX(Id) FROM Coordinates);", conn);
                    dataread = command.ExecuteReader();
                    dataread.Read();
                    Date = Convert.ToString(dataread["Date"]);
                    conn.Close();
                }
                string Msg = Date;
                sender++;
                locker.Add(value);
                if (sender == 3)
                {
                    accept = false;
                    if (count != 4)
                        count++;
                    else
                        count = 1;
                }
                return Msg;

            }
            else
            {
                string Msg = "NO";
                return Msg;
            }
        }


        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
