namespace NHOneToOneBug;

public class Entity2
{
    public virtual Guid Id { get; set; }

    public virtual Entity1? Parent { get; set; }
}