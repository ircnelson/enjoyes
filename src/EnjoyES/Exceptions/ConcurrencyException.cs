using System;

namespace EnjoyES.Exceptions
{
    public class ConcurrencyException : Exception
    {
        public long CurrentVersion { get; }
        public long ExpectedVersion { get; }

        public ConcurrencyException(long currentVersion, long expectedVersion)
        {
            CurrentVersion = currentVersion;
            ExpectedVersion = expectedVersion;
        }
    }
}