//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace wa_hecsa
{
    using System;
    using System.Collections.Generic;
    
    public partial class inf_usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public inf_usuario()
        {
            this.inf_centro_ctrl = new HashSet<inf_centro_ctrl>();
            this.inf_usr_bancario = new HashSet<inf_usr_bancario>();
            this.inf_usr_contacto = new HashSet<inf_usr_contacto>();
            this.inf_usr_escolar = new HashSet<inf_usr_escolar>();
            this.inf_usr_medicos = new HashSet<inf_usr_medicos>();
            this.inf_usr_personales = new HashSet<inf_usr_personales>();
            this.inf_usr_rh = new HashSet<inf_usr_rh>();
        }
    
        public System.Guid usuario_ID { get; set; }
        public Nullable<int> est_usr_ID { get; set; }
        public string cod_usr { get; set; }
        public string usr { get; set; }
        public string clave { get; set; }
        public string correo_corp { get; set; }
        public System.Guid centro_ID { get; set; }
        public Nullable<System.DateTime> registro { get; set; }
    
        public virtual fact_est_usr fact_est_usr { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inf_centro_ctrl> inf_centro_ctrl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inf_usr_bancario> inf_usr_bancario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inf_usr_contacto> inf_usr_contacto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inf_usr_escolar> inf_usr_escolar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inf_usr_medicos> inf_usr_medicos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inf_usr_personales> inf_usr_personales { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inf_usr_rh> inf_usr_rh { get; set; }
    }
}
