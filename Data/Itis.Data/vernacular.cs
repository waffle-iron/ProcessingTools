//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProcessingTools.Itis.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class vernacular
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public vernacular()
        {
            this.vern_ref_links = new HashSet<vern_ref_links>();
        }
    
        public int tsn { get; set; }
        public string vernacular_name { get; set; }
        public string language { get; set; }
        public string approved_ind { get; set; }
        public System.DateTime update_date { get; set; }
        public int vern_id { get; set; }
    
        public virtual taxonomic_units taxonomic_units { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<vern_ref_links> vern_ref_links { get; set; }
    }
}