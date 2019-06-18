
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace wa_hecsa
{
    public partial class pnl_control : System.Web.UI.Page
    {
        public static Guid empf_ID = Guid.Empty, usr_ID = Guid.Empty, usrf_ID = Guid.Empty, cnt_f = Guid.Empty, inv_f = Guid.Empty, comp_f = Guid.Empty, prov_f = Guid.Empty, clte_f = Guid.Empty, vnta_f = Guid.Empty;
        public static DataTable tbl_vnta_inv;
        public static int int_idperf = 0, est_usr = 0, est_cnt = 0, est_inv = 0, est_comp = 0, est_prov = 0, est_clte = 0, est_vnta = 0;
        public static string str_pnlID = string.Empty, nombre_rpt = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    inf_usr_oper();
                    try
                    {
                        //inf_totales();
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
                Response.Redirect("acceso.aspx");
            }

        }

        //private void inf_totales()
        //{
        //    using (db_hecsaEntities m_usuario = new db_hecsaEntities())
        //    {
        //        var i_usuario = (from i_u in m_usuario.fn_totales_gerentesc(empf_ID)
        //                         select i_u).ToList();

        //        string t_tipo_clte = i_usuario[0].Tipo;
        //        string t_cout_clte = i_usuario[0].d_count.ToString();
        //        string t_sum_clte = i_usuario[0].dd_sum.ToString();
        //        string t_tipo_vnta = i_usuario[2].Tipo;
        //        string t_cout_vnta = i_usuario[2].d_count.ToString();
        //        string t_sum_vnta = i_usuario[2].dd_sum.ToString();
        //        string t_tipo_comp = i_usuario[1].Tipo;
        //        string t_cout_comp = i_usuario[1].d_count.ToString();
        //        string t_sum_comp = i_usuario[1].dd_sum.ToString();

        //        LinkButton1.Text = t_cout_clte;
        //        LinkButton2.Text = t_cout_vnta + ": " + string.Format("{0:C2}", decimal.Parse(t_sum_vnta));
        //        LinkButton3.Text = t_cout_comp + ": " + string.Format("{0:C2}", decimal.Parse(t_sum_comp));
        //        LinkButton4.Text = string.Format("{0:C2}", decimal.Parse(t_sum_vnta) - decimal.Parse(t_sum_comp));
        //    }
        //}

        private void inf_usr_oper()
        {
            //usr_ID = Guid.Parse(Request.Cookies[1].Value);
            usr_ID = (Guid)(Session["ss_idusr"]);

            using (db_hecsaEntities m_usuario = new db_hecsaEntities())
            {
                var i_usuario = (from i_u in m_usuario.inf_usuario
                                 join i_up in m_usuario.inf_usr_personales on i_u.usuario_ID equals i_up.usuario_ID
                                 join i_uh in m_usuario.inf_usr_rh on i_u.usuario_ID equals i_uh.usuario_ID
                                 join i_pu in m_usuario.fact_perfil on i_uh.perfil_ID equals i_pu.perfil_ID
                                 join i_e in m_usuario.inf_centro on i_u.centro_ID equals i_e.centro_ID
                                 where i_u.usuario_ID == usr_ID
                                 select new
                                 {
                                     i_u.usuario_ID,
                                     i_u.cod_usr,
                                     i_up.nombres,
                                     i_up.apaterno,
                                     i_up.amaterno,
                                     i_pu.perfil_desc,
                                     i_pu.perfil_ID,
                                     i_e.centro_ID,
                                     i_e.centro_nom
                                 }).FirstOrDefault();

                lkb_usr_f.Text = i_usuario.nombres + " " + i_usuario.apaterno + " " + i_usuario.amaterno;
                lkb_usr_fp.Text = i_usuario.perfil_desc;
                int_idperf = i_usuario.perfil_ID;
                lkb_usr_fc.Text = i_usuario.centro_nom;
                Session["ss_idcnt"] = Guid.Parse(i_usuario.centro_ID.ToString());
                empf_ID = (Guid)(Session["ss_idcnt"]);
                switch (int_idperf)
                {
                    case 1:

                        break;

                    case 2:

                        break;

                    case 3:

                        break;

                    case 4:

                       
                        li_config.Visible = false;
                        li_inv.Visible = false;

                        break;

                    case 5:
                        li_comp.Visible = false;
                        li_prov.Visible = false;
                        li_usrs.Visible = false;
                       
                        li_config.Visible = false;
                        li_inv.Visible = false;

                        break;

                    case 6:
                        li_comp.Visible = false;
                        li_prov.Visible = false;
                        li_usrs.Visible = false;
                       
                        li_config.Visible = false;
                        li_inv.Visible = false;

                        break;

                    case 8:

                       
                        li_config.Visible = false;
                        li_inv.Visible = false;

                        break;

                    default:

                        break;
                }
            }
        }

        #region empresa_filtro

        protected void btn_iemp_Click(object sender, EventArgs e)
        {
            string i_emp_nom = Request.Form["iemp_nom"];
            string i_email = Request.Form["iemp_email"];
            string i_tel = Request.Form["iemp_tel"];
            string i_callenum = Request.Form["iemp_callenum"];
            string i_cp = Request.Form["iemp_cp"];
            string i_colonia = Request.Form["iemp_colonia"];

            using (var m_emp = new db_hecsaEntities())
            {
                var d_emp = (from c in m_emp.inf_centro
                             where c.centro_ID == empf_ID
                             select c).FirstOrDefault();

                d_emp.centro_nom = i_emp_nom;
                d_emp.email = i_email;
                d_emp.telefono = i_tel;
                d_emp.callenum = i_callenum;
                d_emp.d_codigo = i_cp;
                d_emp.id_asenta_cpcons = i_colonia;

                m_emp.SaveChanges();
            }

            Mensaje("Datos actualizados con éxito");
        }

        protected void btn_iemp_cp_Click(object sender, EventArgs e)
        {
            string str_cp = iemp_cp.Value;

            using (db_hecsaEntities db_sepomex = new db_hecsaEntities())
            {
                var tbl_sepomex = (from c in db_sepomex.inf_sepomex
                                   where c.d_codigo == str_cp
                                   select c).ToList();

                iemp_colonia.DataSource = tbl_sepomex;
                iemp_colonia.DataTextField = "d_asenta";
                iemp_colonia.DataValueField = "id_asenta_cpcons";
                iemp_colonia.DataBind();

                if (tbl_sepomex.Count == 1)
                {
                    iemp_municipio.Value = tbl_sepomex[0].d_mnpio;
                    iemp_estado.Value = tbl_sepomex[0].d_estado;
                    iemp_colonia.Attributes.Add("required", "required");
                    iemp_callenum.Attributes.Add("required", "required");
                }
                if (tbl_sepomex.Count > 1)
                {
                    iemp_colonia.Items.Insert(0, new ListItem("Colonia", string.Empty));

                    iemp_municipio.Value = tbl_sepomex[0].d_mnpio;
                    iemp_estado.Value = tbl_sepomex[0].d_estado;
                    iemp_colonia.Attributes.Add("required", "required");
                    iemp_callenum.Attributes.Add("required", "required");
                }
                else if (tbl_sepomex.Count == 0)
                {
                    iemp_colonia.Items.Clear();
                    iemp_colonia.Items.Insert(0, new ListItem("Colonia", string.Empty));
                    iemp_municipio.Value = string.Empty;
                    iemp_estado.Value = string.Empty;
                    iemp_colonia.Attributes.Add("required", string.Empty);
                    iemp_callenum.Attributes.Add("required", string.Empty);
                }
            }

            iemp_colonia.Focus();
        }

        private void ctrl_demp()
        {
            iemp_nom.Value = string.Empty;
            iemp_tel.Value = string.Empty;
            iemp_email.Value = string.Empty;
            iemp_callenum.Value = string.Empty;
            iemp_cp.Value = string.Empty;
            iemp_colonia.Items.Clear();
            iemp_colonia.Items.Insert(0, new ListItem("Colonia", string.Empty));
            iemp_municipio.Value = string.Empty;
            iemp_estado.Value = string.Empty;
        }

        private void inf_fc()
        {
            using (db_hecsaEntities m_emp = new db_hecsaEntities())
            {
                var i_usuariof = (from i_u in m_emp.inf_usuario
                                  join i_uh in m_emp.inf_usr_rh on i_u.usuario_ID equals i_uh.usuario_ID
                                  where i_uh.usuario_ID == usr_ID
                                  select new
                                  {
                                      i_uh.perfil_ID
                                  }).FirstOrDefault();

                var d_emp = (from i_e in m_emp.inf_centro

                             where i_e.centro_ID == empf_ID
                             select new
                             {
                                 i_e.centro_nom,
                                 i_e.email,
                                 i_e.telefono,
                                 i_e.callenum,
                                 i_e.d_codigo,
                                 i_e.id_asenta_cpcons
                             }).FirstOrDefault();

                iemp_nom.Value = d_emp.centro_nom;
                iemp_tel.Value = d_emp.telefono;
                iemp_email.Value = d_emp.email;
                iemp_callenum.Value = d_emp.callenum;
                iemp_cp.Value = d_emp.d_codigo;
                try
                {
                    string f_cp = d_emp.id_asenta_cpcons.ToString();

                    var tbl_sepomex = (from c in m_emp.inf_sepomex
                                       where c.d_codigo == d_emp.d_codigo
                                       where c.id_asenta_cpcons == f_cp
                                       select c).ToList();

                    iemp_colonia.DataSource = tbl_sepomex;
                    iemp_colonia.DataTextField = "d_asenta";
                    iemp_colonia.DataValueField = "id_asenta_cpcons";
                    iemp_colonia.DataBind();

                    if (tbl_sepomex.Count == 0)
                    {
                        iemp_callenum.Value = string.Empty;
                        iemp_cp.Value = string.Empty;
                        iemp_colonia.Items.Clear();
                        iemp_colonia.Items.Insert(0, new ListItem("Colonia", string.Empty));
                        iemp_municipio.Value = string.Empty;
                        iemp_estado.Value = string.Empty;
                    }
                    else
                    {
                        iemp_colonia.Value = f_cp;
                        iemp_municipio.Value = tbl_sepomex[0].d_mnpio;
                        iemp_estado.Value = tbl_sepomex[0].d_estado;
                    }
                }
                catch
                {
                }
            }
        }

        #endregion empresa_filtro

        #region ctrl_menu_navegación

        protected void lkb_config_Click(object sender, EventArgs e)
        {
            ctrl_config();
            card_usrf.Visible = false;
            card_empf.Visible = false;
            card_resumen.Visible = false;
            card_ventas.Visible = false;
            card_compras.Visible = false;
            card_inventario.Visible = false;
            card_proveedores.Visible = false;
            card_clientes.Visible = false;
      
            card_usr.Visible = false;
            card_configuracion.Visible = true;

            up_usrf.Update();
            up_empf.Update();
            up_resumen.Update();
            up_ventas.Update();
            up_compras.Update();
            up_inventario.Update();
            up_proveedores.Update();
            up_clientes.Update();
      
            up_usr.Update();
            up_configuracion.Update();
        }



        protected void lkb_ctrl_clte_Click(object sender, EventArgs e)
        {
            str_pnlID = "pnl_clte";

            est_clte = 0;
            ctrl_clte();

            i_clte_buscar.Text = string.Empty;
            gv_clte.Visible = false;
            div_i_clte.Visible = false;

            card_usrf.Visible = false;
            card_empf.Visible = false;
            card_resumen.Visible = false;
            card_ventas.Visible = false;
            card_compras.Visible = false;
            card_inventario.Visible = false;
            card_proveedores.Visible = false;
            card_clientes.Visible = true;
         
            card_usr.Visible = false;
            card_configuracion.Visible = false;

            up_usrf.Update();
            up_empf.Update();
            up_resumen.Update();
            up_ventas.Update();
            up_compras.Update();
            up_inventario.Update();
            up_proveedores.Update();
            up_clientes.Update();
      
            up_usr.Update();
            up_configuracion.Update();
        }

        protected void lkb_ctrl_compras_Click(object sender, EventArgs e)
        {
            str_pnlID = "pnl_comp";

            est_comp = 0;
            ctrl_comp();

            i_comp_buscar.Text = string.Empty;
            gv_comp.Visible = false;
            div_i_comp.Visible = false;

            card_usrf.Visible = false;
            card_empf.Visible = false;
            card_resumen.Visible = false;
            card_ventas.Visible = false;
            card_compras.Visible = true;
            card_inventario.Visible = false;
            card_proveedores.Visible = false;
            card_clientes.Visible = false;
        
            card_usr.Visible = false;
            card_configuracion.Visible = false;

            up_usrf.Update();
            up_empf.Update();
            up_resumen.Update();
            up_ventas.Update();
            up_compras.Update();
            up_inventario.Update();
            up_proveedores.Update();
            up_clientes.Update();
         
            up_usr.Update();
            up_configuracion.Update();
        }

        protected void lkb_ctrl_inv_Click(object sender, EventArgs e)
        {
            str_pnlID = "pnl_inv";

            est_inv = 0;
            ctrl_inv();

            i_inv_buscar.Text = string.Empty;
            gv_inv.Visible = false;
            div_i_inv.Visible = false;

            card_usrf.Visible = false;
            card_empf.Visible = false;
            card_resumen.Visible = false;
            card_ventas.Visible = false;
            card_compras.Visible = false;
            card_inventario.Visible = true;
            card_proveedores.Visible = false;
            card_clientes.Visible = false;
     
            card_usr.Visible = false;
            card_configuracion.Visible = false;

            up_usrf.Update();
            up_empf.Update();
            up_resumen.Update();
            up_ventas.Update();
            up_compras.Update();
            up_inventario.Update();
            up_proveedores.Update();
            up_clientes.Update();
        
            up_usr.Update();
            up_configuracion.Update();
        }

        protected void lkb_ctrl_prov_Click(object sender, EventArgs e)
        {
            str_pnlID = "pnl_prov";

            est_prov = 0;
            ctrl_prov();

            i_prov_buscar.Text = string.Empty;
            gv_prov.Visible = false;
            div_i_prov.Visible = false;

            card_usrf.Visible = false;
            card_empf.Visible = false;
            card_resumen.Visible = false;
            card_ventas.Visible = false;
            card_compras.Visible = false;
            card_inventario.Visible = false;
            card_proveedores.Visible = true;
            card_clientes.Visible = false;
    
            card_usr.Visible = false;
            card_configuracion.Visible = false;

            up_usrf.Update();
            up_empf.Update();
            up_resumen.Update();
            up_ventas.Update();
            up_compras.Update();
            up_inventario.Update();
            up_proveedores.Update();
            up_clientes.Update();
          
            up_usr.Update();
            up_configuracion.Update();
        }

        protected void lkb_ctrl_usrs_Click(object sender, EventArgs e)
        {
            str_pnlID = "pnl_usr";

            est_usr = 0;
            ctrl_usr();

            i_usuario_buscar.Text = string.Empty;
            gv_usrs.Visible = false;
            div_i_usr.Visible = false;

            card_usrf.Visible = false;
            card_empf.Visible = false;
            card_resumen.Visible = false;
            card_ventas.Visible = false;
            card_compras.Visible = false;
            card_inventario.Visible = false;
            card_proveedores.Visible = false;
            card_clientes.Visible = false;
    
            card_usr.Visible = true;
            card_configuracion.Visible = false;

            up_usrf.Update();
            up_empf.Update();
            up_resumen.Update();
            up_ventas.Update();
            up_compras.Update();
            up_inventario.Update();
            up_proveedores.Update();
            up_clientes.Update();

            up_usr.Update();
            up_configuracion.Update();
        }

        protected void lkb_ctrl_vnta_Click(object sender, EventArgs e)
        {
            str_pnlID = "pnl_vnta";

            est_vnta = 0;
            ctrl_vnta();

            i_vnta_buscar.Text = string.Empty;
            gv_vntaff.Visible = false;
            div_i_vnta.Visible = false;

            card_usrf.Visible = false;
            card_empf.Visible = false;
            card_resumen.Visible = false;
            card_ventas.Visible = true;
            card_compras.Visible = false;
            card_inventario.Visible = false;
            card_proveedores.Visible = false;
            card_clientes.Visible = false;
        
            card_usr.Visible = false;
            card_configuracion.Visible = false;

            up_usrf.Update();
            up_empf.Update();
            up_resumen.Update();
            up_ventas.Update();
            up_compras.Update();
            up_inventario.Update();
            up_proveedores.Update();
            up_clientes.Update();

            up_usr.Update();
            up_configuracion.Update();
        }

        protected void lkb_resumen_Click(object sender, EventArgs e)
        {
            card_usrf.Visible = false;
            card_empf.Visible = false;
            card_resumen.Visible = true;
            card_ventas.Visible = false;
            card_compras.Visible = false;
            card_inventario.Visible = false;
            card_proveedores.Visible = false;
            card_clientes.Visible = false;
           
            card_usr.Visible = false;
            card_configuracion.Visible = false;

            up_usrf.Update();
            up_empf.Update();
            up_resumen.Update();
            up_ventas.Update();
            up_compras.Update();
            up_inventario.Update();
            up_proveedores.Update();
            up_clientes.Update();

            up_usr.Update();
            up_configuracion.Update();
            //inf_totales();
        }

        protected void lkb_salir_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("acceso.aspx");
        }

        protected void lkb_usr_f_Click(object sender, EventArgs e)
        {
            inf_usrf();

            card_usrf.Visible = true;
            card_empf.Visible = false;
            card_resumen.Visible = false;
            card_ventas.Visible = false;
            card_compras.Visible = false;
            card_inventario.Visible = false;
            card_proveedores.Visible = false;
            card_clientes.Visible = false;
        
            card_usr.Visible = false;
            card_configuracion.Visible = false;

            up_usrf.Update();
            up_empf.Update();
            up_resumen.Update();
            up_ventas.Update();
            up_compras.Update();
            up_inventario.Update();
            up_proveedores.Update();
            up_clientes.Update();
       
            up_usr.Update();
            up_configuracion.Update();
        }

        protected void lkb_usr_fc_Click(object sender, EventArgs e)
        {
            ctrl_demp();
            inf_fc();

            card_usrf.Visible = false;
            card_empf.Visible = true;
            card_resumen.Visible = false;
            card_ventas.Visible = false;
            card_compras.Visible = false;
            card_inventario.Visible = false;
            card_proveedores.Visible = false;
            card_clientes.Visible = false;
    
            card_usr.Visible = false;
            card_configuracion.Visible = false;

            up_usrf.Update();
            up_empf.Update();
            up_resumen.Update();
            up_ventas.Update();
            up_compras.Update();
            up_inventario.Update();
            up_proveedores.Update();
            up_clientes.Update();
      
            up_usr.Update();
            up_configuracion.Update();
        }

        #endregion ctrl_menu_navegación

        #region usuario_filtrado

        protected void btn_usrf_Click(object sender, EventArgs e)
        {
            string i_nombres = string.Empty, i_aparterno = string.Empty, i_amaterno = string.Empty, cod_usr = string.Empty, str_clave = string.Empty;
            string i_nombres_o = Request.Form["i_nombresf"];
            string i_aparterno_o = Request.Form["i_apaternof"];
            string i_amaterno_o = Request.Form["i_amaternof"];
            str_clave = encrypta.Encrypt(Request.Form["i_clavef"]);
            cod_usr = Request.Form["i_f_bf"];

            using (var m_usr = new db_hecsaEntities())
            {
                var d_usrp = (from c in m_usr.inf_usr_personales
                              where c.usuario_ID == usr_ID
                              select c).FirstOrDefault();

                d_usrp.nombres = i_nombres_o;
                d_usrp.apaterno = i_aparterno_o;
                d_usrp.amaterno = i_amaterno_o;

                var d_usr = (from c in m_usr.inf_usuario
                             where c.usuario_ID == usr_ID
                             select c).FirstOrDefault();

                d_usr.usr = cod_usr;
                d_usr.clave = str_clave;

                m_usr.SaveChanges();
            }

            Mensaje("Datos actualizados con éxito");
        }

        private void inf_usrf()
        {
            using (db_hecsaEntities m_usrf = new db_hecsaEntities())
            {
                var d_usrf = (from i_u in m_usrf.inf_usuario
                              join i_up in m_usrf.inf_usr_personales on i_u.usuario_ID equals i_up.usuario_ID
                              where i_u.usuario_ID == usr_ID
                              select new
                              {
                                  i_u.usr,
                                  i_u.correo_corp,
                                  i_up.nombres,
                                  i_up.apaterno,
                                  i_up.amaterno,
                              }).FirstOrDefault();

                i_nombresf.Value = d_usrf.nombres;
                i_apaternof.Value = d_usrf.apaterno;
                i_amaternof.Value = d_usrf.amaterno;
                i_usrf.Value = d_usrf.usr;
                i_emalf.Value = d_usrf.correo_corp;
            }
        }

        #endregion usuario_filtrado

        #region funciones

        [WebMethod]
        public static List<string> busca_clte(string prefixText, int count)
        {
            List<String> columnData = new List<String>();
            string d_f = prefixText.ToUpper();

            using (db_hecsaEntities m_df = new db_hecsaEntities())
            {
                var i_df = (from i_u in m_df.inf_clte

                            where i_u.nombres.Contains(d_f)

                            select new
                            {
                                nom_comp = i_u.nombres + " " + i_u.apaterno + " " + i_u.amaterno,
                                i_u.cod_clte,
                            }).ToList();

                foreach (var ff_d in i_df)
                {
                    columnData.Add(ff_d.nom_comp + " | " + ff_d.cod_clte);
                }
            }

            return columnData;
        }

        [WebMethod]
        public static List<string> busca_inv(string prefixText, int count)
        {
            List<String> columnData = new List<String>();
            string d_f = prefixText.ToUpper();

            using (db_hecsaEntities m_df = new db_hecsaEntities())
            {
                var i_df = (from i_u in m_df.inf_inv
                            join i_ge in m_df.fact_grado_escolar on i_u.grado_escolar_ID equals i_ge.grado_escolar_ID
                            join i_ne in m_df.fact_nivel_escolar on i_ge.nivel_escolar_ID equals i_ne.nivel_escolar_ID
                            where i_u.categoria.Contains(d_f)

                            select new
                            {
                                i_ne.nivel_escolar_desc,
                                i_ge.grado_escolar_desc,
                                i_u.categoria,
                                i_u.cod_inv,
                            }).ToList();

                foreach (var ff_d in i_df)
                {
                    columnData.Add(ff_d.nivel_escolar_desc + " | " + ff_d.grado_escolar_desc + " | " + ff_d.categoria + " | " + ff_d.cod_inv);
                }
            }

            return columnData;
        }

        [WebMethod]
        public static List<string> busca_pnl(string prefixText, int count)
        {
            List<String> columnData = new List<String>();
            string d_f = prefixText.ToUpper();

            if (str_pnlID == "pnl_usr")
            {
                using (db_hecsaEntities m_df = new db_hecsaEntities())
                {
                    if (int_idperf <= 3)
                    {
                        var i_df = (from i_u in m_df.inf_usuario
                                    join i_up in m_df.inf_usr_personales on i_u.usuario_ID equals i_up.usuario_ID
                                    where i_up.nombres.Contains(d_f)
                                    where i_u.usuario_ID != usr_ID
                                    select new
                                    {
                                        nom_comp = i_up.nombres + " " + i_up.apaterno + " " + i_up.amaterno,
                                        i_u.cod_usr,
                                    }).ToList();

                        foreach (var ff_d in i_df)
                        {
                            columnData.Add(ff_d.nom_comp + " | " + ff_d.cod_usr);
                        }
                    }
                    else
                    {
                        var i_df = (from i_u in m_df.inf_usuario
                                    join i_up in m_df.inf_usr_personales on i_u.usuario_ID equals i_up.usuario_ID
                                    where i_up.nombres.Contains(d_f)
                                    where i_u.usuario_ID != usr_ID
                                    where i_u.centro_ID == empf_ID
                                    select new
                                    {
                                        nom_comp = i_up.nombres + " " + i_up.apaterno + " " + i_up.amaterno,
                                        i_u.cod_usr,
                                    }).ToList();

                        foreach (var ff_d in i_df)
                        {
                            columnData.Add(ff_d.nom_comp + " | " + ff_d.cod_usr);
                        }
                    }
                }
            }
            else if (str_pnlID == "pnl_cnt")
            {
                using (db_hecsaEntities m_df = new db_hecsaEntities())
                {
                    var i_df = (from i_u in m_df.inf_centro
                                where i_u.centro_tipo_ID == 2
                                where i_u.centro_nom.Contains(d_f)

                                select new
                                {
                                    i_u.centro_nom,
                                    i_u.cod_centro,
                                }).ToList();

                    foreach (var ff_d in i_df)
                    {
                        columnData.Add(ff_d.centro_nom + " | " + ff_d.cod_centro);
                    }
                }
            }
            else if (str_pnlID == "pnl_inv")
            {
                using (db_hecsaEntities m_df = new db_hecsaEntities())
                {
                    var i_df = (from i_u in m_df.inf_inv
                                join i_ge in m_df.fact_grado_escolar on i_u.grado_escolar_ID equals i_ge.grado_escolar_ID
                                join i_ne in m_df.fact_nivel_escolar on i_ge.nivel_escolar_ID equals i_ne.nivel_escolar_ID
                                where i_u.categoria.Contains(d_f)

                                select new
                                {
                                    i_ne.nivel_escolar_desc,
                                    i_ge.grado_escolar_desc,
                                    i_u.categoria,
                                    i_u.cod_inv,
                                }).ToList();

                    foreach (var ff_d in i_df)
                    {
                        columnData.Add(ff_d.nivel_escolar_desc + " | " + ff_d.grado_escolar_desc + " | " + ff_d.categoria + " | " + ff_d.cod_inv);
                    }
                }
            }
            else if (str_pnlID == "pnl_comp")
            {
                using (db_hecsaEntities m_df = new db_hecsaEntities())
                {
                    if (int_idperf <= 3)
                    {
                        var i_df = (from i_u in m_df.inf_comp

                                    where i_u.comp_desc.Contains(d_f)

                                    select new
                                    {
                                        i_u.comp_desc,
                                        i_u.cod_comp,
                                    }).ToList();

                        foreach (var ff_d in i_df)
                        {
                            columnData.Add(ff_d.comp_desc + " | " + ff_d.cod_comp);
                        }
                    }
                    else
                    {
                        var i_df = (from i_u in m_df.inf_comp

                                    where i_u.comp_desc.Contains(d_f)
                                    where i_u.centro_ID == empf_ID
                                    select new
                                    {
                                        i_u.comp_desc,
                                        i_u.cod_comp,
                                    }).ToList();

                        foreach (var ff_d in i_df)
                        {
                            columnData.Add(ff_d.comp_desc + " | " + ff_d.cod_comp);
                        }
                    }
                }
            }
            else if (str_pnlID == "pnl_prov")
            {
                using (db_hecsaEntities m_df = new db_hecsaEntities())
                {
                    if (int_idperf <= 3)
                    {
                        var i_df = (from i_u in m_df.inf_proveedor

                                    where i_u.razon_social.Contains(d_f)

                                    select new
                                    {
                                        i_u.razon_social,
                                        i_u.cod_prov,
                                    }).ToList();

                        foreach (var ff_d in i_df)
                        {
                            columnData.Add(ff_d.razon_social + " | " + ff_d.cod_prov);
                        }
                    }
                    else
                    {
                        var i_df = (from i_u in m_df.inf_proveedor

                                    where i_u.razon_social.Contains(d_f)
                                    where i_u.centro_ID == empf_ID
                                    select new
                                    {
                                        i_u.razon_social,
                                        i_u.cod_prov,
                                    }).ToList();

                        foreach (var ff_d in i_df)
                        {
                            columnData.Add(ff_d.razon_social + " | " + ff_d.cod_prov);
                        }
                    }
                }
            }
            else if (str_pnlID == "pnl_clte")
            {
                using (db_hecsaEntities m_df = new db_hecsaEntities())
                {
                    if (int_idperf <= 3)
                    {
                        var i_df = (from i_u in m_df.inf_clte

                                    where i_u.nombres.Contains(d_f)

                                    select new
                                    {
                                        nom_comp = i_u.nombres + " " + i_u.apaterno + " " + i_u.amaterno,
                                        i_u.cod_clte,
                                    }).ToList();

                        foreach (var ff_d in i_df)
                        {
                            columnData.Add(ff_d.nom_comp + " | " + ff_d.cod_clte);
                        }
                    }
                    else
                    {
                        var i_df = (from i_u in m_df.inf_clte

                                    where i_u.nombres.Contains(d_f)
                                    where i_u.centro_ID == empf_ID
                                    select new
                                    {
                                        nom_comp = i_u.nombres + " " + i_u.apaterno + " " + i_u.amaterno,
                                        i_u.cod_clte,
                                    }).ToList();

                        foreach (var ff_d in i_df)
                        {
                            columnData.Add(ff_d.nom_comp + " | " + ff_d.cod_clte);
                        }
                    }
                }
            }
            return columnData;
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
            lblModalTitle.Text = "Intelimundo";
            lblModalBody.Text = contenido;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
            upModal.Update();
        }

        private string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, @"[^0-9A-Za-z]", string.Empty, RegexOptions.None);
        }

        #endregion funciones

        #region ctrl_venta

        protected void btn_vnta_Click(object sender, EventArgs e)
        {
            if (est_vnta == 0)
            {
                Mensaje("Favor de seleccionar una acción");
            }
            else
            {
                string d_vnta_clte = Request.Form["i_vnta_codclte"];

                if (est_vnta == 1)
                {
                    Guid guid_vnta = Guid.NewGuid();
                    Guid guid_vnta_clte;
                    string cod_usrf;

                    using (db_hecsaEntities m_usr = new db_hecsaEntities())
                    {
                        var i_clte = (from i_i in m_usr.inf_clte
                                      where i_i.cod_clte == d_vnta_clte
                                      select i_i).FirstOrDefault();

                        guid_vnta_clte = i_clte.clte_ID;

                        var i_f_b = (from c in m_usr.inf_vnta
                                     select c).ToList();

                        if (i_f_b.Count == 0)
                        {
                            cod_usrf = "VNTA0001";
                        }
                        else
                        {
                            cod_usrf = "VNTA" + string.Format("{0:0000}", i_f_b.Count + 1);
                        }

                        var i_f_bf = (from c in m_usr.inf_vnta

                                      select c).ToList();
                        usr_ID = Guid.Empty;
                        empf_ID = Guid.Empty;
                        usr_ID = (Guid)(Session["ss_idusr"]);
                        empf_ID = (Guid)(Session["ss_idcnt"]);
                        if (i_f_bf.Count == 0)
                        {
                            var d_cnt = new inf_vnta
                            {
                                vnta_ID = guid_vnta,
                                est_vnta_ID = 1,
                                cod_vnta = cod_usrf,
                                vnta_tipo_ID = 1,
                                centro_ID = empf_ID,
                                usuario_ID = usr_ID,
                                clte_ID = guid_vnta_clte,
                                registro = DateTime.Now
                            };

                            m_usr.inf_vnta.Add(d_cnt);

                            for (int i = tbl_vnta_inv.Rows.Count - 1; i >= 0; i--)
                            {
                                DataRow dr = tbl_vnta_inv.Rows[i];
                                Guid inv_dd = Guid.Parse(dr[0].ToString());
                                decimal dd_costo = decimal.Parse(dr[6].ToString().Replace("$", ""));
                                string f_cant = dr[7].ToString().Replace(".00", "");
                                int dd_cantidad = int.Parse(f_cant);
                                int dd_desc = int.Parse(dr[8].ToString().Replace("%", ""));

                                var d_vnta_inv = new inf_vnta_inv
                                {
                                    vnta_inv_ID = Guid.NewGuid(),
                                    inventario_ID = inv_dd,
                                    costo = dd_costo,
                                    cantidad = dd_cantidad,
                                    porcentaje_desc = dd_desc,
                                    vnta_ID = guid_vnta
                                };
                                m_usr.inf_vnta_inv.Add(d_vnta_inv);
                            }
                            m_usr.SaveChanges();

                            est_vnta = 0;
                            Session["vntaf_ID"] = guid_vnta;
                            //defual_rpt(guid_vnta);
                            est_vnta = 0;
                            gv_inv_vnta.Visible = false;
                            gv_inv_vntaf.Visible = false;
                            gv_vntaff.Visible = false;
                            ctrl_vnta();
                            div_i_vnta.Visible = true;
                            div_i_vnta_busr.Visible = true;
                            div_i_vnta_nusr.Visible = true;
                            btn_vnta.Visible = true;
                            tbl_vnta_inv = new DataTable("tbl_vnta_inv");
                            Mensaje("Datos guardados con éxito");
                            Response.Redirect("rpt_remision.aspx");
                            Session["vntaf_ID"] = Guid.Empty;
                        }
                        else
                        {
                            var d_cnt = new inf_vnta
                            {
                                vnta_ID = guid_vnta,
                                est_vnta_ID = 1,
                                cod_vnta = cod_usrf,
                                vnta_tipo_ID = 1,
                                centro_ID = empf_ID,
                                usuario_ID = usr_ID,
                                clte_ID = guid_vnta_clte,
                                registro = DateTime.Now
                            };

                            m_usr.inf_vnta.Add(d_cnt);
                            for (int i = tbl_vnta_inv.Rows.Count - 1; i >= 0; i--)
                            {
                                DataRow dr = tbl_vnta_inv.Rows[i];
                                Guid inv_dd = Guid.Parse(dr[0].ToString());
                                decimal dd_costo = decimal.Parse(dr[6].ToString().Replace("$", ""));
                                string f_cant = dr[7].ToString().Replace(".00", "");
                                int dd_cantidad = int.Parse(f_cant);
                                int dd_desc = int.Parse(dr[8].ToString().Replace("%", ""));

                                var d_vnta_inv = new inf_vnta_inv
                                {
                                    vnta_inv_ID = Guid.NewGuid(),
                                    inventario_ID = inv_dd,
                                    costo = dd_costo,
                                    cantidad = dd_cantidad,
                                    porcentaje_desc = dd_desc,
                                    vnta_ID = guid_vnta
                                };
                                m_usr.inf_vnta_inv.Add(d_vnta_inv);
                            }
                            m_usr.SaveChanges();
                            est_vnta = 0;
                            Session["vntaf_ID"] = guid_vnta;
                            //defual_rpt(guid_vnta);
                            est_vnta = 0;
                            gv_inv_vnta.Visible = false;
                            gv_inv_vntaf.Visible = false;
                            gv_vntaff.Visible = false;
                            ctrl_vnta();
                            div_i_vnta.Visible = true;
                            div_i_vnta_busr.Visible = true;
                            div_i_vnta_nusr.Visible = true;
                            btn_vnta.Visible = true;
                            tbl_vnta_inv = new DataTable("tbl_vnta_inv");
                            Mensaje("Datos guardados con éxito");
                            Response.Redirect("rpt_remision.aspx");
                            Session["vntaf_ID"] = Guid.Empty;
                        }
                    }
                }
                else
                {
                }
            }

            prgLoadingStatus.Visible = false;
        }

        protected void gv_inv_vnta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            div_i_vnta.Visible = true;
            string vnta_cant = string.Empty;
            string vnta_desc = string.Empty;

            if (e.CommandName == "btn_inv_vnta")
            {
                //try
                //{
                GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                TextBox txt_cant = (TextBox)gv_inv_vnta.Rows[0].FindControl("i_vnta_invcant");
                TextBox txt_desc = (TextBox)gv_inv_vnta.Rows[0].FindControl("i_vnta_invdesc");

                vnta_f = Guid.Parse(gvr.Cells[0].Text.ToString().Trim());
                vnta_cant = string.Format("{0:N2}", int.Parse(txt_cant.Text));
                vnta_desc = string.Format("{0:P0}", decimal.Parse(txt_desc.Text) / 100);

                using (db_hecsaEntities m_usrf = new db_hecsaEntities())
                {
                    var i_f_b = (from i_i in m_usrf.inf_inv
                                 join i_ei in m_usrf.fact_est_inv on i_i.est_inv_ID equals i_ei.est_inv_ID
                                 join i_ge in m_usrf.fact_grado_escolar on i_i.grado_escolar_ID equals i_ge.grado_escolar_ID
                                 join i_ne in m_usrf.fact_nivel_escolar on i_ge.nivel_escolar_ID equals i_ne.nivel_escolar_ID
                                 where i_i.inventario_ID == vnta_f
                                 select new
                                 {
                                     i_i.inventario_ID,
                                     i_i.cod_inv,
                                     i_ne.nivel_escolar_desc,
                                     i_ge.grado_escolar_desc,
                                     i_i.categoria,
                                     i_i.caracteristica,
                                     i_i.costo,
                                     i_i.registro,
                                 }).ToList();

                    if (tbl_vnta_inv.Columns.Count == 0)
                    {
                        tbl_vnta_inv.Columns.Add("inventario_ID");
                        tbl_vnta_inv.Columns.Add("cod_inv");
                        tbl_vnta_inv.Columns.Add("nivel_escolar_desc");
                        tbl_vnta_inv.Columns.Add("grado_escolar_desc");
                        tbl_vnta_inv.Columns.Add("categoria");
                        tbl_vnta_inv.Columns.Add("caracteristica");
                        tbl_vnta_inv.Columns.Add("costo");
                        tbl_vnta_inv.Columns.Add("cantidad");
                        tbl_vnta_inv.Columns.Add("desc");
                        tbl_vnta_inv.Columns.Add("registro");
                    }

                    tbl_vnta_inv.Rows.Add(i_f_b[0].inventario_ID, i_f_b[0].cod_inv, i_f_b[0].nivel_escolar_desc, i_f_b[0].grado_escolar_desc, i_f_b[0].categoria, i_f_b[0].caracteristica, string.Format("{0:C2}", i_f_b[0].costo), vnta_cant, vnta_desc, i_f_b[0].registro);

                    gv_inv_vnta.Visible = false;

                    if (i_f_b.Count == 0)
                    {
                        gv_inv_vntaf.DataSource = tbl_vnta_inv;
                        gv_inv_vntaf.DataBind();
                        gv_inv_vntaf.Visible = true;

                        Mensaje("Venta no encontrado.");
                    }
                    else
                    {
                        gv_inv_vntaf.DataSource = tbl_vnta_inv;
                        gv_inv_vntaf.DataBind();
                        gv_inv_vntaf.Visible = true;
                    }
                }
                //}
                //catch
                //{ }
            }
        }

        protected void gv_inv_vntaf_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btn_inv_vnta")
            {
                //try
                //{
                GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);

                Guid inv_ff = Guid.Parse(gvr.Cells[0].Text.ToString().Trim());

                for (int i = tbl_vnta_inv.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = tbl_vnta_inv.Rows[i];
                    Guid inv_dd = Guid.Parse(dr[0].ToString());
                    if (inv_dd == inv_ff)
                        dr.Delete();
                }

                gv_inv_vntaf.DataSource = tbl_vnta_inv;
                gv_inv_vntaf.DataBind();
                gv_inv_vntaf.Visible = true;
            }
        }

        protected void gv_vntaff_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            div_i_vnta.Visible = true;
            string vnta_cant = string.Empty;
            string vnta_desc = string.Empty;
            GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);

            vnta_f = Guid.Parse(gvr.Cells[0].Text.ToString().Trim());
            if (e.CommandName == "btn_inv_vnta")
            {
                //try
                //{
                using (db_hecsaEntities m_usrf = new db_hecsaEntities())
                {
                    var i_f_b = (from i_i in m_usrf.inf_vnta
                                     //join i_ei in m_usrf.fact_est_inv on i_i.est_inv_ID equals i_ei.est_inv_ID
                                     //join i_ge in m_usrf.fact_grado_escolar on i_i.grado_escolar_ID equals i_ge.grado_escolar_ID
                                     //join i_ne in m_usrf.fact_nivel_escolar on i_ge.nivel_escolar_ID equals i_ne.nivel_escolar_ID

                                 where i_i.vnta_ID == vnta_f
                                 select new
                                 {
                                     i_i.vnta_ID,
                                     i_i.cod_vnta,
                                     i_i.est_vnta_ID,
                                     i_i.centro_ID,
                                     i_i.usuario_ID,
                                     i_i.registro,
                                 }).ToList();

                    if (i_f_b.Count == 0)
                    {
                        gv_vntaff.DataSource = i_f_b;
                        gv_vntaff.DataBind();
                        gv_vntaff.Visible = true;

                        Mensaje("Venta no encontrado.");
                    }
                    else
                    {
                        gv_vntaff.DataSource = i_f_b;
                        gv_vntaff.DataBind();
                        gv_vntaff.Visible = true;
                    }
                }
                //}
                //catch
                //{ }
                Session["vntaf_ID"] = vnta_f;
                Response.Redirect("rpt_remision.aspx");
            }
            else if (e.CommandName == "btn_up_vnta")
            {
                int int_ddl = 0;

                DropDownList dl = (DropDownList)gvr.FindControl("ddl_vnta_estatus");

                int_ddl = int.Parse(dl.SelectedValue);

                using (var m_inv = new db_hecsaEntities())
                {
                    var i_inv = (from c in m_inv.inf_vnta
                                 where c.vnta_ID == vnta_f
                                 select c).FirstOrDefault();

                    i_inv.est_vnta_ID = int_ddl;

                    m_inv.SaveChanges();
                }

                ctrl_prov();
                div_i_prov.Visible = false;
                Mensaje("Datos actualizados con éxito");
            }
        }

        protected void gv_vntaff_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Guid f_ID = Guid.Parse(e.Row.Cells[0].Text);
                int est_id;

                using (db_hecsaEntities m_est = new db_hecsaEntities())
                {
                    var i_est = (from md_usr in m_est.inf_vnta
                                 where md_usr.vnta_ID == f_ID
                                 select new
                                 {
                                     md_usr.est_vnta_ID,
                                 }).FirstOrDefault();

                    est_id = int.Parse(i_est.est_vnta_ID.ToString());

                    DropDownList ddl_est = (e.Row.FindControl("ddl_vnta_estatus") as DropDownList);

                    var tbl_sepomex = (from c in m_est.fact_est_vnta
                                       select c).ToList();

                    ddl_est.DataSource = tbl_sepomex;

                    ddl_est.DataTextField = "est_vnta_desc";
                    ddl_est.DataValueField = "est_vnta_ID";
                    ddl_est.DataBind();
                    ddl_est.Items.Insert(0, new ListItem("Seleccionar", "0"));
                    ddl_est.SelectedValue = est_id.ToString();
                }
            }
        }

        protected void lkb_vnta_agregar_Click(object sender, EventArgs e)
        {
            Session["vntaf_ID"] = Guid.Empty;
            est_vnta = 1;
            gv_inv_vnta.Visible = false;
            gv_inv_vntaf.Visible = false;
            gv_vntaff.Visible = false;
            ctrl_vnta();
            div_i_vnta.Visible = true;
            div_i_vnta_busr.Visible = true;
            div_i_vnta_nusr.Visible = true;
            btn_vnta.Visible = true;
            tbl_vnta_inv = new DataTable("tbl_vnta_inv");
        }

        protected void lkb_vnta_buscar_Click(object sender, EventArgs e)
        {
            gv_inv_vnta.Visible = false;
            gv_inv_vntaf.Visible = false;
            div_i_vnta.Visible = true;
            div_i_vnta_binv.Visible = false;
            div_i_vnta_busr.Visible = false;
            div_i_vnta_nusr.Visible = false;
            btn_vnta.Visible = false;
            string f_busqueda = string.Empty;

            if (string.IsNullOrEmpty(i_vnta_buscar.Text))
            {
            }
            else
            {
                f_busqueda = Request.Form["i_vnta_buscar"].ToString().ToUpper().Trim();
                if (f_busqueda == "TODO")
                {
                    f_busqueda = Request.Form["i_vnta_buscar"].ToString().ToUpper().Trim();

                    using (db_hecsaEntities m_usrf = new db_hecsaEntities())
                    {
                        if (int_idperf <= 3)
                        {
                            var i_f_b = (from i_i in m_usrf.inf_vnta
                                             //join i_ei in m_usrf.fact_est_inv on i_i.est_inv_ID equals i_ei.est_inv_ID
                                             //join i_ge in m_usrf.fact_grado_escolar on i_i.grado_escolar_ID equals i_ge.grado_escolar_ID
                                             //join i_ne in m_usrf.fact_nivel_escolar on i_ge.nivel_escolar_ID equals i_ne.nivel_escolar_ID

                                         select new
                                         {
                                             i_i.vnta_ID,
                                             i_i.cod_vnta,
                                             i_i.est_vnta_ID,
                                             i_i.centro_ID,
                                             i_i.usuario_ID,
                                             i_i.registro,
                                         }).ToList();

                            if (i_f_b.Count == 0)
                            {
                                gv_vntaff.DataSource = i_f_b;
                                gv_vntaff.DataBind();
                                gv_vntaff.Visible = true;

                                Mensaje("Venta no encontrado.");
                            }
                            else
                            {
                                gv_vntaff.DataSource = i_f_b;
                                gv_vntaff.DataBind();
                                gv_vntaff.Visible = true;
                            }
                        }
                        else
                        {
                            var i_f_b = (from i_i in m_usrf.inf_vnta
                                             //join i_ei in m_usrf.fact_est_inv on i_i.est_inv_ID equals i_ei.est_inv_ID
                                             //join i_ge in m_usrf.fact_grado_escolar on i_i.grado_escolar_ID equals i_ge.grado_escolar_ID
                                             //join i_ne in m_usrf.fact_nivel_escolar on i_ge.nivel_escolar_ID equals i_ne.nivel_escolar_ID
                                         where i_i.centro_ID == empf_ID
                                         select new
                                         {
                                             i_i.vnta_ID,
                                             i_i.cod_vnta,
                                             i_i.est_vnta_ID,
                                             i_i.centro_ID,
                                             i_i.usuario_ID,
                                             i_i.registro,
                                         }).ToList();

                            if (i_f_b.Count == 0)
                            {
                                gv_vntaff.DataSource = i_f_b;
                                gv_vntaff.DataBind();
                                gv_vntaff.Visible = true;

                                Mensaje("Venta no encontrado.");
                            }
                            else
                            {
                                gv_vntaff.DataSource = i_f_b;
                                gv_vntaff.DataBind();
                                gv_vntaff.Visible = true;
                            }
                        }
                    }
                }
            }
        }

        protected void lkb_vnta_clte_Click(object sender, EventArgs e)
        {
        }

        protected void lkb_vnta_cltef_Click(object sender, EventArgs e)
        {
            string f_busqueda = string.Empty;
            f_busqueda = Request.Form["i_vnta_clte"].ToString().ToUpper().Trim();
            string n_fv;
            try
            {
                Char char_s = '|';
                string d_rub = f_busqueda;
                String[] de_rub = d_rub.Trim().Split(char_s);

                n_fv = de_rub[1].Trim();

                using (db_hecsaEntities m_clte = new db_hecsaEntities())
                {
                    var i_clte = (from i_i in m_clte.inf_clte
                                  join i_ei in m_clte.fact_est_clte on i_i.est_clte_ID equals i_ei.est_clte_ID
                                  where i_i.cod_clte == n_fv
                                  select new
                                  {
                                      i_i.clte_ID,
                                      i_i.cod_clte,
                                      i_i.nombres,
                                      i_i.apaterno,
                                      i_i.amaterno,
                                      i_i.registro,
                                  }).ToList();

                    if (i_clte.Count == 0)
                    {
                        i_vnta_codclte.Value = string.Empty;
                        i_vnta_nombresc.Value = string.Empty;
                        i_vnta_apaternoc.Value = string.Empty;
                        i_vnta_amaternoc.Value = string.Empty;
                    }
                    else
                    {
                        i_vnta_codclte.Value = i_clte[0].cod_clte;
                        i_vnta_nombresc.Value = i_clte[0].nombres;
                        i_vnta_apaternoc.Value = i_clte[0].apaterno;
                        i_vnta_amaternoc.Value = i_clte[0].amaterno;
                    }
                }
            }
            catch
            {
            }
        }

        protected void lkb_vnta_invf_Click(object sender, EventArgs e)
        {
            string f_busqueda = string.Empty;

            try
            {
                f_busqueda = Request.Form["i_vnta_inv"].ToString().ToUpper().Trim();
                string n_fv;
                Char char_s = '|';
                string d_rub = f_busqueda;
                String[] de_rub = d_rub.Trim().Split(char_s);

                n_fv = de_rub[3].Trim();

                using (db_hecsaEntities m_usrf = new db_hecsaEntities())
                {
                    var i_f_b = (from i_i in m_usrf.inf_inv
                                 join i_ei in m_usrf.fact_est_inv on i_i.est_inv_ID equals i_ei.est_inv_ID
                                 join i_ge in m_usrf.fact_grado_escolar on i_i.grado_escolar_ID equals i_ge.grado_escolar_ID
                                 join i_ne in m_usrf.fact_nivel_escolar on i_ge.nivel_escolar_ID equals i_ne.nivel_escolar_ID
                                 where i_i.cod_inv == n_fv
                                 select new
                                 {
                                     i_i.inventario_ID,
                                     i_i.cod_inv,
                                     i_ei.est_inv_desc,
                                     i_ge.grado_escolar_desc,
                                     i_ne.nivel_escolar_desc,
                                     i_i.categoria,
                                     i_i.caracteristica,
                                     i_i.costo,
                                     i_i.registro,
                                 }).ToList();

                    if (i_f_b.Count == 0)
                    {
                        gv_inv_vnta.DataSource = i_f_b;
                        gv_inv_vnta.DataBind();
                        gv_inv_vnta.Visible = true;
                        gv_inv_vnta.Visible = true;

                        Mensaje("Venta no encontrado.");
                    }
                    else
                    {
                        gv_inv_vnta.DataSource = i_f_b;
                        gv_inv_vnta.DataBind();
                        gv_inv_vnta.Visible = true;
                        gv_inv_vnta.Visible = true;
                    }
                }
            }
            catch
            {
                ctrl_inv();
                div_i_inv.Visible = false;
                Mensaje("Venta no encontrado.");
            }
        }

        private void ctrl_vnta()
        {
            i_vnta_buscar.Text = string.Empty;
            i_vnta_inv.Text = string.Empty;
            i_vnta_clte.Text = string.Empty;
            i_vnta_codclte.Value = string.Empty;
            i_vnta_nombresc.Value = string.Empty;
            i_vnta_apaternoc.Value = string.Empty;
            i_vnta_amaternoc.Value = string.Empty;
        }

        #endregion ctrl_venta

        #region ctrl_compra

        protected void btn_comp_guardar_Click(object sender, EventArgs e)
        {
            if (est_comp == 0)
            {
                Mensaje("Favor de seleccionar una acción");
            }
            else
            {
                string d_comp_cant = Request.Form["i_comp_cant"];
                string d_comp_costo = Request.Form["i_comp_costo"];
                string d_comp_cat = Request.Form["i_comp_cat"];
                string d_comp_desc = Request.Form["i_comp_desc"];

                TextInfo t_comp_cat = new CultureInfo("es-MX", false).TextInfo;
                TextInfo t_comp_desc = new CultureInfo("es-MX", false).TextInfo;
                TextInfo t_comp_carat = new CultureInfo("es-MX", false).TextInfo;

                string dd_comp_cat = t_comp_cat.ToTitleCase(d_comp_cat.ToLower());
                string dd_comp_desc = t_comp_desc.ToTitleCase(d_comp_desc.ToLower());

                if (est_comp == 1)
                {
                    Mensaje("Favor de seleccionar una Acción");

                    guarda_compras(d_comp_cant, d_comp_costo, dd_comp_cat, dd_comp_desc);
                }
                else
                {
                    edita_compras(comp_f, d_comp_cant, d_comp_costo, dd_comp_cat, dd_comp_desc);
                }
            }
        }

        protected void gv_comp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }

        protected void gv_comp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            est_comp = 2;
            div_i_comp.Visible = true;
            if (e.CommandName == "btn_comp")
            {
                try
                {
                    GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);

                    comp_f = Guid.Parse(gvr.Cells[0].Text.ToString().Trim());

                    using (db_hecsaEntities m_comp = new db_hecsaEntities())
                    {
                        var i_comp = (from i_i in m_comp.inf_comp
                                      join i_ei in m_comp.fact_est_comp on i_i.est_comp_ID equals i_ei.est_comp_ID
                                      where i_i.compra_ID == comp_f
                                      select new
                                      {
                                          i_i.cantidad,
                                          i_i.costo,
                                          i_i.comp_desc,
                                          i_i.compra_ID,
                                          i_i.cod_comp,
                                          i_ei.est_comp_desc,
                                          i_i.categoria,
                                          i_i.registro,
                                      }).ToList();

                        if (i_comp.Count == 0)
                        {
                            gv_comp.DataSource = i_comp;
                            gv_comp.DataBind();
                            gv_comp.Visible = true;
                            gv_comp.Visible = true;

                            Mensaje("Inventario no encontrado.");
                        }
                        else
                        {
                            gv_comp.DataSource = i_comp;
                            gv_comp.DataBind();
                            gv_comp.Visible = true;
                            gv_comp.Visible = true;
                        }

                        i_comp_cant.Value = i_comp[0].cantidad.ToString();
                        i_comp_costo.Value = Math.Round(Convert.ToDecimal(i_comp[0].costo.ToString()), 0).ToString();
                        i_comp_cat.Value = i_comp[0].categoria.ToString();
                        i_comp_desc.Value = i_comp[0].comp_desc.ToString();
                    }
                }
                catch
                { }
            }
        }

        protected void gv_comp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Guid f_ID = Guid.Parse(e.Row.Cells[0].Text);
                int est_id;

                using (db_hecsaEntities m_est = new db_hecsaEntities())
                {
                    var i_est = (from md_usr in m_est.inf_comp
                                 where md_usr.compra_ID == f_ID
                                 select new
                                 {
                                     md_usr.est_comp_ID,
                                 }).FirstOrDefault();

                    est_id = int.Parse(i_est.est_comp_ID.ToString());

                    DropDownList ddl_est = (e.Row.FindControl("ddl_comp_estatus") as DropDownList);

                    var tbl_sepomex = (from c in m_est.fact_est_comp
                                       select c).ToList();

                    ddl_est.DataSource = tbl_sepomex;

                    ddl_est.DataTextField = "est_comp_desc";
                    ddl_est.DataValueField = "est_comp_ID";
                    ddl_est.DataBind();
                    ddl_est.Items.Insert(0, new ListItem("Seleccionar", "0"));
                    ddl_est.SelectedValue = est_id.ToString();
                }
            }
        }

        protected void lkb_comp_agregar_Click(object sender, EventArgs e)
        {
            ctrl_comp();
            est_comp = 1;
            gv_comp.Visible = false;
            div_i_comp.Visible = true;
            i_comp_buscar.Text = string.Empty;
        }

        protected void lkb_comp_buscar_Click(object sender, EventArgs e)
        {
            div_i_comp.Visible = false;
            string f_busqueda = string.Empty;

            if (string.IsNullOrEmpty(i_comp_buscar.Text))
            {
            }
            else
            {
                f_busqueda = Request.Form["i_comp_buscar"].ToString().ToUpper().Trim();
                if (f_busqueda == "TODO")
                {
                    f_busqueda = Request.Form["i_comp_buscar"].ToString().ToUpper().Trim();

                    using (db_hecsaEntities m_comp = new db_hecsaEntities())
                    {
                        if (int_idperf <= 3)
                        {
                            var i_comp = (from i_i in m_comp.inf_comp
                                          join i_ei in m_comp.fact_est_comp on i_i.est_comp_ID equals i_ei.est_comp_ID
                                          select new
                                          {
                                              i_i.compra_ID,
                                              i_i.cod_comp,
                                              i_ei.est_comp_desc,
                                              i_i.categoria,
                                              i_i.comp_desc,
                                              i_i.registro,
                                          }).ToList();

                            if (i_comp.Count == 0)
                            {
                                gv_comp.DataSource = i_comp;
                                gv_comp.DataBind();
                                gv_comp.Visible = true;
                                gv_comp.Visible = true;

                                Mensaje("Compra no encontrada.");
                            }
                            else
                            {
                                gv_comp.DataSource = i_comp;
                                gv_comp.DataBind();
                                gv_comp.Visible = true;
                                gv_comp.Visible = true;
                            }
                        }
                        else
                        {
                            var i_comp = (from i_i in m_comp.inf_comp
                                          join i_ei in m_comp.fact_est_comp on i_i.est_comp_ID equals i_ei.est_comp_ID
                                          where i_i.centro_ID == empf_ID
                                          select new
                                          {
                                              i_i.compra_ID,
                                              i_i.cod_comp,
                                              i_ei.est_comp_desc,
                                              i_i.categoria,
                                              i_i.comp_desc,
                                              i_i.registro,
                                          }).ToList();

                            if (i_comp.Count == 0)
                            {
                                gv_comp.DataSource = i_comp;
                                gv_comp.DataBind();
                                gv_comp.Visible = true;
                                gv_comp.Visible = true;

                                Mensaje("Compra no encontrada.");
                            }
                            else
                            {
                                gv_comp.DataSource = i_comp;
                                gv_comp.DataBind();
                                gv_comp.Visible = true;
                                gv_comp.Visible = true;
                            }
                        }
                    }
                }
                else
                {
                    try
                    {
                        f_busqueda = Request.Form["i_comp_buscar"].ToString().ToUpper().Trim();
                        string n_fv;

                        Char char_s = '|';
                        string d_rub = f_busqueda;
                        String[] de_rub = d_rub.Trim().Split(char_s);

                        n_fv = de_rub[1].Trim();

                        using (db_hecsaEntities m_comp = new db_hecsaEntities())
                        {
                            if (int_idperf <= 3)
                            {
                                var i_comp = (from i_i in m_comp.inf_comp
                                              join i_ei in m_comp.fact_est_comp on i_i.est_comp_ID equals i_ei.est_comp_ID
                                              where i_i.cod_comp == n_fv
                                              select new
                                              {
                                                  i_i.compra_ID,
                                                  i_i.cod_comp,
                                                  i_ei.est_comp_desc,
                                                  i_i.categoria,
                                                  i_i.comp_desc,
                                                  i_i.registro,
                                              }).ToList();

                                if (i_comp.Count == 0)
                                {
                                    gv_comp.DataSource = i_comp;
                                    gv_comp.DataBind();
                                    gv_comp.Visible = true;
                                    gv_comp.Visible = true;

                                    Mensaje("Compra no encontrada.");
                                }
                                else
                                {
                                    gv_comp.DataSource = i_comp;
                                    gv_comp.DataBind();
                                    gv_comp.Visible = true;
                                    gv_comp.Visible = true;
                                }
                            }
                            else
                            {
                                var i_comp = (from i_i in m_comp.inf_comp
                                              join i_ei in m_comp.fact_est_comp on i_i.est_comp_ID equals i_ei.est_comp_ID
                                              where i_i.cod_comp == n_fv
                                              where i_i.centro_ID == empf_ID
                                              select new
                                              {
                                                  i_i.compra_ID,
                                                  i_i.cod_comp,
                                                  i_ei.est_comp_desc,
                                                  i_i.categoria,
                                                  i_i.comp_desc,
                                                  i_i.registro,
                                              }).ToList();

                                if (i_comp.Count == 0)
                                {
                                    gv_comp.DataSource = i_comp;
                                    gv_comp.DataBind();
                                    gv_comp.Visible = true;
                                    gv_comp.Visible = true;

                                    Mensaje("Compra no encontrada.");
                                }
                                else
                                {
                                    gv_comp.DataSource = i_comp;
                                    gv_comp.DataBind();
                                    gv_comp.Visible = true;
                                    gv_comp.Visible = true;
                                }
                            }
                        }
                    }
                    catch
                    {
                        ctrl_comp();
                        div_i_comp.Visible = false;
                        Mensaje("Compra no encontrada.");
                    }
                }
            }
        }

        private void ctrl_comp()
        {
            i_comp_cant.Value = string.Empty;
            i_comp_costo.Value = string.Empty;
            i_comp_cat.Value = string.Empty;
            i_comp_desc.Value = string.Empty;
        }

        private void edita_compras(Guid comp_f, string d_comp_cant, string d_comp_costo, string d_comp_cat, string d_comp_desc)
        {
            int int_ddl = 0;
            decimal dd_comp_costo = Decimal.Parse(d_comp_costo);
            int dd_comp_cant = int.Parse(d_comp_cant);

            foreach (GridViewRow row in gv_comp.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList dl = (DropDownList)row.FindControl("ddl_comp_estatus");

                    int_ddl = int.Parse(dl.SelectedValue);
                }
            }

            using (var m_inv = new db_hecsaEntities())
            {
                var i_inv = (from c in m_inv.inf_comp
                             where c.compra_ID == comp_f
                             select c).FirstOrDefault();

                i_inv.est_comp_ID = int_ddl;
                i_inv.cantidad = dd_comp_cant;
                i_inv.categoria = d_comp_cat;
                i_inv.comp_desc = d_comp_desc;

                i_inv.costo = dd_comp_costo;

                m_inv.SaveChanges();
            }
            ctrl_comp();
            div_i_comp.Visible = false;
            Mensaje("Datos actualizados con éxito");
        }

        private void guarda_compras(string d_comp_cant, string d_comp_costo, string d_comp_cat, string d_comp_desc)
        {
            Guid guid_inv = Guid.NewGuid();
            string cod_usrf;

            using (db_hecsaEntities m_usr = new db_hecsaEntities())
            {
                var i_f_b = (from c in m_usr.inf_comp
                             select c).ToList();

                if (i_f_b.Count == 0)
                {
                    cod_usrf = "COMP0001";
                }
                else
                {
                    cod_usrf = "COMP" + string.Format("{0:0000}", i_f_b.Count + 1);
                }

                var i_f_bf = (from c in m_usr.inf_comp
                              where c.categoria == d_comp_cat
                              where c.comp_desc == d_comp_desc

                              select c).ToList();

                usr_ID = Guid.Empty;
                empf_ID = Guid.Empty;
                usr_ID = (Guid)(Session["ss_idusr"]);
                empf_ID = (Guid)(Session["ss_idcnt"]);

                if (i_f_bf.Count == 0)
                {
                    var d_cnt = new inf_comp
                    {
                        compra_ID = guid_inv,
                        est_comp_ID = 1,
                        cod_comp = cod_usrf,
                        cantidad = int.Parse(d_comp_cant),
                        comp_desc = d_comp_desc,
                        categoria = d_comp_cat,

                        costo = int.Parse(d_comp_costo),
                        centro_ID = empf_ID,
                        registro = DateTime.Now
                    };

                    m_usr.inf_comp.Add(d_cnt);
                    m_usr.SaveChanges();

                    ctrl_comp();
                    div_i_comp.Visible = false;
                    Mensaje("Datos guardados con éxito");
                }
                else
                {
                    var d_cnt = new inf_comp
                    {
                        compra_ID = guid_inv,
                        est_comp_ID = 1,
                        cod_comp = cod_usrf,
                        cantidad = int.Parse(d_comp_cant),
                        comp_desc = d_comp_desc,
                        categoria = d_comp_cat,

                        costo = int.Parse(d_comp_costo),
                        centro_ID = empf_ID,
                        registro = DateTime.Now
                    };

                    m_usr.inf_comp.Add(d_cnt);
                    m_usr.SaveChanges();

                    ctrl_comp();
                    div_i_comp.Visible = false;
                    Mensaje("Datos guardados con éxito");
                }
            }
        }

        #endregion ctrl_compra

        #region ctrl_inventario

        protected void btn_inv_guardar_Click(object sender, EventArgs e)
        {
            if (est_inv == 0)
            {
                Mensaje("Favor de seleccionar una Acción");
            }
            else
            {
                int d_nivesc = int.Parse(Request.Form["i_inv_nivesc"]);
                int d_gradesc = int.Parse(Request.Form["i_inv_gradesc"]);

                string d_inv_costo = Request.Form["i_inv_costo"];
                string d_inv_cat = Request.Form["i_inv_cat"];
                string d_inv_desc = Request.Form["i_inv_desc"];
                string d_inv_carat = Request.Form["i_inv_caract"];

                TextInfo t_inv_cat = new CultureInfo("es-MX", false).TextInfo;
                TextInfo t_inv_desc = new CultureInfo("es-MX", false).TextInfo;
                TextInfo t_inv_carat = new CultureInfo("es-MX", false).TextInfo;

                string dd_inv_cat = t_inv_cat.ToTitleCase(d_inv_cat.ToLower());
                string dd_inv_desc = t_inv_desc.ToTitleCase(d_inv_desc.ToLower());
                string dd_inv_carat = t_inv_carat.ToTitleCase(d_inv_carat.ToLower());

                if (est_inv == 1)
                {
                    Mensaje("Favor de seleccionar una Acción");

                    guarda_inventario(d_nivesc, d_gradesc, d_inv_costo, dd_inv_cat, dd_inv_desc, dd_inv_carat);
                }
                else
                {
                    edita_inventario(inv_f, d_nivesc, d_gradesc, d_inv_costo, dd_inv_cat, dd_inv_desc, dd_inv_carat);
                }
            }
        }

        protected void gv_inv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }

        protected void gv_inv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            est_inv = 2;
            div_i_inv.Visible = true;
            if (e.CommandName == "btn_inv_inv")
            {
                try
                {
                    GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);

                    inv_f = Guid.Parse(gvr.Cells[0].Text.ToString().Trim());

                    using (db_hecsaEntities m_inv = new db_hecsaEntities())
                    {
                        var i_inv = (from i_i in m_inv.inf_inv
                                     join i_ei in m_inv.fact_est_inv on i_i.est_inv_ID equals i_ei.est_inv_ID
                                     join i_ge in m_inv.fact_grado_escolar on i_i.grado_escolar_ID equals i_ge.grado_escolar_ID
                                     join i_ne in m_inv.fact_nivel_escolar on i_ge.nivel_escolar_ID equals i_ne.nivel_escolar_ID
                                     where i_i.inventario_ID == inv_f
                                     select new
                                     {
                                         i_i.inventario_ID,
                                         i_i.cod_inv,
                                         i_ei.est_inv_desc,
                                         i_i.grado_escolar_ID,
                                         i_ge.grado_escolar_desc,
                                         i_ne.nivel_escolar_ID,
                                         i_ne.nivel_escolar_desc,
                                         i_i.categoria,
                                         i_i.caracteristica,
                                         i_i.registro,
                                         i_i.costo,
                                         i_i.inv_desc
                                     }).ToList();

                        if (i_inv.Count == 0)
                        {
                            gv_inv.DataSource = i_inv;
                            gv_inv.DataBind();
                            gv_inv.Visible = true;
                            gv_inv.Visible = true;

                            Mensaje("Inventario no encontrado.");
                        }
                        else
                        {
                            gv_inv.DataSource = i_inv;
                            gv_inv.DataBind();
                            gv_inv.Visible = true;
                            gv_inv.Visible = true;
                        }

                        i_inv_costo.Value = Math.Round(Convert.ToDecimal(i_inv[0].costo.ToString()), 0).ToString();
                        i_inv_cat.Value = i_inv[0].categoria;
                        i_inv_desc.Value = i_inv[0].inv_desc;
                        i_inv_caract.Value = i_inv[0].caracteristica;

                        i_inv_nivesc.Items.Clear();
                        i_inv_gradesc.Items.Clear();

                        using (db_hecsaEntities m_ss = new db_hecsaEntities())
                        {
                            var i_ne = (from c in m_ss.fact_nivel_escolar
                                        select c).OrderBy(x => x.nivel_escolar_ID).ToList();

                            i_inv_nivesc.DataSource = i_ne;
                            i_inv_nivesc.DataTextField = "nivel_escolar_desc";
                            i_inv_nivesc.DataValueField = "nivel_escolar_ID";
                            i_inv_nivesc.DataBind();

                            int f_neID = int.Parse(i_inv[0].nivel_escolar_ID.ToString());

                            i_inv_nivesc.Items.Insert(0, new ListItem("*Nivel", string.Empty));
                            i_inv_nivesc.SelectedValue = f_neID.ToString();

                            var i_ge = (from c in m_ss.fact_grado_escolar
                                        where c.nivel_escolar_ID == f_neID
                                        select c).OrderBy(x => x.nivel_escolar_ID).ToList();

                            i_inv_gradesc.DataSource = i_ge;
                            i_inv_gradesc.DataTextField = "grado_escolar_desc";
                            i_inv_gradesc.DataValueField = "grado_escolar_ID";
                            i_inv_gradesc.DataBind();

                            i_inv_gradesc.Items.Insert(0, new ListItem("*Grado", string.Empty));

                            i_inv_gradesc.Value = i_inv[0].grado_escolar_ID.ToString();
                        }
                    }
                }
                catch
                { }
            }
        }

        protected void gv_inv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Guid f_ID = Guid.Parse(e.Row.Cells[0].Text);
                int est_id;

                using (db_hecsaEntities m_est = new db_hecsaEntities())
                {
                    var i_est = (from md_usr in m_est.inf_inv
                                 where md_usr.inventario_ID == f_ID
                                 select new
                                 {
                                     md_usr.est_inv_ID,
                                 }).FirstOrDefault();

                    est_id = int.Parse(i_est.est_inv_ID.ToString());

                    DropDownList ddl_est = (e.Row.FindControl("ddl_inv_estatus") as DropDownList);

                    var tbl_sepomex = (from c in m_est.fact_est_inv
                                       select c).ToList();

                    ddl_est.DataSource = tbl_sepomex;

                    ddl_est.DataTextField = "est_inv_desc";
                    ddl_est.DataValueField = "est_inv_ID";
                    ddl_est.DataBind();
                    ddl_est.Items.Insert(0, new ListItem("Seleccionar", "0"));
                    ddl_est.SelectedValue = est_id.ToString();
                }
            }
        }

        protected void i_inv_nivesc_SelectedIndexChanged(object sender, EventArgs e)
        {
            int int_nivesc = int.Parse(i_inv_nivesc.SelectedValue);

            i_inv_gradesc.Items.Clear();

            using (db_hecsaEntities m_ss = new db_hecsaEntities())
            {
                var i_ma = (from c in m_ss.fact_grado_escolar
                            where c.nivel_escolar_ID == int_nivesc
                            select c).ToList();

                i_inv_gradesc.DataSource = i_ma;
                i_inv_gradesc.DataTextField = "grado_escolar_desc";
                i_inv_gradesc.DataValueField = "grado_escolar_ID";
                i_inv_gradesc.DataBind();

                i_inv_gradesc.Items.Insert(0, new ListItem("*Grado", string.Empty));
            }
        }

        protected void lkb_inv_agregar_Click(object sender, EventArgs e)
        {
            ctrl_inv();
            est_inv = 1;
            gv_inv.Visible = false;
            div_i_inv.Visible = true;
            i_inv_buscar.Text = string.Empty;
        }

        protected void lkb_inv_buscar_Click(object sender, EventArgs e)
        {
            div_i_inv.Visible = false;
            string f_busqueda = string.Empty;
            int f_usrp = 0;

            if (string.IsNullOrEmpty(i_inv_buscar.Text))
            {
            }
            else
            {
                f_busqueda = Request.Form["i_inv_buscar"].ToString().ToUpper().Trim();
                if (f_busqueda == "TODO")
                {
                    f_busqueda = Request.Form["i_inv_buscar"].ToString().ToUpper().Trim();

                    {
                        if (int_idperf == 3)
                            f_usrp = 2;
                    }

                    using (db_hecsaEntities md_fb = new db_hecsaEntities())
                    {
                        var i_f_b = (from i_i in md_fb.inf_inv
                                     join i_ei in md_fb.fact_est_inv on i_i.est_inv_ID equals i_ei.est_inv_ID
                                     join i_ge in md_fb.fact_grado_escolar on i_i.grado_escolar_ID equals i_ge.grado_escolar_ID
                                     join i_ne in md_fb.fact_nivel_escolar on i_ge.nivel_escolar_ID equals i_ne.nivel_escolar_ID

                                     select new
                                     {
                                         i_i.inventario_ID,
                                         i_i.cod_inv,
                                         i_ei.est_inv_desc,
                                         i_ge.grado_escolar_desc,
                                         i_ne.nivel_escolar_desc,
                                         i_i.categoria,
                                         i_i.caracteristica,
                                         i_i.registro,
                                     }).ToList();

                        if (i_f_b.Count == 0)
                        {
                            gv_inv.DataSource = i_f_b;
                            gv_inv.DataBind();
                            gv_inv.Visible = true;
                            gv_inv.Visible = true;

                            Mensaje("Inventario no encontrado.");
                        }
                        else
                        {
                            gv_inv.DataSource = i_f_b;
                            gv_inv.DataBind();
                            gv_inv.Visible = true;
                            gv_inv.Visible = true;
                        }
                    }
                }
                else
                {
                    try
                    {
                        f_busqueda = Request.Form["i_inv_buscar"].ToString().ToUpper().Trim();
                        string n_fv;
                        Char char_s = '|';
                        string d_rub = f_busqueda;
                        String[] de_rub = d_rub.Trim().Split(char_s);

                        n_fv = de_rub[3].Trim();

                        using (db_hecsaEntities m_usrf = new db_hecsaEntities())
                        {
                            var i_f_b = (from i_i in m_usrf.inf_inv
                                         join i_ei in m_usrf.fact_est_inv on i_i.est_inv_ID equals i_ei.est_inv_ID
                                         join i_ge in m_usrf.fact_grado_escolar on i_i.grado_escolar_ID equals i_ge.grado_escolar_ID
                                         join i_ne in m_usrf.fact_nivel_escolar on i_ge.nivel_escolar_ID equals i_ne.nivel_escolar_ID
                                         where i_i.cod_inv == n_fv
                                         select new
                                         {
                                             i_i.inventario_ID,
                                             i_i.cod_inv,
                                             i_ei.est_inv_desc,
                                             i_ge.grado_escolar_desc,
                                             i_ne.nivel_escolar_desc,
                                             i_i.categoria,
                                             i_i.caracteristica,
                                             i_i.registro,
                                         }).ToList();

                            if (i_f_b.Count == 0)
                            {
                                gv_inv.DataSource = i_f_b;
                                gv_inv.DataBind();
                                gv_inv.Visible = true;
                                gv_inv.Visible = true;

                                Mensaje("Inventario no encontrado.");
                            }
                            else
                            {
                                gv_inv.DataSource = i_f_b;
                                gv_inv.DataBind();
                                gv_inv.Visible = true;
                                gv_inv.Visible = true;
                            }
                        }
                    }
                    catch
                    {
                        ctrl_inv();
                        div_i_inv.Visible = false;
                        Mensaje("Inventario no encontrado.");
                    }
                }
            }
        }

        private void ctrl_inv()
        {
            i_inv_costo.Value = string.Empty;
            i_inv_cat.Value = string.Empty;
            i_inv_desc.Value = string.Empty;
            i_inv_caract.Value = string.Empty;

            i_inv_nivesc.Items.Clear();
            i_inv_gradesc.Items.Clear();

            using (db_hecsaEntities m_ss = new db_hecsaEntities())
            {
                var i_ma = (from c in m_ss.fact_nivel_escolar
                            select c).OrderBy(x => x.nivel_escolar_ID).ToList();

                i_inv_nivesc.DataSource = i_ma;
                i_inv_nivesc.DataTextField = "nivel_escolar_desc";
                i_inv_nivesc.DataValueField = "nivel_escolar_ID";
                i_inv_nivesc.DataBind();

                i_inv_nivesc.Items.Insert(0, new ListItem("*Nivel", string.Empty));

                i_inv_gradesc.Items.Insert(0, new ListItem("*Grado", string.Empty));
            }
        }

        private void edita_inventario(Guid inv_f, int d_nivesc, int d_gradesc, string d_inv_costo, string d_inv_cat, string d_inv_desc, string d_inv_carat)
        {
            int int_ddl = 0;
            decimal dd_inv_costo = Decimal.Parse(d_inv_costo);

            foreach (GridViewRow row in gv_inv.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList dl = (DropDownList)row.FindControl("ddl_inv_estatus");

                    int_ddl = int.Parse(dl.SelectedValue);
                }
            }

            using (var m_inv = new db_hecsaEntities())
            {
                var i_inv = (from c in m_inv.inf_inv
                             where c.inventario_ID == inv_f
                             select c).FirstOrDefault();

                i_inv.est_inv_ID = int_ddl;
                i_inv.grado_escolar_ID = d_gradesc;
                i_inv.categoria = d_inv_cat;
                i_inv.inv_desc = d_inv_desc;
                i_inv.caracteristica = d_inv_carat;
                i_inv.costo = dd_inv_costo;

                m_inv.SaveChanges();
            }

            Mensaje("Datos actualizados con éxito");
        }

        private void guarda_inventario(int d_nivesc, int d_gradesc, string d_inv_costo, string dd_inv_cat, string dd_inv_desc, string dd_inv_carat)
        {
            Guid guid_inv = Guid.NewGuid();
            Guid guid_emp;
            string cod_usrf;

            using (db_hecsaEntities m_usr = new db_hecsaEntities())
            {
                var i_f_b = (from c in m_usr.inf_inv
                             select c).ToList();

                if (i_f_b.Count == 0)
                {
                    cod_usrf = "INV0001";
                }
                else
                {
                    cod_usrf = "INV" + string.Format("{0:0000}", i_f_b.Count + 1);
                }

                var i_emp = (from c in m_usr.inf_emp
                             select c).First();

                guid_emp = i_emp.empresa_ID;

                var i_f_bf = (from c in m_usr.inf_inv
                              where c.grado_escolar_ID == d_gradesc
                              where c.categoria == dd_inv_cat
                              where c.inv_desc == dd_inv_desc
                              where c.caracteristica == dd_inv_carat
                              select c).ToList();

                var i_registro = new db_hecsaEntities();

                usr_ID = Guid.Empty;
                empf_ID = Guid.Empty;
                usr_ID = (Guid)(Session["ss_idusr"]);
                empf_ID = (Guid)(Session["ss_idcnt"]);

                if (i_f_bf.Count == 0)
                {
                    var d_cnt = new inf_inv
                    {
                        inventario_ID = guid_inv,
                        est_inv_ID = 1,
                        cod_inv = cod_usrf,
                        grado_escolar_ID = d_gradesc,
                        categoria = dd_inv_cat,
                        inv_desc = dd_inv_desc,
                        caracteristica = dd_inv_carat,
                        costo = int.Parse(d_inv_costo),
                        centro_ID = empf_ID,
                        empresa_ID = guid_emp,
                        registro = DateTime.Now
                    };

                    i_registro.inf_inv.Add(d_cnt);

                    i_registro.SaveChanges();
                    ctrl_inv();
                    Mensaje("Datos guardados con éxito");
                }
                else
                {
                    var d_cnt = new inf_inv
                    {
                        inventario_ID = guid_inv,
                        est_inv_ID = 1,
                        cod_inv = cod_usrf,
                        grado_escolar_ID = d_gradesc,
                        categoria = dd_inv_cat,
                        inv_desc = dd_inv_desc,
                        caracteristica = dd_inv_carat,
                        costo = int.Parse(d_inv_costo),
                        centro_ID = empf_ID,
                        empresa_ID = guid_emp,
                        registro = DateTime.Now
                    };

                    i_registro.inf_inv.Add(d_cnt);

                    i_registro.SaveChanges();

                    ctrl_inv();
                    Mensaje("Datos guardados con éxito");
                }
            }
        }

        #endregion ctrl_inventario

        #region ctrl_cliente

        protected void btn_clte_Click(object sender, EventArgs e)
        {
            if (est_clte == 0)
            {
                Mensaje("Favor de seleccionar una acción.");
            }
            else
            {
                string i_nombres_c = Request.Form["i_clte_nombresc"];
                string i_aparterno_c = Request.Form["i_clte_apaternoc"];
                string i_amaterno_c = Request.Form["i_clte_amaternoc"];
                string i_emailc = Request.Form["i_clte_emailc"];
                string i_telc = Request.Form["i_clte_telc"];

                int i_tcc = int.Parse(Request.Form["i_clte_tcont"]);

                string i_nombres = Request.Form["i_clte_nombres"];
                string i_aparterno = Request.Form["i_clte_apaterno"];
                string i_amaterno = Request.Form["i_clte_amaterno"];
                string i_email = Request.Form["i_clte_email"];
                string i_tel = Request.Form["i_clte_tel"];
                string i_callenum = Request.Form["i_clte_callenum"];
                string i_cp = Request.Form["i_clte_cp"];
                string i_colonia = Request.Form["i_clte_scolonia"];

                TextInfo t_nombres = new CultureInfo("es-MX", false).TextInfo;
                TextInfo t_apateno = new CultureInfo("es-MX", false).TextInfo;
                TextInfo t_amateno = new CultureInfo("es-MX", false).TextInfo;
                TextInfo t_callenum = new CultureInfo("es-MX", false).TextInfo;

                string dd_nombresc = t_nombres.ToTitleCase(i_nombres_c.ToLower());
                string dd_apaternoc = t_apateno.ToTitleCase(i_aparterno_c.ToLower());
                string dd_amaternoc = t_amateno.ToTitleCase(i_amaterno_c.ToLower());
                string dd_nombres = t_nombres.ToTitleCase(i_nombres.ToLower());
                string dd_apaterno = t_apateno.ToTitleCase(i_aparterno.ToLower());
                string dd_amaterno = t_amateno.ToTitleCase(i_amaterno.ToLower());
                string dd_callenum = t_callenum.ToTitleCase(i_callenum.ToLower());

                string cod_usr = string.Empty, i_nombres_o = string.Empty, i_aparterno_o = string.Empty, i_amaterno_o = string.Empty;

                try
                {
                    i_nombres = RemoveSpecialCharacters(RemoveAccentsWithRegEx(i_nombres.Trim().ToLower()));
                    string[] separados;

                    separados = i_nombres_o.Split(" ".ToCharArray());

                    i_aparterno = RemoveSpecialCharacters(RemoveAccentsWithRegEx(i_aparterno.Trim().ToLower()));
                    i_amaterno = RemoveSpecialCharacters(RemoveAccentsWithRegEx(i_amaterno.Trim().ToLower()));

                    cod_usr = left_right_mid.left(i_nombres, 1) + i_aparterno + left_right_mid.left(i_amaterno, 1);
                }
                catch
                {
                    Mensaje("Se requiere mínimo 2 letras por cada campo(nombre,apellido paterno, apellido materno) para generar el usuario.");
                }

                if (est_clte == 1)
                {
                    guarda_clte(dd_nombresc, dd_apaternoc, dd_amaternoc, i_emailc, i_telc, dd_nombres, dd_apaterno, dd_amaterno, i_email, i_tel, dd_callenum, i_cp, i_colonia, i_tcc, cod_usr);
                }
                else
                {
                    edita_clte(dd_nombres, dd_apaterno, dd_amaterno, i_emailc, i_telc, dd_nombres, dd_apaterno, dd_amaterno, i_email, i_tel, dd_callenum, i_cp, i_colonia, i_tcc);
                }
            }
        }

        protected void btn_i_clte_cp_Click(object sender, EventArgs e)
        {
            string str_cp = i_clte_cp.Value;

            using (db_hecsaEntities db_sepomex = new db_hecsaEntities())
            {
                var tbl_sepomex = (from c in db_sepomex.inf_sepomex
                                   where c.d_codigo == str_cp
                                   select c).ToList();

                i_clte_scolonia.DataSource = tbl_sepomex;
                i_clte_scolonia.DataTextField = "d_asenta";
                i_clte_scolonia.DataValueField = "id_asenta_cpcons";
                i_clte_scolonia.DataBind();

                if (tbl_sepomex.Count == 1)
                {
                    i_clte_municipio.Value = tbl_sepomex[0].d_mnpio;
                    i_clte_estado.Value = tbl_sepomex[0].d_estado;
                    i_clte_scolonia.Attributes.Add("required", "required");
                    i_clte_callenum.Attributes.Add("required", "required");
                }
                if (tbl_sepomex.Count > 1)
                {
                    i_clte_scolonia.Items.Insert(0, new ListItem("Colonia", string.Empty));

                    i_clte_municipio.Value = tbl_sepomex[0].d_mnpio;
                    i_clte_estado.Value = tbl_sepomex[0].d_estado;
                    i_clte_scolonia.Attributes.Add("required", "required");
                    i_clte_callenum.Attributes.Add("required", "required");
                }
                else if (tbl_sepomex.Count == 0)
                {
                    i_clte_scolonia.Items.Clear();
                    i_clte_scolonia.Items.Insert(0, new ListItem("Colonia", "0"));
                    i_clte_municipio.Value = null;
                    i_clte_estado.Value = null;
                    i_clte_callenum.Attributes.Add("required", string.Empty);
                    i_clte_scolonia.Attributes.Add("required", string.Empty);
                }
            }

            i_clte_scolonia.Focus();
        }

        protected void gv_clte_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }

        protected void gv_clte_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            est_clte = 2;
            div_i_clte.Visible = true;
            if (e.CommandName == "btn_clte")
            {
                //try
                //{
                GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);

                clte_f = Guid.Parse(gvr.Cells[0].Text.ToString().Trim());
                using (db_hecsaEntities m_clte = new db_hecsaEntities())
                {
                    var i_clte = (from i_i in m_clte.inf_clte
                                  join i_ei in m_clte.fact_est_clte on i_i.est_clte_ID equals i_ei.est_clte_ID
                                  join i_pe in m_clte.inf_clte_contacto on i_i.clte_ID equals i_pe.clte_ID
                                  where i_i.clte_ID == clte_f
                                  select new
                                  {
                                      i_i.clte_ID,
                                      i_i.cod_clte,
                                      nombre_completo = i_i.nombres + " " + i_i.apaterno + " " + i_i.amaterno,
                                      i_i.nombres,
                                      i_i.apaterno,
                                      i_i.amaterno,
                                      i_i.email,
                                      i_i.telefono,
                                      i_i.callenum,
                                      i_i.d_codigo,
                                      i_i.id_asenta_cpcons,
                                      nombresc = i_pe.nombres,
                                      apaternoc = i_pe.apaterno,
                                      amaternoc = i_pe.amaterno,
                                      emailc = i_pe.email,
                                      i_pe.tel,
                                      i_pe.clte_cont_tipo_ID,
                                      i_i.registro,
                                  }).ToList();

                    if (i_clte.Count == 0)
                    {
                        gv_clte.DataSource = i_clte;
                        gv_clte.DataBind();
                        gv_clte.Visible = true;
                        gv_clte.Visible = true;

                        Mensaje("Inventario no encontrado.");
                    }
                    else
                    {
                        gv_clte.DataSource = i_clte;
                        gv_clte.DataBind();
                        gv_clte.Visible = true;
                        gv_clte.Visible = true;
                    }

                    i_clte_nombresc.Value = i_clte[0].nombresc;
                    i_clte_apaternoc.Value = i_clte[0].apaternoc;
                    i_clte_amaternoc.Value = i_clte[0].amaternoc;
                    i_clte_emailc.Value = i_clte[0].emailc;
                    i_clte_telc.Value = i_clte[0].tel;

                    i_clte_nombres.Value = i_clte[0].nombres;
                    i_clte_apaterno.Value = i_clte[0].apaterno;
                    i_clte_amaterno.Value = i_clte[0].amaterno;
                    i_clte_email.Value = i_clte[0].email;
                    i_clte_tel.Value = i_clte[0].telefono;
                    i_clte_callenum.Value = i_clte[0].callenum;
                    i_clte_cp.Value = i_clte[0].d_codigo;

                    i_clte_tcont.Items.Clear();
                    i_clte_scolonia.Items.Clear();

                    var i_ma = (from c in m_clte.fact_clte_cont_tipo
                                select c).ToList();

                    i_clte_tcont.DataSource = i_ma;
                    i_clte_tcont.DataTextField = "clte_cont_tipo_desc";
                    i_clte_tcont.DataValueField = "clte_cont_tipo_ID";
                    i_clte_tcont.DataBind();

                    i_clte_tcont.Items.Insert(0, new ListItem("*Tipo Padre/Tutor", string.Empty));
                    i_clte_tcont.Value = i_clte[0].clte_cont_tipo_ID.ToString();

                    string d_codigo = i_clte[0].d_codigo;

                    var tbl_sepomex = (from c in m_clte.inf_sepomex
                                       where c.d_codigo == d_codigo
                                       select c).ToList();

                    i_clte_scolonia.DataSource = tbl_sepomex;
                    i_clte_scolonia.DataTextField = "d_asenta";
                    i_clte_scolonia.DataValueField = "id_asenta_cpcons";
                    i_clte_scolonia.DataBind();

                    i_clte_scolonia.Items.Insert(0, new ListItem("Colonia", string.Empty));
                    i_clte_scolonia.Value = i_clte[0].id_asenta_cpcons.ToString();

                    //i_clte_municipio.Value = tbl_sepomex[0].d_mnpio;
                    //i_clte_estado.Value = tbl_sepomex[0].d_estado;
                }
                //}
                //catch
                //{ }
            }
        }

        protected void gv_clte_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Guid f_ID = Guid.Parse(e.Row.Cells[0].Text);
                int est_id;

                using (db_hecsaEntities m_est = new db_hecsaEntities())
                {
                    var i_est = (from md_usr in m_est.inf_clte
                                 where md_usr.clte_ID == f_ID
                                 select new
                                 {
                                     md_usr.est_clte_ID,
                                 }).FirstOrDefault();

                    est_id = int.Parse(i_est.est_clte_ID.ToString());

                    DropDownList ddl_est = (e.Row.FindControl("ddl_clte_estatus") as DropDownList);

                    var tbl_sepomex = (from c in m_est.fact_est_clte
                                       select c).ToList();

                    ddl_est.DataSource = tbl_sepomex;

                    ddl_est.DataTextField = "desc_est_clte";
                    ddl_est.DataValueField = "est_clte_ID";
                    ddl_est.DataBind();
                    ddl_est.Items.Insert(0, new ListItem("Seleccionar", "0"));
                    ddl_est.SelectedValue = est_id.ToString();
                }
            }
        }

        protected void lkb_clte_agregar_Click(object sender, EventArgs e)
        {
            ctrl_clte();
            est_clte = 1;
            gv_clte.Visible = false;
            div_i_clte.Visible = true;
            i_clte_buscar.Text = string.Empty;
        }

        protected void lkb_clte_buscar_Click(object sender, EventArgs e)
        {
            div_i_clte.Visible = false;
            string f_busqueda = string.Empty;

            if (string.IsNullOrEmpty(i_clte_buscar.Text))
            {
            }
            else
            {
                f_busqueda = Request.Form["i_clte_buscar"].ToString().ToUpper().Trim();
                if (f_busqueda == "TODO")
                {
                    f_busqueda = Request.Form["i_clte_buscar"].ToString().ToUpper().Trim();

                    using (db_hecsaEntities m_clte = new db_hecsaEntities())
                    {
                        if (int_idperf <= 3)
                        {
                            var i_clte = (from i_i in m_clte.inf_clte
                                          join i_ei in m_clte.fact_est_clte on i_i.est_clte_ID equals i_ei.est_clte_ID
                                          select new
                                          {
                                              i_i.clte_ID,
                                              i_i.cod_clte,
                                              nombre_completo = i_i.nombres + " " + i_i.apaterno + " " + i_i.amaterno,
                                              i_i.registro,
                                          }).ToList();

                            if (i_clte.Count == 0)
                            {
                                gv_clte.DataSource = i_clte;
                                gv_clte.DataBind();
                                gv_clte.Visible = true;
                                gv_clte.Visible = true;

                                Mensaje("Cliente no encontrado.");
                            }
                            else
                            {
                                gv_clte.DataSource = i_clte;
                                gv_clte.DataBind();
                                gv_clte.Visible = true;
                                gv_clte.Visible = true;
                            }
                        }
                        else
                        {
                            var i_clte = (from i_i in m_clte.inf_clte
                                          join i_ei in m_clte.fact_est_clte on i_i.est_clte_ID equals i_ei.est_clte_ID
                                          where i_i.centro_ID == empf_ID
                                          select new
                                          {
                                              i_i.clte_ID,
                                              i_i.cod_clte,
                                              nombre_completo = i_i.nombres + " " + i_i.apaterno + " " + i_i.amaterno,
                                              i_i.registro,
                                          }).ToList();

                            if (i_clte.Count == 0)
                            {
                                gv_clte.DataSource = i_clte;
                                gv_clte.DataBind();
                                gv_clte.Visible = true;
                                gv_clte.Visible = true;

                                Mensaje("Cliente no encontrado.");
                            }
                            else
                            {
                                gv_clte.DataSource = i_clte;
                                gv_clte.DataBind();
                                gv_clte.Visible = true;
                                gv_clte.Visible = true;
                            }
                        }
                    }
                }
                else
                {
                    try
                    {
                        f_busqueda = Request.Form["i_clte_buscar"].ToString().ToUpper().Trim();
                        string n_fv;

                        Char char_s = '|';
                        string d_rub = f_busqueda;
                        String[] de_rub = d_rub.Trim().Split(char_s);

                        n_fv = de_rub[1].Trim();

                        using (db_hecsaEntities m_clte = new db_hecsaEntities())
                        {
                            if (int_idperf <= 3)
                            {
                                var i_clte = (from i_i in m_clte.inf_clte
                                              join i_ei in m_clte.fact_est_clte on i_i.est_clte_ID equals i_ei.est_clte_ID
                                              where i_i.cod_clte == n_fv

                                              select new
                                              {
                                                  i_i.clte_ID,
                                                  i_i.cod_clte,
                                                  nombre_completo = i_i.nombres + " " + i_i.apaterno + " " + i_i.amaterno,
                                                  i_i.registro,
                                              }).ToList();

                                if (i_clte.Count == 0)
                                {
                                    gv_clte.DataSource = i_clte;
                                    gv_clte.DataBind();
                                    gv_clte.Visible = true;
                                    gv_clte.Visible = true;

                                    Mensaje("Cliente no encontrado.");
                                }
                                else
                                {
                                    gv_clte.DataSource = i_clte;
                                    gv_clte.DataBind();
                                    gv_clte.Visible = true;
                                    gv_clte.Visible = true;
                                }
                            }
                            else
                            {
                                var i_clte = (from i_i in m_clte.inf_clte
                                              join i_ei in m_clte.fact_est_clte on i_i.est_clte_ID equals i_ei.est_clte_ID
                                              where i_i.cod_clte == n_fv
                                              where i_i.centro_ID == empf_ID
                                              select new
                                              {
                                                  i_i.clte_ID,
                                                  i_i.cod_clte,
                                                  nombre_completo = i_i.nombres + " " + i_i.apaterno + " " + i_i.amaterno,
                                                  i_i.registro,
                                              }).ToList();

                                if (i_clte.Count == 0)
                                {
                                    gv_clte.DataSource = i_clte;
                                    gv_clte.DataBind();
                                    gv_clte.Visible = true;
                                    gv_clte.Visible = true;

                                    Mensaje("Cliente no encontrado.");
                                }
                                else
                                {
                                    gv_clte.DataSource = i_clte;
                                    gv_clte.DataBind();
                                    gv_clte.Visible = true;
                                    gv_clte.Visible = true;
                                }
                            }
                        }
                    }
                    catch
                    {
                        ctrl_clte();
                        div_i_clte.Visible = false;
                        Mensaje("Cliente no encontrado.");
                    }
                }
            }
        }

        private void ctrl_clte()
        {
            i_clte_nombresc.Value = string.Empty;
            i_clte_apaternoc.Value = string.Empty;
            i_clte_amaternoc.Value = string.Empty;
            i_clte_emailc.Value = string.Empty;
            i_clte_telc.Value = string.Empty;
            i_clte_nombres.Value = string.Empty;
            i_clte_apaterno.Value = string.Empty;
            i_clte_amaterno.Value = string.Empty;
            i_clte_email.Value = string.Empty;
            i_clte_tel.Value = string.Empty;
            i_clte_callenum.Value = string.Empty;
            i_clte_cp.Value = string.Empty;

            i_clte_tcont.Items.Clear();
            i_clte_scolonia.Items.Clear();

            using (db_hecsaEntities m_ss = new db_hecsaEntities())
            {
                var i_ma = (from c in m_ss.fact_clte_cont_tipo
                            select c).ToList();

                i_clte_tcont.DataSource = i_ma;
                i_clte_tcont.DataTextField = "clte_cont_tipo_desc";
                i_clte_tcont.DataValueField = "clte_cont_tipo_ID";
                i_clte_tcont.DataBind();

                i_clte_tcont.Items.Insert(0, new ListItem("Tipo Padre/Tutor", string.Empty));
                i_clte_scolonia.Items.Insert(0, new ListItem("Colonia", string.Empty));
            }
        }

        private void edita_clte(string dd_nombresc, string dd_apaternoc, string dd_amaternoc, string i_emailc, string i_telc, string dd_nombres, string dd_apaterno, string dd_amaterno, string i_email, string i_tel, string dd_callenum, string i_cp, string i_colonia, int i_tcc)
        {
            int int_ddl = 0;

            foreach (GridViewRow row in gv_clte.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList dl = (DropDownList)row.FindControl("ddl_clte_estatus");

                    int_ddl = int.Parse(dl.SelectedValue);
                }
            }

            using (var m_inv = new db_hecsaEntities())
            {
                var i_inv = (from c in m_inv.inf_clte
                             where c.clte_ID == clte_f
                             select c).FirstOrDefault();

                i_inv.est_clte_ID = int_ddl;

                i_inv.email = i_email;
                i_inv.telefono = i_tel;
                i_inv.callenum = dd_callenum;
                i_inv.d_codigo = i_cp;
                i_inv.id_asenta_cpcons = i_colonia;

                var i_clte_cont = (from c in m_inv.inf_clte_contacto
                                   where c.clte_ID == clte_f
                                   select c).FirstOrDefault();

                i_clte_cont.nombres = dd_nombres;
                i_clte_cont.apaterno = dd_apaterno;
                i_clte_cont.amaterno = dd_amaterno;
                i_clte_cont.email = i_emailc;
                i_clte_cont.tel = i_telc;

                m_inv.SaveChanges();
            }

            ctrl_clte();
            div_i_clte.Visible = false;
            Mensaje("Datos actualizados con éxito");
        }

        private void guarda_clte(string dd_nombresc, string dd_apaternoc, string dd_amaternoc, string i_emailc, string i_telc, string dd_nombres, string dd_apaterno, string dd_amaterno, string i_email, string i_tel, string dd_callenum, string i_cp, string i_colonia, int i_tcc, string cod_usr)
        {
            Guid guid_clte_cont = Guid.NewGuid();
            Guid guid_clte = Guid.NewGuid();
            Guid guid_usr_clte = Guid.NewGuid();
            string cod_cltef = null;
            string dd_clave = encrypta.Encrypt("poc123");
            using (db_hecsaEntities m_clte = new db_hecsaEntities())
            {
                var i_clte = (from c in m_clte.inf_clte

                              select c).ToList();

                if (i_clte.Count == 0)
                {
                    cod_cltef = "CLTE0001";
                }
                else
                {
                    cod_cltef = "CLTE" + string.Format("{0:0000}", i_clte.Count + 1);
                }

                var i_f_bf = (from c in m_clte.inf_clte
                              where c.nombres == dd_nombres
                              where c.apaterno == dd_apaterno
                              where c.amaterno == dd_amaterno
                              select c).ToList();

                usr_ID = Guid.Empty;
                empf_ID = Guid.Empty;
                usr_ID = (Guid)(Session["ss_idusr"]);
                empf_ID = (Guid)(Session["ss_idcnt"]);
                if (i_f_bf.Count == 0)
                {
                    var d_clte = new inf_clte
                    {
                        clte_ID = guid_clte,
                        est_clte_ID = 1,
                        cod_clte = cod_cltef,
                        nombres = dd_nombres,
                        apaterno = dd_apaterno,
                        amaterno = dd_amaterno,
                        email = i_email,
                        telefono = i_tel,
                        callenum = dd_callenum,
                        d_codigo = i_cp,
                        id_asenta_cpcons = i_colonia,
                        centro_ID = empf_ID,
                        usuario_ID = usr_ID,
                        registro = DateTime.Now
                    };

                    var d_clte_cont = new inf_clte_contacto
                    {
                        clte_contacto_ID = guid_clte_cont,
                        clte_cont_tipo_ID = i_tcc,
                        nombres = dd_nombresc,
                        apaterno = dd_apaternoc,
                        amaterno = dd_amaternoc,
                        email = i_emailc,
                        tel = i_telc,
                        clte_ID = guid_clte,
                        registro = DateTime.Now
                    };

                    var i_f_b_v = (from i_u in m_clte.inf_usuario
                                   where i_u.usr == cod_usr
                                   select i_u).ToList();

                    if (i_f_b_v.Count == 0)
                    {
                        var i_f_b_c = (from i_u in m_clte.inf_usuario
                                       select i_u).ToList();

                        var dn_usr = new inf_usuario
                        {
                            centro_ID = empf_ID,
                            usr = cod_usr,
                            clave = dd_clave.ToString(),
                            correo_corp = cod_usr + "@intelimundo.com.mx",
                            usuario_ID = guid_usr_clte,
                            est_usr_ID = 1,
                            cod_usr = cod_cltef,
                            registro = DateTime.Now
                        };

                        var d_usr_p = new inf_usr_personales
                        {
                            usr_personales_ID = Guid.NewGuid(),
                            nombres = dd_nombres,
                            apaterno = dd_apaterno,
                            amaterno = dd_amaterno,

                            usuario_ID = guid_usr_clte,
                            registro = DateTime.Now
                        };

                        var d_usr_rh = new inf_usr_rh
                        {
                            usr_rh_ID = Guid.NewGuid(),
                            fecha_ingreso = DateTime.Now,
                            area_ID = 4,
                            perfil_ID = 7,
                            usuario_ID = guid_usr_clte,
                            registro = DateTime.Now
                        };

                        var d_corp_ctrl = new inf_centro_ctrl
                        {
                            corporativo_ctrl_ID = Guid.NewGuid(),
                            centro_ID = empf_ID,
                            usuario_ID = guid_usr_clte
                        };

                        m_clte.inf_centro_ctrl.Add(d_corp_ctrl);
                        m_clte.inf_usuario.Add(dn_usr);
                        m_clte.inf_usr_personales.Add(d_usr_p);
                        m_clte.inf_usr_rh.Add(d_usr_rh);
                        m_clte.inf_clte.Add(d_clte);
                        m_clte.inf_clte_contacto.Add(d_clte_cont);
                        m_clte.SaveChanges();
                        ctrl_clte();
                        div_i_clte.Visible = false;
                        Mensaje("Datos guardados con éxito.");
                    }
                    else
                    {
                        ctrl_clte();

                        Mensaje("Cliente ya existe, favor de revisar..");
                    }
                }
            }
        }

        #endregion ctrl_cliente

        #region ctrl_proveedore

        protected void btn_i_prov_cp_Click(object sender, EventArgs e)
        {
            string str_cp = i_prov_cp.Value;

            using (db_hecsaEntities db_sepomex = new db_hecsaEntities())
            {
                var tbl_sepomex = (from c in db_sepomex.inf_sepomex
                                   where c.d_codigo == str_cp
                                   select c).ToList();

                i_prov_scolonia.DataSource = tbl_sepomex;
                i_prov_scolonia.DataTextField = "d_asenta";
                i_prov_scolonia.DataValueField = "id_asenta_cpcons";
                i_prov_scolonia.DataBind();

                if (tbl_sepomex.Count == 1)
                {
                    i_prov_municipio.Value = tbl_sepomex[0].d_mnpio;
                    i_prov_estado.Value = tbl_sepomex[0].d_estado;
                    i_prov_scolonia.Attributes.Add("required", "required");
                    i_prov_callenum.Attributes.Add("required", "required");
                }
                if (tbl_sepomex.Count > 1)
                {
                    i_prov_scolonia.Items.Insert(0, new ListItem("Colonia", string.Empty));

                    i_prov_municipio.Value = tbl_sepomex[0].d_mnpio;
                    i_prov_estado.Value = tbl_sepomex[0].d_estado;
                    i_prov_scolonia.Attributes.Add("required", "required");
                    i_prov_callenum.Attributes.Add("required", "required");
                }
                else if (tbl_sepomex.Count == 0)
                {
                    i_prov_scolonia.Items.Clear();
                    i_prov_scolonia.Items.Insert(0, new ListItem("Colonia", "0"));
                    i_prov_municipio.Value = null;
                    i_prov_estado.Value = null;
                    i_prov_scolonia.Attributes.Add("required", string.Empty);
                    i_prov_callenum.Attributes.Add("required", string.Empty);
                }
            }

            i_prov_scolonia.Focus();
        }

        protected void btn_prov_Click(object sender, EventArgs e)
        {
            if (est_prov == 0)
            {
                Mensaje("Favor de seleccionar una acción.");
            }
            else
            {
                int i_trfc;
                if (i_prov_trfc.Value == string.Empty)
                {
                    i_trfc = 0;
                }
                else
                {
                    i_trfc = int.Parse(Request.Form["i_prov_trfc"]);
                }

                string i_nombres_o = Request.Form["i_prov_nombres"];
                string i_aparterno_o = Request.Form["i_prov_apaterno"];
                string i_amaterno_o = Request.Form["i_prov_amaterno"];

                string i_prov_rfc = Request.Form["i_prov_rfc"];
                string i_prov_nom = Request.Form["i_prov_nom"];
                string i_dpto = Request.Form["i_prov_dpto"];
                string i_emailc = Request.Form["i_prov_emailc"];
                string i_telc = Request.Form["i_prov_telc"];
                string i_email = Request.Form["i_prov_email"];
                string i_tel = Request.Form["i_prov_tel"];
                string i_callenum = Request.Form["i_prov_callenum"];
                string i_cp = Request.Form["i_prov_cp"];
                string i_colonia = Request.Form["i_prov_scolonia"];

                TextInfo t_prov_nom = new CultureInfo("es-MX", false).TextInfo;
                TextInfo t_nombres = new CultureInfo("es-MX", false).TextInfo;
                TextInfo t_apateno = new CultureInfo("es-MX", false).TextInfo;
                TextInfo t_amateno = new CultureInfo("es-MX", false).TextInfo;
                TextInfo t_dpto = new CultureInfo("es-MX", false).TextInfo;
                TextInfo t_callenum = new CultureInfo("es-MX", false).TextInfo;

                string dd_prov_nom = t_prov_nom.ToTitleCase(i_prov_nom.ToLower());
                string dd_nombres = t_nombres.ToTitleCase(i_nombres_o.ToLower());
                string dd_apaterno = t_apateno.ToTitleCase(i_aparterno_o.ToLower());
                string dd_amaterno = t_amateno.ToTitleCase(i_amaterno_o.ToLower());
                string dd_dpto = t_dpto.ToTitleCase(i_dpto.ToLower());
                string dd_callenum = t_callenum.ToTitleCase(i_callenum.ToLower());

                if (est_prov == 1)
                {
                    guarda_proveedor(dd_nombres, dd_apaterno, dd_amaterno, i_emailc, i_telc, i_trfc, i_prov_rfc, dd_dpto, dd_prov_nom, i_email, i_tel, dd_callenum, i_cp, i_colonia);
                }
                else
                {
                    edita_proveedor(dd_nombres, dd_apaterno, dd_amaterno, i_emailc, i_telc, i_trfc, i_prov_rfc, dd_dpto, dd_prov_nom, i_email, i_tel, dd_callenum, i_cp, i_colonia);
                }
            }
        }

        protected void gv_prov_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }

        protected void gv_prov_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            est_prov = 2;
            div_i_prov.Visible = true;
            if (e.CommandName == "btn_prov")
            {
                try
                {
                    GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);

                    prov_f = Guid.Parse(gvr.Cells[0].Text.ToString().Trim());
                    using (db_hecsaEntities m_prov = new db_hecsaEntities())
                    {
                        var i_prov = (from i_i in m_prov.inf_proveedor
                                      join i_ei in m_prov.fact_est_prov on i_i.est_prov_ID equals i_ei.est_prov_ID
                                      join i_pe in m_prov.inf_prov_contacto on i_i.prov_ID equals i_pe.prov_ID
                                      where i_i.prov_ID == prov_f
                                      select new
                                      {
                                          i_i.tipo_rfc_ID,
                                          i_i.rfc,
                                          i_i.callenum,
                                          i_i.d_codigo,
                                          i_i.id_asenta_cpcons,
                                          i_i.prov_ID,
                                          i_i.email,
                                          i_i.telefono,
                                          i_i.cod_prov,
                                          i_i.razon_social,
                                          i_i.registro,
                                          i_pe.nombres,
                                          i_pe.apaterno,
                                          i_pe.amaterno,
                                          i_pe.dpto,
                                          emailc = i_pe.email,
                                          i_pe.tel,
                                      }).ToList();

                        if (i_prov.Count == 0)
                        {
                            gv_prov.DataSource = i_prov;
                            gv_prov.DataBind();
                            gv_prov.Visible = true;
                            gv_prov.Visible = true;

                            Mensaje("Inventario no encontrado.");
                        }
                        else
                        {
                            gv_prov.DataSource = i_prov;
                            gv_prov.DataBind();
                            gv_prov.Visible = true;
                            gv_prov.Visible = true;
                        }

                        i_prov_dpto.Value = i_prov[0].dpto;
                        i_prov_nombres.Value = i_prov[0].nombres;
                        i_prov_apaterno.Value = i_prov[0].apaterno;
                        i_prov_amaterno.Value = i_prov[0].amaterno;
                        i_prov_emailc.Value = i_prov[0].emailc;
                        i_prov_telc.Value = i_prov[0].tel;
                        i_prov_nom.Value = i_prov[0].razon_social;
                        i_prov_rfc.Value = i_prov[0].rfc;
                        i_prov_email.Value = i_prov[0].email;
                        i_prov_tel.Value = i_prov[0].telefono;
                        i_prov_callenum.Value = i_prov[0].callenum;
                        i_prov_cp.Value = i_prov[0].d_codigo;

                        i_prov_trfc.Items.Clear();
                        i_prov_scolonia.Items.Clear();

                        var i_ma = (from c in m_prov.fact_rfc_tipo
                                    select c).ToList();

                        i_prov_trfc.DataSource = i_ma;
                        i_prov_trfc.DataTextField = "tipo_rfc_desc";
                        i_prov_trfc.DataValueField = "tipo_rfc_ID";
                        i_prov_trfc.DataBind();

                        i_prov_trfc.Items.Insert(0, new ListItem("Tipo RFC", string.Empty));
                        i_prov_trfc.SelectedIndex = int.Parse(i_prov[0].tipo_rfc_ID.ToString());

                        string d_codigo = i_prov[0].d_codigo;

                        var tbl_sepomex = (from c in m_prov.inf_sepomex
                                           where c.d_codigo == d_codigo
                                           select c).ToList();

                        i_prov_scolonia.DataSource = tbl_sepomex;
                        i_prov_scolonia.DataTextField = "d_asenta";
                        i_prov_scolonia.DataValueField = "id_asenta_cpcons";
                        i_prov_scolonia.DataBind();

                        i_prov_scolonia.Items.Insert(0, new ListItem("Colonia", string.Empty));
                        i_prov_scolonia.Value = i_prov[0].id_asenta_cpcons.ToString();

                        i_prov_municipio.Value = tbl_sepomex[0].d_mnpio;
                        i_prov_estado.Value = tbl_sepomex[0].d_estado;
                    }
                }
                catch
                { }
            }
        }

        protected void gv_prov_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Guid f_ID = Guid.Parse(e.Row.Cells[0].Text);
                int est_id;

                using (db_hecsaEntities m_est = new db_hecsaEntities())
                {
                    var i_est = (from md_usr in m_est.inf_proveedor
                                 where md_usr.prov_ID == f_ID
                                 select new
                                 {
                                     md_usr.est_prov_ID,
                                 }).FirstOrDefault();

                    est_id = int.Parse(i_est.est_prov_ID.ToString());

                    DropDownList ddl_est = (e.Row.FindControl("ddl_prov_estatus") as DropDownList);

                    var tbl_sepomex = (from c in m_est.fact_est_prov
                                       select c).ToList();

                    ddl_est.DataSource = tbl_sepomex;

                    ddl_est.DataTextField = "est_prov_desc";
                    ddl_est.DataValueField = "est_prov_ID";
                    ddl_est.DataBind();
                    ddl_est.Items.Insert(0, new ListItem("Seleccionar", "0"));
                    ddl_est.SelectedValue = est_id.ToString();
                }
            }
        }

        protected void lkb_prov_agregar_Click(object sender, EventArgs e)
        {
            ctrl_prov();
            est_prov = 1;
            gv_prov.Visible = false;
            div_i_prov.Visible = true;
            i_prov_buscar.Text = string.Empty;
        }

        protected void lkb_prov_buscar_Click(object sender, EventArgs e)
        {
            div_i_prov.Visible = false;
            string f_busqueda = string.Empty;

            if (string.IsNullOrEmpty(i_prov_buscar.Text))
            {
            }
            else
            {
                f_busqueda = Request.Form["i_prov_buscar"].ToString().ToUpper().Trim();
                if (f_busqueda == "TODO")
                {
                    f_busqueda = Request.Form["i_prov_buscar"].ToString().ToUpper().Trim();

                    using (db_hecsaEntities m_prov = new db_hecsaEntities())
                    {
                        if (int_idperf <= 3)
                        {
                            var i_prov = (from i_i in m_prov.inf_proveedor
                                          join i_ei in m_prov.fact_est_prov on i_i.est_prov_ID equals i_ei.est_prov_ID
                                          select new
                                          {
                                              i_i.prov_ID,
                                              i_i.cod_prov,
                                              i_ei.est_prov_desc,
                                              i_i.razon_social,

                                              i_i.registro,
                                          }).ToList();

                            if (i_prov.Count == 0)
                            {
                                gv_prov.DataSource = i_prov;
                                gv_prov.DataBind();
                                gv_prov.Visible = true;
                                gv_prov.Visible = true;

                                Mensaje("Proveedor no encontrado.");
                            }
                            else
                            {
                                gv_prov.DataSource = i_prov;
                                gv_prov.DataBind();
                                gv_prov.Visible = true;
                                gv_prov.Visible = true;
                            }
                        }
                        else
                        {
                            var i_prov = (from i_i in m_prov.inf_proveedor
                                          join i_ei in m_prov.fact_est_prov on i_i.est_prov_ID equals i_ei.est_prov_ID
                                          where i_i.centro_ID == empf_ID
                                          select new
                                          {
                                              i_i.prov_ID,
                                              i_i.cod_prov,
                                              i_ei.est_prov_desc,
                                              i_i.razon_social,

                                              i_i.registro,
                                          }).ToList();

                            if (i_prov.Count == 0)
                            {
                                gv_prov.DataSource = i_prov;
                                gv_prov.DataBind();
                                gv_prov.Visible = true;
                                gv_prov.Visible = true;

                                Mensaje("Proveedor no encontrado.");
                            }
                            else
                            {
                                gv_prov.DataSource = i_prov;
                                gv_prov.DataBind();
                                gv_prov.Visible = true;
                                gv_prov.Visible = true;
                            }
                        }
                    }
                }
                else
                {
                    try
                    {
                        f_busqueda = Request.Form["i_prov_buscar"].ToString().ToUpper().Trim();
                        string n_fv;

                        Char char_s = '|';
                        string d_rub = f_busqueda;
                        String[] de_rub = d_rub.Trim().Split(char_s);

                        n_fv = de_rub[1].Trim();

                        using (db_hecsaEntities m_prov = new db_hecsaEntities())
                        {
                            if (int_idperf <= 3)
                            {
                                var i_prov = (from i_i in m_prov.inf_proveedor
                                              join i_ei in m_prov.fact_est_prov on i_i.est_prov_ID equals i_ei.est_prov_ID
                                              where i_i.cod_prov == n_fv
                                              select new
                                              {
                                                  i_i.prov_ID,
                                                  i_i.cod_prov,
                                                  i_ei.est_prov_desc,
                                                  i_i.razon_social,
                                                  i_i.registro
                                              }).ToList();

                                if (i_prov.Count == 0)
                                {
                                    gv_prov.DataSource = i_prov;
                                    gv_prov.DataBind();
                                    gv_prov.Visible = true;
                                    gv_prov.Visible = true;

                                    Mensaje("Proveedor no encontrado.");
                                }
                                else
                                {
                                    gv_prov.DataSource = i_prov;
                                    gv_prov.DataBind();
                                    gv_prov.Visible = true;
                                    gv_prov.Visible = true;
                                }
                            }
                            else
                            {
                                var i_prov = (from i_i in m_prov.inf_proveedor
                                              join i_ei in m_prov.fact_est_prov on i_i.est_prov_ID equals i_ei.est_prov_ID
                                              where i_i.cod_prov == n_fv
                                              where i_i.centro_ID == empf_ID
                                              select new
                                              {
                                                  i_i.prov_ID,
                                                  i_i.cod_prov,
                                                  i_ei.est_prov_desc,
                                                  i_i.razon_social,
                                                  i_i.registro
                                              }).ToList();

                                if (i_prov.Count == 0)
                                {
                                    gv_prov.DataSource = i_prov;
                                    gv_prov.DataBind();
                                    gv_prov.Visible = true;
                                    gv_prov.Visible = true;

                                    Mensaje("Proveedor no encontrado.");
                                }
                                else
                                {
                                    gv_prov.DataSource = i_prov;
                                    gv_prov.DataBind();
                                    gv_prov.Visible = true;
                                    gv_prov.Visible = true;
                                }
                            }
                        }
                    }
                    catch
                    {
                        ctrl_prov();
                        div_i_prov.Visible = false;
                        Mensaje("Proveedor no encontrado.");
                    }
                }
            }
        }

        private void ctrl_prov()
        {
            i_prov_nombres.Value = string.Empty;
            i_prov_apaterno.Value = string.Empty;
            i_prov_amaterno.Value = string.Empty;
            i_prov_emailc.Value = string.Empty;
            i_prov_telc.Value = string.Empty;
            i_prov_nom.Value = string.Empty;
            i_prov_rfc.Value = string.Empty;
            i_prov_email.Value = string.Empty;
            i_prov_tel.Value = string.Empty;
            i_prov_callenum.Value = string.Empty;
            i_prov_cp.Value = string.Empty;

            i_prov_trfc.Items.Clear();
            i_prov_scolonia.Items.Clear();

            using (db_hecsaEntities m_ss = new db_hecsaEntities())
            {
                var i_ma = (from c in m_ss.fact_rfc_tipo
                            select c).ToList();

                i_prov_trfc.DataSource = i_ma;
                i_prov_trfc.DataTextField = "tipo_rfc_desc";
                i_prov_trfc.DataValueField = "tipo_rfc_ID";
                i_prov_trfc.DataBind();

                i_prov_trfc.Items.Insert(0, new ListItem("Tipo RFC", string.Empty));
                i_prov_scolonia.Items.Insert(0, new ListItem("Colonia", string.Empty));
            }
        }

        private void edita_proveedor(string dd_nombres, string dd_apaterno, string dd_amaterno, string i_ecmailc, string i_telc, int i_trfc, string i_prov_rfc, string dd_dpto, string dd_prov_nom, string i_email, string i_tel, string dd_callenum, string i_cp, string i_colonia)
        {
            int int_ddl = 0;

            foreach (GridViewRow row in gv_prov.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList dl = (DropDownList)row.FindControl("ddl_prov_estatus");

                    int_ddl = int.Parse(dl.SelectedValue);
                }
            }

            using (var m_inv = new db_hecsaEntities())
            {
                var i_inv = (from c in m_inv.inf_proveedor
                             where c.prov_ID == prov_f
                             select c).FirstOrDefault();

                i_inv.est_prov_ID = int_ddl;
                i_inv.razon_social = dd_prov_nom;
                i_inv.tipo_rfc_ID = i_trfc;
                i_inv.rfc = i_prov_rfc;
                i_inv.email = i_email;
                i_inv.telefono = i_tel;
                i_inv.callenum = dd_callenum;
                i_inv.d_codigo = i_cp;
                i_inv.id_asenta_cpcons = i_colonia;

                var i_prov_cont = (from c in m_inv.inf_prov_contacto
                                   where c.prov_ID == prov_f
                                   select c).FirstOrDefault();

                i_prov_cont.dpto = dd_dpto;
                i_prov_cont.nombres = dd_nombres;
                i_prov_cont.apaterno = dd_apaterno;
                i_prov_cont.amaterno = dd_amaterno;
                i_prov_cont.email = i_ecmailc;
                i_prov_cont.tel = i_telc;

                m_inv.SaveChanges();
            }

            ctrl_prov();
            div_i_prov.Visible = false;
            Mensaje("Datos actualizados con éxito");
        }

        private void guarda_proveedor(string dd_nombres, string dd_apaterno, string dd_amaterno, string i_emailc, string i_telc, int i_trfc, string i_prov_rfc, string dd_dpto, string dd_prov_nom, string i_email, string i_tel, string dd_callenum, string i_cp, string i_colonia)
        {
            Guid guid_prov_cont = Guid.NewGuid();
            Guid guid_prov = Guid.NewGuid();
            string cod_provf = null;

            using (db_hecsaEntities m_prov = new db_hecsaEntities())
            {
                var i_prov = (from c in m_prov.inf_proveedor

                              select c).ToList();

                if (i_prov.Count == 0)
                {
                    cod_provf = "PROV0001";
                }
                else
                {
                    cod_provf = "PROV" + string.Format("{0:0000}", i_prov.Count + 1);
                }

                var i_f_bf = (from c in m_prov.inf_proveedor
                              where c.razon_social == dd_prov_nom
                              select c).ToList();
                usr_ID = Guid.Empty;
                empf_ID = Guid.Empty;
                usr_ID = (Guid)(Session["ss_idusr"]);
                empf_ID = (Guid)(Session["ss_idcnt"]);
                if (i_f_bf.Count == 0)
                {
                    var d_prov = new inf_proveedor
                    {
                        prov_ID = guid_prov,
                        est_prov_ID = 1,
                        cod_prov = cod_provf,
                        tipo_rfc_ID = i_trfc,
                        rfc = i_prov_rfc,
                        razon_social = dd_prov_nom,
                        email = i_email,
                        telefono = i_tel,
                        callenum = dd_callenum,
                        d_codigo = i_cp,
                        id_asenta_cpcons = i_colonia,
                        centro_ID = empf_ID,
                        usuario_ID = usr_ID,
                        registro = DateTime.Now
                    };

                    var d_prov_cont = new inf_prov_contacto
                    {
                        prov_contacto_ID = guid_prov_cont,
                        dpto = dd_dpto,
                        nombres = dd_nombres,
                        apaterno = dd_apaterno,
                        amaterno = dd_amaterno,
                        email = i_emailc,
                        tel = i_telc,
                        prov_ID = guid_prov,
                        registro = DateTime.Now
                    };

                    m_prov.inf_proveedor.Add(d_prov);

                    m_prov.inf_prov_contacto.Add(d_prov_cont);

                    m_prov.SaveChanges();
                    ctrl_prov();
                    div_i_prov.Visible = false;
                    Mensaje("Datos guardados con éxito.");
                }
                else
                {
                    ctrl_prov();

                    Mensaje("Proveedor ya existe, favor de revisar..");
                }
            }
        }

        #endregion ctrl_proveedore

        #region ctrl_usuario

        protected void btn_usr_ctrl_Click(object sender, EventArgs e)
        {
            if (est_usr == 0)
            {
                Mensaje("Favor de seleccionar una acción.");
            }
            else
            {
                Guid guid_usr = Guid.NewGuid();

                string i_nombres = string.Empty, i_aparterno = string.Empty, i_amaterno = string.Empty, cod_usr = string.Empty, str_clave = string.Empty;
                string i_nombres_o = Request.Form["i_nombres"];
                string i_aparterno_o = Request.Form["i_apaterno"];
                string i_amaterno_o = Request.Form["i_amaterno"];
                str_clave = encrypta.Encrypt("poc123");

                try
                {
                    i_nombres = RemoveSpecialCharacters(RemoveAccentsWithRegEx(i_nombres_o.Trim().ToLower()));
                    string[] separados;

                    separados = i_nombres_o.Split(" ".ToCharArray());

                    i_aparterno = RemoveSpecialCharacters(RemoveAccentsWithRegEx(i_aparterno_o.Trim().ToLower()));
                    i_amaterno = RemoveSpecialCharacters(RemoveAccentsWithRegEx(i_amaterno_o.Trim().ToLower()));

                    cod_usr = left_right_mid.left(i_nombres, 1) + i_aparterno + left_right_mid.left(i_amaterno, 1);

                    i_usr.Value = cod_usr;
                    i_clave.Value = str_clave;
                    i_emal_c.Value = cod_usr + "@intelimundo.com.mx";
                    btn_usr_guarda.Visible = true;
                }
                catch
                {
                    Mensaje("Se requiere mínimo 2 letras por cada campo(nombre,apellido paterno, apellido materno) para generar el usuario.");
                }
            }
        }

        protected void btn_usr_guarda_Click(object sender, EventArgs e)
        {
            if (est_usr == 0)
            {
                Mensaje("Favor de seleccionar una acción.");
            }
            else
            {
                int i_area = int.Parse(Request.Form["s_area"]);
                int i_perfil = int.Parse(Request.Form["s_perfil"]);
                int i_genero = int.Parse(Request.Form["s_genero"]);
                DateTime d_nacimiento = DateTime.Parse(Request.Form["i_nacimiento"]);
                int int_ddl = 0;

                string i_nombres_o = Request.Form["i_nombres"];
                string i_aparterno_o = Request.Form["i_apaterno"];
                string i_amaterno_o = Request.Form["i_amaterno"];
                string i_f_b = Request.Form["i_usr"];
                string i_emal_p = Request.Form["i_emal_p"];
                string i_emal_c = Request.Form["i_emal_c"];

                TextInfo t_nombres = new CultureInfo("es-MX", false).TextInfo;
                TextInfo t_apateno = new CultureInfo("es-MX", false).TextInfo;
                TextInfo t_amateno = new CultureInfo("es-MX", false).TextInfo;

                string dd_nombres = t_nombres.ToTitleCase(i_nombres_o.ToLower());
                string dd_apaterno = t_apateno.ToTitleCase(i_aparterno_o.ToLower());
                string dd_amaterno = t_amateno.ToTitleCase(i_amaterno_o.ToLower());
                string dd_clave;

                if (string.IsNullOrEmpty(i_f_b))
                {
                    Mensaje("Favor de generar datos de control");
                }
                else
                {
                    if (est_usr == 1)
                    {
                        dd_clave = encrypta.Encrypt("poc123");

                        agrega_usuario(usrf_ID, int_ddl, i_area, i_perfil, i_genero, d_nacimiento, i_f_b, dd_clave, dd_nombres, dd_apaterno, dd_amaterno, i_emal_p, i_emal_c);
                    }
                    else
                    {
                        dd_clave = encrypta.Encrypt(Request.Form["i_clave"]);

                        using (var m_usrs = new db_hecsaEntities())
                        {
                            var i_f_b_v = (from i_u in m_usrs.inf_usuario
                                           where i_u.usuario_ID == usrf_ID
                                           select i_u).FirstOrDefault();

                            if (i_f_b_v.usr != i_f_b)
                            {
                                var i_f_b_f = (from i_u in m_usrs.inf_usuario
                                               where i_u.usuario_ID == usrf_ID
                                               select i_u).FirstOrDefault();

                                if (i_f_b_f.usr == i_f_b)
                                {
                                    div_i_usr.Visible = false;
                                    ctrl_usr();
                                    Mensaje("Usuario ya existe, favor de re-intentar.");
                                }
                                else
                                {
                                    actualiza_usuario(usrf_ID, int_ddl, i_area, i_perfil, i_genero, d_nacimiento, i_f_b, dd_clave, dd_nombres, dd_apaterno, dd_amaterno, i_emal_p, i_emal_c);
                                    div_i_usr.Visible = false;
                                    ctrl_usr();
                                    Mensaje("Usuario actualizado con éxito");
                                }
                            }
                            else
                            {
                                actualiza_usuario(usrf_ID, int_ddl, i_area, i_perfil, i_genero, d_nacimiento, i_f_b, dd_clave, dd_nombres, dd_apaterno, dd_amaterno, i_emal_p, i_emal_c);
                                div_i_usr.Visible = false;
                                ctrl_usr();
                                Mensaje("Usuario actualizado con éxito");
                            }
                        }
                    }
                }
            }
        }

        protected void gv_usrs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_usrs.PageIndex = e.NewPageIndex;

            div_i_usr.Visible = false;
            string f_busqueda = string.Empty;
            int f_usrp = 0;

            if (string.IsNullOrEmpty(i_usuario_buscar.Text))
            {
            }
            else
            {
                f_busqueda = Request.Form["i_usuario_buscar"].ToString().ToUpper().Trim();
                if (f_busqueda == "TODO")
                {
                    f_busqueda = Request.Form["i_usuario_buscar"].ToString().ToUpper().Trim();

                    if (int_idperf == 3)
                    {
                        f_usrp = 2;
                    }

                    using (db_hecsaEntities md_fb = new db_hecsaEntities())
                    {
                        var i_f_b = (from i_u in md_fb.inf_usuario
                                     join i_up in md_fb.inf_usr_personales on i_u.usuario_ID equals i_up.usuario_ID
                                     join i_rh in md_fb.inf_usr_rh on i_u.usuario_ID equals i_rh.usuario_ID
                                     where i_rh.perfil_ID != f_usrp
                                     where i_u.usuario_ID != usr_ID
                                     select new
                                     {
                                         i_u.usuario_ID,
                                         i_u.cod_usr,
                                         nom_comp = i_up.nombres + " " + i_up.apaterno + " " + i_up.amaterno,
                                         i_u.registro
                                     }).OrderBy(x => x.cod_usr).ToList();

                        if (i_f_b.Count == 0)
                        {
                            gv_usrs.DataSource = i_f_b;
                            gv_usrs.DataBind();
                            gv_usrs.Visible = true;
                            gv_usrs.Visible = true;

                            Mensaje("Usuario no encontrado.");
                        }
                        else
                        {
                            gv_usrs.DataSource = i_f_b;
                            gv_usrs.DataBind();
                            gv_usrs.Visible = true;
                            gv_usrs.Visible = true;
                        }
                    }
                }
                else
                {
                    try
                    {
                        f_busqueda = Request.Form["i_usuario_buscar"].ToString().ToUpper().Trim();
                        string n_fv;
                        Guid guid_usrid;
                        Char char_s = '|';
                        string d_rub = f_busqueda;
                        String[] de_rub = d_rub.Trim().Split(char_s);

                        n_fv = de_rub[1].Trim();

                        using (db_hecsaEntities m_usrf = new db_hecsaEntities())
                        {
                            var i_f_bf = (from c in m_usrf.inf_usuario
                                          where c.cod_usr == n_fv
                                          select c).FirstOrDefault();

                            guid_usrid = i_f_bf.usuario_ID;

                            var i_f_bff = (from i_u in m_usrf.inf_usuario
                                           join i_up in m_usrf.inf_usr_personales on i_u.usuario_ID equals i_up.usuario_ID
                                           where i_u.usuario_ID == guid_usrid
                                           select new
                                           {
                                               i_u.usuario_ID,
                                               i_u.cod_usr,
                                               nom_comp = i_up.nombres + " " + i_up.apaterno + " " + i_up.amaterno,
                                               i_u.registro
                                           }).OrderBy(x => x.cod_usr).ToList();

                            if (i_f_bff.Count == 0)
                            {
                                gv_usrs.DataSource = i_f_bff;
                                gv_usrs.DataBind();
                                gv_usrs.Visible = true;
                                gv_usrs.Visible = true;

                                Mensaje("Usuario no encontrado.");
                            }
                            else
                            {
                                gv_usrs.DataSource = i_f_bff;
                                gv_usrs.DataBind();
                                gv_usrs.Visible = true;
                                gv_usrs.Visible = true;
                            }
                        }
                    }
                    catch
                    {
                        //limp_usr_ctrl();
                        //div_prospecto.Visible = false;
                        Mensaje("Usuario no encontrado.");
                    }
                }
            }
        }

        protected void gv_usrs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            est_usr = 2;
            if (e.CommandName == "btn_usr_ve")
            {
                try
                {
                    GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);

                    usrf_ID = Guid.Parse(gvr.Cells[0].Text.ToString().Trim());
                    btn_usr_ctrl.Enabled = false;
                    i_usr.Attributes.Remove("disabled");
                    i_clave.Attributes.Remove("disabled");
                    i_emal_c.Attributes.Remove("disabled");

                    using (db_hecsaEntities edm_usr = new db_hecsaEntities())
                    {
                        var i_f_bf = (from i_u in edm_usr.inf_usuario
                                      join i_uh in edm_usr.inf_usr_rh on i_u.usuario_ID equals i_uh.usuario_ID
                                      join i_up in edm_usr.inf_usr_personales on i_u.usuario_ID equals i_up.usuario_ID
                                      where i_u.usuario_ID == usrf_ID
                                      select new
                                      {
                                          i_u.usuario_ID,
                                          i_u.est_usr_ID,
                                          i_u.cod_usr,
                                          i_u.usr,
                                          i_u.clave,
                                          i_u.correo_corp,
                                          i_uh.area_ID,
                                          i_uh.perfil_ID,
                                          i_uh.comentarios,
                                          i_uh.fecha_ingreso,
                                          i_up.nombres,
                                          i_up.apaterno,
                                          i_up.amaterno,
                                          i_up.genero_ID,
                                          i_up.estcivil_ID,
                                          i_up.cumple,
                                          i_up.hijos,
                                          i_up.nss,
                                          i_up.curp,
                                          i_up.licencia,
                                          i_up.rfc,
                                      }).FirstOrDefault();

                        s_area.Value = i_f_bf.area_ID.ToString();
                        s_perfil.Value = i_f_bf.perfil_ID.ToString();
                        s_genero.Value = i_f_bf.genero_ID.ToString();
                        //ddl_usr_estciv.SelectedValue = i_f_b.estcivil_ID.ToString();
                        DateTime f_nac = DateTime.MinValue;
                        if (i_f_bf.cumple == null)
                        {
                        }
                        else
                        {
                            f_nac = Convert.ToDateTime(i_f_bf.cumple);
                            i_nacimiento.Value = f_nac.ToString("yyyy-MM-dd");
                        }

                        DateTime f_ing = Convert.ToDateTime(i_f_bf.fecha_ingreso);

                        i_nombres.Value = i_f_bf.nombres;
                        i_apaterno.Value = i_f_bf.apaterno;
                        i_amaterno.Value = i_f_bf.amaterno;

                        i_usr.Value = i_f_bf.usr;
                        i_emal_c.Value = i_f_bf.correo_corp;

                        var i_f_b_ff = (from i_u in edm_usr.inf_usuario
                                        join i_up in edm_usr.inf_usr_personales on i_u.usuario_ID equals i_up.usuario_ID
                                        where i_u.usuario_ID == usrf_ID
                                        select new
                                        {
                                            i_u.usuario_ID,
                                            i_u.cod_usr,
                                            nom_comp = i_up.nombres + " " + i_up.apaterno + " " + i_up.amaterno,
                                            i_u.registro
                                        }).OrderBy(x => x.cod_usr).ToList();

                        if (i_f_b_ff.Count == 0)
                        {
                            gv_usrs.DataSource = i_f_b_ff;
                            gv_usrs.DataBind();
                            gv_usrs.Visible = true;
                            gv_usrs.Visible = true;

                            Mensaje("Usuario no encontrado.");
                        }
                        else
                        {
                            gv_usrs.DataSource = i_f_b_ff;
                            gv_usrs.DataBind();
                            gv_usrs.Visible = true;
                            gv_usrs.Visible = true;
                        }

                        div_i_usr.Visible = true;
                    }
                }
                catch
                {
                    ctrl_usr();
                    div_i_usr.Visible = true;
                    Mensaje("Usuario no encontrado.");
                }
            }
        }

        protected void gv_usrs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Guid usr_ID = Guid.Parse(e.Row.Cells[0].Text);
                int est_id;

                using (db_hecsaEntities m_usr = new db_hecsaEntities())
                {
                    var i_f_b = (from md_usr in m_usr.inf_usuario
                                 where md_usr.usuario_ID == usr_ID
                                 select new
                                 {
                                     md_usr.est_usr_ID,
                                 }).FirstOrDefault();

                    est_id = int.Parse(i_f_b.est_usr_ID.ToString());

                    DropDownList ddl_est = (e.Row.FindControl("ddl_usr_estatus") as DropDownList);

                    var tbl_sepomex = (from c in m_usr.fact_est_usr
                                       select c).ToList();

                    ddl_est.DataSource = tbl_sepomex;

                    ddl_est.DataTextField = "est_usr_desc";
                    ddl_est.DataValueField = "est_usr_ID";
                    ddl_est.DataBind();
                    ddl_est.Items.Insert(0, new ListItem("Seleccionar", "0"));
                    ddl_est.SelectedValue = est_id.ToString();
                }
            }
        }

        protected void lkb_usuario_agregar_Click(object sender, EventArgs e)
        {
            ctrl_usr();
            est_usr = 1;

            i_usr.Attributes.Add("disabled", "disabled");
            i_clave.Attributes.Add("disabled", "disabled");
            i_emal_c.Attributes.Add("disabled", "disabled");
            gv_usrs.Visible = false;
            div_i_usr.Visible = true;
            i_usuario_buscar.Text = string.Empty;
            btn_usr_ctrl.Enabled = true;
        }

        protected void lkb_usuario_buscar_Click(object sender, EventArgs e)
        {
            div_i_usr.Visible = false;
            string f_busqueda = string.Empty;
            int f_usrp = 0;

            if (string.IsNullOrEmpty(i_usuario_buscar.Text))
            {
            }
            else
            {
                f_busqueda = Request.Form["i_usuario_buscar"].ToString().ToUpper().Trim();
                if (f_busqueda == "TODO")
                {
                    f_busqueda = Request.Form["i_usuario_buscar"].ToString().ToUpper().Trim();

                    using (db_hecsaEntities md_fb = new db_hecsaEntities())
                    {
                        if (int_idperf <= 3)
                        {
                            var i_f_b = (from i_u in md_fb.inf_usuario
                                         join i_up in md_fb.inf_usr_personales on i_u.usuario_ID equals i_up.usuario_ID
                                         join i_rh in md_fb.inf_usr_rh on i_u.usuario_ID equals i_rh.usuario_ID
                                         where i_rh.perfil_ID != f_usrp
                                         where i_u.usuario_ID != usr_ID
                                         select new
                                         {
                                             i_u.usuario_ID,
                                             i_u.cod_usr,
                                             nom_comp = i_up.nombres + " " + i_up.apaterno + " " + i_up.amaterno,
                                             i_u.registro
                                         }).Distinct().ToList();

                            if (i_f_b.Count == 0)
                            {
                                gv_usrs.DataSource = i_f_b;
                                gv_usrs.DataBind();
                                gv_usrs.Visible = true;
                                gv_usrs.Visible = true;

                                Mensaje("Usuario no encontrado.");
                            }
                            else
                            {
                                gv_usrs.DataSource = i_f_b;
                                gv_usrs.DataBind();
                                gv_usrs.Visible = true;
                                gv_usrs.Visible = true;
                            }
                        }
                        else
                        {
                            var i_f_b = (from i_u in md_fb.inf_usuario
                                         join i_up in md_fb.inf_usr_personales on i_u.usuario_ID equals i_up.usuario_ID
                                         join i_rh in md_fb.inf_usr_rh on i_u.usuario_ID equals i_rh.usuario_ID
                                         where i_u.centro_ID == empf_ID
                                         where i_u.usuario_ID != usr_ID
                                         select new
                                         {
                                             i_u.usuario_ID,
                                             i_u.cod_usr,
                                             nom_comp = i_up.nombres + " " + i_up.apaterno + " " + i_up.amaterno,
                                             i_u.registro
                                         }).Distinct().ToList();

                            if (i_f_b.Count == 0)
                            {
                                gv_usrs.DataSource = i_f_b;
                                gv_usrs.DataBind();
                                gv_usrs.Visible = true;
                                gv_usrs.Visible = true;

                                Mensaje("Usuario no encontrado.");
                            }
                            else
                            {
                                gv_usrs.DataSource = i_f_b;
                                gv_usrs.DataBind();
                                gv_usrs.Visible = true;
                                gv_usrs.Visible = true;
                            }
                        }
                    }
                }
                else
                {
                    try
                    {
                        f_busqueda = Request.Form["i_usuario_buscar"].ToString().ToUpper().Trim();
                        string n_fv;
                        Guid guid_usrid;
                        Char char_s = '|';
                        string d_rub = f_busqueda;
                        String[] de_rub = d_rub.Trim().Split(char_s);

                        n_fv = de_rub[1].Trim();

                        using (db_hecsaEntities m_usrf = new db_hecsaEntities())
                        {
                            if (int_idperf <= 3)
                            {
                                var i_f_bf = (from c in m_usrf.inf_usuario
                                              where c.cod_usr == n_fv
                                              select c).FirstOrDefault();

                                guid_usrid = i_f_bf.usuario_ID;

                                var i_f_bff = (from i_u in m_usrf.inf_usuario
                                               join i_up in m_usrf.inf_usr_personales on i_u.usuario_ID equals i_up.usuario_ID
                                               where i_u.usuario_ID == guid_usrid
                                               select new
                                               {
                                                   i_u.usuario_ID,
                                                   i_u.cod_usr,
                                                   nom_comp = i_up.nombres + " " + i_up.apaterno + " " + i_up.amaterno,
                                                   i_u.registro
                                               }).OrderBy(x => x.cod_usr).ToList();

                                if (i_f_bff.Count == 0)
                                {
                                    gv_usrs.DataSource = i_f_bff;
                                    gv_usrs.DataBind();
                                    gv_usrs.Visible = true;
                                    gv_usrs.Visible = true;

                                    Mensaje("Usuario no encontrado.");
                                }
                                else
                                {
                                    gv_usrs.DataSource = i_f_bff;
                                    gv_usrs.DataBind();
                                    gv_usrs.Visible = true;
                                    gv_usrs.Visible = true;
                                }
                            }
                            else
                            {
                                var i_f_bf = (from c in m_usrf.inf_usuario
                                              where c.cod_usr == n_fv
                                              select c).FirstOrDefault();

                                guid_usrid = i_f_bf.usuario_ID;

                                var i_f_bff = (from i_u in m_usrf.inf_usuario
                                               join i_up in m_usrf.inf_usr_personales on i_u.usuario_ID equals i_up.usuario_ID
                                               where i_u.usuario_ID == guid_usrid
                                               where i_u.centro_ID == empf_ID
                                               select new
                                               {
                                                   i_u.usuario_ID,
                                                   i_u.cod_usr,
                                                   nom_comp = i_up.nombres + " " + i_up.apaterno + " " + i_up.amaterno,
                                                   i_u.registro
                                               }).OrderBy(x => x.cod_usr).ToList();

                                if (i_f_bff.Count == 0)
                                {
                                    gv_usrs.DataSource = i_f_bff;
                                    gv_usrs.DataBind();
                                    gv_usrs.Visible = true;
                                    gv_usrs.Visible = true;

                                    Mensaje("Usuario no encontrado.");
                                }
                                else
                                {
                                    gv_usrs.DataSource = i_f_bff;
                                    gv_usrs.DataBind();
                                    gv_usrs.Visible = true;
                                    gv_usrs.Visible = true;
                                }
                            }
                        }
                    }
                    catch
                    {
                        //limp_usr_ctrl();
                        //div_prospecto.Visible = false;
                        Mensaje("Usuario no encontrado.");
                    }
                }
            }
        }

        private void actualiza_usuario(Guid usrf_ID, int int_ddl, int i_area, int i_perfil, int i_genero, DateTime d_nacimiento, string i_f_b, string dd_clave, string dd_nombres, string dd_apaterno, string dd_amaterno, string i_emal_p, string i_emal_c)
        {
            foreach (GridViewRow row in gv_usrs.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList dl = (DropDownList)row.FindControl("ddl_usr_estatus");

                    int_ddl = int.Parse(dl.SelectedValue);
                }
            }

            using (var m_usrs = new db_hecsaEntities())
            {
                var i_f_bs_rh = (from c in m_usrs.inf_usr_rh
                                 where c.usuario_ID == usrf_ID
                                 select c).FirstOrDefault();

                i_f_bs_rh.area_ID = i_area;
                i_f_bs_rh.perfil_ID = i_perfil;

                m_usrs.SaveChanges();

                var i_f_bs_personales = (from c in m_usrs.inf_usr_personales
                                         where c.usuario_ID == usrf_ID
                                         select c).FirstOrDefault();

                i_f_bs_personales.nombres = dd_nombres;
                i_f_bs_personales.apaterno = dd_apaterno;
                i_f_bs_personales.amaterno = dd_amaterno;
                i_f_bs_personales.genero_ID = i_genero;
                i_f_bs_personales.cumple = d_nacimiento;

                m_usrs.SaveChanges();

                var i_f_bs = (from c in m_usrs.inf_usuario
                              where c.usuario_ID == usrf_ID
                              select c).FirstOrDefault();

                i_f_bs.est_usr_ID = int_ddl;
                i_f_bs.usr = i_f_b;
                i_f_bs.clave = dd_clave;
                i_f_bs.correo_corp = i_emal_c;
                m_usrs.SaveChanges();
            }
        }

        private void agrega_usuario(Guid usrf_ID, int int_ddl, int i_area, int i_perfil, int i_genero, DateTime d_nacimiento, string i_f_b, object dd_clave, string dd_nombres, string dd_apaterno, string dd_amaterno, string i_emal_p, string i_emal_c)
        {
            Guid guid_usr = Guid.NewGuid();
            string cod_usr;

            using (db_hecsaEntities m_usr = new db_hecsaEntities())
            {
                var i_f_b_v = (from i_u in m_usr.inf_usuario
                               where i_u.usr == i_f_b
                               select i_u).ToList();
                usr_ID = Guid.Empty;
                empf_ID = Guid.Empty;
                usr_ID = (Guid)(Session["ss_idusr"]);
                empf_ID = (Guid)(Session["ss_idcnt"]);
                if (i_f_b_v.Count == 0)
                {
                    var i_f_b_c = (from i_u in m_usr.inf_usuario
                                   select i_u).ToList();

                    cod_usr = "USR" + string.Format("{0:0000}", (object)(i_f_b_c.Count + 1));
                    var dn_usr = new inf_usuario
                    {
                        centro_ID = empf_ID,
                        usr = i_f_b,
                        clave = dd_clave.ToString(),
                        correo_corp = i_emal_c,
                        usuario_ID = guid_usr,
                        est_usr_ID = 1,
                        cod_usr = cod_usr,
                        registro = DateTime.Now
                    };

                    var d_usr_p = new inf_usr_personales
                    {
                        usr_personales_ID = Guid.NewGuid(),
                        nombres = dd_nombres,
                        apaterno = dd_apaterno,
                        amaterno = dd_amaterno,
                        genero_ID = i_genero,
                        cumple = d_nacimiento,
                        usuario_ID = guid_usr,
                        registro = DateTime.Now
                    };

                    var d_usr_rh = new inf_usr_rh
                    {
                        usr_rh_ID = Guid.NewGuid(),
                        fecha_ingreso = DateTime.Now,
                        area_ID = i_area,
                        perfil_ID = i_perfil,
                        usuario_ID = guid_usr,
                        registro = DateTime.Now
                    };

                    var d_usr_cont = new inf_usr_contacto
                    {
                        usr_contacto_ID = Guid.NewGuid(),
                        usuario_ID = guid_usr,
                        correo = i_emal_p,
                        registro = DateTime.Now
                    };

                    var d_corp_ctrl = new inf_centro_ctrl
                    {
                        corporativo_ctrl_ID = Guid.NewGuid(),
                        centro_ID = empf_ID,
                        usuario_ID = guid_usr
                    };

                    m_usr.inf_centro_ctrl.Add(d_corp_ctrl);
                    m_usr.inf_usuario.Add(dn_usr);
                    m_usr.inf_usr_personales.Add(d_usr_p);
                    m_usr.inf_usr_rh.Add(d_usr_rh);
                    m_usr.inf_usr_contacto.Add(d_usr_cont);
                    m_usr.SaveChanges();

                    ctrl_usr();
                    Mensaje("Datos guardados con éxito");
                }
                else
                {
                    div_i_usr.Visible = false;
                    ctrl_usr();
                    Mensaje("Usuario ya existe, favor de re-intentar.");
                }
            }
        }

        private void ctrl_usr()
        {
            str_pnlID = "pnl_usr";

            int[] f_usr_a = null;
            int[] f_usr_p = null;

            s_area.Items.Clear();
            s_perfil.Items.Clear();
            s_genero.Items.Clear();

            if (int_idperf == 2 || int_idperf == 3)
            {
                f_usr_a = new int[] { 3, 4 };
                f_usr_p = new int[] { 3, 4, 5, 6, 7, 8, 9, 10 };
            }
            else if (int_idperf == 4)
            {
                f_usr_a = new int[] { 4 };
                f_usr_p = new int[] { 5, 6, 7, 8, 9, 10 };
            }
            else if (int_idperf == 5)
            {
                f_usr_a = new int[] { 4 };
                f_usr_p = new int[] { 4, 5, 6 };
            }

            using (db_hecsaEntities m_ss = new db_hecsaEntities())
            {
                i_nombres.Value = string.Empty;
                i_apaterno.Value = string.Empty;
                i_amaterno.Value = string.Empty;
                i_nacimiento.Value = string.Empty;
                i_emal_p.Value = string.Empty;
                i_usr.Value = string.Empty;
                i_emal_c.Value = string.Empty;

                s_area.Items.Clear();
                s_perfil.Items.Clear();
                s_genero.Items.Clear();

                var i_ma = (from c in m_ss.fact_area
                            where f_usr_a.Contains(c.area_ID)
                            select c).ToList();

                s_area.DataSource = i_ma;
                s_area.DataTextField = "area_desc";
                s_area.DataValueField = "area_ID";
                s_area.DataBind();

                s_area.Items.Insert(0, new ListItem("Área", string.Empty));

                var i_mp = (from c in m_ss.fact_perfil
                            where f_usr_p.Contains(c.perfil_ID)
                            select c).ToList();

                s_perfil.DataSource = i_mp;
                s_perfil.DataTextField = "perfil_desc";
                s_perfil.DataValueField = "perfil_ID";
                s_perfil.DataBind();

                s_perfil.Items.Insert(0, new ListItem("Perfil", string.Empty));

                var i_mg = (from c in m_ss.fact_genero
                            select c).ToList();

                s_genero.DataSource = i_mg;
                s_genero.DataTextField = "genero_desc";
                s_genero.DataValueField = "genero_ID";
                s_genero.DataBind();

                s_genero.Items.Insert(0, new ListItem("Género", string.Empty));
            }
        }

        #endregion ctrl_usuario

        #region configuración de envío

        protected void btn_ee_Click(object sender, EventArgs e)
        {
            string i_email = Request.Form["i_email"];
            string i_email_usr = Request.Form["i_email_usr"];
            string i_email_clave = Request.Form["i_email_clave"];
            string i_asunto = Request.Form["i_asunto"];
            string i_smtp = Request.Form["i_smtp"];
            int i_puerto = int.Parse(Request.Form["i_puerto"]);

            TextInfo t_asunto = new CultureInfo("es-MX", false).TextInfo;
            using (var m_inv = new db_hecsaEntities())
            {
                var i_inv = (from c in m_inv.inf_email_env
                             select c).FirstOrDefault();

                i_inv.email = i_email;
                i_inv.usuario = i_email_usr;
                i_inv.clave = i_email_clave;
                i_inv.asunto = t_asunto.ToTitleCase(i_asunto);
                i_inv.servidor_smtp = i_smtp;
                i_inv.puerto = i_puerto;

                m_inv.SaveChanges();
            }
            ctrl_config();
            Mensaje("Datos actualizados con éxito");
        }

        private void ctrl_config()
        {
            using (db_hecsaEntities m_comp = new db_hecsaEntities())
            {
                var i_comp = (from i_i in m_comp.inf_email_env
                              select new
                              {
                                  i_i.email,
                                  i_i.usuario,
                                  i_i.clave,
                                  i_i.asunto,
                                  i_i.servidor_smtp,
                                  i_i.puerto,
                              }).First();

                i_email.Value = i_comp.email;
                i_email_usr.Value = i_comp.usuario;
                i_email_clave.Value = i_comp.clave;
                i_asunto.Value = i_comp.asunto;
                i_smtp.Value = i_comp.servidor_smtp;
                i_puerto.Value = i_comp.puerto.ToString();
            }
        }

        #endregion configuración de envío

        #region rpt_ventas
      
        #endregion
    }
}