namespace Common
{
    public abstract class StringEnum<TType> where TType : StringEnum<TType>
    {
        public string Value { get; }

        protected StringEnum(string value)
        {
            Value = value;
        }
        
        private bool Equals(TType other)
        {
            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var @enum = obj as TType;
            return @enum != null && Equals(@enum);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
        
        public static implicit operator string(StringEnum<TType> c)
        {
            return c.Value;
        }
    }
}