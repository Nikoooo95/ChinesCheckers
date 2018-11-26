public class Movement {

    /// <summary>
    /// Numero de movimiento
    /// </summary>
	public byte movement;

    /// <summary>
    /// Puntuacion del movimiento
    /// </summary>
	public int score;

    /// <summary>
    /// Profundidad del movimiento
    /// </summary>
	public byte depth;

    /// <summary>
    /// Ficha del movimiento
    /// </summary>
    public byte checkerIndex;

    /// <summary>
    /// Constructor con tres parametros sin tener en cuenta la profundidad
    /// </summary>
    /// <param name="_score"></param>
    /// <param name="_movement"></param>
    /// <param name="_checker"></param>
	public Movement (int _score, byte _movement, byte _checker)
	{
		movement = _movement;
		score = _score;
        checkerIndex = _checker;
	}

    /// <summary>
    /// Constructor con cuatro parametros teniendo en cuenta la profundidad
    /// </summary>
    /// <param name="_score"></param>
    /// <param name="_movement"></param>
    /// <param name="_depth"></param>
    /// <param name="_checker"></param>
	public Movement(int _score, byte _movement, byte _depth, byte _checker)
	{
		movement = _movement;
		score = _score;
		depth = _depth;
        checkerIndex = _checker;
	}
}
