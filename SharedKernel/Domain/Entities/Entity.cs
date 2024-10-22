namespace SharedKernel.Domain.Entities;

[Serializable]
public abstract class Entity : IEntity
{
    public abstract object[]? GetKeys();

    public override string ToString()
    {
        return $"[ENTITY: {GetType().Name}] Keys = {GetKeys()}";
    }
}

[Serializable]
public abstract class Entity<TKey> : Entity, IEntity<TKey>
{
    public TKey Id { get; protected set; }

    protected Entity()
    {

    }
    protected Entity(TKey id)
    {
        Id = id;
    }
    public override object?[] GetKeys()
    {
        return [Id];
    }
    public override string ToString()
    {
        return $"[ENTITY: {GetType().Name}] Id = {Id}";
    }
}
