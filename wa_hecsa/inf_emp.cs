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
    
    public partial class inf_emp
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public inf_emp()
        {
            this.inf_centro = new HashSet<inf_centro>();
            this.inf_inv = new HashSet<inf_inv>();
        }
    
        public System.Guid empresa_ID { get; set; }
        public Nullable<int> estatus_ID { get; set; }
        public string empresa_nom { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string callenum { get; set; }
        public string d_codigo { get; set; }
        public string id_asenta_cpcons { get; set; }
        public Nullable<System.DateTime> registro { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inf_centro> inf_centro { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inf_inv> inf_inv { get; set; }
    }
}
