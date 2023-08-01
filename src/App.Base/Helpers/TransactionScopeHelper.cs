using System.Transactions;

namespace App.Base.Helpers;

public static class TransactionScopeHelper
{
    public static TransactionScope TxnScope() => new(TransactionScopeAsyncFlowOption.Enabled);
}
