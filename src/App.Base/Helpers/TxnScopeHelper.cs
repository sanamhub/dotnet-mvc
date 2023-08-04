using System.Transactions;

namespace App.Base.Helpers;

public static class TxnScopeHelper
{
    public static TransactionScope NewTxnScope => new(TransactionScopeAsyncFlowOption.Enabled);
}
