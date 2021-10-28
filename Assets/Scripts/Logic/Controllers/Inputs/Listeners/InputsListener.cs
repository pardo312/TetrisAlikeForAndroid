using System.Collections.Generic;

namespace JiufenGames.TetrisAlike.Logic
{
    public interface InputsListener
    {
        List<TetrisInputs> GetCurrentInputsPressed();
    }
}
