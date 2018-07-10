using BlockChainCrypto;
using BlockchainMultiTransactionTest.Interfaces;
using System;
using System.Text;

namespace BlockchainMultiTransactionTest
{
    public class Transaction : ITransaction
    {
        public string ClaimNumber { get; set; }
        public decimal SettlementAmount { get; set; }
        public DateTime SettlementDate { get; set; }
        public string CarRegistration { get; set; }
        public int Milage { get; set; }
        public ClaimType ClaimType { get; set; }

        public Transaction(string claimNumber,
                            decimal settlementAmount,
                            DateTime settlementDate,
                            string carRegistration,
                            int mileage,
                            ClaimType claimType)
        {
            ClaimNumber = claimNumber;
            SettlementAmount = settlementAmount;
            SettlementDate = settlementDate;
            CarRegistration = carRegistration;
            Milage = mileage;
            ClaimType = claimType;
        }

        public string CalculateTransactionHash()
        {
            string txnHash = ClaimNumber + SettlementAmount + SettlementDate + CarRegistration + Milage + ClaimType;
            return Convert.ToBase64String(Hashing.ComputeHashSha256(Encoding.UTF8.GetBytes(txnHash)));
        }
    }
}
