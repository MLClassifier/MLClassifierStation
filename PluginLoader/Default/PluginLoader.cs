using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Threading.Tasks;

namespace PluginLoader.Default
{
    public class PluginLoader<T> : IPluginLoader<T>
    {
        private CompositionContainer container;

        [ImportMany]
        public IEnumerable<T> Plugins { get; private set; }

        public void LoadPlugins(string path)
        {
            try
            {
                DirectoryCatalog catalog = new DirectoryCatalog(path);
                AggregateCatalog agrCatalog = new AggregateCatalog(catalog);
                container = new CompositionContainer(agrCatalog);
                container.ComposeParts(this);
            }
            catch (CompositionException exc)
            {
            }
        }

        public Task<IEnumerable<T>> LoadPluginsAsync(string path)
        {
            TaskCompletionSource<IEnumerable<T>> tcs = new TaskCompletionSource<IEnumerable<T>>();
            Task.Run(() =>
            {
                LoadPlugins(path);
                tcs.TrySetResult(Plugins);
            });

            return tcs.Task;
        }

        public void Dispose()
        {
            if (container != null)
                container.Dispose();
        }
    }
}