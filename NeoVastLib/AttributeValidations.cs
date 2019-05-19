namespace NeoVastLib
{
    using System;

    public partial class VersionAttribute
    {
        protected override bool ValidateImpl()
        {
            if ( String.IsNullOrWhiteSpace(this.Value) )
            {
                this.validationFailureMessage = "version value missing";
                return false;
            }

            return true;
        }
    }

    public partial class IdAttribute
    {
        protected override bool ValidateImpl()
        {
            if (String.IsNullOrWhiteSpace(this.Value))
            {
                this.validationFailureMessage = "id value missing";
                return false;
            }

            return true;
        }
    }

    public partial class DeliveryAttribute
    {
        protected override bool ValidateImpl()
        {
            if (String.IsNullOrWhiteSpace(this.Value))
            {
                this.validationFailureMessage = "delivery value missing";
                return false;
            }

            return true;
        }
    }

    public partial class TypeAttribute
    {
        protected override bool ValidateImpl()
        {
            if (String.IsNullOrWhiteSpace(this.Value))
            {
                this.validationFailureMessage = "type value missing";
                return false;
            }

            switch ( this.Value )
            {
                case "video/mp4":
                case "audio/mpeg":
                case "application/x-mpegURL":
                    return true;
                default:
                    this.validationFailureMessage = "unrecognised content type";
                    return false;
            }
        }
    }

    public partial class HeightAttribute
    {
        protected override bool ValidateImpl()
        {
            if (String.IsNullOrWhiteSpace(this.Value))
            {
                this.validationFailureMessage = "height value missing";
                return false;
            }

            if (!Int32.TryParse(this.Value, out var height))
            {
                this.validationFailureMessage = "height is not a number";
                return false;
            }

            if ( height < 0 )
            {
                this.validationFailureMessage = "height must be a positive number (or zero for audio ads)";
                return false;
            }

            return true;
        }
    }

    public partial class WidthAttribute
    {
        protected override bool ValidateImpl()
        {
            if (String.IsNullOrWhiteSpace(this.Value))
            {
                this.validationFailureMessage = "width value missing";
                return false;
            }

            if (!Int32.TryParse(this.Value, out var width))
            {
                this.validationFailureMessage = "width is not a number";
                return false;
            }

            if (width < 0)
            {
                this.validationFailureMessage = "width must be a positive number (or zero for audio ads)";
                return false;
            }

            return true;
        }
    }

    public partial class BitrateAttribute
    {
        protected override bool ValidateImpl()
        {
            if (String.IsNullOrWhiteSpace(this.Value))
            {
                this.validationFailureMessage = "bitrate value missing";
                return false;
            }

            if (!Int32.TryParse(this.Value, out var bitrate))
            {
                this.validationFailureMessage = "bitrate is not a number";
                return false;
            }

            if (bitrate < 1)
            {
                this.validationFailureMessage = "bitrate must be a positive number";
                return false;
            }

            return true;
        }

    }
}
