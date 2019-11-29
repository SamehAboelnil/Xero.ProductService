using System;
using System.Runtime.Serialization;

namespace Xero.Product.Data
{
    [Serializable]
    public class ProductOptionNotFoundException : Exception
    {
        public ProductOptionNotFoundException()
        {
        }

        public ProductOptionNotFoundException(string message) : base(message)
        {
        }

        public ProductOptionNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProductOptionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}