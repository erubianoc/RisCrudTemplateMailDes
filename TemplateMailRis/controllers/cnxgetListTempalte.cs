using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using TemplateMailRis.models;

namespace TemplateMailRis.controllers
{
    public class cnxgetListTempalte
    {
        private const string cadenaConectDb = "sqldb_connection_dbemail";
        public string StringConnectionDbris { get; set; }
        private string client_code { get; set; }
        private string product_code { get; set; }
        public cnxgetListTempalte ()
        {
            StringConnectionDbris = Environment.GetEnvironmentVariable(cadenaConectDb);
        }
        public cnxgetListTempalte(string cliente, string producto)
        {
            StringConnectionDbris = Environment.GetEnvironmentVariable(cadenaConectDb);
            this.client_code = cliente;
            this.product_code = producto;
        }
        public Tuple<HashSet<mdlTemplateemail>, string> GetTemplate()
        {
            HashSet<mdlTemplateemail> registros = new HashSet<mdlTemplateemail>();
            mdlTemplateemail registroTemplate;
            string errMessagesqlContex = "";
            using (var connection = new SqlConnection(StringConnectionDbris))
            {
                try
                {

                    connection.Open();
                    string query = "";
                    if (this.client_code != null)
                    {
                        query = "SELECT id_template ,id_client ,id_product ,id_service,subject," +
                            "template_value,dinamic,mask, [from] ,active ,activefx,default_id_provider ," +
                            "insertos_activos,inserto_1 ,inserto_2 ,inserto_3 ,inserto_4 ,inserto_5 ," +
                            "inserto_6 ,inserto_7 ,inserto_8,inserto_9 ,inserto_10 " +
                            " FROM TBL_TEMPLATE_MAIL where id_client = @cli and id_product = @pr";
                    }
                    else
                    {
                        query = "SELECT id_template ,id_client ,id_product ,id_service,subject," +
                            "template_value,dinamic,mask, [from] ,active ,activefx,default_id_provider ," +
                            "insertos_activos,inserto_1 ,inserto_2 ,inserto_3 ,inserto_4 ,inserto_5 ," +
                            "inserto_6 ,inserto_7 ,inserto_8,inserto_9 ,inserto_10 " +
                            " FROM TBL_TEMPLATE_MAIL";
                    }
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = query;
                        if (this.client_code != null)
                        {
                            cmd.Parameters.Add("@cli", SqlDbType.Int);
                            cmd.Parameters["@cli"].Value = this.client_code;
                            cmd.Parameters.Add("@pr", SqlDbType.Int);
                            cmd.Parameters["@pr"].Value = this.product_code;
                        }
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                registroTemplate = new mdlTemplateemail();
                                registroTemplate.active = (string)reader["active"].ToString();
                                registroTemplate.inserts_active = (string)reader["insertos_activos"].ToString();
                                registroTemplate.mail_from = (string)reader["from"].ToString();
                                registroTemplate.mail_mask = (string)reader["mask"].ToString();
                                registroTemplate.mail_subject = (string)reader["subject"].ToString();
                                registroTemplate.product_code = (string)reader["id_product"].ToString();
                                registroTemplate.service_code = (string)reader["id_service"].ToString();
                                registroTemplate.template_content = (string)reader["template_value"].ToString();
                                registroTemplate.active_fx = (string)reader["activefx"].ToString();
                                registroTemplate.client_code = (string)reader["id_client"].ToString();
                                registroTemplate.default_code_provider = (string)reader["default_id_provider"].ToString();
                                registroTemplate.dinamic = (string)reader["dinamic"].ToString();
                                List<string> insertTMP = new List<string>();
                                insertTMP.Add((string)reader["inserto_1"].ToString());
                                insertTMP.Add((string)reader["inserto_2"].ToString());
                                insertTMP.Add((string)reader["inserto_3"].ToString());
                                insertTMP.Add((string)reader["inserto_4"].ToString());
                                insertTMP.Add((string)reader["inserto_5"].ToString());
                                insertTMP.Add((string)reader["inserto_6"].ToString());
                                insertTMP.Add((string)reader["inserto_7"].ToString());
                                insertTMP.Add((string)reader["inserto_8"].ToString());
                                insertTMP.Add((string)reader["inserto_9"].ToString());
                                insertTMP.Add((string)reader["inserto_10"].ToString());
                                for (int i = 0; i < insertTMP.Count; i++)
                                {
                                    if (insertTMP[i].Length > 1)
                                    {
                                        mdlInserts insertTemp = new mdlInserts();
                                        insertTemp.path_insert = insertTMP[i];
                                        registroTemplate.inserts.Add(insertTemp);
                                    }
                                }
                                registros.Add(registroTemplate);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    errMessagesqlContex = e.Message;
                }
            }
            return new Tuple<HashSet<mdlTemplateemail>, string>(registros, errMessagesqlContex);
        }
    }
}
