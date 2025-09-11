namespace Tecnica.Infra
{

    [Serializable]
    public class DomainException : Exception
    {
        internal List<DomainMessage> Messages;
        internal object Context { get; private set; }

        internal const string ContextKey = "context";

        public DomainException() { }
        public DomainException(string? message) : base(message) { }
        public DomainException(List<string> messages)
        {
            Messages = new List<DomainMessage>();

            this.Messages = messages.Select(x => new DomainMessage(x)).ToList();
        }

        public DomainException(
       System.Runtime.Serialization.SerializationInfo info,
       System.Runtime.Serialization.StreamingContext context)
         : base(info, context) { }


        public DomainException(DomainMessage message)
        {
            Messages = new List<DomainMessage>();

            this.Messages.Add(message);
        }


        public DomainException(string message, Exception inner) : base(message, inner) { }

        public DomainException(string message, object context)
         : base(message)
        {
            Messages = new List<DomainMessage>();

            this.Messages.Add(new DomainMessage(message));

            Context = context;
        }
    }
}

