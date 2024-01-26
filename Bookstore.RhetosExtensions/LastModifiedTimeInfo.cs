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
    [ConceptKeyword("LastModifiedTime")]
    public class LastModifiedTimeInfo : IConceptInfo
    {
        [ConceptKey]
        public DateTimePropertyInfo ModificationDateTime { get; set; }
    
    }
}
