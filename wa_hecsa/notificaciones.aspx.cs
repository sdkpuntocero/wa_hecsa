using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace wa_hecsa
{
    public partial class notificaciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                }
                else
                {
                    Page.Validate();

                    if (Page.IsValid == true)
                    {
                        guarda_registro();
                    }
                }
            }
            catch
            {
                Response.Redirect("acceso.aspx");
            }
        }

        private void guarda_registro()
        {
            Guid guid_env = Guid.NewGuid();

            string i_email = Request.Form["i_email"];
            string i_email_usr = Request.Form["i_email_usr"];
            string i_email_clave = Request.Form["i_email_clave"];
            string i_asunto = Request.Form["i_asunto"];
            string i_smtp = Request.Form["i_smtp"];
            int i_puerto = int.Parse(Request.Form["i_puerto"]);

            TextInfo t_asunto = new CultureInfo("es-MX", false).TextInfo;

            var i_registro = new db_hecsaEntities();

            var d_emp = new inf_email_env
            {
                email_env_ID = guid_env,
                email_env_est = 1,
                email = i_email,
                usuario = i_email_usr,
                clave = i_email_clave,
                asunto = t_asunto.ToTitleCase(i_asunto),
                servidor_smtp = i_smtp,
                puerto = i_puerto,
                registro = DateTime.Now
            };

            i_registro.inf_email_env.Add(d_emp);

            i_registro.SaveChanges();

            Mensaje("Datos guardados con éxito");
        }

        private string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, @"[^0-9A-Za-z]", string.Empty, RegexOptions.None);
        }

        public static string RemoveAccentsWithRegEx(string inputString)
        {
            Regex replace_a_Accents = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex replace_e_Accents = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex replace_i_Accents = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex replace_o_Accents = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex replace_u_Accents = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            inputString = replace_a_Accents.Replace(inputString, "a");
            inputString = replace_e_Accents.Replace(inputString, "e");
            inputString = replace_i_Accents.Replace(inputString, "i");
            inputString = replace_o_Accents.Replace(inputString, "o");
            inputString = replace_u_Accents.Replace(inputString, "u");
            return inputString;
        }

        private void Mensaje(string contenido)
        {
            lblModalTitle.Text = "HECSA";
            lblModalBody.Text = contenido;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
            upModal.Update();
        }
    }
}