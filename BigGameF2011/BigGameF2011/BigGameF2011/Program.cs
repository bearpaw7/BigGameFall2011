using System;

namespace BigGameF2011
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Shmup game = new Shmup())
            {
                game.Run();
            }
        }
    }
#endif
}

