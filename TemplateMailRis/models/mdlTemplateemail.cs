using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateMailRis.models
{
    public class mdlTemplateemail
    {
        public string client_code { get; set; }
        public string product_code { get; set; }
        public string service_code { get; set; }
        public string mail_subject { get; set; }
        public string template_content { get; set; }
        public string dinamic { get; set; }
        public string mail_mask { get; set; }
        public string mail_from { get; set; }
        public string active { get; set; }
        public string active_fx { get; set; }
        public string default_code_provider { get; set; }
        public string inserts_active { get; set; }
        public string id_template { get; set; }
        
        public List<mdlInserts> inserts { get; set; }

        public mdlTemplateemail ()
        {
            this.inserts = new List<mdlInserts>();
        }
    }
}
