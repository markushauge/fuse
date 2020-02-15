using HotChocolate;

namespace Fuse.Plugins.GraphQL
{
    public interface ISchemaProvider
    {
        string Name { get; }
        ISchema Schema { get; }
    }
}
