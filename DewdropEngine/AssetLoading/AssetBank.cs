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
            public AssetLoadException(string name, string type) : base($"\"{name}\" was not found in {type}'s asset bank")
            {
            }
        }

        private Dictionary<string, T> _assets;

        public AssetBank(string directory, SearchOption option)
        {
            _assets = new Dictionary<string, T>();
            LoadAssets(directory, option);
        }

        /// <summary>
        /// Load all assets of type in the specified directory
        /// </summary>
        /// <param name="type"></param>
        /// <param name="directory"></param>
        /// <param name="searchOption"></param>
        private void LoadAssets(string directory, SearchOption searchOption = SearchOption.AllDirectories)
        {
            Logger.LogDebug($"Loading from: {directory}");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if (!Directory.Exists(Game1.instance.ContentManager.RootDirectory + "\\" + directory))
            {
                Logger.LogError($"Directory {directory} doesn't exist. Did you build the pipeline?", new DirectoryNotFoundException($"Invalid directory {directory}"));
            }

            string[] files = Directory.GetFiles(Game1.instance.ContentManager.RootDirectory + "\\" + directory, "*", searchOption);

            foreach (string file in files)
            {
                string ext = Path.GetExtension(file);
                string path = file.Substring(file.IndexOf('\\') + 1);
                path = path.Substring(0, path.Length - ext.Length);

                // we just need to remove the directory name plus the slash
                // hence the +1
                _assets.Add(path.Substring(directory.Length + 1), Game1.instance.ContentManager.Load<T>(path));
            }

            stopwatch.Stop();
            Logger.LogInfo($"Assets loaded: {_assets.Count} <=> Time: {stopwatch.ElapsedMilliseconds}ms");
        }

        public T GetAssetByName(string name)
        {
            T asset = default;
            if (!_assets.TryGetValue(name, out asset))
            {
                Logger.LogError($"Couldn't load asset '{name}'", new AssetLoadException("name", asset.ToString()));
            }
            return asset;
        }
    }
}
