namespace Core.Entities
{
    public abstract class EntityBase<T>
    {
        protected EntityBase(T id)
        {
            this.Id = id;
        }

        public T Id { get; }
    }
}
