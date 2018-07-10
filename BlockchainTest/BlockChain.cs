using BlockchainTest.Interfaces;
using System;
using System.Collections.Generic;

namespace BlockchainTest
{
    public class BlockChain : IBlockChain
    {
        public IBlock CurrentBlock { get; private set; }
        public IBlock HeadBlock { get; private set; }

        public List<IBlock> Blocks { get; }

        public BlockChain()
        {
            Blocks = new List<IBlock>();
        }

        public void AcceptBlock(IBlock block)
        {
            if(HeadBlock == null)
            {
                HeadBlock = block;
                HeadBlock.PreviousBlockHash = null;
            }

            CurrentBlock = block;
            Blocks.Add(block);
        }

        public void VerifyChain()
        {
            if(HeadBlock == null)
            {
                throw new InvalidOperationException("Genesis block not set");
            }

            var isValid = HeadBlock.IsValidChain(null, true);

            if(isValid)
            {
                Console.WriteLine("Blockchain integrity intact.");
            }
            else
            {
                Console.WriteLine("Blockchain integrity NOT intact.");
            }
        }
    }
}
