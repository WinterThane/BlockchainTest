using System;
using System.Collections.Generic;

namespace BlockchainMultiTransactionTest.Interfaces
{
    public interface IBlock
    {
        //list of transactions
        List<ITransaction> Transactions { get; }

        //block header
        int BlockNumber { get; }
        DateTime CreatedDate { get; set; }
        string BlockHash { get; }
        string PreviousBlockHash { get; set; }

        void AddTransaction(ITransaction transaction);
        string CalculateBlockHash(string previousBlockHash);
        void SetBlockHash(IBlock parent);
        IBlock NextBlock { get; set; }
        bool IsValidChain(string prevBlockHash, bool verbose);
    }
}
