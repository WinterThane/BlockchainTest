using BlockChainCrypto;
using BlockchainTest.Interfaces;
using System;
using System.Text;

namespace BlockchainTest
{
    public class Block : IBlock
    {
        //-- data
        public string ClaimNumber { get; set; }
        public decimal SettlementAmount { get; set; }
        public DateTime SettlementDate { get; set; }
        public string CarRegistration { get; set; }
        public int Milage { get; set; }
        public ClaimType ClaimType { get; set; }

        public int BlockNumber { get; private set; }
        public DateTime CreatedDate { get; set; }
        public string BlockHash { get; private set; }
        public string PreviousBlockHash { get; set; }
        public IBlock NextBlock { get; set; }


        public Block(int blockNumber, 
                     string claimNumber, 
                     decimal settlementAmount, 
                     DateTime settlementDate, 
                     string carRegistration, 
                     int milage, 
                     ClaimType claimType,                   
                     IBlock parent)
        {
            BlockNumber = blockNumber;
            ClaimNumber = claimNumber;
            SettlementAmount = settlementAmount;
            SettlementDate = settlementDate;
            CarRegistration = carRegistration;
            Milage = milage;
            ClaimType = claimType;
            CreatedDate = DateTime.UtcNow;
            SetBlockHash(parent);
        }

        public string CalculateBlockHash(string previousBlockHash)
        {
            var tnxHash = ClaimNumber + SettlementAmount + SettlementDate + CarRegistration + Milage + ClaimType;
            var blockHeader = BlockNumber + CreatedDate.ToString() + previousBlockHash;
            var combined = tnxHash + blockHeader;

            return Convert.ToBase64String(Hashing.ComputeHashSha256(Encoding.UTF8.GetBytes(combined)));
        }

        public bool IsValidChain(string prevBlockHash, bool verbose)
        {
            bool isValid = true;

            var newBlockHash = CalculateBlockHash(prevBlockHash);
            if(newBlockHash != prevBlockHash)
            {
                isValid = false;
            }
            else
            {
                isValid |= PreviousBlockHash == prevBlockHash;
            }

            PrintVerificationMessage(verbose, isValid);

            if(NextBlock != null)
            {
                return NextBlock.IsValidChain(newBlockHash, verbose);
            }

            return isValid;
        }

        private void PrintVerificationMessage(bool verbose, bool isValid)
        {
            if(verbose)
            {
                if(!isValid)
                {
                    Console.WriteLine($"Block Number {BlockNumber} : FAILED verification");
                }
                else
                {
                    Console.WriteLine($"Block Number {BlockNumber} : PASS verification");
                }
            }
        }

        public void SetBlockHash(IBlock parent)
        {
            if(parent != null)
            {
                PreviousBlockHash = parent.BlockHash;
                parent.NextBlock = this;
            }
            else
            {
                PreviousBlockHash = null;
            }

            BlockHash = CalculateBlockHash(PreviousBlockHash);
        }
    }
}
