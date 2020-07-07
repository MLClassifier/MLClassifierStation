namespace MLCS.LearningStatistics.Entities
{
    public interface IConfusionMatrix
    {
        int TruePositives { get; set; }
        int FalsePositives { get; set; }
        int FalseNegatives { get; set; }
        int TrueNegatives { get; set; }

        int Positives { get; }
        int Negatives { get; }
        int PredictedPositives { get; }
        int PredictedNegatives { get; }
        int CorrectPredictions { get; }
        int Total { get; }
    }
}