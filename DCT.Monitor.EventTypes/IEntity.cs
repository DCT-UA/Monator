namespace DCT.Monitor.Entities
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
