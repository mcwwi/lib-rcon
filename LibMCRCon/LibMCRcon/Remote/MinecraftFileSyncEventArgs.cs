using System;

namespace LibMCRcon.Remote
{
    public class MinecraftFileSyncEventArgs : EventArgs
    {
        public MinecraftFileSyncEventArgs(Action Completed) { this.Completed = Completed; }
        public Action Completed { get; private set; }
    }
  

    


    
}