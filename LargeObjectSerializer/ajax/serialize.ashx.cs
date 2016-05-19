
using System.Web;


namespace LargeObjectSerializer.ajax
{


    /// <summary>
    /// Zusammenfassungsbeschreibung für serialize
    /// </summary>
    public class serialize : IHttpHandler
    {


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            System.Data.SqlClient.SqlConnectionStringBuilder csb = new System.Data.SqlClient.SqlConnectionStringBuilder();

            csb.DataSource = @"VMSTZHDB\HBD_DBH";
            csb.InitialCatalog = "HBD_CAFM3_Produktiv";
            csb.IntegratedSecurity = true;



            Newtonsoft.Json.JsonSerializer ser = new Newtonsoft.Json.JsonSerializer();

            using (Newtonsoft.Json.JsonTextWriter jsonWriter = new Newtonsoft.Json.JsonTextWriter(context.Response.Output))
            {
                jsonWriter.Formatting = Newtonsoft.Json.Formatting.Indented;

                jsonWriter.WriteStartObject();


                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(csb.ConnectionString))
                {
                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();

                    using (System.Data.SqlClient.SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "SELECT TOP 10000 * FROM T_LOG_SAP_Interface";

                        using (System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.SequentialAccess
                             | System.Data.CommandBehavior.CloseConnection
                            ))
                        {


                            jsonWriter.WritePropertyName("Columns");
                            jsonWriter.WriteStartArray();



                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                string colName = dr.GetName(i);
                                System.Type t = dr.GetFieldType(i);

                                jsonWriter.WriteStartObject();

                                jsonWriter.WritePropertyName("ColumnName");
                                jsonWriter.WriteValue(colName);

                                jsonWriter.WritePropertyName("DataType");
                                jsonWriter.WriteValue(GetAssemblyQualifiedNoVersionName(t.AssemblyQualifiedName));

                                // jsonWriter.WritePropertyName("DateTimeMode");
                                // jsonWriter.WriteValue(column.DateTimeMode.ToString());

                                jsonWriter.WriteEndObject();
                            }


                            jsonWriter.WriteEndArray();


                            jsonWriter.WritePropertyName("Rows");
                            jsonWriter.WriteStartArray();


                            if (dr.HasRows)
                            {
                                int fieldCount = dr.FieldCount;

                                while (dr.Read())
                                {
                                    jsonWriter.WriteStartArray();

                                    for (int i = 0; i < fieldCount; ++i)
                                    {
                                        object obj = dr.GetValue(i);
                                        jsonWriter.WriteValue(obj);
                                    } // Next i

                                    jsonWriter.WriteEndArray();

                                    jsonWriter.Flush();
                                    context.Response.Output.Flush();
                                    context.Response.Flush();
                                } // Whend while (dr.Read())

                            } // End if (dr.HasRows)

                            dr.Close();
                            jsonWriter.WriteEndArray();
                        } // End using dr 
                         
                    } // End using cmd 

                    if (con.State != System.Data.ConnectionState.Closed)
                        con.Close();
                } // End using con 


                
                jsonWriter.WriteEndObject();

                jsonWriter.Flush();
                context.Response.Output.Flush();
                context.Response.Flush();
            } // End Using jsonWriter 


        }





        public static string GetAssemblyQualifiedNoVersionName(string input)
        {
            int i = 0;
            bool isNotFirst = false;
            for (; i < input.Length; ++i)
            {
                if (input[i] == ',')
                {
                    if (isNotFirst)
                        break;

                    isNotFirst = true;
                }
            }

            return input.Substring(0, i);
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}