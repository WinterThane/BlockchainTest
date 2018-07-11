using System;
using System.Collections.Generic;

namespace BlockchainTransactionPoolTest.Interfaces
{
    public interface IBlock
    {
        List<ITransaction> Transactions { get; }

        //data
        int BlockNumber { get; }
        DateTime CreatedDate { get; set; }
        string BlockHash { get; }
        string PreviousBlockHash { get; set; }
        string BlockSignature { get; }

        void AddTransaction(ITransaction transaction);
        string CalculateBlockHash(string previousBlockHash);
        void SetBlockHash(IBlock parent);
        IBlock NextBlock { get; set; }
        bool IsValidChain(string prevBlockHash, bool verbose);
        IKeyStore KeyStore { get; }
    }
}
