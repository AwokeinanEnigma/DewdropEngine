using fNbt;
using fNbt.Tags;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DewdropEngine.Graphics
{
    public static class NBTImageLoader
    {
        /// <summary>
        /// Creates an IndexedTexture from an NBTCompound
        /// </summary>
        /// <param name="root">The NBTCompound to load an IndexedTexture from</param>
        /// <returns></returns>
        public static IndexedTexture2D LoadFromNbtTag(NbtCompound root)
        {
            // this code is all dave/carbine code therefore i wil not look at it!
            NbtTag paletteTag = root.Get("pal");
            IEnumerable<NbtTag> palettes = (paletteTag is NbtList) ? ((NbtList)paletteTag) : ((NbtCompound)paletteTag).Tags;

            uint intValue = (uint)root.Get<NbtInt>("w").IntValue;
            byte[] byteArrayValue = root.Get<NbtByteArray>("img").ByteArrayValue;
            List<int[]> list = new List<int[]>();
            foreach (NbtTag palette in palettes)
            {
                if (palette.TagType == NbtTagType.IntArray)
                {
                    list.Add(((NbtIntArray)palette).IntArrayValue);
                }
            }
            SpriteDefinition spriteDefinition = null;
            Dictionary<int, SpriteDefinition> spriteDefinitions = new Dictionary<int, SpriteDefinition>();

            NbtCompound allSprites = root.Get<NbtCompound>("spr");
            if (allSprites != null)
            {
                foreach (NbtTag potentialSprite in allSprites.Tags)
                {
                    if (potentialSprite is NbtCompound)
                    {
                        NbtCompound spriteCompound = (NbtCompound)potentialSprite;
                        string text = spriteCompound.Name.ToLowerInvariant();

                        NbtIntArray dummyIntArray;
                        int[] coordinatesArray = spriteCompound.TryGet<NbtIntArray>("crd", out dummyIntArray) ? dummyIntArray.IntArrayValue : new int[2];
                        int[] boundsArray = spriteCompound.TryGet<NbtIntArray>("bnd", out dummyIntArray) ? dummyIntArray.IntArrayValue : new int[2];
                        int[] originArray = spriteCompound.TryGet<NbtIntArray>("org", out dummyIntArray) ? dummyIntArray.IntArrayValue : new int[2];

                        byte[] optionsArray = spriteCompound.TryGet<NbtByteArray>("opt", out NbtByteArray nbtByteArray) ? nbtByteArray.ByteArrayValue : new byte[3];

                        IList<NbtTag> speedSet = spriteCompound.Get<NbtList>("spd");
                        int frames = spriteCompound.TryGet<NbtInt>("frm", out NbtInt nbtInt) ? nbtInt.IntValue : 1;

                        // this is only found on tilesets put through VEMC
                        NbtIntArray dataArray = spriteCompound.Get<NbtIntArray>("d");
                        int[] data = (dataArray == null) ? null : dataArray.IntArrayValue;

                        Vector2 coords = new Vector2(coordinatesArray[0], coordinatesArray[1]);
                        Vector2 bounds = new Vector2(boundsArray[0], boundsArray[1]);
                        Vector2 origin = new Vector2(originArray[0], originArray[1]);

                        // options are encoded as arrays
                        // you can guess the values from here but i'll elaborate

                        // 0 - flip the sprite horizontally
                        // 1 - flip the sprite vertically 
                        // 2 - animation mode

                        bool flipX = optionsArray[0] == 1;
                        bool flipY = optionsArray[1] == 1;
                        int mode = optionsArray[2];

                        float[] speeds = (speedSet != null) ? new float[speedSet.Count] : new float[0];
                        for (int i = 0; i < speeds.Length; i++)
                        {
                            NbtTag speedValue = speedSet[i];
                            speeds[i] = speedValue.FloatValue;
                        }
                        SpriteDefinition newSpriteDefinition = new SpriteDefinition(text, coords, bounds, origin, frames, speeds, flipX, flipY, mode, data);
                        if (spriteDefinition == null)
                        {
                            spriteDefinition = newSpriteDefinition;
                        }

                        int key = text.GetHashCode();
                        spriteDefinitions.Add(key, newSpriteDefinition);
                    }
                }
            }
            return new IndexedTexture2D(intValue, list.ToArray(), byteArrayValue, spriteDefinitions, spriteDefinition);
        }
    }
}
