using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Repro.Api.Function.Exceptions;

[ExcludeFromCodeCoverage]
[Serializable]
public class NotFoundException : Exception
{
    protected NotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(
        serializationInfo,
        streamingContext)
    { }

    public NotFoundException(Type type, Guid id, string? languageCode)
        : base($"{type.Name} with id '{id}' and languageCode '{languageCode}' could not be found.") { }
}