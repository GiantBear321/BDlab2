using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BDGA2.Controllers
{
    public class EmailController : BaseController
    {
        public EmailController(string connectionString) : base(connectionString) { }

        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select id, address, reg_date, phone_num from email";

            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("address: {0}", rdr.GetValue(1));
                    Console.WriteLine("reg_date: {0}", rdr.GetValue(2));
                    Console.WriteLine("phone_num: {0}", rdr.GetValue(3));
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            finally
            {
                sqlConnection.Close();
            }


            Console.ReadLine();
        }

        public override void Create()
        {
            string sqlInsert = "Insert into email(address, reg_date, phone_num) VALUES(@address, @reg_date, @phone_num)";

            string address = null;
            string phone_num = null;
            DateTime reg_date = new DateTime(0,0,0);

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter email properties:");
                Console.WriteLine("address:");
                address = Console.ReadLine();
                Console.WriteLine("phone_num:");
                phone_num = Console.ReadLine();
                Console.WriteLine("Registration date:");
                Console.WriteLine("Year:");
                int year = Int32.Parse(Console.ReadLine());
                Console.WriteLine("Month:");
                int month = Int32.Parse(Console.ReadLine());
                Console.WriteLine("Day:");
                int day = Int32.Parse(Console.ReadLine());
                reg_date = new DateTime(year, month, day);
                correct = true;
            } while (correct == false);

            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("address", address);
            cmd.Parameters.AddWithValue("phone_num", phone_num);
            cmd.Parameters.AddWithValue("reg_date", reg_date);
            cmd.Prepare();

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public override void Delete()
        {
            base.Delete("delete from email where id = ");
        }
        public override void Update()
        {
            base.Update("Update email ");
        }
        public override void Find()
        {
            base.Find();
        }

        public override void Generate()
        {
            Console.WriteLine("How many records do you want?");
            bool correct = false;
            int recordsAmount;

            correct = Int32.TryParse(Console.ReadLine(), out recordsAmount);

            string sqlGenerate = "insert into email(address, reg_date, phone_num) (select "
                + base.sqlRandomString + ","
                + base.sqlRandomString + ","
                + base.sqlRandomDate 
                + " from generate_series(1, 1000000),  limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }
    }
}
