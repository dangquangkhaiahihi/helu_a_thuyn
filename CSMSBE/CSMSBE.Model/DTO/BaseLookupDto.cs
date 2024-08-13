namespace CSMSBE.Services
{
    public class BaseLookup<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
    }
    
    public abstract class BaseLookupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public abstract class BaseLookupDto<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
    }
}
