using System;

namespace EnjoyES.Exceptions
{
    public class WrongExpectedVersionException : Exception
    {
        public long CurrentVersion { get; }
        public long ExpectedVersion { get; }

        public WrongExpectedVersionException(long currentVersion, long expectedVersion)
        {
            CurrentVersion = currentVersion;
            ExpectedVersion = expectedVersion;
        }
    }
}