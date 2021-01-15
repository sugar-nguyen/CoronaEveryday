using CoronaEveryday.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaEveryday.Data
{
    public class AccessDb
    {
        SqlConnection connection = null;

        public AccessDb(string connectionString)
        {
            if (connection == null)
                connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch
            {
                throw new Exception("Không thể kết nối csdl.");
            }
            finally
            {
                connection.Close();
            }
        }

        public class RowEfftedResult
        {
            public int Global { get; set; }
            public int Countries { get; set; }

            public RowEfftedResult()
            {
                this.Global = 0;
                this.Countries = 0;
            }

        }

        public RowEfftedResult InsertData(CoronaRecieve model)
        {

            var global = model.Global;
            var countries = model.Countries;
            DateTime now = DateTime.Now.Date;
            RowEfftedResult rowEfftedResult = new RowEfftedResult();

            try
            {
                connection.Open();
                string sql = "select COUNT(*) from GET_ALL_GLOBAL_BY_DATE(@date)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@date", SqlDbType.DateTime).Value = now;
                    int output = 0;
                    int row = (Int32)command.ExecuteScalar();
                    if (row <= 0)
                    {
                        using (SqlCommand cmd = new SqlCommand("INSERT_TB_GLOBAL", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] sqlParameters = new SqlParameter[]
                            {
                            new SqlParameter("@new_confirm",global.NewConfirmed),
                            new SqlParameter("@total_confirm",global.TotalConfirmed),
                            new SqlParameter("@new_deaths",global.NewDeaths),
                            new SqlParameter("@total_deaths",global.TotalDeaths),
                            new SqlParameter("@new_recovered",global.NewRecovered),
                            new SqlParameter("@total_recovered",global.TotalRecovered),
                            new SqlParameter("@date",now),
                            };

                            cmd.Parameters.AddRange(sqlParameters);
                            cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();

                            output = Convert.ToInt32(cmd.Parameters["@output"].Value);
                            rowEfftedResult.Global = output;
                        }

                        output = 0;
                        foreach (var country in countries)
                        {
                            using (SqlCommand cmd = new SqlCommand("INSERT_TB_COUNTRIES", connection))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                SqlParameter[] sqlParameters = new SqlParameter[]
                                {
                                new SqlParameter("@country_code",country.CountryCode),
                                new SqlParameter("@country",country.Country),
                                new SqlParameter("@new_confirm",country.NewConfirmed),
                                new SqlParameter("@total_confirm",country.TotalConfirmed),
                                new SqlParameter("@new_deaths",country.NewDeaths),
                                new SqlParameter("@total_deaths",country.TotalDeaths),
                                new SqlParameter("@new_recovered",country.NewRecovered),
                                new SqlParameter("@total_recovered",country.TotalRecovered),
                                new SqlParameter("@date",country.Date.Date),
                                };

                                cmd.Parameters.AddRange(sqlParameters);
                                cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;
                                cmd.ExecuteNonQuery();

                                output += Convert.ToInt32(cmd.Parameters["@output"].Value);
                            }
                            rowEfftedResult.Countries = output;
                        }



                    }
                }

                connection.Close();
            }
            catch
            {
                connection.Close();
            }

            return rowEfftedResult;
        }

        public IEnumerable<Global> GetAllGlobal()
        {
            connection.Open();
            List<Global> globals = new List<Global>();
            string sql = "select * from GET_ALL_GLOBAL";
            using (var cmd = new SqlCommand(sql, connection))
            {
                cmd.CommandType = CommandType.Text;

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Global global = new Global()
                    {
                        Date = reader.GetDateTime("Date"),
                        NewConfirmed = reader.GetInt32("NewConfirmed"),
                        TotalConfirmed = reader.GetInt32("TotalConfirmed"),
                        NewRecovered = reader.GetInt32("NewRecovered"),
                        TotalRecovered = reader.GetInt32("TotalRecovered"),
                        NewDeaths = reader.GetInt32("NewDeaths"),
                        TotalDeaths = reader.GetInt32("TotalDeaths")
                    };
                    globals.Add(global);
                }
            }

            connection.Close();
            return globals;
        }
        public IEnumerable<Countries> GetAlLCountries()
        {
            connection.Open();
            List<Countries> countries = new List<Countries>();
            string sql = "select * from GET_ALL_COUNTRIES";
            using (var cmd = new SqlCommand(sql, connection))
            {
                cmd.CommandType = CommandType.Text;
                var reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    Countries country = new Countries()
                    {
                        CountryCode = reader["CountryCode"].ToString(),
                        Country = reader["Country"].ToString(),
                        NewConfirmed = reader.GetInt32("NewConfirmed"),
                        TotalConfirmed = reader.GetInt32("TotalConfirmed"),
                        NewRecovered = reader.GetInt32("NewRecovered"),
                        TotalRecovered = reader.GetInt32("TotalRecovered"),
                        NewDeaths = reader.GetInt32("NewDeaths"),
                        TotalDeaths = reader.GetInt32("TotalDeaths")
                    };

                    countries.Add(country);

                }
            }

            connection.Close();
            return countries;
        }

        public IEnumerable<Countries> GetAllCountriesByDate(DateTime date)
        {
            connection.Open();
            List<Countries> countries = new List<Countries>();
            string sql = "select * from GET_ALL_COUNTRIES_BY_DATE(@date)";
            using (var cmd = new SqlCommand(sql, connection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@date",SqlDbType.Date).Value = date.Date;

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Countries country = new Countries()
                    {
                        CountryCode = reader["CountryCode"].ToString(),
                        Country = reader["Country"].ToString(),
                        NewConfirmed = reader.GetInt32("NewConfirmed"),
                        TotalConfirmed = reader.GetInt32("TotalConfirmed"),
                        NewRecovered = reader.GetInt32("NewRecovered"),
                        TotalRecovered = reader.GetInt32("TotalRecovered"),
                        NewDeaths = reader.GetInt32("NewDeaths"),
                        TotalDeaths = reader.GetInt32("TotalDeaths")
                    };

                    countries.Add(country);

                }
            }

            connection.Close();
            return countries;
        }
    }
}
