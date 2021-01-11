using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BDGA2.Controllers
{
    public class ChannelController : BaseController
    {
        public ChannelController(string connectionString) : base(connectionString) { }

        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select id, name, email from channel";

            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("Name: {0}", rdr.GetValue(1));
                    Console.WriteLine("Email: {0}", rdr.GetValue(2));
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
            string sqlInsert = "Insert into channel(name, email) VALUES(@name, @email)";

            string name = null;
            int email = 0;

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter Channel properties:");
                Console.WriteLine("Name:");
                name = Console.ReadLine();
                Console.WriteLine("Mail:");
                email = Int32.Parse(Console.ReadLine());
                correct = true;
            } while (correct == false);

            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("email", email);
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
            base.Delete("delete from channel where id = ");
        }
        public override void Update()
        {
            base.Update("Update channel ");
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

            string sqlGenerate = "insert into channel(name, email) (select "
                + base.sqlRandomString
                + ", email.id from generate_series(1, 1000000), email  limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }

    }
}
