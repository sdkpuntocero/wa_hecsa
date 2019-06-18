using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace wa_hecsa
{
    public partial class registro_inicial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    carga_select();
                }
             
            }
            catch
            {
                Response.Redirect("acceso.aspx");
            }
        }

        private void carga_select()
        {
        
            scnt_colonia.Items.Clear();
            scnt_colonia.Items.Insert(0, new ListItem("*Colonia", string.Empty));

        }

        protected void btn_i_cnt_cp_Click(object sender, EventArgs e)
        {
            string str_cp = i_cnt_cp.Value;

            using (db_hecsaEntities db_sepomex = new db_hecsaEntities())
            {
                var tbl_sepomex = (from c in db_sepomex.inf_sepomex
                                   where c.d_codigo == str_cp
                                   select c).ToList();

                scnt_colonia.DataSource = tbl_sepomex;
                scnt_colonia.DataTextField = "d_asenta";
                scnt_colonia.DataValueField = "id_asenta_cpcons";
                scnt_colonia.DataBind();

                if (tbl_sepomex.Count == 1)
                {
                    i_cnt_municipio.Value = tbl_sepomex[0].d_mnpio;
                    i_cnt_estado.Value = tbl_sepomex[0].d_estado;
                    scnt_colonia.Attributes.Add("required", "required");
                    i_cnt_callenum.Attributes.Add("required", "required");
                }
                if (tbl_sepomex.Count > 1)
                {
                    scnt_colonia.Items.Insert(0, new ListItem("*Colonia", string.Empty));

                    i_cnt_municipio.Value = tbl_sepomex[0].d_mnpio;
                    i_cnt_estado.Value = tbl_sepomex[0].d_estado;
                    scnt_colonia.Attributes.Add("required", "required");
                    i_cnt_callenum.Attributes.Add("required", "required");
                }
                else if (tbl_sepomex.Count == 0)
                {
                    scnt_colonia.Items.Clear();
                    scnt_colonia.Items.Insert(0, new ListItem("*Colonia", "0"));
                    i_cnt_municipio.Value = null;
                    i_cnt_estado.Value = null;
                    scnt_colonia.Attributes.Add("required", string.Empty);
                    i_cnt_callenum.Attributes.Add("required", string.Empty);
                }
            }

            scnt_colonia.Focus();
        }

        protected void btn_guarda_Click(object sender, EventArgs e)
        {
            guarda_registro();
        }

        private void guarda_registro()
        {
            Guid guid_emp = Guid.NewGuid();
            Guid guid_usr = Guid.NewGuid();
            Guid guid_cnt = Guid.NewGuid();

            string i_nombres = null, i_aparterno = null, i_amaterno = null, cod_usr = null, str_clave = null, cod_usrf = null;
            string i_nombres_o = Request.Form["i_nombres"];
            string i_aparterno_o = Request.Form["i_apaterno"];
            string i_amaterno_o = Request.Form["i_amaterno"];
            string i_emp_nom = Request.Form["i_emp_nom"];
            string i_cnt_nom = Request.Form["i_cnt_nom"];
            string i_email = Request.Form["i_email"];
            string i_tel = Request.Form["i_tel"];
            string i_callenum = Request.Form["i_cnt_callenum"];
            string i_cp = Request.Form["i_cnt_cp"];
            string i_colonia = Request.Form["scnt_colonia"];
            int i_ap = int.Parse("2");
    
            TextInfo t_empresan = new CultureInfo("es-MX", false).TextInfo;
            TextInfo t_cntn = new CultureInfo("es-MX", false).TextInfo;
            TextInfo t_nombres = new CultureInfo("es-MX", false).TextInfo;
            TextInfo t_apateno = new CultureInfo("es-MX", false).TextInfo;
            TextInfo t_amateno = new CultureInfo("es-MX", false).TextInfo;
            TextInfo t_callenum = new CultureInfo("es-MX", false).TextInfo;

            t_nombres.ToTitleCase(i_nombres_o);

            str_clave = encrypta.Encrypt("poc123");

            try
            {
                i_nombres = RemoveSpecialCharacters(RemoveAccentsWithRegEx(i_nombres_o.Trim().ToLower()));
                string[] separados;

                separados = i_nombres_o.Split(" ".ToCharArray());

                i_aparterno = RemoveSpecialCharacters(RemoveAccentsWithRegEx(i_aparterno_o.Trim().ToLower()));
                i_amaterno = RemoveSpecialCharacters(RemoveAccentsWithRegEx(i_amaterno_o.Trim().ToLower()));

                cod_usr = left_right_mid.left(i_nombres, 1) + i_aparterno;
            }
            catch
            {
                Mensaje("Se requiere mínimo 2 letras por cada campo(nombre,apellido paterno, apellido materno) para generar el usuario.");
            }

            var i_registro = new db_hecsaEntities();

            var d_emp = new inf_emp
            {
                empresa_ID = guid_emp,
                estatus_ID = 1,
                empresa_nom = t_empresan.ToTitleCase(i_emp_nom),
                email = i_email,
                telefono = i_tel,
                registro = DateTime.Now
            };
            using (db_hecsaEntities m_usr = new db_hecsaEntities())
            {
                var i_usr = (from c in m_usr.inf_usuario
                             select c).ToList();

                if (i_usr.Count == 0)
                {
                    cod_usrf = "USR0001";
                }
                else
                {
                    cod_usrf = "USR" + string.Format("{0:0000}", i_usr.Count + 1);
                }
            }

            var d_cnt = new inf_centro
            {
                
                centro_tipo_ID = 1,
                centro_ID = guid_cnt,
                cod_centro = "CNT0001",
                empresa_ID = guid_emp,
                est_cnt_ID = 1,
                centro_nom = t_cntn.ToTitleCase(i_emp_nom),
                email = i_email,
                telefono = i_tel,
                callenum = t_callenum.ToTitleCase(i_callenum),
                d_codigo = i_cp,
                id_asenta_cpcons = i_colonia,
                registro = DateTime.Now
            };

            var d_cnt_ctrl = new inf_centro_ctrl
            {
                corporativo_ctrl_ID = Guid.NewGuid(),
                centro_ID = guid_cnt,
                usuario_ID = guid_usr
            };

            var d_usr = new inf_usuario
            {
                centro_ID = guid_cnt,
                usr = cod_usr,
                clave = str_clave,
                correo_corp = cod_usr + "@HECSA.com.mx",
                usuario_ID = guid_usr,
                est_usr_ID = 1,
                cod_usr = cod_usrf,
                registro = DateTime.Now
            };

            var d_usr_p = new inf_usr_personales
            {
                usr_personales_ID = Guid.NewGuid(),
                nombres = t_nombres.ToTitleCase(i_nombres_o),
                apaterno = t_apateno.ToTitleCase(i_aparterno_o),
                amaterno = t_amateno.ToTitleCase(i_amaterno_o),
                usuario_ID = guid_usr,
                registro = DateTime.Now
            };

            var d_usr_rh = new inf_usr_rh
            {
                usr_rh_ID = Guid.NewGuid(),
                fecha_ingreso = DateTime.Now,
                area_ID = i_ap,
                perfil_ID = i_ap,
                usuario_ID = guid_usr,
                registro = DateTime.Now
            };

            var d_usr_cont = new inf_usr_contacto
            {
                usr_contacto_ID = Guid.NewGuid(),
                usuario_ID = guid_usr,
                correo = i_email,
                registro = DateTime.Now
            };

            i_registro.inf_emp.Add(d_emp);
            i_registro.inf_centro.Add(d_cnt);
            i_registro.inf_centro_ctrl.Add(d_cnt_ctrl);
            i_registro.inf_usuario.Add(d_usr);
            i_registro.inf_usr_personales.Add(d_usr_p);
            i_registro.inf_usr_rh.Add(d_usr_rh);
            i_registro.inf_usr_contacto.Add(d_usr_cont);
            i_registro.SaveChanges();

            using (var m_env = new db_hecsaEntities())
            {
                var i_ev = (from c in m_env.inf_email_env

                            select c).FirstOrDefault();

                string f_clave = i_ev.clave.ToString();
                string detalle_e = "Registro de Compañía y Director, satisfactorio.";
                string nomcompleto = t_empresan.ToTitleCase(i_nombres_o) + " " + t_apateno.ToTitleCase(i_aparterno_o) + " " + t_apateno.ToTitleCase(i_amaterno_o);
                enviarcorreo(i_ev.email, i_ev.usuario, i_ev.clave, i_ev.asunto, detalle_e, i_ev.servidor_smtp, int.Parse(i_ev.puerto.ToString()), DateTime.Now, i_email, nomcompleto, cod_usr, "poc123");

                Mensaje("Datos guardados con éxito, favor de revisar su correo donde se le enviaran las credenciales de acceso, revisar su bandeja de spam");
            }
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

        private void enviarcorreo(string correo_e, string usuario_e, string clave_e, string asunto_e, string detalle_e, string smtp_e, int puerto_e, DateTime registro_e, string correo_r, string nombrecompleto_reg, string usuario_r, string clave_reg)
        {
            string cuerpo_e = createEmailBody(detalle_e, nombrecompleto_reg, usuario_r, clave_reg, registro_e);

            SendHtmlFormattedEmail(correo_e, asunto_e, cuerpo_e, correo_r, smtp_e, puerto_e, usuario_e, clave_e);
        }

        public string createEmailBody(string detalle_e, string nombrecompleto_reg, string usuario_r, string clave_reg, DateTime registro_e)

        {
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlTemplate.html")))

            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{detalle_envio}", detalle_e);

            body = body.Replace("{nombrecompleto_reg}", nombrecompleto_reg);

            body = body.Replace("{usuario_reg}", usuario_r);

            body = body.Replace("{clave_reg}", clave_reg);

            body = body.Replace("{registro}", registro_e.ToShortDateString());

            return body;
        }

        private void SendHtmlFormattedEmail(string correo_e, string asunto_e, string cuerpo_e, string correo_r, string smtp_e, int puerto_e, string usuario_e, string clave_e)

        {
            using (MailMessage mailMessage = new MailMessage())

            {
                mailMessage.From = new MailAddress(correo_e);

                mailMessage.Subject = asunto_e;

                mailMessage.Body = cuerpo_e;

                mailMessage.IsBodyHtml = true;

                mailMessage.To.Add(new MailAddress(correo_r));

                SmtpClient smtp = new SmtpClient();

                smtp.Host = smtp_e;

                smtp.EnableSsl = true;

                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();

                NetworkCred.UserName = usuario_e;

                NetworkCred.Password = clave_e;
                smtp.UseDefaultCredentials = true;

                smtp.Credentials = NetworkCred;

                smtp.Port = puerto_e;

                try
                {
                    smtp.Send(mailMessage);
                }
                catch
                {
                }
            }
        }
    }
}