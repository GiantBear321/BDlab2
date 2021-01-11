using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BDGA2.Controllers
{
    public class ChannelFollowController : BaseController
    {
        public ChannelFollowController(string connectionString) : base(connectionString) { }

        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select id, channel, follow from channel_follow";

            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("Channel: {0}", rdr.GetValue(1));
                    Console.WriteLine("Follow: {0}", rdr.GetValue(2));
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
            string sqlInsert = "Insert into channel_follow(channel, follow) VALUES(@channel, @follow)";

            int channel = 0;
            int follow = 0;

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter Channel_follow properties:");
                Console.WriteLine("Channel:");
                channel = Int32.Parse(Console.ReadLine());
                Console.WriteLine("Follow:");
                follow = Int32.Parse(Console.ReadLine());
                correct = true;
            } while (correct == false);

            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("channel", channel);
            cmd.Parameters.AddWithValue("follow", follow);
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
            base.Delete("delete from channel_follow where id = ");
        }
        public override void Update()
        {
            base.Update("Update channel_follow ");
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

            string sqlGenerate = "insert into channel_follow(channel, follow) (select "
                + "channel.id , channel.id from channel limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }
    }
}
