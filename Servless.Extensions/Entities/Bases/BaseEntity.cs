using Flunt.Notifications;
using MongoDB.Bson;

namespace Serverless.Extensions.Entities.Bases;

public abstract class BaseEntity : Notifiable<Notification>
{
    public ObjectId Id { get; private set; }

    public abstract void Validate();
}
