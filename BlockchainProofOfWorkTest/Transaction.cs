﻿using BlockChainCrypto;
using BlockchainProofOfWorkTest.Interfaces;
using System;
using System.Text;

namespace BlockchainProofOfWorkTest
{
    public class Transaction : ITransaction
    {
        public string ClaimNumber { get; set; }
        public decimal SettlementAmount { get; set; }
        public DateTime SettlementDate { get; set; }
        public string CarRegistration { get; set; }
        public int Mileage { get; set; }
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
            Mileage = mileage;
            ClaimType = claimType;
        }

        public string CalculateTransactionHash()
        {
            string txnHash = ClaimNumber + SettlementAmount + SettlementDate + CarRegistration + Mileage + ClaimType;
            return Convert.ToBase64String(Hashing.ComputeHashSha256(Encoding.UTF8.GetBytes(txnHash)));
        }
    }
}
