using Dewdrop.Debugging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Dewdrop.AssetLoading
{
    /// <summary>
    /// An asset bank is a container that stores and manages assets of a specific type, T. It provides a convenient way to access and retrieve the assets later.
    /// </summary>
    /// <typeparam name="T">The type of the assets that are stored in this asset bank.</typeparam>
    public class AssetBank<T>
    {
        /// <summary>
        /// Represents an exception that is thrown when an asset is not found in an asset bank.
        /// </summary>
        internal class AssetLoadException : Exception
        {
            /// <summary>
            /// Initializes a new instance of the AssetLoadException class with a specified error message that includes the asset name and type.
            /// </summary>
            /// <param name="name">The name of the asset that could not be found.</param>
            /// <param name="type">The name of the type of the asset that could not be found.</param>
            public AssetLoadException(string name, string type) : base($"\"{name}\" was not found in the {type} asset bank.")
            {
            }
        }

        private Dictionary<string, T> _assets;

        /// <summary>
        /// Creates a new asset bank. The asset bank loads assets of type T from the specified directory and its subdirectories (if specified)
        /// </summary>
        /// <param name="directory">The directory to load assets from. </param>
        /// <param name="searchOption">Determines if the asset bank will load assets from subdirectories. It is set to SearchOption.AllDirectories by default</param>
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

        /// <summary>
        /// Retrieves an asset of type T by its name. 
        /// </summary>
        /// <param name="name">The name of the asset to be retrieved.</param>
        /// <returns>The asset of type T, if found.</returns>
        /// <exception cref="AssetLoadException">Thrown when the asset with the provided name is not found. The exception contains the asset name and type as arguments.</exception>
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
