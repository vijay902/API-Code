using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using WebApplication1.models;

namespace WebApplication1.Controllers
{
    public class GodownController : ApiController
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IEnumerable<Godown> Get()
        {
            List<Godown> godowns = new List<Godown>();

            string connStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            string sql = "SELECT * from godown";
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();

            try
            {
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Godown godown = new Godown();

                        godown.Id = reader.GetInt32(0);
                        godown.Code = reader.GetString(1);
                        godown.Description = reader.GetString(2);
                        godown.Address = reader.GetString(3);
                        godown.IsDefault = reader.GetBoolean(4);
                        godown.IsActive = reader.GetBoolean(5);

                        godowns.Add(godown);
                    }
                }

                conn.Close();

                return godowns;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public Godown Get(int id)
        {
            Godown godown = new Godown();

            string connStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            string sql = "SELECT * from godown where id='" + id + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();

            try
            {
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        godown.Id = reader.GetInt32(0);
                        godown.Code = reader.GetString(1);
                        godown.Description = reader.GetString(2);
                        godown.Address = reader.GetString(3);
                        godown.IsDefault = reader.GetBoolean(4);
                        godown.IsActive = reader.GetBoolean(5);
                    }
                }

                conn.Close();

                return godown;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public Godown Post([FromBody] Godown objGodown)
        {
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string connStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();

            string sql = "INSERT INTO [dbo].[godown] ([code],[description],[address],[isdefault],[isactive]) VALUES(" + "'" + objGodown.Code + "','" + objGodown.Description + "','" + objGodown.Address + "','" + Convert.ToString(objGodown.IsDefault) + "','" + Convert.ToString(objGodown.IsActive) + "')";
            command = new SqlCommand(sql, conn);
            adapter.InsertCommand = new SqlCommand(sql, conn);
            adapter.InsertCommand.ExecuteNonQuery();

            sql = "select max(id) from [dbo].[godown]";
            SqlCommand cmd = new SqlCommand(sql, conn);
           

            int id = (int)cmd.ExecuteScalar();

            command.Dispose();

            conn.Close();

            objGodown.Id = id;

            return objGodown;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [System.Web.Http.Route("api/godown/update")]
        public Godown Update([FromBody] Godown objGodown)
        {
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string connStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();
            string sql = "UPDATE [dbo].[godown]" +
                               " SET [code] = '" + objGodown.Code + "'" +
                               ",[description] = '" + objGodown.Description + "'" +
                               ",[address] = '" + objGodown.Address + "'" +
                               ",[isdefault] = '" + objGodown.IsDefault + "'" +
                               ",[isactive] = '" + objGodown.IsActive + "'" +
                               " WHERE id=" + objGodown.Id + "";

            //string sql = "INSERT INTO [dbo].[racks] ([code],[description],[capacity],[isdefault],[isactive]) VALUES(" + "'" + objRack.Code + "','" + objRack.Description + "'," + objRack.Capacity + ",'" + Convert.ToString(objRack.IsDefault) + "','" + Convert.ToString(objRack.IsActive) + "')";
            command = new SqlCommand(sql, conn);
            adapter.UpdateCommand = new SqlCommand(sql, conn);
            adapter.UpdateCommand.ExecuteNonQuery();

            command.Dispose();

            conn.Close();

            return objGodown;
        }

        // DELETE api/values/5
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public Boolean Delete(int id)
        {
            string connStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();

            SqlDataAdapter adapter = new SqlDataAdapter();
            String sql = "";

            sql = "Delete racks where godownid=" + id;


            adapter.DeleteCommand = new SqlCommand(sql, conn);
            adapter.DeleteCommand.ExecuteNonQuery();

            sql = "Delete godown where id=" + id;


            adapter.DeleteCommand = new SqlCommand(sql, conn);
            adapter.DeleteCommand.ExecuteNonQuery();

            conn.Close();

            return true;

        }
    }
}
