namespace ECUMasterEditor.Core.Binary;

public static class EcuFileLoader
{
    public static EcuBinary Load(string filePath)
    {
        string extension = Path.GetExtension(filePath)
            .ToLowerInvariant();

        return extension switch
        {
            ".bin" => BinReader.Load(filePath),

            _ => throw new NotSupportedException(
                $"Unsupported ECU file type: {extension}")
        };
    }
}