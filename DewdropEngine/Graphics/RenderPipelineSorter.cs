using System.Collections.Generic;

namespace Dewdrop.Graphics
{
    /// <summary>
    /// Sorts and renders render pipelines according to their depths.
    /// </summary>
    public class RenderPipelineSorter
    {
        private readonly List<RenderPipeline> _renderPipelines;
        private readonly Dictionary<RenderPipeline, int> _pipelineIds;

        private readonly PipelineComparer _comparer;

        private int pipelineCount;
        private bool _sort;
        private class PipelineComparer : IComparer<RenderPipeline>
        {
            //used for uids
            private readonly RenderPipelineSorter pipeline;

            /// <summary>
            /// Creates a new renderable comparer using a renderpipeline
            /// </summary>
            /// <param name="pipeline">The render pipeline to use</param>
            public PipelineComparer(RenderPipelineSorter pipeline)
            {
                this.pipeline = pipeline;
            }

            // i'll break this down
            //
            // if our depths aren't equal to each other
            // then subtract their depths to see which one is greater
            //
            // but if they are not
            // then subtract their ids to see which one is greater.
            public int Compare(RenderPipeline a, RenderPipeline b)
            {
                return a.PipelineDepth != b.PipelineDepth ? a.PipelineDepth - b.PipelineDepth : this.pipeline._pipelineIds[b] - this.pipeline._pipelineIds[a];
            }
        }

        /// <summary>
        /// Creates a new RenderPipelineSorter.
        /// </summary>
        public RenderPipelineSorter()
        {
            _renderPipelines = new List<RenderPipeline>();
            _pipelineIds = new Dictionary<RenderPipeline, int>();
            _comparer = new PipelineComparer(this);
        }

        /// <summary>
        /// Adds a RenderPipeline to the sorter.
        /// </summary>
        /// <param name="pipeline">The RenderPipeline to add.</param>
        public void Add(RenderPipeline pipeline)
        {
            _renderPipelines.Add(pipeline);
            _pipelineIds.Add(pipeline, pipelineCount);
            ++pipelineCount;

            _sort = true;
        }

        /// <summary>
        /// Removes a RenderPipeline from the sorter.
        /// </summary>
        /// <param name="pipeline">The RenderPipeline to remove.</param>
        public void Remove(RenderPipeline pipeline)
        {
            _renderPipelines.Remove(pipeline);
            _pipelineIds.Remove(pipeline);
        }

        /// <summary>
        /// Forces a sort of the RenderPipelines in the sorter.
        /// </summary>
        public void ForceSort()
        {
            _sort = true;
        }

        /// <summary>
        /// Draws all RenderPipelines in the sorter.
        /// </summary>
        public void Render()
        {
            if (this._sort)
            {
                this._renderPipelines.Sort(_comparer);
                this._sort = false;
            }


            int count = this._pipelineIds.Count;
            for (int index = 0; index < count; ++index)
            {
                // get renderable at index
                RenderPipeline pipeline = this._renderPipelines[index];
                pipeline.Draw();

            }

        }
    }
}
