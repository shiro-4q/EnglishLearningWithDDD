namespace Q.DomainCommons.Models
{
    public interface IHasModificationTime
    {
        DateTime? LastModificationTime { get; }
    }
}
