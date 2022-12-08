using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(OMFSSectionSrcModule))]
    internal class ModelOMFSSectionSrcModule : ModelOMFSSection<OMFSSectionSrcModule>
        {
        public ModelOMFSSectionSrcModule(OMFSSectionSrcModule source)
            : base(source)
            {
            Task.Factory.StartNew(()=>{
                var builder = new StringBuilder();
                using (var writer = XmlWriter.Create(new StringWriter(builder), new XmlWriterSettings
                    {
                    Indent = true
                    }))
                    {
                    source.WriteXml(writer);
                    }
                return;
                });
            }
        }
    }