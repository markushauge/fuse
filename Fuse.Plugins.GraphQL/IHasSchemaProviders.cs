using System.Collections.Generic;

namespace Fuse.Plugins.GraphQL
{
    public interface IHasSchemaProviders
    {
        IEnumerable<ISchemaProvider> SchemaProviders { get; }
    }
}