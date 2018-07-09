using System;

namespace BlockchainTest.Interfaces
{
    public interface IBlock
    {
        //-- data
        string ClaimNumber { get; set; }
        decimal SettlementAmount { get; set; }
        DateTime SettlementDate { get; set; }
        string CarRegistration { get; set; }
        int Milage { get; set; }
        ClaimType ClaimType { get; set; }

        //-- block header
        int BlockNumber { get; }
        DateTime CreatedDate { get; set; }
        string BlockHash { get; }
        string PreviousBlockHash { get; set; }

        //-- methods
        string CalculateBlockHash(string previousBlockHash);
        void SetBlockHash(IBlock parent);
        IBlock NextBlock { get; set; }
        bool IsValidChain(string prevBlockHash, bool verbose);
    }
}
