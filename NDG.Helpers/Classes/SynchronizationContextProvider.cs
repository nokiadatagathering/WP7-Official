using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;

namespace NDG.Helpers.Classes
{
    public static class SynchronizationContextProvider
    {
        private static bool isInitialized;
        private static SynchronizationContext syncContext;

        public static SynchronizationContext UIThreadSyncContext
        {
            get
            {
                if (!SynchronizationContextProvider.isInitialized)
                    SynchronizationContextProvider.Initialize();
                return SynchronizationContextProvider.syncContext;
            }
            set
            {
                SynchronizationContextProvider.syncContext = value;
            }
        }

        public static void Initialize()
        {
            SynchronizationContextProvider.syncContext = SynchronizationContext.Current;
            if (SynchronizationContextProvider.syncContext == null)
                throw new InvalidOperationException("Initialization of SynchronizationContexProvider must be called only from UI thread.");
            SynchronizationContextProvider.isInitialized = true;
        }
    }
}
