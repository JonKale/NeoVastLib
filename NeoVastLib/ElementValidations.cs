namespace NeoVastLib
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    public partial class VastElement
    {
        protected override bool ValidateImpl()
        {
            if ( !(this.Attributes().Any(a => a is VersionAttribute)) )
            {
                this.validationFailureMessage = "version attribute missing";
                return false;
            }

            return true;

        }
    }

    public partial class AdElement
    {
        protected override bool ValidateImpl()
        {
            if (!(this.Attributes().Any(a => a is IdAttribute)))
            {
                this.validationFailureMessage = "id attribute missing";
                return false;
            }

            return true;

        }
    }

    public partial class ErrorElement
    {
        protected override bool ValidateImpl() =>
            this.FirstNode is XCData errorUrl && Uri.IsWellFormedUriString(errorUrl.Value, UriKind.RelativeOrAbsolute);
    }

    public partial class ImpressionElement
    {
        protected override bool ValidateImpl() =>
            this.FirstNode is XCData errorUrl && Uri.IsWellFormedUriString(errorUrl.Value, UriKind.RelativeOrAbsolute);
    }

    public partial class AdSystemElement
    {
        protected override bool ValidateImpl() => !String.IsNullOrWhiteSpace(this.Value);
    }

    public partial class AdTitleElement
    {
        protected override bool ValidateImpl() => !String.IsNullOrWhiteSpace(this.Value);
    }

    public partial class AdServingIdElement
    {
        protected override bool ValidateImpl() => !String.IsNullOrWhiteSpace(this.Value);
    }

    public partial class DurationElement
    {
        protected override bool ValidateImpl() => TimeSpan.TryParse(this.Value, out var duration);
    }

    public partial class MediaFileElement
    {
        protected override bool ValidateImpl()
        {
            if ( !(this.FirstNode is XCData mediaFileUrl) )
            {
                this.validationFailureMessage = $@"child must be CDATA";
                return false;
            }

            if ( !Uri.IsWellFormedUriString(mediaFileUrl.Value, UriKind.RelativeOrAbsolute) )
            {
                this.validationFailureMessage = $@"""{(mediaFileUrl ?? new XCData("")).Value}"" is not a valid URI";
                return false;
            }

            foreach ( var mandatoryAttribute in new[] {"delivery", "type", "width", "height"} )
            {
                if ( this.Attribute(mandatoryAttribute) == null )
                {
                    this.validationFailureMessage = $@"required ""{mandatoryAttribute}"" attribute missing";
                    return false;
                }
            }

            return true;

        }
    }
}
