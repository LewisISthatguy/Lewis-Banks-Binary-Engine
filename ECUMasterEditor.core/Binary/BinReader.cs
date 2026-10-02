namespace ECUMasterEditor.Core.Binary;

public static class BinReader
{
    public static EcuBinary Load(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException(
                "BIN file not found.",
                filePath);

        byte[] data = File.ReadAllBytes(filePath);

        if (data.Length == 0)
            throw new InvalidDataException(
                "The BIN file is empty.");

        return new EcuBinary(data);
    }
}