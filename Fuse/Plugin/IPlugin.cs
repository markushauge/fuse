namespace Fuse.Plugin
{
    public interface IPlugin
    {
        void OnEnable(IPluginCollection pluginManager);
        void OnDisable(IPluginCollection pluginManager);
    }
}
