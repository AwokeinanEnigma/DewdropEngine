using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewdrop.Graphics
{
    /// <summary>
    /// Sorts render pipelines
    /// </summary>
    public class RenderPipelineSorter
    {
        private List<RenderPipeline> _renderPipelines;
        private Dictionary<RenderPipeline, int> _pipelineIds;

        private PipelineComparer _comparer;

        private int pipelineCount;
        private bool _sort;
        private class PipelineComparer : IComparer<RenderPipeline>
        {
            //used for uids
            private RenderPipelineSorter pipeline;

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
        public RenderPipelineSorter()
        {
            _renderPipelines = new List<RenderPipeline>();
            _pipelineIds = new Dictionary<RenderPipeline, int>();
            _comparer = new PipelineComparer(this);
        }


        public void Add(RenderPipeline pipeline)
        {
            _renderPipelines.Add(pipeline);
            _pipelineIds.Add(pipeline, pipelineCount);
            ++pipelineCount;

            _sort = true;
        }

        public void Remove(RenderPipeline pipeline)
        {
            _renderPipelines.Remove(pipeline);
            _pipelineIds.Remove(pipeline); 
        }

        public void ForceSort() {
            _sort = true;
        }

        public void Render() {
            if (this._sort)
            {
                this._renderPipelines.Sort(_comparer);
                this._sort = false;
            }
            //View view = this.target.GetView();

            //this.viewRect.Left = view.Center.X - view.Size.X / 2f;
            //this.viewRect.Top = view.Center.Y - view.Size.Y / 2f;
            //this.viewRect.Width = view.Size.X;
            //this.viewRect.Height = view.Size.Y;

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
