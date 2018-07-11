using BlockChainCrypto;
using BlockchainTransactionPoolTest.Interfaces;
using Clifton.Blockchain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainTransactionPoolTest
{
    public class Block : IBlock
    {
        public List<ITransaction> Transactions { get; private set; }

        public int BlockNumber { get; private set; }
        public DateTime CreatedDate { get; set; }
        public string BlockHash { get; private set; }
        public string PreviousBlockHash { get; set; }
        public string BlockSignature { get; private set; }

        public IBlock NextBlock { get; set; }
        public IKeyStore KeyStore { get; private set; }
        private MerkleTree merkleTree = new MerkleTree();

        public Block(int blockNumber)
        {
            BlockNumber = blockNumber;

            CreatedDate = DateTime.UtcNow;
            Transactions = new List<ITransaction>();
        }

        public Block(int blockNumber, IKeyStore keyStore)
        {
            BlockNumber = blockNumber;

            CreatedDate = DateTime.UtcNow;
            Transactions = new List<ITransaction>();
            KeyStore = keyStore;
        }

        public void AddTransaction(ITransaction transaction)
        {
            Transactions.Add(transaction);
        }

        public string CalculateBlockHash(string previousBlockHash)
        {
            var blockHeader = BlockNumber + CreatedDate.ToString() + previousBlockHash;
            var combined = merkleTree.RootNode + blockHeader;

            string completeBlockHash;

            if(KeyStore == null)
            {
                completeBlockHash = Convert.ToBase64String(Hashing.ComputeHashSha256(Encoding.UTF8.GetBytes(combined)));
            }
            else
            {
                completeBlockHash = Convert.ToBase64String(HMAC.ComputeHmacSha256(Encoding.UTF8.GetBytes(combined), KeyStore.AuthenticatedHashKey));
            }

            return completeBlockHash;
        }

        public bool IsValidChain(string prevBlockHash, bool verbose)
        {
            bool isValid = true;
            bool validSignature = false;

            BuildMerkleTree();

            validSignature = KeyStore.VerifyBlock(BlockHash, BlockSignature);

            string newBlockHash = CalculateBlockHash(prevBlockHash);

            validSignature = KeyStore.VerifyBlock(newBlockHash, BlockSignature);

            if (newBlockHash != BlockHash)
            {
                isValid = false;
            }
            else
            {
                isValid |= PreviousBlockHash == prevBlockHash;
            }

            PrintVerificationMessage(verbose, isValid, validSignature);

            if (NextBlock != null)
            {
                return NextBlock.IsValidChain(newBlockHash, verbose);
            }

            return isValid;
        }

        private void PrintVerificationMessage(bool verbose, bool isValid, bool validSignature)
        {
            if (verbose)
            {
                if (!isValid)
                {
                    Console.WriteLine("Block Number " + BlockNumber + " : FAILED VERIFICATION");
                }
                else
                {
                    Console.WriteLine("Block Number " + BlockNumber + " : PASS VERIFICATION");
                }

                if (!validSignature)
                {
                    Console.WriteLine("Block Number " + BlockNumber + " : Invalid Digital Signature");
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

            if(KeyStore != null)
            {
                BlockSignature = KeyStore.SignBlock(BlockHash);
            }
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
