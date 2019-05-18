namespace NeoVastLib
{
    using System;

    [Flags]
    public enum VastValidationLevel
    {
        Unknown = 0,
        Vast20 = 1,
        Vast30 = 2,
        Vast40 = 4,
        Vast41 = 8,

        All = VastValidationLevel.Vast20 | 
              VastValidationLevel.Vast30 | 
              VastValidationLevel.Vast40 |
              VastValidationLevel.Vast41
    }
}