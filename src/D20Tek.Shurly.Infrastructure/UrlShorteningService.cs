//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Application.Abstractions;

namespace D20Tek.Shurly.Infrastructure;

internal class UrlShorteningService : IUrlShorteningService
{
    public const int NumCharsInCode = 8;   // Adjust the buffer size to 8 characters
    private const string EncodingAlphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const int _guidByteCount = 10;
    private readonly IShortenedUrlRepository _repository;

    public UrlShorteningService(IShortenedUrlRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> GenerateUniqueCodeAsync(Guid? guidSeed = null)
    {
        var guid = guidSeed ?? Guid.NewGuid();
        var ctr = 10;

        while (ctr > 0)
        {
            var number = ConvertToNumber(guid);
            var code = EncodeBase62(number);

            // ensure the generate code isn't already in use
            if (await _repository.IsUrlCodeUniqueAsync(code))
            {
                return code;
            }

            guid = Guid.NewGuid();
            ctr--;
        }

        throw new InvalidOperationException(
            "Unable to generate a unique short url code.");
    }

    private ulong ConvertToNumber(Guid guid)
    {
        byte[] guidBytes = guid.ToByteArray();

        // take a subset of bytes (adjust as needed to get unique results)
        byte[] subsetBytes = new byte[_guidByteCount];
        Array.Copy(guidBytes, 0, subsetBytes, 0, _guidByteCount);

        // convert to number
        ulong number = BitConverter.ToUInt64(subsetBytes, 0);
        return number;
    }

    private string EncodeBase62(ulong id)
    {
        var buffer = new char[NumCharsInCode];

        buffer[0] = EncodingAlphabet[(int)(id >> 54) & 61];
        buffer[1] = EncodingAlphabet[(int)(id >> 48) & 61];
        buffer[2] = EncodingAlphabet[(int)(id >> 42) & 61];
        buffer[3] = EncodingAlphabet[(int)(id >> 36) & 61];
        buffer[4] = EncodingAlphabet[(int)(id >> 30) & 61];
        buffer[5] = EncodingAlphabet[(int)(id >> 24) & 61];
        buffer[6] = EncodingAlphabet[(int)(id >> 18) & 61];
        buffer[7] = EncodingAlphabet[(int)(id >> 12) & 61];

        return new string(buffer, 0, buffer.Length);

    }
}
