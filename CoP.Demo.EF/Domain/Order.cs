using CoP.Demo.EF.Domain.DomainEvents;

namespace CoP.Demo.EF.Domain;

public class Order : Entity
{
    public Status Status { get; set; }

    public static Order Create()
    {
        var order = new Order { Id = Guid.NewGuid(), Status = Status.New };
        order.RaiseEvent(new OrderCreatedDomainEvent(order));
        return order;
    }

    public void Update(Status status)
    {
        Status = status;
        RaiseEvent(new OrderUpdatedDomainEvent(this));
    }
}

public enum Status
{
    None,
    New,
    Processing,
    Completed
}