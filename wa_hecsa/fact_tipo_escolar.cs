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
    
    public partial class fact_tipo_escolar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public fact_tipo_escolar()
        {
            this.inf_usr_escolar = new HashSet<inf_usr_escolar>();
        }
    
        public int tipo_escolar_ID { get; set; }
        public string tipo_escolar_desc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inf_usr_escolar> inf_usr_escolar { get; set; }
    }
}
