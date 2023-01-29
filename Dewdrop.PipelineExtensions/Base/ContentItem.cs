using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;

namespace Dewdrop.PipelineExtensions.Base
{
    /// <summary>
    /// Provides typed raw data for a game asset.
    /// </summary>
    /// <typeparam name="T">The type of asset data described by the content.</typeparam>
    public abstract class ContentItem<T> : ContentItem, IContentItem
    {
        private readonly Dictionary<string, ContentItem> _references = new();

        /// <summary>
            /// Initializes a new instance of the <see cref="ContentItem{T}"/> class.
            /// </summary>
            /// <param name="asset">The configuration data for the game asset.</param>
        protected ContentItem(T asset)
    => Asset = asset;

        /// <summary>
            /// Gets the configuration data for the game asset.
            /// </summary>
        public T Asset
        { get; }

        /// <inheritdoc/>
        public void AddReference<TContent>(ContentProcessorContext context,
    string filename,
    OpaqueDataDictionary processorParameters)
        {

            var sourceAsset = new ExternalReference<TContent>(filename);

            var reference =
            context.BuildAsset<TContent, TContent>(sourceAsset,
            string.Empty,
            processorParameters,
            string.Empty,
            string.Empty);
            _references.Add(filename, reference);
        }

        /// <inheritdoc/>
        public ExternalReference<TContent> GetReference<TContent>(string filename)
        {
            if (!_references.TryGetValue(filename, out ContentItem contentItem))
            {
                throw new ArgumentException(filename);

            }

            return (ExternalReference<TContent>)contentItem;
        }
    }
}
