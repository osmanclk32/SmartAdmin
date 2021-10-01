namespace SmartAdmin.Infra.Data
{
    //public class UnitOfWork : IUnitOfWork
    //{
    //    private readonly DbSession _session;

    //    public UnitOfWork(DbSession session)
    //    {
    //        _session = session;
    //    }

    //    public void BeginTrans()
    //    {
    //        _session.Transaction = _session.Connection.BeginTransaction();
    //    }

    //    public void CommitTrans()
    //    {
    //        _session.Transaction.Commit();

    //        Dispose();
    //    }

    //    public void Rollback()
    //    {
    //        _session.Transaction.Rollback();

    //        Dispose();
    //    }

    //    public void Dispose() => _session.Transaction?.Dispose();
    //}
}
