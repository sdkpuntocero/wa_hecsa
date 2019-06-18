using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace wa_hecsa
{
    public partial class acceso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                inf_demp();
            }
            else
            {
            }
        }

        private void inf_demp()
        {
            using (var m_emp = new db_hecsaEntities())
            {
                var i_env = (from i_e in m_emp.inf_email_env
                             select i_e).ToList();

                if (i_env.Count == 0)
                {
                    Server.Transfer("notificaciones.aspx");
                }
                else
                {
                    var i_emp = (from i_e in m_emp.inf_emp
                                 select i_e).ToList();

                    if (i_emp.Count == 0)
                    {
                        lkb_registro_inicial.Visible = true;
                        Mensaje("Sin registro de empresa, favor de registrar una.");
                    }
                }
            }
        }

        protected void btn_acceso_Click(object sender, EventArgs e)
        {
            string user, pass, pass_qa;
            int cod_perf;
            Guid usrf_ID;

            user = i_usuario.Value;
            pass = encrypta.Encrypt(i_clave.Value);

            try

            {
                using (var m_usr = new db_hecsaEntities())
                {
                    var i_usr = (from i_u in m_usr.inf_usuario
                                 join i_uh in m_usr.inf_usr_rh on i_u.usuario_ID equals i_uh.usuario_ID
                                 where i_u.usr == user
                                 select new
                                 {
                                     i_u.usuario_ID,
                                     i_u.clave,
                                     i_uh.perfil_ID
                                 }).ToList();

                    cod_perf = i_usr[0].perfil_ID;
                    usrf_ID = i_usr[0].usuario_ID;
                    pass_qa = i_usr[0].clave;

                    var d_corp = (from i_corp in m_usr.inf_centro
                                  select i_corp).ToList();

                    if (d_corp.Count == 0)
                    {
                        Session["ss_idusr"] = usrf_ID;

                        Response.Redirect("pnl_corporativo.aspx");
                    }
                    else
                    {
                        if (i_usr.Count == 0)
                        {
                            Mensaje("Usuario no existe, favor de re-intentar");
                        }
                        else
                        {
                            if (pass == pass_qa)
                            {
                                Session["ss_idusr"] = usrf_ID;
                                switch (cod_perf)
                                {
                                    case 2:

                                        Response.Redirect("pnl_control.aspx");
                                        break;

                                    case 3:

                                        Response.Redirect("pnl_control.aspx");
                                        break;

                                    case 4:

                                        Response.Redirect("pnl_control.aspx");

                                        break;

                                    case 5:
                                        Mensaje("Sin Acceso, favor de contactar al Corporativo");
                                        break;

                                    case 6:
                                        Mensaje("Sin Acceso, favor de contactar al Corporativo");
                                        break;

                                    case 7:

                                        Mensaje("Sin Acceso, favor de contactar al Corporativo");
                                        break;

                                    case 8:

                                        Response.Redirect("pnl_control.aspx");
                                        break;

                                    case 9:

                                        Mensaje("Sin Acceso, favor de contactar al Corporativo");
                                        break;

                                    case 10:

                                        Mensaje("Sin Acceso, favor de contactar al Corporativo");
                                        break;

                                    default:

                                        break;
                                }
                            }
                            else
                            {
                                Mensaje("Contraseña incorrecta, favor de re-intentar");
                            }
                        }
                    }
                }
            }
            catch
            {
                Mensaje("Datos incorrectos, favor de re-intentar");
            }
        }

        private void Mensaje(string contenido)
        {
            lblModalTitle.Text = "HECSA";
            lblModalBody.Text = contenido;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
            upModal.Update();
        }

        protected void lkb_registro_inicial_Click(object sender, EventArgs e)
        {
            Response.Redirect("registro_inicial.aspx");
        }
    }
}