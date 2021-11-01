using System.Collections.Generic;

namespace JiufenGames.TetrisAlike.Logic
{
    public interface InputsListener<T>
    {
        List<T> GetCurrentInputsPressed();
    }
}