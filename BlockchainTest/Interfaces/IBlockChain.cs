namespace BlockchainTest.Interfaces
{
    public interface IBlockChain
    {
        void AcceptBlock(IBlock block);
        void VerifyChain();
    }
}
