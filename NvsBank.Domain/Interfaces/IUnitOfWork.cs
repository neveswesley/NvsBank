namespace NvsBank.Application.Interfaces;

public interface IUnitOfWork
{
    Task Commit(CancellationToken cancellationToken);
}