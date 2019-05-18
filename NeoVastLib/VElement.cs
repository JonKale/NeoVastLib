namespace NeoVastLib
{
    using System;
    using System.Xml.Linq;

    public interface IValidatable
    {
        bool Validate();
    }

    public abstract class VElement : XElement, IValidatable
    {
        protected string validationFailureMessage = String.Empty;
        protected VastValidationLevel validationLevel = VastValidationLevel.Unknown;

        protected VElement(XName name) : base(name)
        {
        }

        protected VElement(XName name, object content) : base(name, content)
        {
        }

        protected VElement(XName name, params object[] content) : base(name, content)
        {
        }

        protected VElement(VElement other) : base(other)
        {
        }

        protected VElement(XStreamingElement other) : base(other)
        {
        }

        protected virtual bool ValidateImpl() => false;

        protected virtual bool ValidateContained() => true;

        protected virtual bool ValidateGroups() => true;

        public bool Validate() =>
            VastBuilderContext.SkipValidation || (this.ValidateContained() && this.ValidateGroups() && this.ValidateImpl());
    }

    public sealed class InvalidVastElementException : Exception
    {
        public string ElementName { get; }

        public InvalidVastElementException(string name, string message) : base(message) => this.ElementName = name;
    }
}
