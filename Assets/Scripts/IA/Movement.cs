public class Movement {
	public byte movement;
	public int score;
	public byte depth;
    public byte checkerIndex;

	public Movement (int _score, byte _movement, byte _checker)
	{
		movement = _movement;
		score = _score;
        checkerIndex = _checker;
	}
	public Movement(int _score, byte _movement, byte _depth, byte _checker)
	{
		movement = _movement;
		score = _score;
		depth = _depth;
        checkerIndex = _checker;
	}
}
