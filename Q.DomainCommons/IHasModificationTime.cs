namespace Q.DomainCommons
{
    public interface IHasModificationTime
    {
        DateTime? LastModificationTime { get; }
    }
}
