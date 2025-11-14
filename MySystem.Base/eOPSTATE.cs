using System;
using System.ComponentModel;


namespace MySystem.Base
{
    [TypeConverter(typeof(EnumConverter))]
    [Flags] public enum eOPSTATE
    {
        INIT = 1,
        NEW = 2,
        DIRTY = 4,
        DELETE = 16,
    }
    public interface IOPState
    {
        eOPSTATE GetOPState();
        void SetOPState(eOPSTATE state);
        bool IsContentModified { get; }
    }
}
