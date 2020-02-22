namespace Fuse.Plugin
{
    public interface IPlugin
    {
        void OnEnable(IPluginCollection plugins);
        void OnDisable(IPluginCollection plugins);
    }
}
