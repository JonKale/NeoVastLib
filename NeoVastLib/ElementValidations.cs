namespace NeoVastLib
{
    using System.Linq;

    public partial class VastElement
    {
        protected override bool ValidateImpl()
        {
            if ( !(this.HasAttributes && this.Attributes().Any(a => a is VersionAttribute)) )
            {
                this.validationFailureMessage = "version attribute missing";
                return false;
            }

            return true;

        }
    }
}
