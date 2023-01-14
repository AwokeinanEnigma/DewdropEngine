using Dewdrop.Debugging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Dewdrop.AssetLoading
{
    public class AssetBank<T>
    {
        internal class AssetLoadException : Exception
        {
            public AssetLoadException(string name, string type) : base($"\"{name}\" was not found in the {type} asset bank.")
            {
            }
        }

        private Dictionary<string, T> _assets;

        /// <summary>
        /// Creates a new asset bank.
        /// </summary>
        /// <param name="directory">The directory to load assets from.</param>
        /// <param name="option">Determines if the asset bank will load assets from subdirectories.</param>
        public AssetBank(string directory, SearchOption searchOption = SearchOption.AllDirectories)
        {
            _assets = new Dictionary<string, T>();
            LoadAssets(directory, searchOption);
        }

        private void LoadAssets(string directory, SearchOption searchOption)
        {
            DBG.LogDebug($"Loading {typeof(T).Name} assets.");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if (!Directory.Exists(Engine.instance.ContentManager.RootDirectory + "\\" + directory))
            {
                DBG.LogError($"Directory {directory} doesn't exist. Did you build the pipeline?", new DirectoryNotFoundException($"Invalid directory {directory}"));
            }

            string[] files = Directory.GetFiles(Engine.instance.ContentManager.RootDirectory + "\\" + directory, "*", searchOption);

            foreach (string file in files)
            {
                string ext = Path.GetExtension(file);
                string path = file.Substring(file.IndexOf('\\') + 1);
                path = path.Substring(0, path.Length - ext.Length);

                // we just need to remove the directory name plus the slash
                // hence the +1
                _assets.Add(path.Substring(directory.Length + 1), Engine.instance.ContentManager.Load<T>(path));
            }

            stopwatch.Stop();
            DBG.LogInfo($"Assets loaded: {_assets.Count} <=> Time: {stopwatch.ElapsedMilliseconds}ms");
        }

        public T GetAssetByName(string name)
        {
            T asset = default;
            if (!_assets.TryGetValue(name, out asset))
            {
                DBG.LogError($"Couldn't load '{name}'", new AssetLoadException(name, typeof(T).Name));
            }
            return asset;
        }
    }
}
