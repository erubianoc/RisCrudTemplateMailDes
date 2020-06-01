using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Text;
using TemplateMailRis.models;

namespace TemplateMailRis.controllers
{
    public class cnxCreateTempalte
    {
        private const string cadenaConectDb = "sqldb_connection_dbemail";
        public string StringConnectionDbris { get; set; }
        private mdlTemplateemail registro;
        private string idNewtemplate { get; set; }
        public  cnxCreateTempalte (mdlTemplateemail registro)
        {
            StringConnectionDbris = Environment.GetEnvironmentVariable(cadenaConectDb);
            this.registro = registro;
            this.getMaxID();
        }
        public void getMaxID()
        {
            using (var connection = new SqlConnection(StringConnectionDbris))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT MAX (id_template) + 1 AS idtemp from TBL_TEMPLATE_MAIL";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = query;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                this.idNewtemplate = (string)reader["idtemp"].ToString();
                            }
                        }
                    }
                }
                catch (Exception a)
                {

                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public HttpResponseMessage insertTemplate()
        {
            HttpResponseMessage respuesta = new HttpResponseMessage(HttpStatusCode.BadGateway);
            string query = " insert into TBL_TEMPLATE_MAIL (id_template ,id_client ,id_product ,id_service,subject," +
                            "template_value,dinamic,mask, [from] ,active ,activefx,default_id_provider ," +
                            "insertos_activos";
            for (int i = 0; i < this.registro.inserts.Count; i++)
            {
                query += ",inserto_" + (i + 1);
            }
            query += ") Values (@id_template ,@id_client ,@id_product ,@id_service,@subject," +
                            "@template_value,@dinamic,@mask, @from ,@active ,@activefx,@default_id_provider ," +
                            "@insertos_activos";
            for (int i = 0; i < this.registro.inserts.Count; i++)
            {
                query += ",@inserto_" + (i + 1);
            }
            query += ")";

            using (var connection = new SqlConnection(StringConnectionDbris))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = query;
                        cmd.Parameters.Add("@id_template", SqlDbType.NVarChar);
                        cmd.Parameters["@id_template"].Value = this.idNewtemplate;
                        cmd.Parameters.Add("@id_client", SqlDbType.NVarChar);
                        cmd.Parameters["@id_client"].Value = this.registro.client_code;
                        cmd.Parameters.Add("@id_product", SqlDbType.NVarChar);
                        cmd.Parameters["@id_product"].Value = this.registro.product_code;
                        cmd.Parameters.Add("@id_service", SqlDbType.NVarChar);
                        cmd.Parameters["@id_service"].Value = this.registro.service_code;
                        cmd.Parameters.Add("@subject", SqlDbType.NVarChar);
                        cmd.Parameters["@subject"].Value = this.registro.mail_subject;
                        cmd.Parameters.Add("@template_value", SqlDbType.NVarChar);
                        cmd.Parameters["@template_value"].Value = this.registro.template_content;
                        cmd.Parameters.Add("@dinamic", SqlDbType.NVarChar);
                        cmd.Parameters["@dinamic"].Value = this.registro.dinamic;
                        cmd.Parameters.Add("@mask", SqlDbType.NVarChar);
                        cmd.Parameters["@mask"].Value = this.registro.mail_mask;
                        cmd.Parameters.Add("@from", SqlDbType.NVarChar);
                        cmd.Parameters["@from"].Value = this.registro.mail_from;
                        cmd.Parameters.Add("@active", SqlDbType.NVarChar);
                        cmd.Parameters["@active"].Value = this.registro.active;
                        cmd.Parameters.Add("@activefx", SqlDbType.NVarChar);
                        cmd.Parameters["@activefx"].Value = this.registro.active_fx;
                        cmd.Parameters.Add("@default_id_provider", SqlDbType.NVarChar);
                        cmd.Parameters["@default_id_provider"].Value = this.registro.default_code_provider;
                        cmd.Parameters.Add("@insertos_activos", SqlDbType.NVarChar);
                        cmd.Parameters["@insertos_activos"].Value = this.registro.inserts_active;

                        for (int i = 0; i < this.registro.inserts.Count; i++)
                        {
                            cmd.Parameters.Add("@inserto_" + (i + 1), SqlDbType.NVarChar);
                            if (this.registro.inserts[i].path_insert.Length > 0)
                            {
                                cmd.Parameters["@inserto_" + (i + 1)].Value = this.registro.inserts[i].path_insert;
                            }
                            else
                            {
                                cmd.Parameters["@inserto_" + (i + 1)].Value = " ";
                            }
                        }
                        int result = cmd.ExecuteNonQuery();
                        if (result == 0)
                        {
                            Console.WriteLine("error no inser nada");
                        }
                        else
                        {
                            respuesta = new HttpResponseMessage(HttpStatusCode.Created)
                            {
                                Content = new StringContent("{" +
                                "\"message\" : \"TemplateMail Creado Exitosamente\"  }"
                                , Encoding.UTF8, "json/application")
                            };
                        }
                    }
                }
                catch (Exception e)
                {
                    respuesta = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent("{\"message\" : \"el servidor no puede preocesar la solicitud\" }", Encoding.UTF8, "json/application")
                    };
                    Console.WriteLine("algo paso", e.Message.ToString());
                }
            }
            return respuesta;
        }
    }
}
