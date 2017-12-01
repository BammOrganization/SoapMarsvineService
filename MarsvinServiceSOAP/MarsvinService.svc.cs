﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MarsvinServiceSOAP
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class MarsvinService : IMarsvinService
    {
        private const string ConnectionString = "Server=tcp:bamm.database.windows.net,1433;Initial Catalog=Bamm;Persist Security Info=False;User ID=Bamm;Password=Mik112mik112;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


        #region User metoder


        public User GetUserById()
        {
            const string SelectStudentByID = "Select * from UserTable where id = 6";
            using (var DBconnection = new SqlConnection(ConnectionString))
            {
                DBconnection.Open();
                using (var selectCommand = new SqlCommand(SelectStudentByID, DBconnection))
                {
                    using (var reader = selectCommand.ExecuteReader())
                    {
                        if (!reader.HasRows)
                            return null;
                        reader.Read();
                        var _user = ReadUser(reader);
                        return _user;
                    }
                }
            }
        }


        public int AddUser(User user)
        {
            const string insertUser = "INSERT INTO UserTable (Mail, PhoneNo) VALUES (@Mail, @PhoneNo)";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand insertCommand = new SqlCommand(insertUser, databaseConnection))
                {
                    insertCommand.Parameters.AddWithValue("@Mail", user.Mail);
                    insertCommand.Parameters.AddWithValue("@PhoneNo", user.PhoneNo);
                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    return rowsAffected;
                }
            }
        }

        public void DeleteUser(User user)
        {
            const string deleteUser = "DELETE FROM UserTable WHERE user LIKE @user";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand deleteCommand = new SqlCommand(deleteUser, databaseConnection))
                {
                    using (SqlDataReader reader = deleteCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                        }
                    }
                }
            }

        }

        public IList<User> GetAllUsers()
        {
            const string selectAllUsers = "SELECT * FROM UserTable";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectAllUsers, databaseConnection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        List<User> userList = new List<User>();
                        while (reader.Read())
                        {
                            User user = ReadUser(reader);
                            userList.Add(user);
                        }
                        return userList;
                    }
                }
            }
        }

        public IList<User> GetUserByMail(string mail)
        {
            const string selectAllWithMail = "SELECT * FROM UserTable WHERE Mail LIKE @mail";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectAllWithMail, databaseConnection))
                {
                    selectCommand.Parameters.AddWithValue("@mail", "%" + mail + "%");
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        IList<User> userList = new List<User>();
                        while (reader.Read())
                        {
                            User user = ReadUser(reader);
                            userList.Add(user);
                        }
                        return userList;
                    }
                }
            }
        }

        public IList<User> GetUserByPhoneNo(int phoneno)
        {
            const string selectAllWithPhoneNo = "SELECT * FROM UserTable WHERE PhoneNo LIKE @phoneno";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectAllWithPhoneNo, databaseConnection))
                {
                    selectCommand.Parameters.AddWithValue("@phoneno", "%" + phoneno + "%");
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        IList<User> userList = new List<User>();
                        while (reader.Read())
                        {
                            User user = ReadUser(reader);
                            userList.Add(user);
                        }
                        return userList;
                    }
                }
            }
        }



        private static User ReadUser(IDataRecord reader)
        {
            int id = reader.GetInt32(0);
            string mail = reader.GetString(1);
            int phoneno = reader.GetInt32(2);
            User user = new User
            {
                Id = id,
                Mail = mail,
                PhoneNo = phoneno,
            };
            return user;
        }

        #endregion


        #region Measurement Metoder

        public int AddMeasurement(Measurement measurement)
        {
            const string insertMeasurement = "INSERT INTO Measurement (dB, ImageLink) values (@dB, @ImageLink)";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand insertCommand = new SqlCommand(insertMeasurement, databaseConnection))
                {
                    insertCommand.Parameters.AddWithValue("@dB", measurement.dB);
                    insertCommand.Parameters.AddWithValue("@ImageLink", measurement.ImageLink);
                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    if (measurement.dB > 49)
                    {
                        NooootiiiiEmail();
                    }
                    return rowsAffected;
                }
            }
        }

        public int AddMeasurementNoLink(Measurement measurement)
        {
            const string insertMeasurement = "INSERT INTO Measurement (dB) values (@dB)";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand insertCommand = new SqlCommand(insertMeasurement, databaseConnection))
                {
                    insertCommand.Parameters.AddWithValue("@dB", measurement.dB);
                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    return rowsAffected;
                }
            }
        }

        #endregion

        //metode til email
     
        private void NooootiiiiEmail() 
        {

            string BrugerEmail = GetUserById().Mail;
            string subject = "Subject";
            string body = "Body";


            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("bammbruger@gmail.com");
            mail.To.Add(BrugerEmail);
            mail.Subject = subject;
            mail.Body = body;

            SmtpServer.Port = 587; //587 er gmail
            SmtpServer.Credentials = new System.Net.NetworkCredential("bammbruger@gmail.com", "mik112mik112");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }







    }
}
