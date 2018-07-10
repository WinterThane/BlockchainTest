using System;

namespace BlockchainMultiTransactionTest.Interfaces
{
    public interface ITransaction
    {
        string ClaimNumber { get; set; }
        decimal SettlementAmount { get; set; }
        DateTime SettlementDate { get; set; }
        string CarRegistration { get; set; }
        int Milage { get; set; }
        ClaimType ClaimType { get; set; }

        string CalculateTransactionHash();
    }
}
