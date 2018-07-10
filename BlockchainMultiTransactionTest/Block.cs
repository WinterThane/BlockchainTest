using BlockChainCrypto;
using BlockchainMultiTransactionTest.Interfaces;
using Clifton.Blockchain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainMultiTransactionTest
{
    class Block : IBlock
    {
        public List<ITransaction> Transactions { get; private set; }

        public int BlockNumber { get; private set; }
        public DateTime CreatedDate { get; set; }
        public string BlockHash { get; private set; }
        public string PreviousBlockHash { get; set; }
        public IBlock NextBlock { get; set; }
        private MerkleTree merkleTree = new MerkleTree();

        public Block(int blockNumber)
        {
            BlockNumber = blockNumber;

            CreatedDate = DateTime.UtcNow;
            Transactions = new List<ITransaction>();
        }

        public void AddTransaction(ITransaction transaction)
        {
            Transactions.Add(transaction);
        }

        public string CalculateBlockHash(string previousBlockHash)
        {
            var blockHeader = BlockNumber + CreatedDate.ToString() + previousBlockHash;
            var combined = merkleTree.RootNode + blockHeader;

            return Convert.ToBase64String(Hashing.ComputeHashSha256(Encoding.UTF8.GetBytes(combined)));
        }

        public bool IsValidChain(string prevBlockHash, bool verbose)
        {
            var isValid = true;

            BuildMerkleTree();

            var newBlockHash = CalculateBlockHash(prevBlockHash);
            if(newBlockHash != BlockHash)
            {
                isValid = false;
            }
            else
            {
                isValid |= PreviousBlockHash == prevBlockHash;
            }

            PrintVerificationMessage(verbose, isValid);

            if (NextBlock != null)
            {
                return NextBlock.IsValidChain(newBlockHash, verbose);
            }

            return isValid;
        }

        private void PrintVerificationMessage(bool verbose, bool isValid)
        {
            if (verbose)
            {
                if (!isValid)
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

            BuildMerkleTree();

            BlockHash = CalculateBlockHash(PreviousBlockHash);
        }

        private void BuildMerkleTree()
        {
            merkleTree = new MerkleTree();

            foreach (ITransaction item in Transactions)
            {
                merkleTree.AppendLeaf(MerkleHash.Create(item.CalculateTransactionHash()));
            }

            merkleTree.BuildTree();
        }
    }
}
