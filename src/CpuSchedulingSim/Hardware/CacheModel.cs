namespace CpuSchedulingSim.Hardware;

public sealed class CacheModel
{
    private readonly double _hitProbability;
    private readonly int _missPenalty;

    public CacheModel(double hitProbability, int missPenalty)
    {
        _hitProbability = hitProbability;
        _missPenalty = missPenalty;
    }

    public bool IsHit(Random rng) => rng.NextDouble() < _hitProbability;

    public int MissPenalty => _missPenalty;
}
