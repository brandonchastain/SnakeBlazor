using System;
namespace SnakeLib
{
    public interface IPlayerInput
    {
        void SetEnterPressed();
        void SetEscapePressed();
        void SetHPressed();
        bool GetEscapePressed();
        bool GetEnterPressed();
        bool GetHPressed();
        void OnKeyDown(string keyCode);
        Direction GetPlayerDirection();
    }
}
