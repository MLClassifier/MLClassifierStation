using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PluginLoader
{
    public interface IPluginLoader<T> : IDisposable
    {
        IEnumerable<T> Plugins { get; }

        void LoadPlugins(string path);

        Task<IEnumerable<T>> LoadPluginsAsync(string path);
    }
}