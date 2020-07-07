namespace MLCS.LearningStatistics.Entities.Default
{
    public class ConfusionMatrix : IConfusionMatrix
    {
        public int TruePositives { get; set; }
        public int FalsePositives { get; set; }
        public int FalseNegatives { get; set; }
        public int TrueNegatives { get; set; }

        public int Positives => TruePositives + FalseNegatives;
        public int Negatives => TrueNegatives + FalsePositives;
        public int PredictedPositives => TruePositives + FalsePositives;
        public int PredictedNegatives => TrueNegatives + FalseNegatives;
        public int CorrectPredictions => TruePositives + TrueNegatives;
        public int Total => TruePositives + FalsePositives + FalseNegatives + TrueNegatives;

        public ConfusionMatrix(int truePositives, int falsePositives, int falseNegatives, int trueNegatives)
        {
            TruePositives = truePositives;
            FalsePositives = falsePositives;
            FalseNegatives = falseNegatives;
            TrueNegatives = trueNegatives;
        }
    }
}