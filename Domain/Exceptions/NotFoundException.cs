using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Domain.Exceptions;

[Serializable]
public class NotFoundException : Exception
{
    public NotFoundException() : base() { }

    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string message, Exception innerException) : base(message, innerException) { }

    public NotFoundException(string entityName, string id) : base($"\"{entityName}\" with id {id} was not found") { }

    protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
