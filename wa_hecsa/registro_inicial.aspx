<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="registro_inicial.aspx.cs" Inherits="wa_hecsa.registro_inicial" %>

<!DOCTYPE html>

<html lang="es-MX">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/fontawesome-free-5.7.0-web/css/all.min.css" rel="stylesheet" />

    <title></title>
    <script src="Scripts/jquery-3.4.1.min.js"></script>
    <script src="Scripts/popper.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
</head>
<body>
    <form id="frm" runat="server">
        <asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>
        <div class="container">
            <asp:UpdatePanel ID="up_usr_banc" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <div class="py-3">
                        <div class="container">

                            <div class="row">
                                <div class="col-md-5 p-4">
                                    <h3>Punto Cero</h3>
                                    <p>Innovación en Informática</p>
                                    <p class="lead mt-3"><b>Soporte</b></p>
                                    <p><a href="tel:5525178930">+52 1 55.25.17.89.30</a> </p>
                                    <p><a href="mailto:soporte@puntocero.tech">soporte@puntocero.tech</a></p>

                                    <p class="lead mt-3"><b>Informes</b></p>
                                    <p><a href="http://www.puntocero.tech/">www.puntocero.tech</a></p>
                                </div>
                                <div class="col-md-7 p-4">

                                    <h4 class="mb-3 text-right">Registro de Compañía</h4>

                                    <div class="row">
                                        <h5 class="mb-3">Datos de Director</h5>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <input type="text" class="form-control" runat="server" id="i_nombres" required="required" placeholder="*Nombre(s)" tabindex="1" />
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <input type="text" class="form-control" runat="server" id="i_apaterno" required="required" placeholder="*Apellido Paterno" tabindex="2" />
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <input type="text" class="form-control" runat="server" id="i_amaterno" required="required" placeholder="*Apellido Materno" tabindex="3" />
                                            </div>
                                        </div>
                                    </div>
                                    <h5>Datos de Inicial</h5>
                                    <div class="row">

                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <input type="text" class="form-control" runat="server" id="i_emp_nom" required="required" placeholder="*Nombre Compañia" tabindex="4" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <input type="email" class="form-control" runat="server" id="i_email" required="required" placeholder="*Email" tabindex="5" />
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <input type="tel" class="form-control" runat="server" id="i_tel" placeholder="Teléfono (Opcional)" tabindex="6" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <input type="text" class="form-control" runat="server" id="i_cnt_callenum" placeholder="*Calle y Número" required="required" tabindex="7" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">

                                        <div class="col-md-6">
                                            <div class="input-group">
                                                <input type="text" class="form-control" runat="server" id="i_cnt_cp" placeholder="*Código Postal" required="required" tabindex="8" />
                                                <span class="input-group-btn">
                                                    <asp:LinkButton ID="btn_i_corp_cp" runat="server" CssClass="btn btn-info form-control" TabIndex="9" OnClick="btn_i_cnt_cp_Click">
                                                                     <i class="fas fa-search"></i>
                                                    </asp:LinkButton>
                                                </span>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <select class="form-control" runat="server" id="scnt_colonia" tabindex="10" required="required">
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <input type="text" class="form-control" runat="server" id="i_cnt_municipio" placeholder="Municipio" disabled="disabled" tabindex="11" />
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <input type="text" class="form-control" runat="server" id="i_cnt_estado" placeholder="Estado" disabled="disabled" tabindex="12" />
                                            </div>
                                        </div>
                                    </div>
                                    <asp:Button CssClass="btn btn-info" ID="btn_guarda" runat="server" Text="Guardar" TabIndex="13" OnClick="btn_guarda_Click" />
                                </div>
                            </div>
                        </div>
                    </div>

                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>

            <div class="modal" id="myModal">
                <div class="modal-dialog" role="document">
                    <asp:UpdatePanel ID="upModal" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="modal-content">
                                <div class="modal-header">
                                    <asp:Label ID="lblModalTitle" CssClass="modal-title" runat="server" Text=""></asp:Label>
                                    <button type="button" class="close" data-dismiss="modal" onclick="window.location.href='/acceso.aspx'"><span>×</span> </button>
                                </div>
                                <div class="modal-body">
                                    <asp:Label ID="lblModalBody" CssClass="login100-form-title" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="modal-footer">

                                    <button type="button" class="btn btn-primary" data-dismiss="modal" onclick="window.location.href='/acceso.aspx'">Aceptar</button>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </form>

</body>
</html>
