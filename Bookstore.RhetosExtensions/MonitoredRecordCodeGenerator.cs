using Rhetos.Compiler;
using Rhetos.Dom.DefaultConcepts;
using Rhetos.Dsl;
using Rhetos.Extensibility;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.RhetosExtensions
{
    [Export(typeof(IConceptCodeGenerator))]
    [ExportMetadata(MefProvider.Implements, typeof(MonitoredRecordInfo))]
    public class MonitoredRecordCodeGenerator : IConceptCodeGenerator
    {
        public void GenerateCode(IConceptInfo conceptInfo, ICodeBuilder codeBuilder)
        {

            var info = conceptInfo as MonitoredRecordInfo;

            var code = $@"if (checkUserPermissions) 
            {{
                foreach (var item in insertedNew)
                    if (item.CreatedAt == null)
                        item.CreatedAt = DateTime.Now;

                updatedNew = updatedNew.Concat(deleted).ToArray();
                updated = updated.Concat(deleted).ToArray();
}}
            ";
            

        }
    }
}
