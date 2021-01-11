using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BDGA2.Controllers
{
    public class VideoController : BaseController
    {
        public VideoController(string connectionString) : base(connectionString) { }


        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select id, title, url, creator from video";

            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("title: {0}", rdr.GetValue(1));
                    Console.WriteLine("url: {0}", rdr.GetValue(2));
                    Console.WriteLine("creator: {0}", rdr.GetValue(3));
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
            string sqlInsert = "Insert into email(title, url, creator) VALUES(@title, @url, @creator)";

            string title = null;
            string url = null;
            int creator = 0;

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter video properties:");
                Console.WriteLine("title:");
                title = Console.ReadLine();
                Console.WriteLine("url:");
                url = Console.ReadLine();
                Console.WriteLine("creator:");
                creator = Int32.Parse(Console.ReadLine());
                correct = true;
            } while (correct == false);

            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("title", title);
            cmd.Parameters.AddWithValue("creator", creator);
            cmd.Parameters.AddWithValue("url", url);
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
            base.Delete("delete from video where id = ");
        }
        public override void Update()
        {
            base.Update("Update video");
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

            string sqlGenerate = "insert into video(title, url ,creator) (select "
                + base.sqlRandomString + ","
                + base.sqlRandomString 
                + " channel.id from generate_series(1, 1000000), channel  limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }
    }
}
