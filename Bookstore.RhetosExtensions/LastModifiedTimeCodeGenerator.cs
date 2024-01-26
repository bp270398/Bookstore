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
    [ExportMetadata(MefProvider.Implements, typeof(LastModifiedTimeInfo))]
    public class LastModifiedTimeCodeGenerator : IConceptCodeGenerator
    {
        public void GenerateCode(IConceptInfo conceptInfo, ICodeBuilder codeBuilder)
        {
            var info = (LastModifiedTimeInfo)conceptInfo;

            string snippet =
            $@"{{ 
                var now = DateTime.Now;

                foreach (var newItem in updatedNew)
                    if(newItem.{info.ModificationDateTime.Name} == null)
                        newItem.{info.ModificationDateTime.Name} = now;
            }}
            ";

            codeBuilder.InsertCode(snippet, WritableOrmDataStructureCodeGenerator.InitializationTag, info.ModificationDateTime.DataStructure);
        }
    }
}
