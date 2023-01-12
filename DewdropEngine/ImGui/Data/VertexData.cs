﻿using Microsoft.Xna.Framework.Graphics;

namespace Dewdrop.DewGui.Data; 

/// <summary>
///     Contains information regarding the vertex buffer used by the GUIRenderer.
/// </summary>
public class VertexData {
    public VertexBuffer Buffer;
    public int BufferSize;
    public byte[] Data;
}