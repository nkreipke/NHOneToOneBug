using NHibernate.Cfg;
using NHOneToOneBug;

var factory = new Configuration()
    .Configure()
    .AddAssembly(typeof(Entity1).Assembly)
    .BuildSessionFactory();

Guid id;

// Insert Entity1 -> Entity2
using (var session = factory.OpenSession())
{
    var entity = new Entity1
    {
        Child = new Entity2()
    };

    entity.Child.Parent = entity;

    using (var transaction = session.BeginTransaction())
    {
        session.Save(entity);
        transaction.Commit();
    }

    id = entity.Id;
}

// Remove Entity1 -> Entity2 association (cascade all-delete-orphan should delete Entity2)
using (var session = factory.OpenSession())
{
    Console.WriteLine("Deleting entity");

    using (var transaction = session.BeginTransaction())
    {
        var entity = session.Query<Entity1>().First(x => x.Id == id);

        entity.Child = null;

        session.Update(entity);

        // !!! This is required for the bug to trigger
        session.Flush();

        // !!! InvalidCastException
        transaction.Commit();
    }
}
