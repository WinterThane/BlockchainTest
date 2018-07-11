using BlockChainCrypto;
using System;
using System.Diagnostics;
using System.Text;

namespace ProofOfWork
{
    public class ProofOfWork
    {
        public string MyData { get; private set; }
        public int Difficulty { get; private set; }
        public int Nonce { get; private set; }

        public ProofOfWork(string dataToHash, int difficulty)
        {
            MyData = dataToHash;
            Difficulty = difficulty;
        }

        public string CalculateProofOfWork()
        {
            string difficulty = DifficultyString();
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            while(true)
            {
                var hashedData = Convert.ToBase64String(Hashing.ComputeHashSha256(Encoding.UTF8.GetBytes(Nonce + MyData)));

                if(hashedData.StartsWith(difficulty, StringComparison.Ordinal))
                {
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;

                    var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                    Console.WriteLine($"Difficulty level {Difficulty} - Nonce = {Nonce} - Elapsed time: {elapsedTime} - {hashedData}");
                    return hashedData;
                }

                Nonce++;
            }
        }

        private string DifficultyString()
        {
            var difficultyString = string.Empty;

            for (int i = 0; i < Difficulty; i++)
            {
                difficultyString += "0";
            }

            return difficultyString;
        }
    }
}
