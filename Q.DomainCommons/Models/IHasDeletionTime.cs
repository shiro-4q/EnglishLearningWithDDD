namespace Q.DomainCommons.Models
{
    public interface IHasDeletionTime
    {
        DateTime? DeletionTime { get; }
    }
}
