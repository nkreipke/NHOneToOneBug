namespace NHOneToOneBug;

public class Entity1
{
    public virtual Guid Id { get; set; }

    public virtual Entity2? Child { get; set; }
}