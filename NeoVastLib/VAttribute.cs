namespace NeoVastLib
{
    using System;
    using System.Xml.Linq;

    public abstract class VAttribute : XAttribute, IValidatable
    {
        protected string validationFailureMessage = String.Empty;
        protected VastValidationLevel validationLevel = VastValidationLevel.Unknown;

        protected VAttribute(XName name, object value) : base(name, value)
        {
        }

        protected VAttribute(VAttribute other) : base(other)
        {
        }

        protected virtual bool ValidateImpl() => false;

        public bool Validate() => VastBuilderContext.SkipValidation || this.ValidateImpl();
    }

    public sealed class InvalidVastAttributeException : InvalidVastException
    {
        public string AttributeName { get; }

        public override string Name => this.AttributeName;

        public object AttributeValue { get; }

        public InvalidVastAttributeException(string name, object value, string message) : base(message)
        {
            this.AttributeName = name;
            this.AttributeValue = value;
        }
    }
}