//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behaviour and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedMember.Global
namespace NeoVastLib
{
    using System.Linq;

    public sealed partial class AdElement : VElement
    {
        protected override bool ValidateGroups()
        {
            if (this.Elements().Any(e => e is InLineElement) ^ this.Elements().Any(e => e is WrapperElement))
            {
                return true;
            }

            this.validationFailureMessage = "children may not contain both the InLineElement and WrapperElement types";
            return false;
        }
    }

    public sealed partial class CompanionElement : VElement
    {
        protected override bool ValidateGroups()
        {
            if (this.Elements().Any(e => e is StaticResourceElement) | this.Elements().Any(e => e is IFrameResourceElement) | this.Elements().Any(e => e is HtmlResourceElement))
            {
                return true;
            }

            this.validationFailureMessage = "children must contain at least one of the StaticResourceElement, IFrameResourceElement, HtmlResourceElement types";
            return false;
        }
    }

    public sealed partial class CreativeElement : VElement
    {
        protected override bool ValidateGroups()
        {
            if (this.Elements().Any(e => e is LinearElement) | this.Elements().Any(e => e is NonLinearAdsElement))
            {
                return true;
            }

            this.validationFailureMessage = "children must contain at least one of the LinearElement, NonLinearAdsElement types";
            return false;
        }
    }

    public sealed partial class IconElement : VElement
    {
        protected override bool ValidateGroups()
        {
            if (this.Elements().Any(e => e is StaticResourceElement) | this.Elements().Any(e => e is IFrameResourceElement) | this.Elements().Any(e => e is HtmlResourceElement))
            {
                return true;
            }

            this.validationFailureMessage = "children must contain at least one of the StaticResourceElement, IFrameResourceElement, HtmlResourceElement types";
            return false;
        }
    }

    public sealed partial class IconClicksElement : VElement
    {
        protected override bool ValidateGroups()
        {
            if (this.Elements().Any(e => e is IconClickThroughElement) | this.Elements().Any(e => e is IconClickTrackingElement))
            {
                return true;
            }

            this.validationFailureMessage = "children must contain at least one of the IconClickThroughElement, IconClickTrackingElement types";
            return false;
        }
    }

    public sealed partial class NonLinearElement : VElement
    {
        protected override bool ValidateGroups()
        {
            if (this.Elements().Any(e => e is StaticResourceElement) | this.Elements().Any(e => e is IFrameResourceElement) | this.Elements().Any(e => e is HtmlResourceElement))
            {
                return true;
            }

            this.validationFailureMessage = "children must contain at least one of the StaticResourceElement, IFrameResourceElement, HtmlResourceElement types";
            return false;
        }
    }

    public sealed partial class VastElement : VElement
    {
        protected override bool ValidateGroups()
        {
            if (this.Elements().Any(e => e is ErrorElement) ^ this.Elements().Any(e => e is AdElement))
            {
                return true;
            }

            this.validationFailureMessage = "children may not contain both the ErrorElement and AdElement types";
            return false;
        }
    }

    public sealed partial class VerificationElement : VElement
    {
        protected override bool ValidateGroups()
        {
            if (this.Elements().Any(e => e is JavaScriptResourceElement) | this.Elements().Any(e => e is ExecutableResourceElement))
            {
                return true;
            }

            this.validationFailureMessage = "children must contain at least one of the JavaScriptResourceElement, ExecutableResourceElement types";
            return false;
        }
    }

    public sealed partial class ViewableImpressionElement : VElement
    {
        protected override bool ValidateGroups()
        {
            if (this.Elements().Any(e => e is ViewableElement) | this.Elements().Any(e => e is NotViewableElement) | this.Elements().Any(e => e is ViewUndeterminedElement))
            {
                return true;
            }

            this.validationFailureMessage = "children must contain at least one of the ViewableElement, NotViewableElement, ViewUndeterminedElement types";
            return false;
        }
    }
}