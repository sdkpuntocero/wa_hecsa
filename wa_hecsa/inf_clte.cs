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
    
    public partial class inf_clte
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public inf_clte()
        {
            this.inf_clte_contacto = new HashSet<inf_clte_contacto>();
        }
    
        public System.Guid clte_ID { get; set; }
        public Nullable<int> est_clte_ID { get; set; }
        public string cod_clte { get; set; }
        public string nombres { get; set; }
        public string apaterno { get; set; }
        public string amaterno { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string callenum { get; set; }
        public string d_codigo { get; set; }
        public string id_asenta_cpcons { get; set; }
        public string comentarios { get; set; }
        public Nullable<System.DateTime> registro { get; set; }
        public System.Guid centro_ID { get; set; }
        public System.Guid usuario_ID { get; set; }
    
        public virtual fact_est_clte fact_est_clte { get; set; }
        public virtual inf_centro inf_centro { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inf_clte_contacto> inf_clte_contacto { get; set; }
    }
}
