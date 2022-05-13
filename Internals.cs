using System;

namespace Shipstone.OpenBook
{
    internal static class Internals
    {
        internal const int _PasswordMaxLength = 32;
        internal const int _PasswordMinLength = 8;
        internal const int _PostContentMaxLength = 256;

        internal const String _UserNameAllowedCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";

        internal const int _UserNameMaxLength = 16;
        internal const int _UserNameMinLength = 8;

        internal static readonly TimeSpan _Lockout;

        static Internals() => Internals._Lockout = TimeSpan.FromMinutes(15);
    }
}
