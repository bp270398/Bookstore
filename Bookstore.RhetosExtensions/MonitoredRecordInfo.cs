using Rhetos.Dsl;
using Rhetos.Dsl.DefaultConcepts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.RhetosExtensions
{
    [Export(typeof(IConceptInfo))]
    [ConceptKeyword("MonitoredRecord")]
    public class MonitoredRecordInfo : EntityInfo
    {
        //  DateTime CreatedAt { CreationTime; DenyUserEdit; }
        //  Logging { AllProperties; }

        public DateTimePropertyInfo DateTimePropertyInfo { get; set; }
        public DenyUserEditPropertyInfo DenyUserEditPropertyInfo { get; set; }
        public CreationTimeInfo CreationTimeInfo { get; set; }
        public EntityLoggingInfo EntityLoggingInfo { get; set; }
        public AllPropertiesLoggingInfo AllPropertiesLoggingInfo { get; set; }

    }
}
