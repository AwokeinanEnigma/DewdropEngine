using Dewdrop.Utilities.fNbt.Tags;

namespace Dewdrop.Utilities.fNbt.Serialization; 

public interface INbtSerializable
{
    NbtTag Serialize(string tagName);
    void Deserialize(NbtTag value);
}