using Flunt.Notifications;
using Serverless.Extensions.Shared.Enums;

namespace Serverless.Extensions.Shared.Notifications;

public class NotificationServices : INotificationServices
{
    public StatusCodeOperation StatusCode { get; set; }

    private readonly ICollection<Notification> _notifications;

    public NotificationServices()
    {
        _notifications = new List<Notification>();
    }

    public void AddNotification(Notification notificacao) => _notifications.Add(notificacao);

    public void AddNotification(Notification notificacao, StatusCodeOperation statusCode)
    {
        AddNotification(notificacao);
        AddStatusCode(statusCode);
    }

    public void AddNotifications(IEnumerable<Notification> notificacoes)
    {
        foreach (var notificacao in notificacoes)
        {
            _notifications.Add(notificacao);
        }
    }

    public void AddNotifications(IEnumerable<Notification> notificacoes, StatusCodeOperation statusCode)
    {
        AddNotifications(notificacoes);
        AddStatusCode(statusCode);
    }

    public void AddStatusCode(StatusCodeOperation statusCode) => StatusCode = statusCode;

    public bool HasNotifications() => GetNotifications().Any();

    public IEnumerable<Notification> GetNotifications() => _notifications;

    public void ClearNotifications()
    {
        _notifications.Clear();
    }
}
