namespace ECUMasterEditor.Core.Binary;

public class EcuBinary
{
    public byte[] Data { get; }

    public int Size => Data.Length;

    public EcuBinary(byte[] data)
    {
        Data = data;
    }
}