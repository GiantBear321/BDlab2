using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BDGA2.Controllers
{
    public class CommentController : BaseController
    {
        public CommentController(string connectionString) : base(connectionString) { }

        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select id, creator, video, text from comment";

            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("Creator: {0}", rdr.GetValue(1));
                    Console.WriteLine("Video: {0}", rdr.GetValue(2));
                    Console.WriteLine("Text: {0}", rdr.GetValue(3));
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
            string sqlInsert = "Insert into comment(creator, video, text) VALUES(@creator, @video, @text)";

            int creator = 0;
            int video = 0;
            string text = null;

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter Comment properties:");
                Console.WriteLine("creator:");
                creator = Int32.Parse(Console.ReadLine());
                Console.WriteLine("video:");
                video = Int32.Parse(Console.ReadLine());
                Console.WriteLine("text:");
                text = Console.ReadLine();
                correct = true;
            } while (correct == false);

            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("creator", creator);
            cmd.Parameters.AddWithValue("video", video);
            cmd.Parameters.AddWithValue("text", text);
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
            base.Delete("delete from comment where id = ");
        }
        public override void Update()
        {
            base.Update("Update comment ");
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

            string sqlGenerate = "insert into comment(video, creator, text) (select video.id, channel.id"
                + base.sqlRandomString
                + ", email.id from generate_series(1, 1000000), video, channel  limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }
    }
}
