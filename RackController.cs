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
    public class RackController : ApiController
    {

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public Rack Get(int id)
        {
            Rack rack = new Rack();

            string connStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            string sql = "SELECT * from racks where id='" + id + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();

            try
            {
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rack.Id = reader.GetInt32(0);
                        rack.Code = reader.GetString(1);
                        rack.Description = reader.GetString(2);
                        rack.Capacity = reader.GetInt32(3);
                        rack.IsDefault = reader.GetBoolean(4);
                        rack.IsActive = reader.GetBoolean(5);
                    }
                }

                conn.Close();

                return rack;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [System.Web.Http.HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [System.Web.Http.Route("api/rack/getgodownracks/{id}")]
        public IEnumerable<Rack> GetRacks(int id)
        {
            List<Rack> racks = new List<Rack>();

            string connStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            string sql = "SELECT * from racks where godownid="+id;
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();

            try
            {
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Rack rack = new Rack();

                        rack.Id = reader.GetInt32(0);
                        rack.Code = reader.GetString(1);
                        rack.Description = reader.GetString(2);
                        rack.Capacity = reader.GetInt32(3);
                        rack.IsDefault = reader.GetBoolean(4);
                        rack.IsActive = reader.GetBoolean(5);

                        racks.Add(rack);
                    }
                }

                conn.Close();

                return racks;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [System.Web.Http.HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [System.Web.Http.Route("api/rack/isdefault")]
        public Boolean IsDefault()
        {
            int count = 0;

            string connStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            string sql = "select count(*) from [dbo].[racks] where isdefault='true'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();

            count = (int)cmd.ExecuteScalar();

            if (count >= 1)
            {
                return true;
            }


            return false;
        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public Rack Post([FromBody] Rack objRack)
        {
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string connStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();

            string sql = "INSERT INTO [dbo].[racks] ([code],[description],[capacity],[isdefault],[isactive],[godownid]) VALUES(" + "'" + objRack.Code + "','" + objRack.Description + "'," + objRack.Capacity + ",'" + Convert.ToString(objRack.IsDefault) + "','" + Convert.ToString(objRack.IsActive) + "',"+ objRack.GoDownId + ")";
            command = new SqlCommand(sql, conn);
            adapter.InsertCommand = new SqlCommand(sql, conn);
            adapter.InsertCommand.ExecuteNonQuery();

            sql = "select max(id) from [dbo].[racks]";
            SqlCommand cmd = new SqlCommand(sql, conn);


            int id = (int)cmd.ExecuteScalar();

            objRack.Id = id;

            command.Dispose();

            conn.Close();

            return objRack;
        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [System.Web.Http.Route("api/rack/update")]
        public Rack Update([FromBody] Rack objRack)
        {
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string connStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();
            string sql = "UPDATE [dbo].[racks]" +
                               " SET [code] = '" + objRack.Code + "'" +
                               ",[description] = '" + objRack.Description + "'" +
                               ",[capacity] = '" + objRack.Capacity + "'" +
                               ",[isdefault] = '" + objRack.IsDefault + "'" +
                               ",[isactive] = '" + objRack.IsActive + "'" +
                               " WHERE id=" + objRack.Id + "";

            //string sql = "INSERT INTO [dbo].[racks] ([code],[description],[capacity],[isdefault],[isactive]) VALUES(" + "'" + objRack.Code + "','" + objRack.Description + "'," + objRack.Capacity + ",'" + Convert.ToString(objRack.IsDefault) + "','" + Convert.ToString(objRack.IsActive) + "')";
            command = new SqlCommand(sql, conn);
            adapter.UpdateCommand = new SqlCommand(sql, conn);
            adapter.UpdateCommand.ExecuteNonQuery();

            command.Dispose();

            conn.Close();

            return objRack;
        }

        // DELETE api/values/5
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public Boolean Delete(int id)
        {
            string connStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();

            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            String sql = "";

            sql = "Delete racks where id=" + id;

            command = new SqlCommand(sql, conn);

            adapter.DeleteCommand = new SqlCommand(sql, conn);
            adapter.DeleteCommand.ExecuteNonQuery();

            command.Dispose();
            conn.Close();

            return true;

        }

    }

}