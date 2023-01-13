using Microsoft.Xna.Framework.Graphics;

namespace Dewdrop.DewGui.Data;

/// <summary>
///     Contains information regarding the index buffer used by the GUIRenderer.
/// </summary>
public class IndexData
{
    public IndexBuffer Buffer;
    public int BufferSize;
    public byte[] Data;
}